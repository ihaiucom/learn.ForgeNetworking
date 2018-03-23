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
using BeardedManStudios.Forge.Networking.Nat;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace BeardedManStudios.Forge.Networking
{
	public class UDPServer : BaseUDP, IServer
	{
		private CommonServerLogic commonServerLogic;

        // 玩家字典
		public Dictionary<string, UDPNetworkingPlayer> udpPlayers = new Dictionary<string, UDPNetworkingPlayer>();

        // 当前处理的客户端, 读取改客户端数据
		private UDPNetworkingPlayer currentReadingPlayer = null;

		public UDPServer(int maxConnections) : base(maxConnections)
		{
			AcceptingConnections = true;
			BannedAddresses = new List<string>();
			commonServerLogic = new CommonServerLogic(this);
		}

		public NatHolePunch nat = new NatHolePunch();

		protected List<FrameStream> bufferedMessages = new List<FrameStream>();

        /// <summary>
        /// 被禁玩家IP的列表
        /// </summary>
		public List<string> BannedAddresses { get; set; }

        /// <summary>
        /// 用于确定此服务器当前是否正在接受连接
        /// Used to determine if this server is currently accepting connections
        /// </summary>
        public bool AcceptingConnections { get; private set; }

		public void Send(NetworkingPlayer player, FrameStream frame, bool reliable = false)
		{
			UDPPacketComposer composer = new UDPPacketComposer(this, player, frame, reliable);

            //如果这个信息是可靠的，那么一定要保持对作曲家的引用
            //这样就没有任何失控的线程
            // If this message is reliable then make sure to keep a reference to the composer
            // so that there are not any run-away threads
            if (reliable)
			{
				lock (pendingComposers)
				{
                    //使用完成的事件从内存中清理对象
                    // Use the completed event to clean up the object from memory
                    composer.completed += ComposerCompleted;
					pendingComposers.Add(composer);
				}
			}
		}

		public override void Send(FrameStream frame, bool reliable = false)
		{
			Send(frame, reliable, null);
		}

		public void Send(FrameStream frame, bool reliable = false, NetworkingPlayer skipPlayer = null)
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
						Send(player, frame, reliable);
					}
					catch
					{
						Disconnect(player, true);
					}
				}
			}
		}

        /// <summary>
        /// Sends binary message to the specified receiver(s)
        /// </summary>
        /// <param name="receivers">The client to receive the message</param>
        /// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
        /// <param name="reliable">True if message must be delivered</param>
        /// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
        /// <summary>
        ///发送二进制消息到指定的接收者
        /// </ summary>
        /// <param name =“receivers”>接收消息的客户端</ param>
        /// <param name =“messageGroupId”>按摩的Binary.GroupId，使用MessageGroupIds.START_OF_GENERIC_IDS + desired_id </ param>
        /// <param name =“reliable”>如果消息必须被传递，则为真</ param>
        /// <param name =“objectsToSend”>要发送的变量数组，使用Binary.StreamData.GetBasicType <typeOfObject>（）</ param>
        public virtual void Send(NetworkingPlayer player, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, bool reliable = false, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, Receivers.Target, messageGroupId, false);
			Send(player, sendFrame, reliable);
		}

        /// <summary>
        /// Sends binary message to all clients ignoring the specific one
        /// </summary>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="playerToIgnore"></param>
        /// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
        /// <param name="reliable">True if message must be delivered</param>
        /// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
        /// <summary>
        ///向所有客户发送二进制消息，忽略特定的消息
        /// </ summary>
        /// <param name =“receivers”>接收消息的客户机/服务器</ param>
        /// <param name =“playerToIgnore”> </ param>
        /// <param name =“messageGroupId”>按摩的Binary.GroupId，使用MessageGroupIds.START_OF_GENERIC_IDS + desired_id </ param>
        /// <param name =“reliable”>如果消息必须被传递，则为真</ param>
        /// <param name =“objectsToSend”>要发送的变量数组，使用Binary.StreamData.GetBasicType <typeOfObject>（）</ param>
        public virtual void Send(Receivers receivers = Receivers.Target, NetworkingPlayer playerToIgnore = null, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, bool reliable = false, params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, receivers, messageGroupId, false);
			Send(sendFrame, reliable, playerToIgnore);
		}

		public void Connect(string host = "0.0.0.0", ushort port = DEFAULT_PORT, string natHost = "", ushort natPort = NatHolePunch.DEFAULT_NAT_SERVER_PORT)
		{
			if (Disposed)
				throw new ObjectDisposedException("UDPServer", "此对象已被处置且不能用于连接，请使用新的UDPServer。This object has been disposed and can not be used to connect, please use a new UDPServer");

			try
			{
				Client = new CachedUdpClient(port);
				Client.EnableBroadcast = true;
				Me = new NetworkingPlayer(ServerPlayerCounter++, host, true, ResolveHost(host, port), this);
				Me.InstanceGuid = InstanceGuid.ToString();

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

				//Set the port
				SetPort((ushort)((IPEndPoint)Client.Client.LocalEndPoint).Port);

				if (!string.IsNullOrEmpty(natHost))
				{
					nat.Register((ushort)Me.IPEndPointHandle.Port, natHost, natPort);
					nat.clientConnectAttempt += NatClientConnectAttempt;
				}
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
        /// Disconnects this server and all of it's clients
        /// </summary>
        /// <param name="forced">Used to tell if this disconnect was intentional <c>false</c> or caused by an exception <c>true</c></param>
        /// <summary>
        ///断开这个服务器及其所有的客户端
        /// </ summary>
        /// <param name =“forced”>用来判断这个断开连接是故意的<c> false </ c>还是由异常引起<c> true </ c> </ param>
        public override void Disconnect(bool forced)
		{
			nat.Disconnect();

            //因为我们正在断开连接，所以我们需要停止读取线程
            // Since we are disconnecting we need to stop the read thread
            readThreadCancel = true;

			lock (Players)
			{
                //通过所有玩家并断开连接
                // Go through all of the players and disconnect them
                foreach (NetworkingPlayer player in Players)
				{
					if (player != Me)
						Disconnect(player, forced);
				}

				CleanupDisconnections();

				int counter = 0;
				for (; ; counter++)
				{
					if (counter >= 10 || Players.Count == 1)
						break;

					Thread.Sleep(100);
				}

                //发送信号到注册到断开事件的方法
                // Send signals to the methods registered to the disconnect events
                if (!forced)
					OnDisconnected();
				else
					OnForcedDisconnect();

                //停止监听新的连接
                // Stop listening for new connections
                Client.Close();
			}
		}

        /// <summary>
        /// Disconnects a client
        /// </summary>
        /// <param name="client">The target client to be disconnected</param>
        /// <summary>
        ///断开一个客户端
        /// </ summary>
        /// <param name =“client”>要断开连接的目标客户端</ param>
        public void Disconnect(NetworkingPlayer player, bool forced)
		{
			commonServerLogic.Disconnect(player, forced, DisconnectingPlayers, ForcedDisconnectingPlayers);
		}

        /// <summary>
        /// 调用基本断开挂起的方法来删除所有挂起的断开连接的客户端
        /// Call the base disconnect pending method to remove all pending disconnecting clients
        /// </summary>
        private void CleanupDisconnections() { DisconnectPending(RemovePlayer); }

        /// <summary>
        /// 提交断开连接
        /// Commit the disconnects
        /// </summary>
        public void CommitDisconnects() { CleanupDisconnections(); }

        /// <summary>
        /// Fully remove the player from the network
        /// </summary>
        /// <param name="player">The target player</param>
        /// <param name="forced">If the player is being forcibly removed from an exception</param>
        /// <summary>
        ///完全从网络中删除player
        /// </ summary>
        /// <param name =“player”>目标玩家</ param>
        /// <param name =“forced”>如果玩家被强行从异常中移除</ param>
        private void RemovePlayer(NetworkingPlayer player, bool forced)
		{
			lock (Players)
			{
				if (player.IsDisconnecting)
					return;

				player.IsDisconnecting = true;
			}

            //告诉玩家他们正在断开连接
            // Tell the player that they are getting disconnected
            Send(player, new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, false), !forced);

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

        // 删除玩家
        private void FinalizeRemovePlayer(NetworkingPlayer player, bool forced)
		{
			udpPlayers.Remove(player.Ip + "+" + player.Port);
			OnPlayerDisconnected(player);

			if (forced)
				ForcedDisconnectingPlayers.Remove(player);
			else
				DisconnectingPlayers.Remove(player);
		}

        /// <summary>
        /// Infinite loop listening for new data from all connected clients on a separate thread.
        /// This loop breaks when readThreadCancel is set to true
        /// </summary>
        /// <summary>
        ///无限循环在单独的线程上监听来自所有连接客户端的新数据。
        ///当readThreadCancel设置为true时，此循环中断
        /// </ summary>
        private void ReadClients()
		{
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);
			string incomingEndpoint = string.Empty;

			BMSByte packet = null;

            // 故意无限循环
            // Intentional infinite loop
            while (IsBound)
			{
                //如果读取已被标记为取消，则从此循环中断开
                // If the read has been flagged to be canceled then break from this loop
                if (readThreadCancel)
					return;

                incomingEndpoint = string.Empty;

                try
				{
                    //从网络读取数据包
                    // Read a packet from the network
                    packet = Client.Receive(ref groupEP, ref incomingEndpoint);

                    // 模拟丢包
					if (PacketLossSimulation > 0.0f && new Random().NextDouble() <= PacketLossSimulation)
					{
                        // 丢掉这个消息
						// Skip this message
						continue;
					}
                    // 统计 宽带接收数据大小
					BandwidthIn += (ulong)packet.Size;
				}
				catch(Exception e)
				{
                    // 如果出错， 就查找该IP PROT的玩家，将该玩家踢掉
					UDPNetworkingPlayer player;
					if (udpPlayers.TryGetValue(incomingEndpoint, out player))
					{
						FinalizeRemovePlayer(player, true);
                    }
                    //UnityEngine.Debug.LogError( player.NetworkId + " " + incomingEndpoint + e.ToString());

                    continue;
				}

                //检查以确保收到消息
                // Check to make sure a message was received
                if (packet == null || packet.Size <= 0)
					continue;

                //如果玩家列表里不包含该包的发送者
				if (!udpPlayers.ContainsKey(incomingEndpoint))
				{
                    // 创建该发送者的结构体保存 UDPNetworkingPlayer
                    SetupClient(packet, incomingEndpoint, groupEP);
					continue;
				}
				else
				{
					currentReadingPlayer = udpPlayers[incomingEndpoint];

					if (!currentReadingPlayer.Accepted && !currentReadingPlayer.PendingAccepted)
					{
                        //响应验证可能会被丢弃
                        //检查客户端是否正在重新发送响应
                        // It is possible that the response validation was dropped so
                        // check if the client is resending for a response
                        byte[] response = Websockets.ValidateConnectionHeader(packet.CompressBytes());

                        //客户端再次发送连接请求
                        // The client has sent the connection request again
                        if (response != null)
						{
							Client.Send(response, response.Length, groupEP);
							continue;
						}
						else
						{
                            // 将该玩家设置为等待接受确认
							currentReadingPlayer.PendingAccepted = true;
                            // 读取该玩家发来的数据包
							ReadPacket(packet);
						}
					}
					else
					{
                        //由于Forge网络协议，数据包唯一的时间1
                        //将是71，第二个数据包是69是强制断开连接
                        // Due to the Forge Networking protocol, the only time that packet 1
                        // will be 71 and the second packet be 69 is a forced disconnect reconnect
                        if (packet[0] == 71 && packet[1] == 69)
						{
                            //UnityEngine.Debug.LogError("将是71，第二个数据包是69是强制断开连接 NetworkId=" + currentReadingPlayer.NetworkId + "   " + currentReadingPlayer.Ip + "+" + currentReadingPlayer.Port);
							udpPlayers.Remove(currentReadingPlayer.Ip + "+" + currentReadingPlayer.Port);
							FinalizeRemovePlayer(currentReadingPlayer, true);
							continue;
						}

                        // 设置玩家最好ping时间
						currentReadingPlayer.Ping();
                        // 读取该玩家发来的数据包
                        ReadPacket(packet);
					}
				}
			}
		}

        /// <summary>
        /// 处理是否接受这个玩家的连接 
        /// 如果接受将会给一个反馈 并且 创建UDPNetworkingPlayer存到对应的列表里
        /// 如果是不再接受连接，反馈原因
        /// 否则啥也不做
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="incomingEndpoint"></param>
        /// <param name="groupEP"></param>
		private void SetupClient(BMSByte packet, string incomingEndpoint, IPEndPoint groupEP)
		{
            //检查本地列表请求
            // Check for a local listing request
            if (packet.Size.Between(2, 4) && packet[0] == BROADCAST_LISTING_REQUEST_1 && packet[1] == BROADCAST_LISTING_REQUEST_2 && packet[2] == BROADCAST_LISTING_REQUEST_3)
			{
                //如果服务器当前不接受连接，则不要回复
                // Don't reply if the server is not currently accepting connections
                if (!AcceptingConnections)
					return;

                //这可能是一个本地列表请求，所以用服务器标志字节进行响应
                // This may be a local listing request so respond with the server flag byte
                Client.Send(new byte[] { SERVER_BROADCAST_CODE }, 1, groupEP);
				return;
			}

			if (Players.Count == MaxConnections)
			{
                //告诉客户端为什么断开连接
                // Tell the client why they are being disconnected
                Send(Error.CreateErrorMessage(Time.Timestep, "服务器上达到的最大玩家数 Max Players Reached On Server", false, MessageGroupIds.MAX_CONNECTIONS, true));

                //将关闭连接帧发送到客户端
                // Send the close connection frame to the client
                Send(new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, false));

				return;
			}
			else if (!AcceptingConnections)
			{
                //告诉客户端为什么断开连接
                // Tell the client why they are being disconnected
                Send(Error.CreateErrorMessage(Time.Timestep, "服务器正忙，不接受连接 The server is busy and not accepting connections", false, MessageGroupIds.MAX_CONNECTIONS, true));

                //将关闭连接帧发送到客户端
                // Send the close connection frame to the client
                Send(new ConnectionClose(Time.Timestep, false, Receivers.Target, MessageGroupIds.DISCONNECT, false));

				return;
			}

            //验证连接头是否格式正确
            // Validate that the connection headers are properly formatted
            byte[] response = Websockets.ValidateConnectionHeader(packet.CompressBytes());

            //如果发送的头是无效的，那么响应将是空的，如果是这样，那么断开客户端，因为它们发送无效头
            // The response will be null if the header sent is invalid, if so then disconnect client as they are sending invalid headers
            if (response == null)
				return;

			UDPNetworkingPlayer player = new UDPNetworkingPlayer(ServerPlayerCounter++, incomingEndpoint, false, groupEP, this);

            //如果一切顺利，则将验证的响应发送给客户端
            // If all is in order then send the validated response to the client
            Client.Send(response, response.Length, groupEP);

			OnPlayerConnected(player);
			udpPlayers.Add(incomingEndpoint, player);

            //player已成功连接
            // The player has successfully connected
            player.Connected = true;
		}

        /// <summary>
        /// 接收客户端数据包
        /// </summary>
        /// <param name="packet"></param>
		private void ReadPacket(BMSByte packet)
		{
			if (packet.Size < 17)
				return;

			try
			{
                // 格式的字节数据到一个udppacket结构
                // Format the byte data into a UDPPacket struct
                UDPPacket formattedPacket = TranscodePacket(currentReadingPlayer, packet);

                //检查这是否是一个确认数据包，这是正确的
                //一个数据包说可靠的数据包已被读取
                // Check to see if this is a confirmation packet, which is just
                // a packet to say that the reliable packet has been read
                if (formattedPacket.isConfirmation)
				{
                    //一旦玩家确认已被接受，则调用
                    // Called once the player has confirmed that it has been accepted
                    if (formattedPacket.groupId == MessageGroupIds.NETWORK_ID_REQUEST && !currentReadingPlayer.Accepted)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("[{0}] 请求的ID已收到 REQUESTED ID RECEIVED", DateTime.Now.Millisecond));
                        //玩家已被接受
                        // The player has been accepted
                        OnPlayerAccepted(currentReadingPlayer);
					}

                    // 确认消息被对方收到
					OnMessageConfirmed(currentReadingPlayer, formattedPacket);
					return;
				}

                //将数据包添加到管理器，以便可以在完成时跟踪和执行
                // Add the packet to the manager so that it can be tracked and executed on complete
                currentReadingPlayer.PacketManager.AddPacket(formattedPacket, PacketSequenceComplete, this);
			}
			catch (Exception ex)
			{
				Logging.BMSLog.LogException(ex);

                //player发送无效数据，请断开连接
                // The player is sending invalid data so disconnect them
                Disconnect(currentReadingPlayer, true);
			}
		}

        /// <summary>
        /// 接收完 一个消息的完整包
        /// </summary>
        /// <param name="data">完整包的数据</param>
        /// <param name="groupId">组ID</param>
        /// <param name="receivers">接收消息玩家方案</param>
        /// <param name="isReliable">是否可靠的</param>
		private void PacketSequenceComplete(BMSByte data, int groupId, byte receivers, bool isReliable)
		{
            //从发送的消息中拉出帧
            // Pull the frame from the sent message
            FrameStream frame = Factory.DecodeMessage(data.CompressBytes(), false, groupId, currentReadingPlayer, receivers);

			if (isReliable)
			{
				frame.ExtractReliableId();

                // TODO：如果这个玩家当前的可靠指数不是
                //指定的索引，那么它需要等待正确的排序
                // TODO:  If the current reliable index for this player is not at
                // the specified index, then it needs to wait for the correct ordering
                currentReadingPlayer.WaitReliable(frame);
			}
			else
				FireRead(frame, currentReadingPlayer);
		}

        /// <summary>
        /// 执行消息数据包
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="currentPlayer">发送消息的玩家</param>
		public override void FireRead(FrameStream frame, NetworkingPlayer currentPlayer)
		{
            //检查默认消息
            // Check for default messages
            if (frame is Text)
			{
                //如果播放器没有收到网络标识，则发送此数据包
                // This packet is sent if the player did not receive it's network id
                if (frame.GroupId == MessageGroupIds.NETWORK_ID_REQUEST)
				{
					currentPlayer.InstanceGuid = frame.ToString();

					OnPlayerGuidAssigned(currentPlayer);

                    //如果是这样，只需重新发送 玩家ID
                    // If so, just resend the player id
                    writeBuffer.Clear();
					writeBuffer.Append(BitConverter.GetBytes(currentPlayer.NetworkId));
					Send(currentPlayer, new Binary(Time.Timestep, false, writeBuffer, Receivers.Target, MessageGroupIds.NETWORK_ID_REQUEST, false), true);

					SendBuffer(currentPlayer);
					return;
				}
			}

            // 踢该玩家
			if (frame is ConnectionClose)
			{
				//Send(currentReadingPlayer, new ConnectionClose(Time.Timestep, false, Receivers.Server, MessageGroupIds.DISCONNECT, false), false);
                
				Disconnect(currentReadingPlayer, true);
				CleanupDisconnections();
				return;
			}

            //发送一个事件关闭已经被读取的数据包
            // Send an event off that a packet has been read
            OnMessageReceived(currentReadingPlayer, frame);
		}

        /// <summary>
        /// A callback from the NatHolePunch object saying that a client is trying to connect
        /// </summary>
        /// <param name="host">The host address of the client trying to connect</param>
        /// <param name="port">The port number to communicate with the client on</param>
        /// <summary>
        ///从NatHolePunch对象回调，说客户端正在尝试连接
        /// </ summary>
        /// <param name =“host”>尝试连接的客户端的主机地址</ param>
        /// <param name =“port”>与</ param>上的客户端进行通信的端口号
        private void NatClientConnectAttempt(string host, ushort port)
		{
			var x = ResolveHost(host, port);
			Logging.BMSLog.LogFormat("ATTEMPTING CONNECT ON {0} AND PORT IS {1}", host, port);
			Logging.BMSLog.LogFormat("RESOLVED IS {0} AND {1}", x.Address.ToString(), x.Port);

            //在这个客户端的nat中打一个洞
            // Punch a hole in the nat for this client
            Client.Send(new byte[1] { 0 }, 1, ResolveHost(host, port));
		}

        /// <summary>
        /// 发送缓存消息给这个玩家
        /// </summary>
        /// <param name="player"></param>
		private void SendBuffer(NetworkingPlayer player)
		{
			foreach (FrameStream frame in bufferedMessages)
				Send(player, frame, true);
		}

		public override void Ping()
		{
			// I am the server, so 0 ms...
			OnPingRecieved(0, Me);
		}

        /// <summary>
        /// 反馈ping, 将发送者发送时的时间一起发给对方
        /// Pong back to the client
        /// </summary>
        /// <param name="playerRequesting"></param>
        protected override void Pong(NetworkingPlayer playerRequesting, DateTime time)
		{
			Send(playerRequesting, GeneratePong(time));
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
        /// <param name="networkId"></param>
        /// <param name="minutes"></param>
		public void BanPlayer(ulong networkId, int minutes)
		{
			NetworkingPlayer player = Players.FirstOrDefault(p => p.NetworkId == networkId);

			if (player == null)
				return;

			BannedAddresses.Add(player.Ip);
			Disconnect(player, true);
			CommitDisconnects();
		}
	}
}
