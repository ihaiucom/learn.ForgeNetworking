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

using BeardedManStudios.Forge.Networking.DataStore;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BeardedManStudios.Forge.Networking
{
	public abstract class NetWorker
	{
        // 服务器广播编号 (检测局域网UDP服务器列表用)
        public const byte SERVER_BROADCAST_CODE = 42;
        // 客户端广播编号 (检测局域网UDP服务器列表用)
        public const byte CLIENT_BROADCAST_CODE = 24;

        // 广播列表请求  (检测局域网UDP服务器列表用)
        public const byte BROADCAST_LISTING_REQUEST_1 = 42;
		public const byte BROADCAST_LISTING_REQUEST_2 = 24;
		public const byte BROADCAST_LISTING_REQUEST_3 = 9;

        // 默认端口
		public const ushort DEFAULT_PORT = 15937;

        // 本地客户端清单 (检测局域网UDP服务器列表用)
        private static CachedUdpClient localListingsClient;

        // 解析主机
        public static IPEndPoint ResolveHost(string host, ushort port)
		{
            //检查任何本地主机类型的地址
            // Check for any localhost type addresses
            if (host == "0.0.0.0" || host == "127.0.0.1" || host == "::0")
				return new IPEndPoint(IPAddress.Parse(host), port);
			else if (host == "localhost")
				return new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

			IPAddress ipAddress;

			if (!IPAddress.TryParse(host, out ipAddress))
			{
				IPHostEntry hostCheck = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress ip in hostCheck.AddressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						if (ip.ToString() == host)
							return new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
					}
				}

				try
				{
					IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
					ipAddress = ipHostInfo.AddressList[0];
				}
				catch
				{
					Logging.BMSLog.Log("Failed to find host");
				}
			}

			return new IPEndPoint(ipAddress, port);
		}

        // 广播端点
		public struct BroadcastEndpoints
		{
			public string Address { get; private set; }
			public ushort Port { get; private set; }
			public bool IsServer { get; private set; }

			public BroadcastEndpoints(string address, ushort port, bool isServer) : this()
			{
				this.Address = address;
				this.Port = port;
				this.IsServer = isServer;
			}
		}

        // 局域网服务器列表
		public static List<BroadcastEndpoints> LocalEndpoints { get; private set; }

        // 是否在清理Session中
		public static bool EndingSession { get; private set; }

        #region Delegates
        /// <summary>
        /// 任何一种网络事件的基本代理
        /// A base delegate for any kind of network event
        /// </summary>
        public delegate void BaseNetworkEvent(NetWorker sender);

        /// <summary>
        /// 用于触发与广播端点相关的事件, 广播局域网服务器列
        /// Used to fire events that relate to a broadcast endpoint
        /// </summary>
        public delegate void BroadcastEndpointEvent(BroadcastEndpoints endpoint, NetWorker sender);

        /// <summary>
        /// 任何类型的网络ping事件的基本代理
        /// A base delegate for any kind of network ping event
        /// </summary>
        /// <param name="ping">The latency between client and server</param>
        public delegate void PingEvent(double ping, NetWorker sender);

        /// <summary>
        /// 用于任何与NetworkingPlayer相关的事件
        /// Used for any events that relate to a NetworkingPlayer
        /// </summary>
        public delegate void PlayerEvent(NetworkingPlayer player, NetWorker sender);

        /// <summary>
        /// Used for any events that relate to a frame and the target player
        /// </summary>
        /// <param name="player">The player the message came from</param>
        /// <param name="frame">The frame data</param>
        /// <summary>
        ///用于任何涉及到帧数据和目标玩家的事件
        /// </ summary>
        /// <param name =“player”>消息来自的player</ param>
        /// <param name =“frame”>帧数据</ param>
        public delegate void FrameEvent(NetworkingPlayer player, FrameStream frame, NetWorker sender);

        /// <summary>
        /// Used for any events that relate to a binary frame and the target player
        /// </summary>
        /// <param name="player">The player the message came from</param>
        /// <param name="frame">The frame data</param>
        /// <summary>
        ///用于任何涉及二进制框架和目标玩家的事件
        /// </ summary>
        /// <param name =“player”>消息来自</ param>的播放器
        /// <param name =“frame”>帧数据</ param>
        public delegate void BinaryFrameEvent(NetworkingPlayer player, Binary frame, NetWorker sender);

        /// <summary>
        /// Used for any events that relate to a text frame and the target player
        /// </summary>
        /// <param name="player">The player the message came from</param>
        /// <param name="frame">The frame data</param>
        /// <summary>
        ///用于任何涉及文本框架和目标玩家的事件
        /// </ summary>
        /// <param name =“player”>消息来自的player</ param>
        /// <param name =“frame”>帧数据</ param>
        public delegate void TextFrameEvent(NetworkingPlayer player, Text frame, NetWorker sender);
        #endregion

        #region Events
        /// <summary>
        /// 通过调用静态的SetupLocalUdpListings方法找到本地服务器时发生
        /// Occurs when a local server has been located by calling the static SetupLocalUdpListings method
        /// </summary>
        public static event BroadcastEndpointEvent localServerLocated;

        /// <summary>
        /// 当tcp监听器成功绑定时发生
        /// Occurs when tcp listener has successfully bound
        /// </summary>
        public event BaseNetworkEvent bindSuccessful;

        /// <summary>
        /// 当tcp侦听器绑定失败时发生
        /// Occurs when tcp listener has failed to bind
        /// </summary>
        public event BaseNetworkEvent bindFailure;

        /// <summary>
        /// 服务器接受这个客户端时发生
        /// Occurs when the server has accepted this client
        /// </summary>
        public event BaseNetworkEvent serverAccepted;

        /// <summary>
        /// 当当前套接字完全断开时发生
        /// Occurs when the current socket has completely disconnected
        /// </summary>
        public event BaseNetworkEvent disconnected;

        /// <summary>
        /// 当当前套接字被强制断开时发生
        /// Occurs when the current socket was forcibly disconnected
        /// </summary>
        public event BaseNetworkEvent forcedDisconnect;

        /// <summary>
        /// 玩家连接时发生
        /// Occurs when a player has connected
        /// </summary>
        public event PlayerEvent playerConnected;

        /// <summary>
        /// 玩家断开连接时发生
        /// Occurs when a player has disconnected
        /// </summary>
        public event PlayerEvent playerDisconnected;

        /// <summary>
        /// 当玩家超时时发生
        /// Occurs when a player has timed out
        /// </summary>
        public event PlayerEvent playerTimeout;

        /// <summary>
        /// 当玩家连接并被服务器验证时发生
        /// Occurs when the player has connected and been validated by the server
        /// </summary>
        public event PlayerEvent playerAccepted;

        /// <summary>
        /// 当玩家连接并且无法通过服务器验证时发生
        /// Occurs when the player has connected and was not able to be validated by the server
        /// </summary>
        public event PlayerEvent playerRejected;

        /// <summary>
        /// 从远程机器通过网络接收到消息时发生
        /// Occurs when a message is received over the network from a remote machine
        /// </summary>
        public event FrameEvent messageReceived;

        /// <summary>
        /// 当从远程计算机接收到二进制消息时发生。
        /// Occurs when a binary message is received over the network from a remote machine
        /// </summary>
        public event BinaryFrameEvent binaryMessageReceived;

        /// <summary>
        /// Occurs when a binary message is received and its router byte is the byte for Rpc
        /// </summary>
        //public event BinaryFrameEvent rpcMessageReceived;

        /// <summary>
        /// 从远程计算机通过网络接收到文本消息时发生
        /// Occurs when a text message is received over the network from a remote machine
        /// </summary>
        public event TextFrameEvent textMessageReceived;

        /// <summary>
        /// 从远程机器通过网络接收到ping时发生
        /// Occurs when a ping is received over the network from a remote machine
        /// </summary>
        public event PingEvent onPingPong;

		/// <summary>
		/// Called when a player has provided it's guid, this is useful for waiting until
		/// the player is uniquely identifiable across networkers
		/// </summary>
		public event PlayerEvent playerGuidAssigned;

        /// <summary>
        /// 当客户端从服务器异步获取属于此NetworkObject的ID时发生
        /// Occurs when a client get's an id from the server asynchronously that belongs to this NetworkObject
        /// </summary>
        public event NetworkObject.CreateEvent objectCreateAttach;

        /// <summary>
        /// 当网络对象已在网络上创建时发生。
        /// Occurs when a network object has been created on the network
        /// </summary>
        public event NetworkObject.NetworkObjectEvent objectCreated;

		/// <summary>
		/// TODO: COMMENT
		/// </summary>
		public event NetworkObject.CreateRequestEvent objectCreateRequested;

		/// <summary>
		/// TODO: COMMENT
		/// </summary>
		public event NetworkObject.NetworkObjectEvent factoryObjectCreated;
        #endregion

        #region Properties
        /// <summary>
        /// 所有联网玩家的列表。 这是本地网络的一个包装
        /// 为每个连接添加额外的元数据
        /// The list of all of the networked players. This is a wrapper around the native network
        /// socket with extra meta-data for each connection
        /// </summary>
        public List<NetworkingPlayer> Players { get; private set; }

        /// <summary>
        /// 所有要断开连接的玩家的列表。 如果玩家需要，这很有用
        /// 在当前被锁定时断开连接
        /// A list of all of the players that are to be disconnected. This is useful if a player needs
        /// to disconnect while they are currently locked
        /// </summary>
        protected List<NetworkingPlayer> DisconnectingPlayers { get; private set; }

        /// <summary>
        /// A list of all of the players that are to be forcibly disconnected.
        /// This is useful if a player needs to disconnect while they are currently locked
        /// </summary>
        /// <summary>
        ///所有被强行断开的玩家列表。
        ///如果玩家在当前被锁定时需要断开连接，这非常有用
        /// </ summary>
        protected List<NetworkingPlayer> ForcedDisconnectingPlayers { get; private set; }

        /// <summary>
        /// Represents the maximum allowed connections to this listener
        /// </summary>
        /// <value>Gets and sets the max allowed connections connections</value>
        /// <summary>
        ///表示允许连接到此侦听器的最大连接数
        /// </ summary>
        /// <value>获取并设置允许的最大连接数</ value>
        public int MaxConnections { get; private set; }

        /// <summary>
        /// This is a count for every player that has successfully connected since the start of this server,
        /// this also serves to be the unique id for this connection
        /// </summary>
        /// <value>The current count of players on the network</value>
        /// <summary>
        ///这是自该服务器启动以来已成功连接的每个玩家的计数，
        ///这也是这个连接的唯一ID
        /// </ summary>
        /// <value>网络上玩家的当前计数</ value>
        public uint ServerPlayerCounter { get; protected set; }

        /// <summary>
        /// 这个网络的端口
        /// The port for this networker
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// 确定此NetWorker是否为服务器的助手
        /// A helper to determine if this NetWorker is a server
        /// </summary>
        public bool IsServer { get { return this is IServer; } }

        /// <summary>
        /// 服务器缓存的一个句柄，用于缓存请求
        /// A handle to the server cache to make cache requests
        /// </summary>
        public Cache ServerCache { get; private set; }

        /// <summary>
        /// 用于确定已读取多少带宽（以字节为单位）
        /// Used to determine how much bandwidth (in bytes) hass been read
        /// </summary>
        public ulong BandwidthIn { get; protected set; }

        /// <summary>
        /// 用于确定已写入多少带宽（以字节为单位）
        /// Used to determine how much bandwidth (in bytes) hass been written
        /// </summary>
        public ulong BandwidthOut { get; set; }

        /// <summary>
        /// 用于模拟丢包，应该是0.0f和1.0f之间的一个数字（百分比）
        /// Used to simulate packet loss, should be a number between 0.0f and 1.0f (percentage)
        /// </summary>
        public float PacketLossSimulation { get; set; }

        /// <summary>
        /// 用于模拟网络延迟以测试高端的体验
        /// Used to simulate network latency to test experience at high pings
        /// </summary>
        public int LatencySimulation { get; set; }

		internal bool ObjectCreatedRegistered { get { return objectCreated != null; } }

        /// <summary>
        /// 缓存的BMSByte，以防止大量的数据包序列垃圾收集
        /// A cached BMSByte to prevent large amounts of garbage collection on packet sequences
        /// </summary>
        public BMSByte PacketSequenceData { get; private set; }
        #endregion

        /// <summary>
        /// 接近位置的距离，以便接收接近度
        /// 来自其他玩家的消息
        /// The distance from the proximity location in order to receive proximity
        /// messages from other players
        /// </summary>
        public float ProximityDistance { get; set; }

        /// <summary>
        /// 允许新创建的网络对象排队进行刷新调用
        /// Allows the newly created network object to be queued for the flush call
        /// </summary>
        public bool PendCreates { get; set; }

        /// <summary>
        /// 一个布尔值，告诉读取线程停止读取和关闭
        /// A boolean to tell the read thread to stop reading and close
        /// </summary>
        protected bool readThreadCancel = false;

        /// <summary>
        /// 当前机器的玩家引用
        /// A player reference to the current machine
        /// </summary>
        public NetworkingPlayer Me { get; protected set; }

        // 缺少对象缓冲区
        public Dictionary<uint, List<Action<NetworkObject>>> missingObjectBuffer = new Dictionary<uint, List<Action<NetworkObject>>>();

        /// <summary>
        /// 确定套接字是否连接
        /// Determine whether the socket is connected
        /// </summary>
        public bool IsConnected
		{
			get
			{
				if (Me != null)
					return Me.Connected;

				return false;
			}
		}

        /// <summary>
        /// 动态调整大小的字节缓冲区以帮助保存长时间的字节内存，
        /// 最大尺寸将是发送的最大消息的尺寸
        /// A cached dynamically resizing byte buffer to aid in holding byte memory for a long period of time,
        /// the max size will be the size of the largest message sent
        /// </summary>
        protected BMSByte writeBuffer = new BMSByte();

        /// <summary>
        /// 由它的id索引的所有网络对象的字典
        /// A dictionary of all of the network objects indexed by it's id
        /// </summary>
        public Dictionary<uint, NetworkObject> NetworkObjects { get; private set; }

        /// <summary>
        /// 所有网络对象的列表
        /// A list of all of the network objects
        /// </summary>
        public List<NetworkObject> NetworkObjectList { get; private set; }

        /// <summary>
        /// 用于为每个添加的网络对象赋予一个唯一的ID
        /// Used to give a unique id to each of the network objects that are added
        /// </summary>
        private uint currentNetworkObjectId = 0;

        /// <summary>
        /// This object is to track the time for this networker which is also known
        /// as a "time step" in this system
        /// </summary>
        /// <summary>
        ///这个对象用来跟踪这个也是已知的联网者的时间
        ///作为这个系统中的“时间步骤”
        /// </ summary>
        public TimeManager Time { get; set; }

        /// <summary>
        /// 用于确定这样的网络已被放弃
        /// Used to determine if this networker has been bound yet
        /// </summary>
        public bool IsBound { get; private set; }

        /// <summary>
        /// 用于确定此NetWorker是否已被释放以避免重新连接
        /// Used to determine if this NetWorker has already been disposed to avoid re-connections
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 表示此流程实例的所有联网用户的唯一GUID
        /// The unique GUID that will represent all networkers for this process instance
        /// </summary>
        public static Guid InstanceGuid { get; private set; }
		private static bool setupInstanceGuid = false;

        /// <summary>
        /// This is the base constructor which is normally used for clients and not classes
        /// acting as hosts
        /// </summary>
        /// <summary>
        ///这是通常用于客户端而不是类的基础构造函数
        ///充当主机
        /// </ summary>
        public NetWorker()
		{
			Initialize();
		}

        /// <summary>
        /// Constructor with a given Maximum allowed connections
        /// </summary>
        /// <param name="maxConnections">The Maximum connections allowed</param>
        /// <summary>
        ///具有给定的最大允许连接的构造函数
        /// </ summary>
        /// <param name =“maxConnections”>允许的最大连接数</ param>
        public NetWorker(int maxConnections)
		{
			Initialize();
			MaxConnections = maxConnections;
		}

        /// <summary>
        /// Used to setup any variables and private set properties, time and other 
        /// network critical variables that relate to a worker
        /// </summary>
        /// <summary>
        ///用于设置任何变量和私有属性，时间和其他
        ///与工作人员相关的网络关键变量
        /// </ summary>
        private void Initialize()
		{
			PacketSequenceData = new BMSByte();

			if (!setupInstanceGuid)
			{
				InstanceGuid = Guid.NewGuid();
				setupInstanceGuid = true;
			}

            //设置时间，如果它还没有被分配
            // Setup the time if it hasn't been assigned already
            Time = new TimeManager();

			Players = new List<NetworkingPlayer>();
			DisconnectingPlayers = new List<NetworkingPlayer>();
			ForcedDisconnectingPlayers = new List<NetworkingPlayer>();
			NetworkObjects = new Dictionary<uint, NetworkObject>();
			NetworkObjectList = new List<NetworkObject>();

			ServerPlayerCounter = 0;

			ServerCache = new Cache(this);
			EndingSession = false;
		}

        /// <summary>
        /// 一旦网络连接被绑定，就被调用
        /// 启动一个任务，刷新网络对象的属性
        /// Called once the network connection has been bound
        /// </summary>
        protected virtual void NetworkInitialize()
		{
			Task.Queue(() =>
			{
				while (IsBound)
				{
					ulong step = Time.Timestep;
					lock (NetworkObjects)
					{
						foreach (NetworkObject obj in NetworkObjects.Values)
						{
							// Only do the heartbeat (update) on network objects that
							// are owned by the current networker
							if ((obj.IsOwner && obj.UpdateInterval > 0) || (IsServer && obj.AuthorityUpdateMode))
								obj.HeartBeat(step);
						}
					}

					Thread.Sleep(10);
				}
			});
		}

        /// <summary>
        /// 网络对象注册完成
        /// </summary>
        /// <param name="networkObject">网络对象</param>
		public void CompleteInitialization(NetworkObject networkObject)
		{
			lock (NetworkObjects)
			{
				if (NetworkObjects.ContainsKey(networkObject.NetworkId))
					return;

				NetworkObjects.Add(networkObject.NetworkId, networkObject);
				NetworkObjectList.Add(networkObject);
			}
		}

        /// <summary>
        /// 网络对象注册完成时 调用
        /// missingObjectBuffer 保存的是该对象在还没被创建完成时，先接受到远程调用的方法 挂起。
        /// 等网络对象注册完成就调之前挂起时没处理的方法
        /// </summary>
        /// <param name="networkObject"></param>
		public void FlushCreateActions(NetworkObject networkObject)
		{
			List<Action<NetworkObject>> actions = null;
			lock (missingObjectBuffer)
			{
				missingObjectBuffer.TryGetValue(networkObject.NetworkId, out actions);
				missingObjectBuffer.Remove(networkObject.NetworkId);
			}

			if (actions == null)
				return;

			foreach (var action in actions)
				action(networkObject);
		}

        /// <summary>
        /// 迭代玩家列表，执行 参数是NetworkObject的表达式
        /// </summary>
        /// <param name="expression">参数是NetworkingPlayer的表达式</param>
		public void IteratePlayers(Action<NetworkingPlayer> expression)
		{
			lock (Players)
			{
				for (int i = 0; i < Players.Count; i++)
					expression(Players[i]);
			}
		}

        /// <summary>
        /// 迭代网络对象列表，执行 参数是NetworkObject的表达式
        /// </summary>
        /// <param name="expression">参数是NetworkObject的表达式</param>
		public void IterateNetworkObjects(Action<NetworkObject> expression)
		{
			lock (Players)
			{
				for (int i = 0; i < NetworkObjectList.Count; i++)
					expression(NetworkObjectList[i]);
			}
		}

        /// <summary>
        /// 获取NetworkId的玩家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public NetworkingPlayer GetPlayerById(uint id)
		{
			lock (Players)
			{
				for (int i = 0; i < Players.Count; i++)
				{
					if (Players[i].NetworkId == id)
						return Players[i];
				}
			}

			return null;
		}

        /// <summary>
        /// 查找玩家， 传一个查找方案的表达式进去
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
		public NetworkingPlayer FindPlayer(Func<NetworkingPlayer, bool> expression)
		{
			lock (Players)
			{
				return Players.FirstOrDefault(expression);
			}
		}

        /// <summary>
        /// 查找 Ip、InstanceGuid相等的玩家
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public NetworkingPlayer FindMatchingPlayer(NetworkingPlayer other)
		{
			if (other.Networker == this)
				return other;

			lock (Players)
			{
				for (int i = 0; i < Players.Count; i++)
				{
					if (Players[i].Ip == other.Ip && Players[i].InstanceGuid == other.InstanceGuid)
						return Players[i];
				}
			}

			return null;
		}

        /// <summary>
        /// Register a networked object with this networker
        /// </summary>
        /// <param name="networkObject">The object that is to be registered with this networker</param>
        /// <returns><c>true</c> if the object was registered successfully, else <c>false</c> if it has already been registered</returns>
        /// <summary>
        ///注册一个网络对象与这个网络, 如果forceId是0，会分配一个ID
        /// </ summary>
        /// <param name =“networkObject”>要使用此联网器注册的对象</ param>
        /// <returns> <c> true </ c>如果对象已经成功注册，否则<c> false </ c>已注册</ returns>
        public bool RegisterNetworkObject(NetworkObject networkObject, uint forceId = 0)
		{
			uint id = currentNetworkObjectId;

			lock (NetworkObjects)
			{
				// If we are forcing this object
				if (forceId != 0)
				{
					if (NetworkObjects.ContainsKey(forceId))
						return false;

					id = forceId;

					if (!networkObject.RegisterOnce(id))
						throw new BaseNetworkException("The supplied network object has already been assigned to a networker and has an id");

					//AddNetworkObject(forceId, networkObject);
					//NetworkObjectList.Add(networkObject);
				}
				else
				{
					do
					{
						if (NetworkObjects.ContainsKey(++currentNetworkObjectId))
							continue;

						if (!networkObject.RegisterOnce(currentNetworkObjectId))
						{
                            //返回，因为下次调用这个方法会在检查之前增加
                            // Backtrack since the next call to this method will increment before checking
                            currentNetworkObjectId--;

							throw new BaseNetworkException("The supplied network object has already been assigned to a networker and has an id");
						}

						//AddNetworkObject(currentNetworkObjectId, networkObject);
						//NetworkObjectList.Add(networkObject);
						break;
					} while (IsBound);
				}
			}

            //将网络ID分配给网络对象
            // Assign the network id to the network object
            networkObject.RegisterOnce(id);

			// When this object is destroyed it needs to remove itself from the list
			networkObject.onDestroy += (NetWorker sender) =>
			{
				lock (NetworkObjects)
				{
					NetworkObjects.Remove(networkObject.NetworkId);
					NetworkObjectList.Remove(networkObject);
				}
			};

			return true;
		}

        /// <summary>
        /// Disconnect this client from the server
        /// </summary>
        /// <param name="forced">Used to tell if this disconnect was intentional <c>false</c> or caused by an exception <c>true</c></param>
        /// <summary>
        ///从服务器断开这个客户端
        /// </ summary>
        /// <param name =“forced”>用来判断这个断开连接是故意的<c> false </ c>还是由异常引起<c> true </ c> </ param>
        public abstract void Disconnect(bool forced);

        /// <summary>
        /// Reads the frame stream as if it were read on the network
        /// </summary>
        /// <param name="frame">The target frame stream to be read</param>
        /// <summary>
        ///读取帧流，就像在网络上读取一样
        /// </ summary>
        /// <param name =“frame”>要读取的目标帧流</ param>
        public abstract void FireRead(FrameStream frame, NetworkingPlayer currentPlayer);

        /// <summary>
        /// Goes through all of the pending disconnect players and disconnects them
        /// Pending disconnects are always forced
        /// </summary>
        /// <summary>
        ///通过所有挂起的断开连接播放器并断开它们
        ///正在等待断开连接总是被迫
        /// </ summary>
        protected void DisconnectPending(Action<NetworkingPlayer, bool> disconnectMethod)
		{
			if (DisconnectingPlayers.Count == 0 && ForcedDisconnectingPlayers.Count == 0)
				return;

			lock (Players)
			{
				for (int i = DisconnectingPlayers.Count - 1; i >= 0; --i)
					disconnectMethod(DisconnectingPlayers[i], false);

				for (int i = ForcedDisconnectingPlayers.Count - 1; i >= 0; --i)
					disconnectMethod(ForcedDisconnectingPlayers[i], true);
			}
		}

        /// <summary>
        /// 这个调用的子项可以调用的绑定成功事件调用的包装器
        /// A wrapper for the bindSuccessful event call that chindren of this calls can call
        /// </summary>
        protected void OnBindSuccessful()
		{
			IsBound = true;
			NetworkInitialize();
			if (bindSuccessful != null)
				bindSuccessful(this);
		}

        /// <summary>
        /// 这个可以调用的bindFailure事件调用的包装器
        /// A wrapper for the bindFailure event call that children of this can call
        /// </summary>
        protected void OnBindFailure()
		{
			if (bindFailure != null)
				bindFailure(this);
		}

        /// <summary>
        /// A wrapper for the playerDisconnected event call that chindren of this can call.
        /// This also is responsible for adding the player to the lookup
        /// </summary>
        /// <summary>
        ///这个可以调用的playerDisconnected事件调用的包装器。
        ///这也是将玩家添加到查找的责任
        /// </ summary>
        protected void OnPlayerConnected(NetworkingPlayer player)
		{
			if (Players.Contains(player))
				throw new BaseNetworkException("无法添加播放器，因为它已经存在于列表中 Cannot add player because it already exists in the list");

			// Removal of clients can be from any thread
			lock (Players)
			{
				Players.Add(player);
			}

			if (playerConnected != null)
				playerConnected(player, this);
		}

		internal void OnObjectCreated(NetworkObject target)
		{
			if (objectCreated != null)
				objectCreated(target);
		}

		internal void OnObjectCreateAttach(int identity, int hash, uint id, FrameStream frame)
		{
			if (objectCreateAttach != null)
				objectCreateAttach(identity, hash, id, frame);
		}

		internal void OnObjectCreateRequested(int identity, uint id, FrameStream frame, Action<NetworkObject> callback)
		{
			if (objectCreateRequested != null)
				objectCreateRequested(this, identity, id, frame, callback);
		}

		internal void OnFactoryObjectCreated(NetworkObject obj)
		{
			if (factoryObjectCreated != null)
				factoryObjectCreated(obj);
		}

        /// <summary>
        /// 这是可以调用的绑定失败事件调用的包装器。 
        /// 这也是从查找中删除玩家的责任
        /// A wrapper for the bindFailure event call that chindren of this can call.
        /// This also is responsible for removing the player from the lookup
        /// </summary>
        protected void OnPlayerDisconnected(NetworkingPlayer player)
		{
			// Removal of clients can be from any thread
			lock (Players)
			{
				Players.Remove(player);
			}

			player.OnDisconnect();

			if (playerDisconnected != null)
				playerDisconnected(player, this);
		}

		protected void OnPlayerTimeout(NetworkingPlayer player)
		{
			if (playerTimeout != null)
				playerTimeout(player, this);
		}

        /// <summary>
        /// 玩家可以调用的接受的事件调用的包装器
        /// A wrapper for the playerAccepted event call that chindren of this can call
        /// </summary>
        protected void OnPlayerAccepted(NetworkingPlayer player)
		{
			player.Accepted = true;
			player.PendingAccepted = false;

			NetworkObject[] currentObjects;
			lock (NetworkObjects)
			{
				currentObjects = NetworkObjects.Values.ToArray();
			}
            // 服务器发送目前所有的网络对象给他
            NetworkObject.PlayerAccepted(player, currentObjects);

			if (playerAccepted != null)
				playerAccepted(player, this);
		}

		/// <summary>
		/// A wrapper for the playerAccepted event call that chindren of this can call
		/// </summary>
		protected void OnPlayerRejected(NetworkingPlayer player)
		{
			player.Accepted = false;

			if (playerRejected != null)
				playerRejected(player, this);
		}

		/// <summary>
		/// Set the port for the networker
		/// </summary>
		/// <param name="port"></param>
		protected void SetPort(ushort port)
		{
			this.Port = port;
		}

        /// <summary>
        /// pingreceived事件调用的包装器，可以调用它的子元素
        /// A wrapper for the pingReceived event call that children of this can call
        /// </summary>
        /// <param name="ping"></param>
        protected void OnPingRecieved(double ping, NetworkingPlayer player)
		{
			if (onPingPong != null)
				onPingPong(ping, this);

			player.RoundTripLatency = (int)ping;
		}

        /// <summary>
        /// 这个可以调用的messageReceived事件调用的包装器
        /// A wrapper for the messageReceived event call that chindren of this can call
        /// </summary>
        protected void OnMessageReceived(NetworkingPlayer player, FrameStream frame)
		{
            // 客户端被服务器接收
			if (frame.GroupId == MessageGroupIds.NETWORK_ID_REQUEST && this is IClient)
			{
				Time.SetStartTime(frame.TimeStep);
                // 读取玩家ID， 创建客户端自己的NetworkingPlayer
				Me = new NetworkingPlayer(frame.StreamData.GetBasicType<uint>(), "0.0.0.0", false, null, this);
				Me.AssignPort(Port);
				OnServerAccepted();
				return;
			}

            // 收到ping
			if (frame.GroupId == MessageGroupIds.PING || frame.GroupId == MessageGroupIds.PONG)
			{
                // 发送ping时的 发送者时间
				long receivedTimestep = frame.StreamData.GetBasicType<long>();
				DateTime received = new DateTime(receivedTimestep);

                // 现在接收到反馈ping的时间 - 自己发送ping是的时间
				TimeSpan ms = DateTime.UtcNow - received;

				if (frame.GroupId == MessageGroupIds.PING)
                    // 反馈ping, 将发送ping时的 发送者时间 一起发给他
                    Pong(player, received);
				else
                    // 接收都ping的反馈
					OnPingRecieved(ms.TotalMilliseconds, player);

				return;
			}

            // 二进制消息
			if (frame is Binary)
			{
				byte routerId = ((Binary)frame).RouterId;
				if (routerId == RouterIds.RPC_ROUTER_ID || routerId == RouterIds.BINARY_DATA_ROUTER_ID || routerId == RouterIds.CREATED_OBJECT_ROUTER_ID)
				{
					uint id = frame.StreamData.GetBasicType<uint>();
					NetworkObject targetObject = null;

					lock (NetworkObjects)
					{
						NetworkObjects.TryGetValue(id, out targetObject);
					}

					if (targetObject == null)
					{
                        // 收到该网络对象的消息包
                        // 但是该玩家机器上还没有创建网络对象
                        // 就将该消息缓存器来，等待网络对象创建完再掉用
						lock (missingObjectBuffer)
						{
							if (!missingObjectBuffer.ContainsKey(id))
								missingObjectBuffer.Add(id, new List<Action<NetworkObject>>());

							missingObjectBuffer[id].Add((networkObject) =>
							{
								ExecuteRouterAction(routerId, networkObject, (Binary)frame, player);
							});
						}

                        // TODO:  If the server is missing an object, it should have a timed buffer
                        // that way useless messages are not setting around in memory
                        // TODO：如果服务器缺少一个对象，它应该有一个定时缓冲区
                        //这种无用的消息不会在内存中设置

                        return;
					}

					ExecuteRouterAction(routerId, targetObject, (Binary)frame, player);
				}
                // 创建网络对象
				else if (routerId == RouterIds.NETWORK_OBJECT_ROUTER_ID)
				{
					NetworkObject.CreateNetworkObject(this, player, (Binary)frame);
				}
                // 在服务器接受客户端时，将服务器现有的所有网络对象 发给该玩家，让他创建
                else if (routerId == RouterIds.ACCEPT_MULTI_ROUTER_ID)
					NetworkObject.CreateMultiNetworkObject(this, player, (Binary)frame);
				else if (binaryMessageReceived != null)
					binaryMessageReceived(player, (Binary)frame, this);
			}
            // 文本消息
			else if (frame is Text && textMessageReceived != null)
				textMessageReceived(player, (Text)frame, this);

			if (messageReceived != null)
				messageReceived(player, frame, this);
		}

		private void ExecuteRouterAction(byte routerId, NetworkObject networkObject, Binary frame, NetworkingPlayer player)
		{
            // 执行RPC函数
			if (routerId == RouterIds.RPC_ROUTER_ID)
				networkObject.InvokeRpc(player, frame.TimeStep, frame.StreamData, frame.Receivers);
			else if (routerId == RouterIds.BINARY_DATA_ROUTER_ID)
				networkObject.ReadBinaryData(frame);
			else if (routerId == RouterIds.CREATED_OBJECT_ROUTER_ID)
				networkObject.CreateConfirmed(player);
		}

        /// <summary>
        /// 当这个插座已经断开
        /// When this socket has been disconnected
        /// </summary>
        protected void OnDisconnected()
		{
			IsBound = false;

			if (Me != null)
			{
				Me.Connected = false;

				if (!(this is IServer))
					Me.OnDisconnect();
			}

			if (disconnected != null)
				disconnected(this);

			Disposed = true;
		}

        /// <summary>
        /// 当这个插座被强行断开
        /// When this socket has been forcibly disconnected
        /// </summary>
        protected void OnForcedDisconnect()
		{
			IsBound = false;

			if (forcedDisconnect != null)
				forcedDisconnect(this);

			Disposed = true;
		}

        /// <summary>
        /// 客户端被服务器接受
        /// 从子类调用serverAccepted事件的包装器
        /// A wrapper around calling the serverAccepted event from child classes
        /// </summary>
        protected void OnServerAccepted()
		{
			Me.Connected = true;

			if (serverAccepted != null)
				serverAccepted(this);
		}

        /// <summary>
        /// A wrapper around calling the playerGuidAssigned event from child classes
        /// </summary>
        /// <param name="player">The player which the guid was assigned to</param>
        /// <summary>
        ///从子类调用playerGuidAssigned事件的包装器
        ///收到玩家的InstanceGuid，给玩家设置完InstanceGuid
        /// </ summary>
        /// <param name =“player”> guid分配给的玩家</ param>
        protected void OnPlayerGuidAssigned(NetworkingPlayer player)
		{
			if (playerGuidAssigned != null)
				playerGuidAssigned(player, this);
		}

		/// <summary>
		/// Used to bind to a port then unbind to trigger any operating system firewall requests
		/// </summary>
		public static void PingForFirewall(ushort port = 0)
		{
			if (port < 1)
			{
				port = DEFAULT_PORT - 1;
			}
			Task.Queue(() =>
			{
				try
				{
					//IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
					//IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, 15937);
					IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

					TcpListener t = new TcpListener(ipLocalEndPoint);
					t.Start();
					t.Stop();
				}
				catch (Exception ex)
				{
					Logging.BMSLog.LogException(ex);
				}
			});
		}

		public static void EndSession()
		{
			EndingSession = true;
			CloseLocalListingsClient();

            //在1000ms后重置结束会话，以便我们知道所有线程已经清理完毕
            //对于可能正在执行此前一个进程的任何剩余线程
            // Reset the ending session after 1000ms so that we know all the threads have cleaned up
            // for any remaining threads that may be going for this previous process
            Task.Queue(() =>
			{
				EndingSession = false;
			}, 1000);
		}

		public Ping GeneratePing()
		{
			BMSByte payload = new BMSByte();
			long ticks = DateTime.UtcNow.Ticks;
			payload.BlockCopy<long>(ticks, sizeof(long));
			return new Ping(Time.Timestep, this is TCPClient, payload, Receivers.Server, MessageGroupIds.PING, this is BaseTCP);
		}

		protected Pong GeneratePong(DateTime time)
		{
			BMSByte payload = new BMSByte();
			long ticks = time.Ticks;
			payload.BlockCopy<long>(ticks, sizeof(long));
			return new Pong(Time.Timestep, this is TCPClient, payload, Receivers.Target, MessageGroupIds.PONG, this is BaseTCP);
		}

        /// <summary>
        /// 请求从服务器ping（pingReceived将被触发，如果它收到）
        /// 这在UDP中不是一个可靠的调用
        /// Request the ping from the server (pingReceived will be triggered if it receives it)
        /// This is not a reliable call in UDP
        /// </summary>
        public abstract void Ping();

		protected abstract void Pong(NetworkingPlayer playerRequesting, DateTime time);

		private static void CloseLocalListingsClient()
		{
			if (localListingsClient != null)
			{
				localListingsClient.Close();
				localListingsClient = null;
			}
		}

        /// <summary>
        /// 找到网络上所有本地UDP服务器和客户端的方法
        /// 搜索局域网服务器
        /// A method to find all of the local UDP servers and clients on the network
        /// </summary>
        public static void RefreshLocalUdpListings(ushort portNumber = DEFAULT_PORT, int responseBuffer = 1000)
		{
			if (localListingsClient != null)
			{
				localListingsClient.Client.Close();
				localListingsClient = null;
			}

            //初始化列表以保存响应请求的所有本地网络端点
            // Initialize the list to hold all of the local network endpoints that respond to the request
            if (LocalEndpoints == null)
				LocalEndpoints = new List<BroadcastEndpoints>();

            //确保清除现有的端点
            // Make sure to clear out the existing endpoints
            lock (LocalEndpoints)
			{
				LocalEndpoints.Clear();
			}

            //创建一个客户端写在网络上，发现其他客户端和服务器
            // Create a client to write on the network and discover other clients and servers
            localListingsClient = new CachedUdpClient(19375);
			localListingsClient.EnableBroadcast = true;
			Task.Queue(() => { CloseLocalListingsClient(); }, responseBuffer);

			Task.Queue(() =>
			{
				IPEndPoint groupEp = default(IPEndPoint);
				string endpoint = string.Empty;

				localListingsClient.Send(new byte[] { BROADCAST_LISTING_REQUEST_1, BROADCAST_LISTING_REQUEST_2, BROADCAST_LISTING_REQUEST_3 }, 3, new IPEndPoint(IPAddress.Parse("255.255.255.255"), portNumber));

				try
				{
					while (localListingsClient != null && !EndingSession)
					{
						var data = localListingsClient.Receive(ref groupEp, ref endpoint);

						if (data.Size != 1)
							continue;

						string[] parts = endpoint.Split('+');
						string address = parts[0];
						ushort port = ushort.Parse(parts[1]);
						if (data[0] == SERVER_BROADCAST_CODE)
						{
							var ep = new BroadcastEndpoints(address, port, true);
							LocalEndpoints.Add(ep);

							if (localServerLocated != null)
								localServerLocated(ep, null);
						}
						else if (data[0] == CLIENT_BROADCAST_CODE)
							LocalEndpoints.Add(new BroadcastEndpoints(address, port, false));
					}
				}
				catch { }
			});
		}
	}
}