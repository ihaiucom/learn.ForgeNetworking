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
using System.Collections.Generic;
using System.Linq;
using System.Net;

#if WINDOWS_UWP
using Windows.Networking.Sockets;
#else
using System.Net.Sockets;
#endif

namespace BeardedManStudios.Forge.Networking
{
	public partial class NetworkingPlayer
	{
        // 默认玩家超时断线时间 30秒
		//private const uint PLAYER_TIMEOUT_DISCONNECT = 30000;
        private const uint PLAYER_TIMEOUT_DISCONNECT = 3000000;

        // 默认发ping间隔时间 5秒
        private const int DEFAULT_PING_INTERVAL = 5000;

        /// <summary>
        /// 这个player断开连接时调用的事件
        /// An event that is called whenever this player has disconnected
        /// </summary>
        public event NetWorker.BaseNetworkEvent disconnected;

        /// <summary>
        /// 网络player的套接字 
        /// IPEndPoint(IP, PORT),   UDPServer 、 UDPClient、 TCPServer(cLIENT)
        /// TcpListener(IP, PROT)， TCPServer（ME）
        /// TcpClient, TCPClientBase(server)
        /// The socket to the Networking player
        /// </summary>
        public object SocketEndpoint { get; private set; }

        /// <summary>
        /// 对此player的原始tcp侦听器的引用（仅在服务器上使用）
        /// A reference to the raw tcp listener for this player (only used on server)
        /// </summary>
#if WINDOWS_UWP
		public StreamSocketListener TcpListenerHandle { get; private set; }
#else
        public TcpListener TcpListenerHandle { get; private set; }
#endif

        /// <summary>
        /// 这个播放器的原始tcp客户端的引用
        /// A reference to the raw tcp client for this player
        /// </summary>
#if WINDOWS_UWP
		public StreamSocket TcpClientHandle { get; private set; }
#else
        public TcpClient TcpClientHandle { get; private set; }
#endif

        /// <summary>
        /// 对这个player的IPEndPoint的引用
        /// A reference to the IPEndPoint for this player
        /// </summary>
        public IPEndPoint IPEndPointHandle { get; private set; }

        /// <summary>
        /// NetworkingPlayer所在的NetworkID
        /// The NetworkID the NetworkingPlayer is
        /// </summary>
        public uint NetworkId { get; private set; }

        /// <summary>
        ///  NetworkingPlayer的IP地址
        /// IP address of the NetworkingPlayer
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 此NetworkingPlayer的端口号
        /// Port number of this NetworkingPlayer
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// NetworkingPlayer的名称
        /// Name of the NetworkingPlayer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 确定服务器是否已经接受了玩家的连接
        /// Determines if the player has been accepted for the connection by the server
        /// </summary>
        public bool Accepted { get; set; }

        /// <summary>
        /// 确定player是否已经发送了接受请求，但服务器
        ///仍在等待接受的确认
        /// Determines if the player has been sent an accept request but the server
        /// is still waiting on a confirmation of the acceptance
        /// </summary>
        public bool PendingAccepted { get; set; }

        /// <summary>
        /// 确定玩家当前是否连接
        /// Determines if the player is currently connected
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// 一旦发生断开连接就设置
        /// Is set once a disconnection happens
        /// </summary>
        public bool Disconnected { get; private set; }

        /// <summary>
        /// 这是这个特定玩家所属的消息组
        /// This is the message group that this particular player is a part of
        /// </summary>
        public ushort MessageGroup { get; private set; }

        /// <summary>
        /// 最后ping发送到NetworkingPlayer
        /// Last ping sent to the NetworkingPlayer
        /// </summary>
        public ulong LastPing { get; private set; }

        /// <summary>
        /// 这个player是否是一个主机
        /// Whether this player is the one hosting
        /// </summary>
        public bool IsHost { get; private set; }

        /// <summary>
        /// 用于确定此播放器是否处于断开连接的过程中
        /// Used to determine if this player is in the process of disconnecting
        /// </summary>
        public bool IsDisconnecting { get; set; }

        /// <summary>
        /// 我们是否被锁定
        /// Whether we are locked
        /// </summary>
        public object MutexLock = new object();

        /// <summary>
        /// 保存所有可靠的作曲家的名单，以便他们按顺序排列。
        /// Keep a list of all of the composers that are reliable so that they are sent in order
        /// </summary>
        private List<UDPPacketComposer> reliableComposers = new List<UDPPacketComposer>();

        /// <summary>
        ///应该用于将此网络player与其他网络player参考进行匹配
        ///在不同的网络上。
        /// Should be used for matching this networking player with another networking player reference
        /// on a different networker.
        /// </summary>
        public string InstanceGuid { get; set; }

        /// <summary>
        /// 超时时间， ping
        /// 如果没有消息发送，断开此player的时间量（以秒为单位）
        /// The amount of time in seconds to disconnect this player if no messages are sent
        /// </summary>
        public uint TimeoutMilliseconds { get; set; }

		private bool composerReady = false;

        // 当前等待发ping时间
		private int currentPingWait = 0;
        // ping的间隔时间
		public int PingInterval { get; set; }

        /// <summary>
        /// 发生ping所花费的时间
        /// The amount of time it took for a ping to happen
        /// </summary>
        public int RoundTripLatency { get; set; }

		public NetWorker Networker { get; private set; }

        /// <summary>
        /// This is used for proximity based updates, this should update with
        /// the player location to properly be used with the NetWorker::ProximityDistance
        /// </summary>
        /// <summary> 
        /// 这用于基于邻近的更新，这应该更新为
        /// 播放器位置，以便正确使用NetWorker :: Proximity Distance 
        /// </ summary>
        public Vector ProximityLocation { get; set; }

        // 当前可靠的Id
		private ulong currentReliableId = 0;
		public Dictionary<ulong, FrameStream> reliablePending = new Dictionary<ulong, FrameStream>();

		public ulong UniqueReliableMessageIdCounter { get; private set; }

		private Queue<ulong> reliableComposersToRemove = new Queue<ulong>();

        /// <summary>
        /// Constructor for the NetworkingPlayer
        /// </summary>
        /// <param name="networkId">NetworkId set for the NetworkingPlayer</param>
        /// <param name="ip">IP address of the NetworkingPlayer</param>
        /// <param name="socketEndpoint">The socket to the Networking player</param>
        /// <param name="name">Name of the NetworkingPlayer</param>
        /// <summary>
        /// NetworkingPlayer的构造函数
        /// </ summary>
        /// <param name =“networkId”>为NetworkingPlayer设置的NetworkId, 也算是玩家Socket ID, 服务器的是0 </ param>
        /// <param name =“ip”> NetworkingPlayer的IP地址</ param>
        /// <param name =“socketEndpoint”>网络播放器的套接字</ param>
        /// <param name =“name”> NetworkingPlayer的名称</ param>
        public NetworkingPlayer(uint networkId, string ip, bool isHost, object socketEndpoint, NetWorker networker)
		{
			Networker = networker;
			NetworkId = networkId;
			Ip = ip.Split('+')[0];
			IsHost = isHost;
			SocketEndpoint = socketEndpoint;
			LastPing = networker.Time.Timestep;
			TimeoutMilliseconds = PLAYER_TIMEOUT_DISCONNECT;
			PingInterval = DEFAULT_PING_INTERVAL;

			if (SocketEndpoint != null)
			{
#if WINDOWS_UWP
				// Check to see if the supplied socket endpoint is TCP, if so
				// assign it to the TcpClientHandle for ease of access
				if (socketEndpoint is StreamSocket)
				{
					TcpClientHandle = (StreamSocket)socketEndpoint;
					IPEndPointHandle = (IPEndPoint)TcpClientHandle.Client.RemoteEndPoint;
				}
				else if (socketEndpoint is StreamSocketListener)
				{
					TcpListenerHandle = (StreamSocketListener)socketEndpoint;
					IPEndPointHandle = (IPEndPoint)TcpListenerHandle.LocalEndpoint;
				}
				else if (SocketEndpoint is IPEndPoint)
					IPEndPointHandle = (IPEndPoint)SocketEndpoint;
#else
                //检查提供的套接字端点是否是TCP，如果是的话
                //将其分配给TcpClientHandle以方便访问
                // Check to see if the supplied socket endpoint is TCP, if so
                // assign it to the TcpClientHandle for ease of access
                if (socketEndpoint is TcpClient)
				{
					TcpClientHandle = (TcpClient)socketEndpoint;
					IPEndPointHandle = (IPEndPoint)TcpClientHandle.Client.RemoteEndPoint;
				}
				else if (socketEndpoint is TcpListener)
				{
					TcpListenerHandle = (TcpListener)socketEndpoint;
					IPEndPointHandle = (IPEndPoint)TcpListenerHandle.LocalEndpoint;
				}
				else if (SocketEndpoint is IPEndPoint)
					IPEndPointHandle = (IPEndPoint)SocketEndpoint;
#endif

				Port = (ushort)IPEndPointHandle.Port;
			}
		}

		public void AssignPort(ushort port)
		{
            //只允许分配一次
            // Only allow to be assigned once
            if (Port != 0)
				return;

			Port = port;
		}

        /// <summary>
        /// Ping the NetworkingPlayer
        /// </summary>
        public void Ping()
		{
			LastPing = Networker.Time.Timestep;
		}

        /// <summary>
        /// Called by the server to check and see if this player has timed out
        /// </summary>
        /// <returns>True if the player has timed out</returns>
        /// <summary>
        ///由服务器调用来检查这个玩家是否超时
        /// </ summary>
        /// <returns>如果玩家超时</ returns>则为真
        public bool TimedOut()
		{
			return LastPing + TimeoutMilliseconds <= Networker.Time.Timestep;
		}

        /// <summary>
        /// Assigns the message group for this player
        /// </summary>
        /// <param name="messageGroup">The numerical identifier of the message group</param>
        /// <summary>
        ///为该player分配消息组
        /// </ summary>
        /// <param name =“messageGroup”>消息组的数字标识符</ param>
        public void SetMessageGroup(ushort messageGroup)
		{
			MessageGroup = messageGroup;
		}

		public void OnDisconnect()
		{
			Disconnected = true;
			Connected = false;

			StopComposers();

			if (disconnected != null)
				disconnected(Networker);
		}

		public void QueueComposer(UDPPacketComposer composer)
		{
			if (Disconnected)
				return;

			lock (reliableComposers)
			{
				reliableComposers.Add(composer);
			}

            //在这个作曲家上启动可靠的发送线程
            // Start the reliable send thread on this composer
            NextComposerInQueue();
		}

		/// <summary>
        /// 启动任务 发送ping
		/// Star the next composer available composer
		/// </summary>
		private void NextComposerInQueue()
		{
            //如果目前没有排队的作曲家，那么我们可以在这里停下来
            // If there are not currently any queued composers then we can stop here
            if (reliableComposers.Count == 0)
				return;

			if (!composerReady && Networker.IsBound && !NetWorker.EndingSession)
			{
				composerReady = true;

                //在一个单独的线程上运行它，这样它就不会干扰读线程
                // Run this on a separate thread so that it doesn't interfere with the reading thread
                Task.Queue(() =>
				{
					int waitTime = 10, composerCount = 0;
					while (Networker.IsBound && !Disconnected)
					{
						lock (reliableComposers)
						{
							composerCount = reliableComposers.Count;
						}

						if (composerCount == 0)
						{
                            // 在ping的间隔时间内发一个ping
							Task.Sleep(waitTime);
							currentPingWait += waitTime;

							if (!(Networker is IServer) && currentPingWait >= PingInterval)
							{
								currentPingWait = 0;
								Networker.Ping();
							}

							continue;
						}

						do
						{
                            //如果发送的数据包太多，一定要发送
                            //一些不会阻塞网络。
                            // If there are too many packets to send, be sure to only send
                            // a few to not clog the network.
                            int counter = UDPPacketComposer.PACKET_SIZE;

                            //发送所有待处理的数据包
                            // Send all the packets that are pending
                            lock (reliableComposers)
							{
								for (int i = 0; i < reliableComposers.Count; i++)
									reliableComposers[i].ResendPackets(Networker.Time.Timestep, ref counter);
							}

							Task.Sleep(10);

                            //检查是否有一些作曲家排队等候删除
                            // Check if we have some composers queued to remove
                            lock (reliableComposersToRemove)
							{
								while (reliableComposersToRemove.Count > 0)
								{
									// Remove 
									lock (reliableComposers)
									{
										ulong id = reliableComposersToRemove.Dequeue();
										reliableComposers.Remove(reliableComposers.First(r => r.Frame.UniqueId == id));
									}
								}
							}

							lock (reliableComposers)
							{
								composerCount = reliableComposers.Count;
							}
						} while (composerCount > 0 && Networker.IsBound && !NetWorker.EndingSession);
						currentPingWait = 0;
					}
				});
			}
		}

        /// <summary>
        /// 清理当前的作曲家并准备启动队列中的下一个。
        /// Cleans up the current composer and prepares to start up the next in the queue
        /// </summary>
        public void CleanupComposer(ulong uniqueId)
		{
			lock (reliableComposers)
			{
				reliableComposers.Remove(reliableComposers.First(r => r.Frame.UniqueId == uniqueId));
			}
		}

        /// <summary>
        /// Add composer to the queue, so it will be removed by sending thread
        /// </summary>
        /// <summary>
        ///将composer添加到队列中，所以它将通过发送线程被删除
        /// </ summary>
        public void EnqueueComposerToRemove(ulong uniqueId)
		{
			lock (reliableComposersToRemove)
			{
				reliableComposersToRemove.Enqueue(uniqueId);
			}
		}

        /// <summary>
        /// Go through and stop all of the reliable composers for this player to prevent
        /// them from being sent
        /// </summary>
        /// <summary>
        ///通过并停止所有可靠的作曲家为了防止这个玩家
        ///他们被发送
        /// </ summary>
        public void StopComposers()
		{
			lock (reliableComposers)
			{
				reliableComposers.Clear();
			}
		}

        /// <summary>
        /// 将接收的可靠消息包 放进来
        /// 如果消息包不是可靠类型的，就不处理
        /// 如果消息包的可靠编号 等于 最后执行到的编号,就执行
        /// 如果消息包的可靠编号 大于 最后执行到的编号, 就放到挂起待执行列表
        /// 否则不处理
        /// </summary>
        /// <param name="frame"></param>
		public void WaitReliable(FrameStream frame)
		{
			if (!frame.IsReliable)
				return;

			if (frame.UniqueReliableId == currentReliableId)
			{
				Networker.FireRead(frame, this);
				currentReliableId++;

				FrameStream next = null;
                // 执行剩余的挂起包
				while (true)
				{
					if (!reliablePending.TryGetValue(currentReliableId, out next))
						break;

					reliablePending.Remove(currentReliableId++);
					Networker.FireRead(next, this);
				}
			}
			else if (frame.UniqueReliableId > currentReliableId)
				reliablePending.Add(frame.UniqueReliableId, frame);
		}

        // 生成一个可靠帧数据的唯一ID
		public ulong GetNextReliableId()
		{
			return UniqueReliableMessageIdCounter++;
		}
	}
}