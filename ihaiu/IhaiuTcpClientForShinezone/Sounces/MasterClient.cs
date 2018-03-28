using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/16/2018 2:07:09 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class MasterClient
    {
        private const int PING_INTERVAL = 10000;

        public bool IsRunning { get; private set; }

        public ProgramSetting parameter = new ProgramSetting();
        internal ShinezoneAccount   account;
        internal ShinezoneLogin     accountLogin;

        internal HTcpClient     tcpClient;
        internal ProtoMaster     protoMaster;
        internal AuthObj        authObj = new AuthObj();

        public MasterClient()
        {
            //Connect(host, port);
        }

        public void Init(ProgramSetting parameter = null)
        {
            if (parameter == null) parameter = this.parameter;
            this.parameter = parameter;
            account = new ShinezoneAccount(parameter.url, parameter.gameId, parameter.channel);
            accountLogin = new ShinezoneLogin(account);

            accountLogin.loginSuccessEvent += (string res) =>
            {
                accountLogin.EnterGame(parameter.masterServerId, parameter.masterServerIp, parameter.masterServerPort);
            };

            accountLogin.loginEnterEvent += (ulong accountId, string accountName, string session) =>
            {
                parameter.accountId = accountId;
                parameter.session = session;
                authObj.from_web_M = session;
                ConnectMaster(parameter.masterServerIp, parameter.masterServerPort);
            };
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login(string username = "warserver01", string password = "")
        {
            accountLogin.Login(username, password);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
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

        private void OnConnectAttemptFailed(NetWorker tcpClient)
        {
            Log("OnConnectAttemptFailed");
        }

        private void OnBindSuccessful(NetWorker tcpClient)
        {
            Log("OnBindSuccessful");
           // protoLogin.C_Ping_1();
        }

        private void OnPlayerConnected(NetworkingPlayer server, NetWorker tcpClient)
        {
            Log("OnPlayerConnected");
        }


        private void OnDisconnected(NetWorker tcpClient)
        {
            Log("OnDisconnected");
        }

        private void OnForcedDisconnect(NetWorker tcpClient)
        {
            Log("OnDisconnected");
        }
        private void OnPlayerDisconnected(NetworkingPlayer server, NetWorker tcpClient)
        {
            Log("OnPlayerDisconnected");
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
            IsRunning = false;
        }
    }
}
