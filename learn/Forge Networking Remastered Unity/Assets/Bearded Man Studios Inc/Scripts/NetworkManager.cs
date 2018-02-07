using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BeardedManStudios.Forge.Networking.Unity
{
    /// <summary>
    /// 负责： 场景加载，网络对象, 向MasterServer注册
    /// </summary>
	public partial class NetworkManager : MonoBehaviour
	{
		public static NetworkManager Instance { get; private set; }
        // 网络 场景加载前 事件
		public UnityAction<int, LoadSceneMode> networkSceneChanging;
        // 网络 场景加载完 事件
        public UnityAction<Scene, LoadSceneMode> networkSceneLoaded;
        // 网络 场景加载完成，并且处理完场景的NetworkBehavior
        public event NetWorker.PlayerEvent playerLoadedScene;

        // 主网络
		public NetWorker Networker { get; private set; }
		public NetWorker MasterServerNetworker { get; private set; }
		public Dictionary<int, INetworkBehavior> pendingObjects = new Dictionary<int, INetworkBehavior>();
		public Dictionary<int, NetworkObject> pendingNetworkObjects = new Dictionary<int, NetworkObject>();
		private string _masterServerHost;
		private ushort _masterServerPort;

        // 当前已加载的场景
		private List<int> loadedScenes = new List<int>();

		public bool IsServer { get { return Networker.IsServer; } }

		/// <summary>
		/// Used to enable or disable the automatic switching for clients
		/// </summary>
		public bool automaticScenes = true;

#if FN_WEBSERVER
		MVCWebServer.ForgeWebServer webserver = null;
#endif

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			MainThreadManager.Create();

			// This object should move through scenes
			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			if (automaticScenes)
				SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		private void OnDisable()
		{
			if (automaticScenes)
				SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		}

		public void Initialize(NetWorker networker, string masterServerHost = "", ushort masterServerPort = 15940, JSONNode masterServerRegisterData = null)
		{
			Networker = networker;
			networker.objectCreated += CreatePendingObjects;
			Networker.binaryMessageReceived += ReadBinary;
			SetupObjectCreatedEvent();

			UnityObjectMapper.Instance.UseAsDefault();
			NetworkObject.Factory = new NetworkObjectFactory();

			if (Networker is IServer)
			{
				if (!string.IsNullOrEmpty(masterServerHost))
				{
					_masterServerHost = masterServerHost;
					_masterServerPort = masterServerPort;

					RegisterOnMasterServer(masterServerRegisterData);
				}

				Networker.playerAccepted += PlayerAcceptedSceneSetup;

#if FN_WEBSERVER
                // 启动Web服
				string pathToFiles = "fnwww/html";
				Dictionary<string, string> pages = new Dictionary<string, string>();
				TextAsset[] assets = Resources.LoadAll<TextAsset>(pathToFiles);
				foreach (TextAsset a in assets)
					pages.Add(a.name, a.text);

				webserver = new MVCWebServer.ForgeWebServer(networker, pages);
				webserver.Start();
#endif
			}
		}

		private void CreatePendingObjects(NetworkObject obj)
		{
			INetworkBehavior behavior;

			if (!pendingObjects.TryGetValue(obj.CreateCode, out behavior))
			{
				if (obj.CreateCode < 0)
					pendingNetworkObjects.Add(obj.CreateCode, obj);

				return;
			}

			behavior.Initialize(obj);
			pendingObjects.Remove(obj.CreateCode);

			if (pendingObjects.Count == 0)
				Networker.objectCreated -= CreatePendingObjects;
		}

        // 向MasterServer 请求匹配的游戏服务器列表
		public void MatchmakingServersFromMasterServer(string masterServerHost,
			ushort masterServerPort,
			int elo,
			System.Action<MasterServerResponse> callback = null,
			string gameId = "myGame",
			string gameType = "any",
			string gameMode = "all")
		{
			// The Master Server communicates over TCP
			TCPMasterClient client = new TCPMasterClient();

			// Once this client has been accepted by the master server it should send it's get request
			client.serverAccepted += (sender) =>
			{
				try
				{
					// Create the get request with the desired filters
					JSONNode sendData = JSONNode.Parse("{}");
					JSONClass getData = new JSONClass();
					getData.Add("id", gameId);
					getData.Add("type", gameType);
					getData.Add("mode", gameMode);
					getData.Add("elo", new JSONData(elo));

					sendData.Add("get", getData);

					// Send the request to the server
					client.Send(Text.CreateFromString(client.Time.Timestep, sendData.ToString(), true, Receivers.Server, MessageGroupIds.MASTER_SERVER_GET, true));
				}
				catch
				{
					// If anything fails, then this client needs to be disconnected
					client.Disconnect(true);
					client = null;

					MainThreadManager.Run(() =>
					{
						if (callback != null)
							callback(null);
					});
				}
			};

			// An event that is raised when the server responds with hosts
			client.textMessageReceived += (player, frame, sender) =>
			{
				try
				{
					// Get the list of hosts to iterate through from the frame payload
					JSONNode data = JSONNode.Parse(frame.ToString());
					MainThreadManager.Run(() =>
					{
						if (data["hosts"] != null)
						{
							MasterServerResponse response = new MasterServerResponse(data["hosts"].AsArray);
							if (callback != null)
								callback(response);
						}
						else
						{
							if (callback != null)
								callback(null);
						}
					});
				}
				finally
				{
					if (client != null)
					{
						// If we succeed or fail the client needs to disconnect from the Master Server
						client.Disconnect(true);
						client = null;
					}
				}
			};

			try
			{
				client.Connect(masterServerHost, masterServerPort);
			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex.Message);
				MainThreadManager.Run(() =>
				{
					if (callback != null)
						callback(null);
				});
			}
		}

        // 生成注册MasterServer的信息数据
		public JSONNode MasterServerRegisterData(NetWorker server, string id, string serverName, string type, string mode, string comment = "", bool useElo = false, int eloRequired = 0)
		{
			// Create the get request with the desired filters
			JSONNode sendData = JSONNode.Parse("{}");
			JSONClass registerData = new JSONClass();
			registerData.Add("id", id);
			registerData.Add("name", serverName);
			registerData.Add("port", new JSONData(server.Port));
			registerData.Add("playerCount", new JSONData(0));
			registerData.Add("maxPlayers", new JSONData(server.MaxConnections));
			registerData.Add("comment", comment);
			registerData.Add("type", type);
			registerData.Add("mode", mode);
			registerData.Add("protocol", server is UDPServer ? "udp" : "tcp");
			registerData.Add("elo", new JSONData(eloRequired));
			registerData.Add("useElo", new JSONData(useElo));
			sendData.Add("register", registerData);

			return sendData;
		}
        /// <summary>
        /// 向MasterServer注册
        /// 1. 创建连接
        /// 2. 发送数据
        /// </summary>
        /// <param name="masterServerData">MasterServerRegisterData()方法生成的josn数据</param>

        private void RegisterOnMasterServer(JSONNode masterServerData)
		{
            // Master Server 通过TCP进行通信
            // The Master Server communicates over TCP
            TCPMasterClient client = new TCPMasterClient();

            // 一旦这个客户端被主服务器接受，它应该发送它的获取请求
            // Once this client has been accepted by the master server it should send it's get request
            client.serverAccepted += (sender) =>
			{
				try
				{
					Text temp = Text.CreateFromString(client.Time.Timestep, masterServerData.ToString(), true, Receivers.Server, MessageGroupIds.MASTER_SERVER_REGISTER, true);

					//Debug.Log(temp.GetData().Length);
					// Send the request to the server
					client.Send(temp);

					Networker.disconnected += s =>
					{
						client.Disconnect(false);
						MasterServerNetworker = null;
					};
				}
				catch
				{
					// If anything fails, then this client needs to be disconnected
					client.Disconnect(true);
					client = null;
				}
			};

			client.Connect(_masterServerHost, _masterServerPort);

			Networker.disconnected += NetworkerDisconnected;
			MasterServerNetworker = client;
		}

		private void NetworkerDisconnected(NetWorker sender)
		{
			Networker.disconnected -= NetworkerDisconnected;
			MasterServerNetworker.Disconnect(false);
			MasterServerNetworker = null;
		}

		public void UpdateMasterServerListing(NetWorker server, string comment = null, string gameType = null, string mode = null)
		{
			JSONNode sendData = JSONNode.Parse("{}");
			JSONClass registerData = new JSONClass();
			registerData.Add("playerCount", new JSONData(server.MaxConnections));
			registerData.Add("comment", comment);
			registerData.Add("type", gameType);
			registerData.Add("mode", mode);
			registerData.Add("port", new JSONData(server.Port));
			sendData.Add("update", registerData);

			UpdateMasterServerListing(sendData);
		}

		private void UpdateMasterServerListing(JSONNode masterServerData)
		{
			if (string.IsNullOrEmpty(_masterServerHost))
			{
				throw new System.Exception("This server is not registered on a master server, please ensure that you are passing a master server host and port into the initialize");
			}

			// The Master Server communicates over TCP
			TCPMasterClient client = (TCPMasterClient)MasterServerNetworker;

			// Once this client has been accepted by the master server it should send it's update request
			client.serverAccepted += (sender) =>
			{
				try
				{
					Text temp = Text.CreateFromString(client.Time.Timestep, masterServerData.ToString(), true, Receivers.Server, MessageGroupIds.MASTER_SERVER_UPDATE, true);

					//Debug.Log(temp.GetData().Length);
					// Send the request to the server
					client.Send(temp);
				}
				catch
				{
					// If anything fails, then this client needs to be disconnected
					client.Disconnect(true);
					client = null;
				}
			};

			client.Connect(_masterServerHost, _masterServerPort);
		}

		public void Disconnect()
		{
#if FN_WEBSERVER
			webserver.Stop();
#endif

			Networker.objectCreated -= CreatePendingObjects;

			if (Networker != null)
				Networker.Disconnect(false);

			NetWorker.EndSession();

			NetworkObject.ClearNetworkObjects(Networker);
			pendingObjects.Clear();
			pendingNetworkObjects.Clear();
			MasterServerNetworker = null;
			Networker = null;
			Instance = null;
			Destroy(gameObject);
		}

		private void OnApplicationQuit()
		{
			if (Networker != null)
				Networker.Disconnect(false);

			NetWorker.EndSession();
		}

		private void FixedUpdate()
		{
			if (Networker != null)
			{
				for (int i = 0; i < Networker.NetworkObjectList.Count; i++)
					Networker.NetworkObjectList[i].InterpolateUpdate();
			}
		}

		private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			SceneReady(scene, mode);
		}

		private void ProcessOthers(Transform obj, NetworkObject createTarget, uint idOffset, NetworkBehavior netBehavior = null)
		{
			int i;

			// Get the order of the components as they are in the inspector
			var components = obj.GetComponents<NetworkBehavior>();

			// Create each network object that is available
			for (i = 0; i < components.Length; i++)
			{
				if (components[i] == netBehavior)
					continue;

				var no = components[i].CreateNetworkObject(Networker, 0);

				if (Networker.IsServer)
					FinalizeInitialization(obj.gameObject, components[i], no, obj.position, obj.rotation, false, true);
				else
					components[i].AwaitNetworkBind(Networker, createTarget, idOffset++);
			}

			for (i = 0; i < obj.transform.childCount; i++)
				ProcessOthers(obj.transform.GetChild(i), createTarget, idOffset);
		}

		private void FinalizeInitialization(GameObject go, INetworkBehavior netBehavior, NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true, bool skipOthers = false)
		{
			if (Networker is IServer)
				InitializedObject(netBehavior, obj);
			else
				obj.pendingInitialized += InitializedObject;

			if (position != null)
			{
				if (rotation != null)
				{
					go.transform.position = position.Value;
					go.transform.rotation = rotation.Value;
				}
				else
					go.transform.position = position.Value;
			}

			//if (sendTransform)
			// obj.SendRpc(NetworkBehavior.RPC_SETUP_TRANSFORM, Receivers.AllBuffered, go.transform.position, go.transform.rotation);

			if (!skipOthers)
			{
				// Go through all associated network behaviors in the hierarchy (including self) and
				// Assign their TempAttachCode for lookup later. Should use an incrementor or something
				ProcessOthers(go.transform, obj, 1, (NetworkBehavior)netBehavior);
			}
		}

        /// <summary>
        /// Called automatically when a new player is accepted and sends the player
        /// the currently loaded scene indexes for the client to load
        /// </summary>
        /// <param name="player">The player that was just accepted</param>
        /// <summary>
        ///接受新玩家并自动发送
        ///当前加载的场景索引为客户端加载
        /// </ summary>
        /// <param name =“player”>刚被接受的玩家</ param>
        private void PlayerAcceptedSceneSetup(NetworkingPlayer player, NetWorker sender)
		{
			BMSByte data = ObjectMapper.BMSByte(loadedScenes.Count);

            // 发送当前加载的创建给连接的玩家
			// Go through all the loaded scene indexes and send them to the connecting player
			for (int i = 0; i < loadedScenes.Count; i++)
				ObjectMapper.Instance.MapBytes(data, loadedScenes[i]);

			Binary frame = new Binary(sender.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.VIEW_INITIALIZE, sender is BaseTCP);

			SendFrame(sender, frame, player);
		}

        // 读取接收的消息
		private void ReadBinary(NetworkingPlayer player, Binary frame, NetWorker sender)
		{
			if (frame.GroupId == MessageGroupIds.VIEW_INITIALIZE)
            {
                // 其他客户端连接上服务器时， 服务器会通知其加载哪些场景
                // 其他客户端，读取服务器发来的要加载哪些场景，并进行加载
                if (Networker is IServer)
					return;

				int count = frame.StreamData.GetBasicType<int>();

				loadedScenes.Clear();
				for (int i = 0; i < count; i++)
					loadedScenes.Add(frame.StreamData.GetBasicType<int>());

				MainThreadManager.Run(() =>
				{
					if (loadedScenes.Count == 0)
						return;

					SceneManager.LoadScene(loadedScenes[0], LoadSceneMode.Single);

					for (int i = 1; i < loadedScenes.Count; i++)
						SceneManager.LoadSceneAsync(loadedScenes[i], LoadSceneMode.Additive);
				});

				return;
			}


			if (frame.GroupId != MessageGroupIds.VIEW_CHANGE)
				return;

			if (Networker.IsServer)
			{
                // 客户端已经加载了这个场景
				// The client has loaded the scene
				if (playerLoadedScene != null)
					playerLoadedScene(player, Networker);

				return;
			}

            // 我们需要暂停创建网络对象直到我们加载场景
            // We need to halt the creation of network objects until we load the scene
            Networker.PendCreates = true;

            // 获取服务器加载的场景索引
            // Get the scene index that the server loaded
            int sceneIndex = frame.StreamData.GetBasicType<int>();

			// Get the mode in which the server loaded the scene
			int modeIndex = frame.StreamData.GetBasicType<int>();

			// Convert the int mode to the enum mode
			LoadSceneMode mode = (LoadSceneMode)modeIndex;

			if (networkSceneChanging != null)
				networkSceneChanging(sceneIndex, mode);

			MainThreadManager.Run(() =>
			{
				// Load the scene that the server loaded in the same LoadSceneMode
				if (mode == LoadSceneMode.Additive)
					SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
				else if (mode == LoadSceneMode.Single)
					SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
			});
		}

        /// <summary>
        /// A wrapper around the various raw send methods for the client and server types
        /// </summary>
        /// <param name="networker">The networker that is going to be sending the data</param>
        /// <param name="frame">The frame that is to be sent across the network</param>
        /// <param name="targetPlayer">The player to send the frame to, if null then will send to all</param>
        /// <summary>
        ///用于客户端和服务器类型的各种原始发送方法的包装
        /// </ summary>
        /// <param name =“networker”>将要发送数据的网络设备</ param>
        /// <param name =“frame”>要通过网络发送的帧</ param>
        /// <param name =“targetPlayer”>发送帧的播放器，如果为null，则发送给所有</ param>
        public static void SendFrame(NetWorker networker, FrameStream frame, NetworkingPlayer targetPlayer = null)
		{
			if (networker is IServer)
			{
				if (targetPlayer != null)
				{
					if (networker is TCPServer)
						((TCPServer)networker).SendToPlayer(frame, targetPlayer);
					else
						((UDPServer)networker).Send(targetPlayer, frame, true);
				}
				else
				{
					if (networker is TCPServer)
						((TCPServer)networker).SendAll(frame);
					else
						((UDPServer)networker).Send(frame, true);
				}
			}
			else
			{
				if (networker is TCPClientBase)
					((TCPClientBase)networker).Send(frame);
				else
					((UDPClient)networker).Send(frame, true);
			}
		}

		private void SceneReady(Scene scene, LoadSceneMode mode)
		{
			// If we are loading a completely new scene then we will need
			// to clear out all the old objects that were stored as they
			// are no longer needed
			if (mode != LoadSceneMode.Additive)
			{
				pendingObjects.Clear();
				pendingNetworkObjects.Clear();
				loadedScenes.Clear();
			}

			loadedScenes.Add(scene.buildIndex);

			if (networkSceneLoaded != null)
				networkSceneLoaded(scene, mode);

			BMSByte data = ObjectMapper.BMSByte(scene.buildIndex, (int)mode);

			Binary frame = new Binary(Networker.Time.Timestep, false, data, Networker is IServer ? Receivers.All : Receivers.Server, MessageGroupIds.VIEW_CHANGE, Networker is BaseTCP);

            //将二进制帧发送到服务器或客户端
            // Send the binary frame to either the server or the clients
            SendFrame(Networker, frame);

            //按照Unity找到的顺序遍历当前的所有NetworkBehavior
            //并将它们与网络将作为查询给予他们的id相关联
            // Go through all of the current NetworkBehaviors in the order that Unity finds them in
            // and associate them with the id that the network will be giving them as a lookup
            int currentAttachCode = 1;
			var behaviors = FindObjectsOfType<NetworkBehavior>().Where(b => !b.Initialized)
				.OrderBy(b => b.GetType().ToString())
				.OrderBy(b => b.name)
				.OrderBy(b => Vector3.Distance(Vector3.zero, b.transform.position))
				.ToList();

			if (behaviors.Count == 0)
			{
				if (Networker is IClient)
					NetworkObject.Flush(Networker);

				return;
			}

			foreach (NetworkBehavior behavior in behaviors)
			{
				behavior.TempAttachCode = scene.buildIndex << 16;
				behavior.TempAttachCode += currentAttachCode++;
				behavior.TempAttachCode = -behavior.TempAttachCode;
			}

			if (Networker is IClient)
			{
				NetworkObject.Flush(Networker);

				NetworkObject foundNetworkObject;
				for (int i = 0; i < behaviors.Count; i++)
				{
					if (pendingNetworkObjects.TryGetValue(behaviors[i].TempAttachCode, out foundNetworkObject))
					{
						behaviors[i].Initialize(foundNetworkObject);
						pendingNetworkObjects.Remove(behaviors[i].TempAttachCode);
						behaviors.RemoveAt(i--);
					}
				}

				if (behaviors.Count == 0)
					return;
			}

			if (Networker is IServer)
			{
                // 如果是服务器， 就给这些行为初始化他们的网络对象
				// Go through all of the pending NetworkBehavior objects and initialize them on the network
				foreach (INetworkBehavior behavior in behaviors)
					behavior.Initialize(Networker);

				return;
			}

            // 客户端，剩余没有网络对象的行为，就添加到等待字典
			foreach (NetworkBehavior behavior in behaviors)
				pendingObjects.Add(behavior.TempAttachCode, behavior);

            // 如果等待的网络对象字典已经空了，就不在监听网络对象创建
			if (pendingNetworkObjects.Count == 0)
				Networker.objectCreated -= CreatePendingObjects;
		}
	}
}