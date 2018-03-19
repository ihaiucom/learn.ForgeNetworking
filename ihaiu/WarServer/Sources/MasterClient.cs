using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Threading;
using ihaiu;
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
namespace ihaiu
{
    public class MasterClient
    {
        private const int PING_INTERVAL = 10000;

        public bool IsRunning { get; private set; }

        public MasterClientParameter parameter = new MasterClientParameter();

        internal HTcpClient     tcpClient;
        internal ProtoMaster     protoMaster;

        public MasterClient()
        {
            //Connect(host, port);
        }

        public void Init(MasterClientParameter parameter = null)
        {
            if (parameter == null) parameter = this.parameter;
            this.parameter = parameter;
           
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Connect(string host, ushort port)
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
        private void StarPing()
        {
            Log("StarPing");
            Task.Queue(() =>
            {
                Log("BeginPing");
                while (tcpClient.IsBound)
                {
                    protoMaster.C_Ping_1();
                    Thread.Sleep(PING_INTERVAL);
                }

                Log("EndPing");
            }, PING_INTERVAL);
        }


        private void OnConnectAttemptFailed(NetWorker tcpClient)
        {
            Log("OnConnectAttemptFailed");
        }

        private void OnBindSuccessful(NetWorker tcpClient)
        {
            Log("OnBindSuccessful");
            StarPing();
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
