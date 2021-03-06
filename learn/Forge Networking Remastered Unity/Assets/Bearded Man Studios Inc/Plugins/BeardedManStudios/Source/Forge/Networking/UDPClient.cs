﻿/*-----------------------------+-------------------------------\
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BeardedManStudios.Forge.Networking
{
	public class UDPClient : BaseUDP, IClient
	{
        /// <summary>
        /// 此客户端尝试连接到服务器的最大尝试次数
        /// 每次尝试之间有3秒钟
        /// The max amount of tries that this client will attempt to connect to the server
        /// where there is 3 seconds between each attempt
        /// </summary>
        public const int CONNECT_TRIES = 10;

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
        /// 作为玩家的服务器的身份
        /// The identity of the server as a player
        /// </summary>
        private NetworkingPlayer server = null;
		public NetworkingPlayer Server { get { return server; } }

		public UDPPacketManager packetManager = new UDPPacketManager();

		public NatHolePunch nat = new NatHolePunch();

		public event BaseNetworkEvent connectAttemptFailed;

		public override void Send(FrameStream frame, bool reliable = false)
		{
			UDPPacketComposer composer = new UDPPacketComposer();

            //如果这个信息是可靠的，那么一定要保持对作曲家的引用
            //这样就没有任何失控的线程
            // If this message is reliable then make sure to keep a reference to the composer
            // so that there are not any run-away threads
            if (reliable)
			{
                //使用完成的事件从内存中清理对象
                // Use the completed event to clean up the object from memory
                composer.completed += ComposerCompleted;
				pendingComposers.Add(composer);
			}

            // TODO：在正则构造函数之前设置回调的新构造函数（如上所示）
            //TODO: New constructor for setting up callbacks before regular constructor (as seen above)
            composer.Init(this, Server, frame, reliable);
		}

		/// <summary>
		/// Sends binary message to the specified receiver(s)
		/// </summary>
		/// <param name="receivers">The clients / server to receive the message</param>
		/// <param name="messageGroupId">The Binary.GroupId of the massage, use MessageGroupIds.START_OF_GENERIC_IDS + desired_id</param>
		/// <param name="reliable">True if message must be delivered</param>
		/// <param name="objectsToSend">Array of vars to be sent, read them with Binary.StreamData.GetBasicType<typeOfObject>()</param>
		public virtual void Send(Receivers receivers = Receivers.Server, int messageGroupId = MessageGroupIds.START_OF_GENERIC_IDS, bool reliable = false , params object[] objectsToSend)
		{
			BMSByte data = ObjectMapper.BMSByte(objectsToSend);
			Binary sendFrame = new Binary(Time.Timestep, false, data, receivers, messageGroupId, false);
			Send(sendFrame, reliable);
		}

		/// <summary>
		/// This will connect a UDP client to a given UDP server
		/// </summary>
		/// <param name="host">The server's host address on the network</param>
		/// <param name="port">The port that the server is hosting on</param>
		/// <param name="natHost">The NAT server host address, if blank NAT will be skipped</param>
		/// <param name="natPort">The port that the NAT server is hosting on</param>
		/// <param name="pendCreates">Immidiately set the NetWorker::PendCreates to true</param>
		public void Connect(string host, ushort port = DEFAULT_PORT, string natHost = "", ushort natPort = NatHolePunch.DEFAULT_NAT_SERVER_PORT, bool pendCreates = false, ushort overrideBindingPort = DEFAULT_PORT + 1)
		{
			if (Disposed)
				throw new ObjectDisposedException("UDPClient", "This object has been disposed and can not be used to connect, please use a new UDPClient");

			// By default pending creates should be true and flushed when ready
			if (!pendCreates)
				PendCreates = true;

			try
			{
				ushort clientPort = overrideBindingPort;

				// Make sure not to listen on the same port as the server for local networks
				if (clientPort == port)
					clientPort++;

				for (; ; clientPort++)
				{
					try
					{
						Client = new CachedUdpClient(clientPort);
						break;
					}
					catch
					{
						if (port == 0)
							throw new BaseNetworkException("There were no ports available starting from port " + port);
					}
				}

				Client.EnableBroadcast = true;

				// If the server is behind a NAT, request for the port punch by the nat server
				if (!string.IsNullOrEmpty(natHost))
					nat.Connect(host, port, clientPort, natHost, natPort);

				// Do any generic initialization in result of the successful bind
				OnBindSuccessful();

				// Get a random hash key that needs to be used for validating that the server was connected to
				headerHash = Websockets.HeaderHashKey();

				// This is a typical Websockets accept header to be validated
				byte[] connectHeader = Websockets.ConnectionHeader(headerHash, port);

				// Setup the identity of the server as a player
				server = new NetworkingPlayer(0, host, true, ResolveHost(host, port), this);

				// Create the thread that will be listening for new data from connected clients and start its execution
				Task.Queue(ReadNetwork);

				//Let myself know I connected successfully
				OnPlayerConnected(server);

				// Set myself as a connected client
				server.Connected = true;

				//Set the port
				SetPort(clientPort);

				int connectCounter = 0;
				Task.Queue(() =>
				{
					do
					{
						// Send the accept headers to the server to validate
						Client.Send(connectHeader, connectHeader.Length, Server.IPEndPointHandle);
						Thread.Sleep(3000);
					} while (!headerExchanged && IsBound && ++connectCounter < CONNECT_TRIES);

					if (connectCounter >= CONNECT_TRIES)
					{
						if (connectAttemptFailed != null)
							connectAttemptFailed(this);
					}
				});
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
        /// Disconnect this client from the server
        /// </summary>
        /// <param name="forced">Used to tell if this disconnect was intentional <c>false</c> or caused by an exception <c>true</c></param>
        /// <summary>
        ///从服务器断开这个客户端
        /// </ summary>
        /// <param name =“forced”>用来判断这个断开连接是故意的<c> false </ c>还是由异常引起<c> true </ c> </ param>
        public override void Disconnect(bool forced)
		{
			if (Client == null)
				return;

			lock (Client)
			{
				if (forced)
					CloseConnection();
				else
				{
					var frame = new ConnectionClose(Time.Timestep, false, Receivers.Server, MessageGroupIds.DISCONNECT, false);
					Send(frame, true);
					Task.Queue(CloseConnection, 1000);
				}

				// Send signals to the methods registered to the disconnect events
				if (forced)
					//	OnDisconnected();
					//else
					OnForcedDisconnect();
			}
		}

        /// <summary>
        /// Infinite loop listening for new data from all connected clients on a separate thread.
        /// This loop breaks when readThreadCancel is set to true
        /// </summary>
        /// <summary>
        ///无限循环在单独的线程上监听来自所有连接客户端的新数据。
        ///当readThreadCancel设置为true时，此循环中断
        /// </ summary>
        private void ReadNetwork()
		{
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);
			string incomingEndpoint = string.Empty;

			try
			{
				BMSByte packet = null;
				// Intentional infinite loop
				while (IsBound)
				{
                    //如果读取已被标记为取消，则从此循环中断开
                    // If the read has been flagged to be canceled then break from this loop
                    if (readThreadCancel)
						return;

					try
					{
						// Read a packet from the network
						packet = Client.Receive(ref groupEP, ref incomingEndpoint);

						if (PacketLossSimulation > 0.0f && new Random().NextDouble() <= PacketLossSimulation)
						{
							// Skip this message
							continue;
						}

						BandwidthIn += (ulong)packet.Size;
					}
					catch (SocketException /*ex*/)
					{
						// This is a common exception when we exit the blocking call
						//Logging.BMSLog.LogException(ex);
						Disconnect(true);
					}

					// Check to make sure a message was received
					if (packet == null || packet.Size <= 0)
						continue;

					// This message was not from the server
					if (groupEP.Address != Server.IPEndPointHandle.Address &&
						groupEP.Port != Server.IPEndPointHandle.Port)
					{
						if (packet.Size == 1 && (packet[0] == SERVER_BROADCAST_CODE || packet[1] == CLIENT_BROADCAST_CODE))
						{

						}
						else if (packet.Size.Between(2, 4) && packet[0] == BROADCAST_LISTING_REQUEST_1 && packet[1] == BROADCAST_LISTING_REQUEST_2 && packet[2] == BROADCAST_LISTING_REQUEST_3)
						{
                            //这可能是一个本地列表请求，所以用客户端标志字节进行响应
                            // This may be a local listing request so respond with the client flag byte
                            Client.Send(new byte[] { CLIENT_BROADCAST_CODE }, 1, groupEP);
						}

						continue;
					}

					// Check to see if the headers have been exchanged
					if (!headerExchanged)
					{
						if (Websockets.ValidateResponseHeader(headerHash, packet.CompressBytes()))
						{
							headerExchanged = true;

							// TODO:  When getting the user id, it should also get the server time
							// by using the current time in the payload and getting it back along with server time

							// Ping the server to finalize the player's connection
							Send(Text.CreateFromString(Time.Timestep, InstanceGuid.ToString(), false, Receivers.Server, MessageGroupIds.NETWORK_ID_REQUEST, false), true);
						}
						else if (packet.Size != 1 || packet[0] != 0)
						{
							Disconnect(true);
							break;
						}
						else
							continue;
					}
					else
					{
						if (packet.Size < 17)
							continue;

                        // 格式的字节数据到一个udppacket结构
                        // Format the byte data into a UDPPacket struct
                        UDPPacket formattedPacket = TranscodePacket(Server, packet);

						// Check to see if this is a confirmation packet, which is just
						// a packet to say that the reliable packet has been read
						if (formattedPacket.isConfirmation)
						{
							if (formattedPacket.groupId == MessageGroupIds.DISCONNECT)
							{
								CloseConnection();
								return;
							}

							OnMessageConfirmed(server, formattedPacket);
							continue;
						}

                        //将数据包添加到管理器，以便可以在完成时跟踪和执行
                        // Add the packet to the manager so that it can be tracked and executed on complete
                        packetManager.AddPacket(formattedPacket, PacketSequenceComplete, this);
					}
				}
			}
			catch (Exception ex)
			{
				Logging.BMSLog.LogException(ex);
				Disconnect(true);
			}
		}

        /// <summary>
        /// 接收完 消息的完整数据包 处理
        /// </summary>
        /// <param name="data">消息的 完整数据</param>
        /// <param name="groupId">消息组ID</param>
        /// <param name="receivers">接收方 方案</param>
        /// <param name="isReliable">是否 可靠的消息</param>
		private void PacketSequenceComplete(BMSByte data, int groupId, byte receivers, bool isReliable)
		{
            //从发送的消息中拉出帧
            // Pull the frame from the sent message
            FrameStream frame = Factory.DecodeMessage(data.CompressBytes(), false, groupId, Server, receivers);

			if (isReliable)
			{
				frame.ExtractReliableId();

                // TODO:  If the current reliable index for this player is not at
                // the specified index, then it needs to wait for the correct ordering
                // TODO：如果这个玩家当前的可靠指数不是
                //指定的索引，那么它需要等待正确的排序
                Server.WaitReliable(frame);
			}
			else
				FireRead(frame, Server);
		}

		private void CloseConnection()
		{
			nat.Disconnect();

			if (Client == null)
				return;

			OnDisconnected();

			// Close our CachedUDPClient so that it can no longer be used
			Client.Close();
			Client = null;
		}

        /// <summary>
        /// 请求从服务器ping（pingReceived将被触发，如果它收到）
        /// 这不是一个可靠的回调
        /// Request the ping from the server (pingReceived will be triggered if it receives it)
        /// This is not a reliable call
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
        /// <summary>
        ///从服务器接收到一个ping，所以我们需要用乒乓回应
        /// </ summary>
        /// <param name =“playerRequesting”>服务器</ param>
        /// <param name =“time”>接收</ param>的时间
        protected override void Pong(NetworkingPlayer playerRequesting, DateTime time)
		{
			Send(GeneratePong(time));
		}

		public override void FireRead(FrameStream frame, NetworkingPlayer currentPlayer)
		{
			if (frame is ConnectionClose)
			{
				CloseConnection();
				return;
			}

			// Send an event off that a packet has been read
			OnMessageReceived(currentPlayer, frame);
		}
	}
}