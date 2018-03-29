using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrueSync;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Games.Wars
{

    public class EventCode
    {
        public const byte Proto = 1;
    }

    public enum WarPhotonServerSelect
    {
        Ihaiu,
        CloudCN,
        CloudEn,
    }

    public class WarPhotonServerSetting
    {
        public ServerSettings.HostingOption HostType = ServerSettings.HostingOption.PhotonCloud;

        [Header("Cloud")]
        public string NameServerHost = "ns-cn.exitgames.com";
        public string AppID = "e1c97d53-7348-4525-999f-de4d5a6683ce";
        public CloudRegionCode RegionCode = CloudRegionCode.cn;

        public string Version = "v1.0";

        [Header("Server")]
        public string ServerAddress = "mbqb.ihaiu.com";
        public int ServerPort = 5055;

        public static WarPhotonServerSetting Ihaiu = new WarPhotonServerSetting()
        {
            HostType = ServerSettings.HostingOption.SelfHosted,
        };
        public static WarPhotonServerSetting CN = new WarPhotonServerSetting();
        public static WarPhotonServerSetting EN = new WarPhotonServerSetting()
        {
            NameServerHost = "ns.exitgames.com",
            AppID = "30a0e169-7d2e-48af-ae00-a22421ff58cf",
            RegionCode = CloudRegionCode.eu
        };
    }

    public class WarPhotonRoom : PunBehaviour
    {
        [Header("Server Select")]
        public static WarPhotonServerSelect ServerSelect = WarPhotonServerSelect.CloudCN;
        public static WarPhotonServerSetting ServerSetting
        {
            get
            {
                switch (ServerSelect)
                {
                    case WarPhotonServerSelect.CloudCN:
                        return WarPhotonServerSetting.CN;
                    case WarPhotonServerSelect.CloudEn:
                        return WarPhotonServerSetting.EN;
                    case WarPhotonServerSelect.Ihaiu:
                        return WarPhotonServerSetting.Ihaiu;
                }

                return WarPhotonServerSetting.CN;
            }
        }

        [Header("Init Setting")]
        public static bool AutoJoinLobby = false;
        public static bool EnableLobbyStatistics = true;
        public static bool RunInBackground = true;


        [Header("Lobby")]
        public static string    LobbyName = "Lobby_MBQB";


        [Header("Room")]
        public static string    RoomName        = "Room_Test";
        public static string    RoomPlugins     = "";
        public static byte      RoomMaxPlayer   = 0;
        public static Action    EventJoinRoom;

        private static Action PunEventOnConnectToMaster;
        private static Action PunEventOnJoinedLobby;
        private static Action PunEventOnJoinedRoom;


        [Header("Install")]
        internal static WarPhotonRoom   install;
        internal static GameObject      go;

        public static WarRoom room;

        // 安装
        public static void Install(WarRoom room)
        {
            WarPhotonRoom.room = room;

            if (go != null)
            {
                return;
            }

            go = new GameObject("WarPhoton");
            install = go.AddComponent<WarPhotonRoom>();
            GameObject.DontDestroyOnLoad(go);

            InitSetting();
        }

        // 卸载
        public static void Unstall()
        {
            GameObject go = GameObject.Find("WarPhoton");
            if (go != null)
            {
                GameObject.DestroyImmediate(go);
            }
        }

        // 是否是 Master
        public static bool IsMaster
        {
            get
            {
                return PhotonNetwork.isMasterClient;
            }
        }

        // 设置房间 游戏状态
        public static void SetRoomState(TSRoomState state)
        {
            Hashtable setting = new Hashtable();
            setting[TSRoomPropertiesKey.State] = state;
            PhotonNetwork.room.SetCustomProperties(setting);
        }

        // 获取房间状态
        public static TSRoomState GetRoomState()
        {
            TSRoomState state = TSRoomState.None;
            object obj;
            if (PhotonNetwork.room.CustomProperties.TryGetValue(TSRoomPropertiesKey.State, out obj))
            {
                state = (TSRoomState)obj;
            }
            return state;
        }

        // 获取房间是否已经开始游戏
        public static bool GetRoomIsGameing()
        {
            return GetRoomState() == TSRoomState.Gameing;
        }


        public static void SetPlayerProperties()
        {
            PhotonNetwork.playerName = room.enterData.ownLegion.roleInfo.roleName;
            Hashtable legion = new Hashtable();
            legion[TSPlayerPropertiesKey.LegionId] = room.enterData.ownLegion.legionId;
            legion[TSPlayerPropertiesKey.RoleId] = room.enterData.ownLegion.roleInfo.roleId;
            PhotonNetwork.player.SetCustomProperties(legion);
        }

        // 第一步 初始化设置
        public static void InitSetting()
        {
            NetworkingPeer.NameServerHost = ServerSetting.NameServerHost;
            PhotonNetwork.PhotonServerSettings.HostType = ServerSetting.HostType;
            PhotonNetwork.PhotonServerSettings.AppID = ServerSetting.AppID;
            PhotonNetwork.PhotonServerSettings.ServerAddress = ServerSetting.ServerAddress;
            PhotonNetwork.PhotonServerSettings.ServerPort = ServerSetting.ServerPort;
            PhotonNetwork.PhotonServerSettings.PreferredRegion = ServerSetting.RegionCode;



            PhotonNetwork.autoCleanUpPlayerObjects = false;
            PhotonNetwork.autoJoinLobby = AutoJoinLobby;
            PhotonNetwork.EnableLobbyStatistics = EnableLobbyStatistics;
            Application.runInBackground = RunInBackground;

            PhotonNetwork.lobby.Name = LobbyName;

        }

        // 第二步 连接到Master服务器
        public static void ConnectToMaster()
        {
            PunEventOnConnectToMaster = OnPunConnectedToMaster;
            PunEventOnJoinedLobby = OnPunJoinedLobby;
            PunEventOnJoinedRoom = OnPunJoinedRoom;

            if (!room.isNetModel)
            {
                OfflineMode();
            }
            else
            {
                ConnectToServerMode();
            }
        }


        public static void ConnectToMasterServer()
        {
            switch (ServerSelect)
            {
                case WarPhotonServerSelect.Ihaiu:
                    PhotonNetwork.ConnectToMaster(ServerSetting.ServerAddress, ServerSetting.ServerPort, ServerSetting.AppID, ServerSetting.Version);
                    break;
                default:
                    PhotonNetwork.ConnectUsingSettings(ServerSetting.Version);
                    break;
            }
        }

        public static void ConnectToServerMode()
        {

            if (PhotonNetwork.offlineMode)
            {
                ConnectToMasterServer();
                return;
            }

            switch (PhotonNetwork.connectionStateDetailed)
            {
                case ClientState.ConnectedToMaster:
                    install.OnConnectedToMaster();
                    break;
                case ClientState.JoinedLobby:
                    install.OnJoinedLobby();
                    break;
                case ClientState.Joined:
                    install.OnJoinedRoom();
                    break;
                default:
                    ConnectToMasterServer();
                    break;
            }
        }

        public static void OfflineMode()
        {
            if (PhotonNetwork.offlineMode == false)
            {
                if (PhotonNetwork.connectedAndReady)
                    PhotonNetwork.Disconnect();

            }
            PhotonNetwork.offlineMode = true;

            Loger.Log("## WarPhotonRoom OfflineMode PhotonNetwork.connectionStateDetailed=" + PhotonNetwork.connectionStateDetailed);
            switch (PhotonNetwork.connectionStateDetailed)
            {
                case ClientState.ConnectedToMaster:
                    install.OnConnectedToMaster();
                    break;
                case ClientState.Joined:
                    install.OnJoinedRoom();
                    break;

            }
        }


        // 【消息】 连接上 Master
        public static void OnPunConnectedToMaster()
        {
            if (!PhotonNetwork.offlineMode)
            {
                JoinLobby();
            }
            else
            {
                JoinOrCreateRoom();
            }
        }

        // 第三步 加入到大厅
        public static void JoinLobby()
        {
            PhotonNetwork.JoinLobby(PhotonNetwork.lobby);
        }


        // 【消息】 加入到 大厅
        public static void OnPunJoinedLobby()
        {
            JoinOrCreateRoom();
        }

        // 第四步 加入到房间
        public static void JoinOrCreateRoom()
        {
            Loger.Log("## WarPhotonRoom JoinOrCreateRoom");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = RoomMaxPlayer;
            if (!string.IsNullOrEmpty(RoomPlugins))
                roomOptions.Plugins = new string[] { RoomPlugins };

            TypedLobby typedLobby = TypedLobby.Default;
            typedLobby.Name = LobbyName;
            string[] expectedUsers = new string[] { };

            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, typedLobby, expectedUsers);
        }



        // 【消息】 加入到 房间
        public static void OnPunJoinedRoom()
        {
            Loger.Log("## WarPhotonRoom OnPunJoinedRoom connectionStateDetailed=" + PhotonNetwork.connectionStateDetailed);
            if (EventJoinRoom != null)
            {
                EventJoinRoom();
            }
        }

        // 离开房间
        public static void LeaveRoom()
        {
            Loger.Log("## WarPhotonRoom LeaveRoom");

            PunEventOnConnectToMaster = null;
            PunEventOnJoinedLobby = null;
            PunEventOnJoinedRoom = null;

            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }


        // 创建主基地
        public static void CreateUnitMainbase()
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            for (int i = 0, count = room.enterData.mainbaseList.Count; i < count; i++)
            {
                WarEnterMainbaseData mainbase = room.enterData.mainbaseList[i];
                WarEnterUnitData item = mainbase.unit;

                object[] data = new object[7];
                data[CreatePrefabPropertiesKey.TypeKey] = UnitProduceType.Normal;
                data[CreateUnitPropertiesKey.UnitType] = UnitType.Build;
                data[CreateUnitPropertiesKey.LegionId] = mainbase.legionId;
                data[CreateUnitPropertiesKey.Rotation] = mainbase.rotation;
                data[CreateUnitPropertiesKey.BuildType] = UnitBuildType.Mainbase;
                data[CreateUnitPropertiesKey.MainbaseIndex] = i;
                data[CreateUnitPropertiesKey.TowerAvatarId] = item.avatarId;

                Vector3 position = mainbase.position;
                PhotonNetwork.InstantiateSceneObject(WarRes.WAR_PREFAB_UNIT_BUILD, position, Quaternion.identity, 0, data);
            }
        }

        // 创建场景控制器
        public static void CreateUnitScne()
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            PhotonNetwork.InstantiateSceneObject(WarRes.WAR_PREFAB_PUN_Scene, Vector3.zero, Quaternion.identity, 0, null);
        }



        // 创建玩家
        public static void CreateUnitPlayer(int legionId)
        {
            object[] data = new object[4];
            data[CreatePrefabPropertiesKey.TypeKey] = UnitProduceType.Normal;
            data[CreateUnitPropertiesKey.UnitType] = UnitType.Player;
            data[CreateUnitPropertiesKey.LegionId] = legionId;
            data[CreateUnitPropertiesKey.Rotation] = Vector3.zero;
            PhotonNetwork.Instantiate(WarRes.WAR_PREFAB_PUN_PLAYER, Vector3.zero, Quaternion.identity, 0, data);
        }

        // 创建英雄
        public static void CreateUnitHero(int legionId)
        {
            WarEnterLegionData legion = room.enterData.GetLegion(legionId);
            Vector3 position = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerPosition();
            Vector3 rotation = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerRotation();

            object[] data = new object[4];
            data[CreatePrefabPropertiesKey.TypeKey] = UnitProduceType.Normal;
            data[CreateUnitPropertiesKey.UnitType] = UnitType.Hero;
            data[CreateUnitPropertiesKey.LegionId] = legionId;
            data[CreateUnitPropertiesKey.Rotation] = rotation;

            PhotonNetwork.Instantiate(WarRes.WAR_PREFAB_UNIT_HERO, position, Quaternion.identity, 0, data);
        }

        // 创建士兵
        public static void CreateUnitSolider(int legionId, int routeId, int unitId, int unitLevel, UnitProduceType produceType = UnitProduceType.Normal)
        {
            Vector3 position = room.sceneData.GetRouteBeginPoint(routeId);
            Vector3 rotation = room.sceneData.GetRouteBeginDirection(routeId);

            object[] data = new object[7];
            data[CreatePrefabPropertiesKey.TypeKey] = produceType;
            data[CreateUnitPropertiesKey.UnitType] = UnitType.Solider;
            data[CreateUnitPropertiesKey.LegionId] = legionId;
            data[CreateUnitPropertiesKey.Rotation] = rotation;
            data[CreateUnitPropertiesKey.SoliderRouteId] = routeId;
            data[CreateUnitPropertiesKey.SoliderUnitId] = unitId;
            data[CreateUnitPropertiesKey.SoliderUnitLevel] = unitLevel;


            PhotonNetwork.InstantiateSceneObject(WarRes.WAR_PREFAB_UNIT_SOLIDER, position, Quaternion.identity, 0, data);

            if (produceType == UnitProduceType.Normal)
            {
                #region 怪物刷新，出生点闪烁
                StageRouteConfig display = room.sceneData.GetRouteParentRoot(routeId);
                WarUI.Instance.mUIMiniMap.OnShowFlash(display.path[0], display.routeId);
                #endregion
            }
        }

        // 创建机关
        public static void CreateUnitTower(int legionId, int cellId, int unitId, int avatarId, int unitLevel, bool isInit = false)
        {
            Vector3 position = room.sceneData.GetBuildCell(cellId).position;
            Vector3 rotation = room.sceneData.GetBuildCell(cellId).rotation;


            object[] data = new object[10];
            data[CreatePrefabPropertiesKey.TypeKey] = UnitProduceType.Normal;
            data[CreateUnitPropertiesKey.UnitType] = UnitType.Build;
            data[CreateUnitPropertiesKey.LegionId] = legionId;
            data[CreateUnitPropertiesKey.Rotation] = rotation;
            data[CreateUnitPropertiesKey.BuildType] = UnitBuildType.Tower;
            data[CreateUnitPropertiesKey.TowerIsInit] = isInit;
            data[CreateUnitPropertiesKey.TowerCellUid] = cellId;
            data[CreateUnitPropertiesKey.TowerUnitId] = unitId;
            data[CreateUnitPropertiesKey.TowerUnitLevel] = unitLevel;



            PhotonNetwork.InstantiateSceneObject(WarRes.WAR_PREFAB_UNIT_BUILD, position, Quaternion.identity, 0, data);
        }



        // 创建初始机关
        public static void GenerateBuildCellInitUnit()
        {
            int count = room.stageConfig.buildCellList.Count;

            for (int i = 0; i < count; i++)
            {
                StageBuildCellConfig cell = room.stageConfig.buildCellList[i];
                StageBuildCellUnitConfig item = cell.initUnit;
                if (item.HasSetting)
                {
                    CreateUnitTower(item.legionId, cell.uid, item.unitId, item.avatarId, item.unitLevel, true);
                }
            }


        }

        // 克隆单位
        public static GameObject CloneUnit(int legionId, int mainUid, UnitConfig unitConfig, Vector3 position, Vector3 rotation, float life, UnitProduceType produceType = UnitProduceType.Clone, int weaponId = 0)
        {

            object[] data = new object[7];
            data[CreatePrefabPropertiesKey.TypeKey] = produceType;
            data[CloneUnitPropertiesKey.MainUid] = mainUid;
            data[CloneUnitPropertiesKey.AvatarId] = unitConfig.avatarId;
            data[CloneUnitPropertiesKey.Rotation] = rotation;
            data[CloneUnitPropertiesKey.UnitId] = unitConfig.unitId;
            data[CloneUnitPropertiesKey.LifeTime] = (int)(life * 100);
            data[CloneUnitPropertiesKey.WeaponId] = weaponId;

            string prefabName = WarRes.WAR_PREFAB_UNIT_HERO;
            switch (unitConfig.unitType)
            {
                case UnitType.Hero:
                    prefabName = WarRes.WAR_PREFAB_UNIT_HERO;
                    break;
            }

            GameObject unitObj = PhotonNetwork.InstantiateSceneObject(prefabName, position, Quaternion.identity, 0, data);
            return unitObj;
        }


        // 【消息】 连接上 Master
        public override void OnConnectedToMaster()
        {
            if (PunEventOnConnectToMaster != null)
                PunEventOnConnectToMaster();
        }

        // 【消息】 加入到 大厅
        public override void OnJoinedLobby()
        {
            if (PunEventOnJoinedLobby != null)
                PunEventOnJoinedLobby();
        }


        // 【消息】 加入到 房间
        public override void OnJoinedRoom()
        {
            if (PunEventOnJoinedRoom != null)
            {
                PunEventOnJoinedRoom();
            }
        }

        // 【消息】 玩家离线
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            LegionData legionData = room.sceneData.GetLegion(otherPlayer.GetLegionId());
            if (legionData != null)
            {
                name = legionData.roleName;
                legionData.isOnline = false;
                if (room.IsGameing)
                    CsharpCallLuaFun.ShowToastMsg(name + " offline");
            }

        }


        // 创建帧同步
        public static void CreateTrueSync()
        {
            TrueSyncManager trueSyncManager = go.AddComponent<TrueSyncManager>();
            trueSyncManager.playerPrefabs = new GameObject[] { room.clientRes.GetAsset<GameObject>(WarRes.WAR_PREFAB_TS_Player) };
        }




        private void Start()
        {
            PhotonNetwork.OnEventCall += OnEvent;
        }

        private void OnDestroy()
        {
            PhotonNetwork.OnEventCall -= OnEvent;
        }

        private void Update()
        {

        }


        /** [协议] 发送消息 */
        public static void SendProtoMsg<T>(T protoMsg)
        {
            Type type = typeof(T);
            IProtoItem item = ProtoC.GetItemByType(type);

            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, protoMsg);
            stream.Position = 0;


            RaiseEventOptions op = new RaiseEventOptions();
            op.CachingOption = EventCaching.DoNotCache;
            op.Receivers = ReceiverGroup.All;

            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
            ht.Add("opcode", item.opcode);
            ht.Add("data", stream.ToArray());
            PhotonNetwork.RaiseEvent(EventCode.Proto, ht, true, op);

            stream.Dispose();
        }

        public void OnEvent(byte eventCode, object content, int senderId)
        {
            if (eventCode == EventCode.Proto)
            {
                ExitGames.Client.Photon.Hashtable ht = (ExitGames.Client.Photon.Hashtable) content;
                int opcode = (int)ht["opcode"];
                if (opcode == 16031) opcode = 16032;
                byte[] bytes = (byte[])ht["data"];

                IProtoItem item = ProtoS.GetItemByOpcode(opcode);
                if (item != null && item.hasListen)
                {
                    MemoryStream stream = new MemoryStream(bytes);
                    item.Handle(stream);
                }
            }
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            base.OnFailedToConnectToPhoton(cause);

            StartCoroutine(DelayReconnect());
        }

        IEnumerator DelayReconnect()
        {
            yield return new WaitForSeconds(1);
            if (ServerSelect == WarPhotonServerSelect.CloudCN)
            {
                ServerSelect = WarPhotonServerSelect.Ihaiu;
            }
            else if (ServerSelect == WarPhotonServerSelect.Ihaiu)
            {
                ServerSelect = WarPhotonServerSelect.CloudEn;
            }

            InitSetting();
            ConnectToMaster();
        }




    }
}