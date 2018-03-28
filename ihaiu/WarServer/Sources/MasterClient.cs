using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Threading;
using ihaiu;
using Rooms.Forge.Networking;
using System;
using System.Collections.Generic;
using System.Threading;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/16/2018 2:07:09 PM
*  @Description:    
* ==============================================================================
*/
namespace WarServers
{
    public class MasterClient
    {
        private const int PING_INTERVAL = 10000;

        public bool IsRunning { get; private set; }

        public ProgramSetting parameter = new ProgramSetting();

        internal HTcpClient  tcpClient;
        internal ProtoMaster protoMaster;
        internal LobbyServer lobbyServer;


        public MasterClient()
        {
        }

        public void Init(ProgramSetting parameter = null)
        {
            if (parameter == null) parameter = this.parameter;
            this.parameter = parameter;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void ConnectMaster()
        {
            ConnectMaster(parameter.masterServerIp, parameter.masterServerPort);
        }

        public void ConnectMaster(string host, ushort port)
        {
            tcpClient = new HTcpClient();
            tcpClient.connectAttemptFailed += OnConnectAttemptFailed;
            tcpClient.bindSuccessful += OnBindSuccessful;
            tcpClient.playerConnected += OnPlayerConnected;
            tcpClient.disconnected += OnDisconnected;
            tcpClient.forcedDisconnect += OnForcedDisconnect;
            tcpClient.playerDisconnected += OnPlayerDisconnected;
            protoMaster = new ProtoMaster(this);
            tcpClient.Connect(host, port);
        }

        // 启动Ping
        private void StarMasterPing()
        {
            Log("MasterClient StarPing");
            Task.Queue(() =>
            {
                Log("MasterClient BeginPing");
                while (tcpClient.IsBound)
                {
                    protoMaster.C_Ping();
                    Thread.Sleep(PING_INTERVAL);
                }

                Log("MasterClient EndPing");
            }, 0);
        }

        /// <summary>
        /// 启动大厅服
        /// </summary>
        public void StarLobbyServer()
        {
            lobbyServer = new LobbyServer(int.MaxValue);
            lobbyServer.StageFactory = new StageFactory();
            lobbyServer.roomOver += OnRoomOver;
            lobbyServer.Connect(parameter.lobbyServerIp, parameter.lobbyServerPort);
            protoMaster.C_RegNewBattleServer(parameter.lobbyServerIp, parameter.lobbyServerPort);
        }

        /// <summary>
        /// 房间结束
        /// </summary>
        private void OnRoomOver(NetRoomBase room)
        {
            protoMaster.C_RoomEnd(room.roomId);
        }


        private void OnConnectAttemptFailed(NetWorker tcpClient)
        {
            Log("MasterClient OnConnectAttemptFailed");
        }

        private void OnBindSuccessful(NetWorker tcpClient)
        {
            Log("MasterClient OnBindSuccessful");
            StarLobbyServer();
            StarMasterPing();
        }

        private void OnPlayerConnected(NetworkingPlayer server, NetWorker tcpClient)
        {
            Log("MasterClient OnPlayerConnected");
        }


        private void OnDisconnected(NetWorker tcpClient)
        {
            Log("MasterClient OnDisconnected");
        }

        private void OnForcedDisconnect(NetWorker tcpClient)
        {
            Log("MasterClient OnDisconnected");
        }

        private void OnPlayerDisconnected(NetworkingPlayer server, NetWorker tcpClient)
        {
            Log("MasterClient OnPlayerDisconnected");
        }
        


        private bool _logging;
        public bool ToggleLogging()
        {
            _logging = !_logging;
            return _logging;
        }

        private void Log(object message)
        {
            //if (!_logging)
            //    return;

            Console.WriteLine(message);
        }

        public void Dispose()
        {
            tcpClient.Disconnect(true);
            lobbyServer.Dispose();
            IsRunning = false;
        }
    }
}
