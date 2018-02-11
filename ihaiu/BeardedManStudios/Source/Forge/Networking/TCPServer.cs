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
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Linq;

#if WINDOWS_UWP
using Windows.Networking.Sockets;
using System.IO;
#else
using System.Net.Sockets;
#endif

namespace BeardedManStudios.Forge.Networking
{
    /// <summary>
    /// This is the main TCP server object responsible for listening for incomming connections
    /// and reading any data sent from clients who are currently connected
    /// 这是负责监听传入连接的主要TCP服务器对象
    /// 并读取当前连接的客户端发送的任何数据
    /// </summary>
    public class TCPServer : BaseTCP, IServer
	{
		private CommonServerLogic commonServerLogic;

        #region Delegates
        /// <summary>
        /// 处理任何原始TcpClient事件的委托
        /// A delegate for handling any raw TcpClient events
        /// </summary>
#if WINDOWS_UWP
		public delegate void RawTcpClientEvent(StreamSocket client);
#else
        public delegate void RawTcpClientEvent(TcpClient client);
#endif
        #endregion

        #region Events
        /// <summary>
        /// 当一个原始的TcpClient成功连接时发生
        /// Occurs when a raw TcpClient has successfully connected
        /// </summary>
        public event RawTcpClientEvent rawClientConnected;

        /// <summary>
        /// 在原始TcpClient断开连接时发生
        /// Occurs when raw TcpClient has successfully connected
        /// </summary>
        public event RawTcpClientEvent rawClientDisconnected;

        /// <summary>
        /// 当原始的TcpClient被强制关闭时发生
        /// Occurs when raw TcpClient has been forcibly closed
        /// </summary>
        public event RawTcpClientEvent rawClientForceClosed;
        #endregion

        /// <summary>
        /// 客户端连接的主要监听器
        /// The main listener for client connections
        /// </summary>
#if WINDOWS_UWP
		private StreamSocketListener listener = null;
#else
        private TcpListener listener = null;
#endif

        /// <summary>
        /// 绑定到的IP地址
        /// The ip address that is being bound to
        /// </summary>
        private IPAddress ipAddress = null;

        /// <summary>
        /// 将持续监听新的客户端连接的主线程
        /// The main thread that will continuiously listen for new client connections
        /// </summary>
        //private Thread connectionThread = null;

        /// <summary>
        /// 连接的所有客户端的原始列表
        /// The raw list of all of the clients that are connected
        /// </summary>
#if WINDOWS_UWP
		private List<StreamSocket> rawClients = new List<StreamSocket>();
#else
        private List<TcpClient> rawClients = new List<TcpClient>();
#endif

		protected List<FrameStream> bufferedMessages = new List<FrameStream>();

        /// <summary>
        /// 用于确定此服务器当前是否正在接受连接
        /// Used to determine if this server is currently accepting connections
        /// </summary>
        public bool AcceptingConnections { get; private set; }

		public List<string> BannedAddresses { get; set; }


		public TCPServer(int maxConnections) : base(maxConnections)
		{
			AcceptingConnections = true;
			BannedAddresses = new List<string>();
			commonServerLogic = new CommonServerLogic(this);
		}

#if WINDOWS_UWP
		private void RawWrite(StreamSocket client, byte[] data)
#else
		private void RawWrite(TcpClient client, byte[] data)
#endif
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
#if WINDOWS_UWP
		public bool Send(StreamSocket client, FrameStream frame)
#else
        public bool Send(TcpClient client, FrameStream frame)
#endif
		{
			if (client == null)
				return false;

            //确保我们没有写入同一个客户端的竞争条件
            // Make sure that we don't have any race conditions with writing to the same client
            lock (client)
			{
				try
				{
                    //从帧中获取原始字节并发送
                    // Get the raw bytes from the frame and send them
                    byte[] data = frame.GetData();

					RawWrite(client, data);
					return true;
				}
				catch
				{
                    //客户端不再连接或无响应
                    // The client is no longer connected or is unresponsive
                }
            }

			return false;
		}

        /// <summary>
        /// Sends binary message to the specific tcp client(s)
        /// </summary>
        /// <param name="client">The client to receive the message</param>
        /// <param name="receivers">Receiver's type</param>
        /// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
        /// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
        /// <summary>
        ///发送二进制消息到特定的tcp客户端
        /// </ summary>
        /// <param name =“client”>接收消息的客户端</ param>
        /// <param name =“receivers”>接收者类型</ param>
        /// <param name =“messageGroupId”>按摩的Binary.GroupId，使用MessageGroupIds.START_OF_GENERIC_IDS + desired_id </ param>
        /// <param name =“objectsToSend”>要发送的变量数组，使用Binary.StreamData.GetBasicType <typeOfObject>（）</ param>
        public bool Send(TcpClient client, Receivers receivers = Receivers.Target, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, Receivers.Target, messageGroupId, false);
#if WINDOWS_UWP
			public bool Send(StreamSocket client, FrameStream frame)
#else
			return Send(client, sendFrame);
#endif
		}

        /// <summary>
        /// Send a frame to a specific player
        /// </summary>
        /// <param name="frame">The frame to send to that specific player</param>
        /// <param name="targetPlayer">The specific player to receive the frame</param>
        /// <summary>
        ///将帧发送给特定的播放器
        /// </ summary>
        /// <param name =“frame”>发送给特定播放器的帧</ param>
        /// <param name =“targetPlayer”>接收帧的特定播放器</ param>
        public void SendToPlayer(FrameStream frame, NetworkingPlayer targetPlayer)
		{
			if (frame.Receivers == Receivers.AllBuffered || frame.Receivers == Receivers.OthersBuffered)
				bufferedMessages.Add(frame);

			lock (Players)
			{
				if (Players.Contains(targetPlayer))
				{
					NetworkingPlayer player = Players[Players.IndexOf(targetPlayer)];
					if (!player.Accepted && !player.PendingAccepted)
						return;

					if (player == frame.Sender)
					{
                        //如果发送给其他人，则不要发送消息给发送方
                        // Don't send a message to the sending player if it was meant for others
                        if (frame.Receivers == Receivers.Others || frame.Receivers == Receivers.OthersBuffered || frame.Receivers == Receivers.OthersProximity)
							return;
					}

                    //检查请求是否基于接近度
                    // Check to see if the request is based on proximity
                    if (frame.Receivers == Receivers.AllProximity || frame.Receivers == Receivers.OthersProximity)
					{
                        //如果目标玩家与发件人不在同一个邻近区域内
                        //那么它不应该发送给该玩家
                        // If the target player is not in the same proximity zone as the sender
                        // then it should not be sent to that player
                        if (player.ProximityLocation.Distance(frame.Sender.ProximityLocation) > ProximityDistance)
						{
							return;
						}
					}

					try
					{
						Send(player.TcpClientHandle, frame);
					}
					catch
					{
						Disconnect(player, true);
					}
				}
			}
		}

        /// <summary>
        /// Sends binary message to the specific client
        /// </summary>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
        /// <param name="targetPlayer">The client to receive the message</param>
        /// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
        /// <summary>
        ///发送二进制消息到特定的客户端
        /// </ summary>
        /// <param name =“receivers”>接收消息的客户端/服务器</ param>
        /// <param name =“messageGroupId”>按摩的Binary.GroupId，使用MessageGroupIds.START_OF_GENERIC_IDS + desired_id </ param>
        /// <param name =“targetPlayer”>客户端接收消息</ param>
        /// <param name =“objectsToSend”>要发送的变量数组，使用Binary.StreamData.GetBasicType <typeOfObject>（）</ param>
        public void SendToPlayer(NetworkingPlayer targetPlayer, Receivers receivers = Receivers.Target, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, receivers, messageGroupId, false);
			SendToPlayer(sendFrame, targetPlayer);
		}

        /// <summary>
        /// Goes through all of the currently connected players and send them the frame
        /// </summary>
        /// <param name="frame">The frame to send to all of the connected players</param>
        /// <summary>
        ///通过所有当前连接的玩家，并将其发送给他们
        /// </ summary>
        /// <param name =“frame”>发送给所有连接的播放器的帧</ param>
        public void SendAll(FrameStream frame, NetworkingPlayer skipPlayer = null)
		{
			if (frame.Receivers == Receivers.AllBuffered || frame.Receivers == Receivers.OthersBuffered)
				bufferedMessages.Add(frame);

			lock (Players)
			{
				foreach (NetworkingPlayer player in Players)
				{
					if (!commonServerLogic.PlayerIsReceiver(player, frame, ProximityDistance, skipPlayer))
						continue;

					try
					{
						Send(player.TcpClientHandle, frame);
					}
					catch
					{
						Disconnect(player, true);
					}
				}
			}
		}

		/// <summary>
		/// Goes through all of the currently connected players and send them the frame
		/// </summary>
		/// <param name="receivers">The clients / server to receive the message</param>
		/// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
		/// <param name="playerToIgnore">The client to ignore</param>
		/// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
		public void SendAll(Receivers receivers = Receivers.All, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, NetworkingPlayer playerToIgnore = null, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, receivers, messageGroupId, true);
			SendAll(sendFrame, playerToIgnore);
		}

        /// <summary>
        /// 调用基本断开挂起的方法来删除所有挂起的断开连接的客户端
        /// Call the base disconnect pending method to remove all pending disconnecting clients
        /// </summary>
        private void CleanupDisconnections() { DisconnectPending(RemovePlayer); }

        /// <summary>
        /// 提交断开连接
        /// Commits the disconnects
        /// </summary>
        public void CommitDisconnects() { CleanupDisconnections(); }

		/// <summary>
		/// This will begin the connection for TCP, this is a thread blocking operation until the connection
		/// is either established or has failed
		/// </summary>
		/// <param name="hostAddress">[127.0.0.1] Ip Address to host from</param>
		/// <param name="port">[15937] Port to allow connections from</param>
		public void Connect(string hostAddress = "0.0.0.0", ushort port = DEFAULT_PORT)
		{
			if (Disposed)
				throw new ObjectDisposedException("TCPServer", "This object has been disposed and can not be used to connect, please use a new TCPServer");

			if (string.IsNullOrEmpty(hostAddress))
				throw new BaseNetworkException("An ip address must be specified to bind to. If you are unsure, you can set to 127.0.0.1");

			// Check to see if this server is being bound to a "loopback" address, if so then bind to any, otherwise bind to specified address
			if (hostAddress == "0.0.0.0" || hostAddress == "localhost")
				ipAddress = IPAddress.Any;
			else
				ipAddress = IPAddress.Parse(hostAddress);

			try
			{
				// Setup and start the base C# TcpListner
				listener = new TcpListener(ipAddress, port);
				//listener.Start();

				Me = new NetworkingPlayer(ServerPlayerCounter++, "0.0.0.0", true, listener, this);
				Me.InstanceGuid = InstanceGuid.ToString();

				// Create the thread that will be listening for clients and start its execution
				//Thread connectionThread = new Thread(new ThreadStart(ListenForConnections));
				//connectionThread.Start();
				//Task.Queue(ListenForConnections);
				listener.Start();
				listener.BeginAcceptTcpClient(ListenForConnections, listener);

                //在成功绑定的结果中执行任何通用初始化
                // Do any generic initialization in result of the successful bind
                OnBindSuccessful();

                //创建将监听来自连接客户端的新数据并开始执行的线程
                // Create the thread that will be listening for new data from connected clients and start its execution
                Task.Queue(ReadClients);

                // 创建将检查播放器超时的线程
                // Create the thread that will check for player timeouts
                Task.Queue(() =>
				{
					commonServerLogic.CheckClientTimeout((player) =>
					{
						Disconnect(player, true);
						OnPlayerTimeout(player);
						CleanupDisconnections();
					});
				});

                //让我知道我连接成功
                //Let myself know I connected successfully
                OnPlayerConnected(Me);
                //将自己设置为连接的客户端
                // Set myself as a connected client
                Me.Connected = true;

                //设置端口
                //Set the port
                SetPort((ushort)((IPEndPoint)listener.LocalEndpoint).Port);
			}
			catch (Exception e)
			{
				Logging.BMSLog.LogException(e);
				// Do any generic initialization in result of the binding failure
				OnBindFailure();

				throw new FailedBindingException("Failed to bind to host/port, see inner exception", e);
			}
		}

        /// <summary>
        /// 无限循环监听单独线程上的客户端连接。
        /// 如果在阻塞接受调用上抛出异常，则此循环会中断
        /// Infinite loop listening for client connections on a separate thread.
        /// This loop breaks if there is an exception thrown on the blocking accept call
        /// </summary>
        private void ListenForConnections(IAsyncResult obj)
		{
			TcpListener asyncListener = (TcpListener)obj.AsyncState;
			TcpClient client = null;

			try
			{
				client = asyncListener.EndAcceptTcpClient(obj);
			}
			catch
			{
				return;
			}

			asyncListener.BeginAcceptTcpClient(ListenForConnections, asyncListener);

			if (rawClients.Count == MaxConnections)
            {
                //告诉客户端为什么断开连接
                // Tell the client why they are being disconnected
                Send(client, Error.CreateErrorMessage(Time.Timestep, "服务器上达到的最大玩家数 Max Players Reached On Server", false, MessageGroupIds.MAX_CONNECTIONS, true));

                //将关闭连接帧发送到客户端
                // Send the close connection frame to the client
                Send(client, new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, true));

                //执行客户端断开逻辑
                // Do disconnect logic for client
                ClientRejected(client, false);
				return;
			}
			else if (!AcceptingConnections)
			{
                //告诉客户端为什么断开连接
                // Tell the client why they are being disconnected
                Send(client, Error.CreateErrorMessage(Time.Timestep, "服务器正忙，不接受连接 The server is busy and not accepting connections", false, MessageGroupIds.MAX_CONNECTIONS, true));

                //将关闭连接帧发送到客户端
                // Send the close connection frame to the client
                Send(client, new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, true));
                
                //执行客户端断开逻辑
                // Do disconnect logic for client
                ClientRejected(client, false);
				return;
			}

            //客户端将在其他线程上循环，因此在添加之前一定要锁定它
            // Clients will be looped through on other threads so be sure to lock it before adding
            lock (Players)
			{
				rawClients.Add(client);

                //为这个玩家创建身份包装
                // Create the identity wrapper for this player
                NetworkingPlayer player = new NetworkingPlayer(ServerPlayerCounter++, client.Client.RemoteEndPoint.ToString(), false, client, this);

                //一般添加玩家，并关闭玩家加入的事件
                // Generically add the player and fire off the events attached to player joining
                OnPlayerConnected(player);
			}

            //让所有事件侦听器知道客户端已经成功连接
            // Let all of the event listeners know that the client has successfully connected
            if (rawClientConnected != null)
				rawClientConnected(client);
		}
        /// <summary>
        /// 无限循环在单独的线程上监听来自所有连接客户端的新数据。
        /// readThreadCancel设置为true时，此循环会中断
        /// 
        /// Infinite loop listening for new data from all connected clients on a separate thread.
        /// This loop breaks when readThreadCancel is set to true
        /// </summary>
        private void ReadClients()
		{
            //故意无限循环
            // Intentional infinite loop
            while (IsBound && !NetWorker.EndingSession)
			{
				try
				{
                    //如果读取已被标记为取消，则从此循环中断开
                    // If the read has been flagged to be canceled then break from this loop
                    if (readThreadCancel)
						return;

                    //这将遍历所有玩家，因此请确保将锁设置为
                    //防止来自其他线程的任何更改
                    // This will loop through all of the players, so make sure to set the lock to
                    // prevent any changes from other threads
                    lock (Players)
					{
						for (int i = 0; i < Players.Count; i++)
                        {
                            //如果读取已被标记为取消，则从此循环中断开
                            // If the read has been flagged to be canceled then break from this loop
                            if (readThreadCancel)
								return;

							NetworkStream playerStream = null;

							if (Players[i].IsHost)
								continue;

							try
							{
								lock (Players[i].MutexLock)
								{
                                    //尝试获取客户端流，如果它仍然可用
                                    // Try to get the client stream if it is still available
                                    playerStream = Players[i].TcpClientHandle.GetStream();
								}
							}
							catch
							{
                                //无法获取客户端的流，因此强制断开连接
                                //Console.WriteLine("Exception异常：无法为客户端获取流（强制断开连接）“）;

                                // Failed to get the stream for the client so forcefully disconnect it
                                //Console.WriteLine("Exception: Failed to get stream for client (Forcefully disconnecting)");
                                Disconnect(Players[i], true);
								continue;
							}

                            //如果播放器不再连接，请确保正确断开连接
                            // If the player is no longer connected, then make sure to disconnect it properly
                            if (!Players[i].TcpClientHandle.Connected)
							{
								Disconnect(Players[i], false);
								continue;
							}

                            //如果有任何数据可用，则只有继续阅读此客户端
                            // Only continue to read for this client if there is any data available for it
                            if (!playerStream.DataAvailable)
								continue;

							int available = Players[i].TcpClientHandle.Available;

                            //确定玩家是否完全连接
                            // Determine if the player is fully connected
                            if (!Players[i].Accepted)
							{
                                //确定玩家是否已被服务器接受
                                // Determine if the player has been accepted by the server
                                if (!Players[i].Connected)
								{
									lock (Players[i].MutexLock)
									{
                                        //由于客户端尚未被接受，所以从流中读取所有内容
                                        // Read everything from the stream as the client hasn't been accepted yet
                                        byte[] bytes = new byte[available];
										playerStream.Read(bytes, 0, bytes.Length);

                                        //验证连接头是否格式正确
                                        // Validate that the connection headers are properly formatted
                                        byte[] response = Websockets.ValidateConnectionHeader(bytes);

                                        //如果发送的头是无效的，那么响应将是空的，如果是这样，那么断开客户端，因为它们发送无效头
                                        // The response will be null if the header sent is invalid, if so then disconnect client as they are sending invalid headers
                                        if (response == null)
										{
											OnPlayerRejected(Players[i]);
											Disconnect(Players[i], false);
											continue;
										}

                                        //如果一切顺利，则将验证的响应发送给客户端
                                        // If all is in order then send the validated response to the client
                                        playerStream.Write(response, 0, response.Length);

                                        //播放器已成功连接
                                        // The player has successfully connected
                                        Players[i].Connected = true;
									}
								}
								else
								{
									lock (Players[i].MutexLock)
									{
                                        //即使消息没有被使用，也会消耗该消息，以便将消息从缓冲区中删除
                                        // Consume the message even though it is not being used so that it is removed from the buffer
                                        Text frame = (Text)Factory.DecodeMessage(GetNextBytes(playerStream, available, true), true, MessageGroupIds.TCP_FIND_GROUP_ID, Players[i]);
										Players[i].InstanceGuid = frame.ToString();

										OnPlayerGuidAssigned(Players[i]);

										lock (writeBuffer)
										{
											writeBuffer.Clear();
											writeBuffer.Append(BitConverter.GetBytes(Players[i].NetworkId));
											Send(Players[i].TcpClientHandle, new Binary(Time.Timestep, false, writeBuffer, Receivers.Target, MessageGroupIds.NETWORK_ID_REQUEST, true));

											SendBuffer(Players[i]);

                                            //所有系统都去了，玩家已经被接受了
                                            // All systems go, the player has been accepted
                                            OnPlayerAccepted(Players[i]);
										}
									}
								}
							}
							else
							{
								try
								{
									lock (Players[i].MutexLock)
									{
										Players[i].Ping();

                                        // Get the frame that was sent by the client, the client
                                        // does send masked data
                                        //TODO: THIS IS CAUSING ISSUES!!! WHY!?!?!!?
                                        //获取客户端发送的帧
                                        //发送屏蔽的数据
                                        // TODO：这是造成的问题！ 为什么！？！？！！？
                                        FrameStream frame = Factory.DecodeMessage(GetNextBytes(playerStream, available, true), true, MessageGroupIds.TCP_FIND_GROUP_ID, Players[i]);

                                        //客户端已经告诉服务器它正在断开连接
                                        // The client has told the server that it is disconnecting
                                        if (frame is ConnectionClose)
										{
                                            //确认连接关闭
                                            // Confirm the connection close
                                            Send(Players[i].TcpClientHandle, new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, true));

											Disconnect(Players[i], false);
											continue;
										}

										FireRead(frame, Players[i]);
									}
								}
								catch
								{
                                    //播放器发送无效数据，请断开连接
                                    // The player is sending invalid data so disconnect them
                                    Disconnect(Players[i], true);
								}
							}
						}
					}

                    //检查所有挂起的断开连接并清理它们
                    //完成并确定断开连接
                    // Go through all of the pending disconnections and clean them
                    // up and finalize the disconnection
                    CleanupDisconnections();

                    //睡眠，这样我们就可以从这个线程释放一些CPU
                    // Sleep so that we free up the CPU a bit from this thread
                    Thread.Sleep(10);
				}
				catch (Exception ex)
				{
					Logging.BMSLog.LogException(ex);
				}
			}
		}

        /// <summary>
        /// Disconnects this server and all of it's clients
        /// </summary>
        /// <param name="forced">Used to tell if this disconnect was intentional <c>false</c> or caused by an exception <c>true</c></param>
        /// <summary>
        ///断开这个服务器及其所有的客户端
        /// </ summary>
        /// <param name =“forced”>用来判断这个断开连接是故意的<c> false </ c>还是由异常引起<c> true </ c> </ param>
        public override void Disconnect(bool forced)
		{
			// Since we are disconnecting we need to stop the read thread
			readThreadCancel = true;

			lock (Players)
			{
				// Stop listening for new connections
				listener.Stop();

				// Go through all of the players and disconnect them
				foreach (NetworkingPlayer player in Players)
					Disconnect(player, forced);

				// Send signals to the methods registered to the disconnec events
				if (!forced)
					OnDisconnected();
				else
					OnForcedDisconnect();
			}
		}

        /// <summary>
        /// Disconnects a client from this listener
        /// </summary>
        /// <param name="client">The target client to be disconnected</param>
        /// <summary>
        ///从这个监听器断开一个客户端
        /// </ summary>
        /// <param name =“client”>要断开连接的目标客户端</ param>
        public void Disconnect(NetworkingPlayer player, bool forced)
		{
			commonServerLogic.Disconnect(player, forced, DisconnectingPlayers, ForcedDisconnectingPlayers);
		}

		/// <summary>
		/// Fully remove the player from the network
		/// </summary>
		/// <param name="player">The target player</param>
		/// <param name="forced">If the player is being forcibly removed from an exception</param>
		private void RemovePlayer(NetworkingPlayer player, bool forced)
		{
			lock (Players)
			{
				if (player.IsDisconnecting)
					return;

				player.IsDisconnecting = true;
			}

			// Tell the player that he is getting disconnected
			Send(player.TcpClientHandle, new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, true));
			
			if (!forced)
			{
				Task.Queue(() =>
				{
					FinalizeRemovePlayer(player, forced);
				}, 1000);
			}
			else
				FinalizeRemovePlayer(player, forced);
		}

		private void FinalizeRemovePlayer(NetworkingPlayer player, bool forced)
		{
			OnPlayerDisconnected(player);
			player.TcpClientHandle.Close();
			rawClients.Remove(player.TcpClientHandle);

			if (!forced)
			{
                //让所有事件侦听器知道客户端已经成功断开连接
                // Let all of the event listeners know that the client has successfully disconnected
                if (rawClientDisconnected != null)
					rawClientDisconnected(player.TcpClientHandle);
				DisconnectingPlayers.Remove(player);
			}
			else
			{
                //让所有的事件监听器知道这是一个强制断开连接
                // Let all of the event listeners know that this was a forced disconnect
                if (forced && rawClientForceClosed != null)
					rawClientForceClosed(player.TcpClientHandle);
				ForcedDisconnectingPlayers.Remove(player);
			}
		}

#if WINDOWS_UWP
		private void ClientRejected(StreamSocket client, bool forced)
#else
		private void ClientRejected(TcpClient client, bool forced)
#endif
		{
            //清理客户端对象
            // Clean up the client objects
            client.Close();
		}

		private void SendBuffer(NetworkingPlayer player)
		{
			foreach (FrameStream frame in bufferedMessages)
				Send(player.TcpClientHandle, frame);
		}

		public override void Ping()
		{
			// I am the server, so 0 ms...
			OnPingRecieved(0, Me);
		}

		/// <summary>
		/// Pong back to the client
		/// </summary>
		/// <param name="playerRequesting"></param>
		protected override void Pong(NetworkingPlayer playerRequesting, DateTime time)
		{
			SendToPlayer(GeneratePong(time), playerRequesting);
		}


        /// <summary>
        /// 设置服务器 不再接受玩家连接
        /// </summary>
        public void StopAcceptingConnections()
		{
			AcceptingConnections = false;
		}


        /// <summary>
        /// 设置服务器 开始接受玩家连接
        /// </summary>
        public void StartAcceptingConnections()
		{
			AcceptingConnections = true;
		}

        /// <summary>
        /// 禁止玩家
        /// </summary>
        public void BanPlayer(ulong networkId, int minutes)
		{
			NetworkingPlayer player = Players.FirstOrDefault(p => p.NetworkId == networkId);

			if (player == null)
				return;

			BannedAddresses.Add(player.Ip);
			Disconnect(player, true);
			CommitDisconnects();
		}

		public override void FireRead(FrameStream frame, NetworkingPlayer currentPlayer)
		{
            //已成功从网络读取消息，以便中继
            //注册到事件的所有方法
            // A message has been successfully read from the network so relay that
            // to all methods registered to the event
            OnMessageReceived(currentPlayer, frame);
		}
	}
}