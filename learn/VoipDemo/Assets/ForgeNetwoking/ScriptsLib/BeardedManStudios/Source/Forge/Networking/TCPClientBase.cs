/*-----------------------------+-------------------------------\
|                                                              |
|                         !!!NOTICE!!!                         |
|                                                              |
|  These libraries are under heavy development so they are     |
|  subject to make many changes as development continues.      |
|  For this reason, the libraries may not be well commented.   |
|  THANK YOU for supporting forge with all your feedback       |
|  suggestions, bug reports and comments!                      |
|                                                              |
|                              - The Forge Team                |
|                                Bearded Man Studios, Inc.     |
|                                                              |
|  This source code, project files, and associated files are   |
|  copyrighted by Bearded Man Studios, Inc. (2012-2017) and    |
|  may not be redistributed without written permission.        |
|                                                              |
\------------------------------+------------------------------*/

using BeardedManStudios.Forge.Networking.Frame;
using System;

#if WINDOWS_UWP
using Windows.Networking.Sockets;
using Windows.Networking;
using System.IO;
#else
using System.Net.Sockets;
#endif

namespace BeardedManStudios.Forge.Networking
{
	public abstract class TCPClientBase : BaseTCP, IClient
	{
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
        /// 被服务器验证的散列
        /// The hash that is / was validated by the server
        /// </summary>
        private string headerHash = string.Empty;

        /// <summary>
        /// 用于确定客户端是否请求被服务器接受
        /// Used to determine if the client has requested to be accepted by the server
        /// </summary>
        private bool headerExchanged = false;

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
        public virtual void Send(FrameStream frame)
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

		/// <summary>
		/// Sends binary message to the specific receiver(s)
		/// </summary>
		/// <param name="receivers">The clients / server to receive the message</param>
		/// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
		/// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
		public virtual void Send(Receivers receivers, int messageGroupId, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, true, data, receivers, messageGroupId, true);
			Send(sendFrame);
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
            //默认情况下待处理的创建应该是真实的，并在准备就绪时刷新
            // By default pending creates should be true and flushed when ready
            PendCreates = true;

            // Get a client stream for reading and writing. 
            // Stream stream = client.GetStream();

            //获取需要用于验证服务器连接的随机哈希键
            // Get a random hash key that needs to be used for validating that the server was connected to
            headerHash = Websockets.HeaderHashKey();

            //这是一个典型的Websockets接受头被验证
            // This is a typical Websockets accept header to be validated
            byte[] connectHeader = Websockets.ConnectionHeader(headerHash, port);

            //将接受标头发送到服务器进行验证
            // Send the accept headers to the server to validate
            RawWrite(connectHeader);

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

		protected void InitializeMasterClient(string host, ushort port)
		{
            // Get a client stream for reading and writing. 
            // Stream stream = client.GetStream();

            //获取需要用于验证服务器连接的随机哈希键
            // Get a random hash key that needs to be used for validating that the server was connected to
            headerHash = Websockets.HeaderHashKey();

            //这是一个典型的Websockets接受头被验证
            // This is a typical Websockets accept header to be validated
            byte[] connectHeader = Websockets.ConnectionHeader(headerHash, port);

            //将接受标头发送到服务器进行验证
            // Send the accept headers to the server to validate
            RawWrite(connectHeader);

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

            //确定这个客户端是否已被服务器接受
            // Determine if this client has been accepted by the server yet
            if (!headerExchanged)
			{
                //读取流中所有可用的字节，因为这个客户端还没有连接
                // Read all available bytes in the stream as this client hasn't connected yet
                byte[] bytes = new byte[available];
				stream.Read(bytes, 0, bytes.Length);

                //来自服务器的第一个数据包响应将是一个字符串
                // The first packet response from the server is going to be a string
                if (Websockets.ValidateResponseHeader(headerHash, bytes))
				{
					headerExchanged = true;

                    //通过Ping服务器来确定玩家的连接
                    // Ping the server to finalize the player's connection
                    Send(Text.CreateFromString(Time.Timestep, InstanceGuid.ToString(), true, Receivers.Server, MessageGroupIds.NETWORK_ID_REQUEST, true));
				}
				else
				{
					// Improper header, so a disconnect is required
					Disconnect(true);
					return ReadState.Disconnect;
				}
			}
			else
			{
				byte[] messageBytes = GetNextBytes(stream, available, false);

                //获取由服务器，服务器发送的帧
                //不发送被屏蔽的数据，只有客户端发送false为掩码
                // Get the frame that was sent by the server, the server
                // does not send masked data, only the client so send false for mask
                FrameStream frame = Factory.DecodeMessage(messageBytes, false, MessageGroupIds.TCP_FIND_GROUP_ID, Server);

				FireRead(frame, Server);
			}

			return ReadState.Void;
		}

		public override void FireRead(FrameStream frame, NetworkingPlayer currentPlayer)
		{
            //已成功从网络读取消息，以便中继
            //注册到事件的所有方法
            // A message has been successfully read from the network so relay that
            // to all methods registered to the event
            OnMessageReceived(currentPlayer, frame);
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
						Send(new ConnectionClose(Time.Timestep, true, Receivers.Server, MessageGroupIds.DISCONNECT, true));

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
			Send(GeneratePing());
		}

		/// <summary>
		/// A ping was receieved from the server so we need to respond with the pong
		/// </summary>
		/// <param name="playerRequesting">The server</param>
		/// <param name="time">The time that the ping was received for</param>
		protected override void Pong(NetworkingPlayer playerRequesting, DateTime time)
		{
			Send(GeneratePong(time));
		}
	}
}