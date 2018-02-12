


using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 9:30:05 AM
*  @Description:    
* ==============================================================================
*/
using BeardedManStudios.Forge.Networking.Frame;
using System;

#if WINDOWS_UWP
using Windows.Networking.Sockets;
using Windows.Networking;
using System.IO;
#else
using System.Net.Sockets;
#endif

namespace ihaiu
{
    public abstract class HTCPClientBase : HBaseTCP, IClient
    {

        public ProtoClient protoClient;
        public HTCPClientBase():base()
        {
            protoClient = new ProtoClient(this);
            proto = protoClient;
        }

        /// <summary>
        /// 连接到服务器的原始客户端的引用
        /// The reference to the raw client that is connected to the server
        /// </summary>
#if WINDOWS_UWP
		public StreamSocket client { get; private set; }
#else
        public TcpClient client { get; private set; }
#endif



        /// <summary>
        /// 我们是否断开连接
        /// Whether we are disconnected or not
        /// </summary>
        private bool disconnectedSelf = false;

        /// <summary>
        /// 作为玩家的服务器的身份
        /// The identity of the server as a player
        /// </summary>
        protected NetworkingPlayer server = null;
        public NetworkingPlayer Server { get { return server; } }

        public event BaseNetworkEvent connectAttemptFailed;

        /// <summary>
        /// 用于确定在读取方法期间发生了什么以及执行了哪些操作
        /// 如果从无限循环线程调用它，则应该采取这种措施
        /// Used to determine what happened during the read method and what actions
        /// that should be taken if it was called from an infinite loop thread
        /// </summary>
        protected enum ReadState
        {
            // 再次调Read()读取方法
            Void,
            // 停止运行，断开连接
            Disconnect,
            // 循环里 睡眠10ms
            Continue
        }

        private void RawWrite(byte[] data)
        {
#if WINDOWS_UWP
			//Write data to the echo server.
			Stream streamOut = client.OutputStream.AsStreamForWrite();
			StreamWriter writer = new StreamWriter(streamOut);
			writer.Write(data);
			writer.Flush();
#else
            client.GetStream().Write(data, 0, data.Length);
#endif
        }

        /// <summary>
        /// The direct byte send method to the specified client
        /// </summary>
        /// <param name="client">The target client that will receive the frame</param>
        /// <param name="frame">The frame that is to be sent to the specified client</param>
        /// <summary>
        ///直接字节发送方法到指定的客户端
        /// </ summary>
        /// <param name =“client”>将接收帧的目标客户端</ param>
        /// <param name =“frame”>要发送到指定客户端的帧</ param>
        public virtual void Send(ProtoMsg frame)
        {
            //确保我们没有写入同一个客户端的竞争条件
            // Make sure that we don't have any race conditions with writing to the same client
            lock (client)
            {
                //从帧中获取原始字节并发送
                // Get the raw bytes from the frame and send them
                byte[] data = frame.GetData();
                RawWrite(data);
            }
        }


        public override void SendToPlayer(TcpClient client, ProtoMsg frame)
        {
            Send(frame);
        }


        /// <summary>
        /// This will begin the connection for TCP, this is a thread blocking operation
        /// until the connection is either established or has failed
        /// </summary>
        /// <param name="hostAddress">Ip Address to connect to</param>
        /// <param name="port">[15937] Port connect to on the server</param>
#if WINDOWS_UWP
		public async virtual void Connect(string host, ushort port = DEFAULT_PORT)
#else
        public virtual void Connect(string host, ushort port = DEFAULT_PORT)
#endif
        {
            if (Disposed)
                throw new ObjectDisposedException("TCPClientBase", "This object has been disposed and can not be used to connect, please use a new TCPClientBase");

            try
            {
                disconnectedSelf = false;
                // Create the raw TcpClient that is to communicate with the server's tcp listener
#if WINDOWS_UWP
				client = new StreamSocket();
				await client.ConnectAsync(new HostName(host), port.ToString());
#else
                try
                {
                    client = new TcpClient(host, port);
                }
                catch
                {
                    if (connectAttemptFailed != null)
                        connectAttemptFailed(this);

                    return;
                }
#endif

                //这是一个成功的约束，所以为它启动事件
                // This was a successful bind so fire the events for it
                OnBindSuccessful();

                Initialize(host, port);
            }
            catch (Exception e)
            {
                // This was a failed bind so fire the events for it
                OnBindFailure();

                throw new BaseNetworkException("Host is invalid or not found", e);
            }
        }

        /// <summary>
        /// Setup everything required for this client to be accepted by the server and
        /// construct / send the connection header
        /// </summary>
        /// <param name="port">The port that was connected to on the remote host</param>
        /// <summary>
        ///设置这个客户端所需要的一切，以便服务器和服务器接受
        ///构造/发送连接头
        /// </ summary>
        /// <param name =“port”>在远程主机上连接的端口</ param>
        protected virtual void Initialize(string host, ushort port)
        {

            //将服务器的身份设置为玩家
            // Setup the identity of the server as a player
            server = new NetworkingPlayer(0, host, true, client, this);

            //让我知道我连接成功
            //Let myself know I connected successfully
            OnPlayerConnected(server);
            //将自己设置为连接的客户端
            // Set myself as a connected client
            server.Connected = true;
        }


        /// <summary>
        /// 无限循环在单独的线程上监听来自所有连接客户端的新数据。
        /// readThreadCancel设置为true时，此循环会中断
        /// 
        /// Infinite loop listening for new data from all connected clients on a separate thread.
        /// This loop breaks when readThreadCancel is set to true
        /// </summary>
        protected ReadState Read()
        {
            if (disconnectedSelf)
                return ReadState.Disconnect;

            NetworkStream stream = client.GetStream();

            if (stream == null) //Some reason the stream is null! //某些原因流为空！
                return ReadState.Continue;

            //如果流不再可读，则断开连接
            // If the stream no longer can read then disconnect
            if (!stream.CanRead)
            {
                Disconnect(true);
                return ReadState.Disconnect;
            }

            //如果没有可用的数据，则通过休眠线程释放CPU
            // If there isn't any data available, then free up the CPU by sleeping the thread
            if (!stream.DataAvailable)
                return ReadState.Continue;

            int available = client.Available;

            if (available == 0)
                return ReadState.Continue;

            
            // 读取消息
            ProtoMsg protoMsg = GetNextBytes(stream, available, false);
            //客户端已经告诉服务器它正在断开连接
            if (protoMsg.protoId == ProtoId.ConnectionClose)
            {
                Disconnect(true);
                return ReadState.Disconnect;
            }

            proto.OnMessage(protoMsg, server);

            return ReadState.Void;
        }


        /// <summary>
        /// Disconnect this client from the server
        /// </summary>
        /// <param name="forced">Used to tell if this disconnect was intentional <c>false</c> or caused by an exception <c>true</c></param>
        public override void Disconnect(bool forced)
        {
            if (client != null)
            {
                lock (client)
                {
                    disconnectedSelf = true;

                    // Close our TcpClient so that it can no longer be used
                    if (forced)
                        client.Close();
                    else
                        protoClient.SendConnectionClose();

                    // Send signals to the methods registered to the disconnec events
                    if (!forced)
                        OnDisconnected();
                    else
                        OnForcedDisconnect();

                    for (int i = 0; i < Players.Count; ++i)
                        OnPlayerDisconnected(Players[i]);
                }
            }
        }

        /// <summary>
        /// Request the ping from the server (pingReceived will be triggered if it receives it)
        /// </summary>
        public override void Ping()
        {
            protoClient.SendPing();
        }

        /// <summary>
        /// A ping was receieved from the server so we need to respond with the pong
        /// </summary>
        /// <param name="playerRequesting">The server</param>
        /// <param name="time">The time that the ping was received for</param>
        protected override void Pong(NetworkingPlayer playerRequesting, DateTime time)
        {
            protoClient.SendPong(time);
        }
    }
}