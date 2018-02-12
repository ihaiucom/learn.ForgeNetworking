using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Threading;
using BeardedManStudios.SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System;
using ihaiu;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/11/2018 11:15:27 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// 房间管理服
    /// </summary>

    public class MasterServer
    {
        private const int PING_INTERVAL = 10000;

        public bool IsRunning { get; private set; }
        private HTcpServer server;
        private List<Host> hosts = new List<Host>();
        private Dictionary<string, int> _playerRequests = new Dictionary<string, int>();
        private bool _eloRangeSet;
        private int _eloRange;
        public int EloRange
        {
            get { return _eloRange; }
            set
            {
                if (value == 0)
                    _eloRangeSet = false;
                else
                    _eloRangeSet = true;
                _eloRange = value;
            }
        }

        private bool _logging;
        public bool ToggleLogging()
        {
            _logging = !_logging;
            return _logging;
        }

        private void Log(object message)
        {
            if (!_logging)
                return;

            Console.WriteLine(message);
        }

        public MasterServer(string host, ushort port)
        {
            server = new HTcpServer(2048);
            server.Connect(host, port);

            IsRunning = true;
            // 停服
            server.disconnected += (sender) =>
            {
                IsRunning = false;
            };

            // 游戏服 断开连接
            server.playerDisconnected += (player, sender) =>
            {
                for (int i = 0; i < hosts.Count; i++)
                {
                    if (hosts[i].Player == player)
                    {
                        Log($"Host [{hosts[i].Address}] on port [{hosts[i].Port}] with name [{hosts[i].Name}] removed");
                        hosts.RemoveAt(i);
                        return;
                    }
                }
            };

            Task.Queue(() =>
            {
                while (server.IsBound)
                {
                    // 每隔一段时间,向所有游戏服 发送一次ping
                    server.SendAll(server.GenerateProtoPing());
                    Thread.Sleep(PING_INTERVAL);
                }
            }, PING_INTERVAL);
        }


        public void Dispose()
        {
            server.Disconnect(true);
            IsRunning = false;
        }
    }
}
