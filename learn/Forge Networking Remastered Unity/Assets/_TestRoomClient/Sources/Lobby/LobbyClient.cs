using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 2:52:19 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class LobbyClient : LobbyBase
    {
        public LobbyClient(string hostAddress = "127.0.0.1", ushort port = 16000)
        {
            Socket = new UDPClient();

            Socket.binaryMessageReceived += OnBinaryMessageReceived;

            Socket.serverAccepted   += OnServerAccepted;
            Socket.disconnected     += OnDisconnected;



            ((UDPClient)Socket).Connect(hostAddress, port);
        }

        // 断开连接
        private void OnDisconnected(NetWorker sender)
        {
            Loger.LogFormat("LobbyClient OnDisconnected {0}", sender);
        }

        // 服务器接收连接了
        private void OnServerAccepted(NetWorker sender)
        {
            Loger.LogFormat("LobbyClient OnServerAccepted {0}", sender);
        }



        // 接收二进制数据
        private void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            Loger.LogFormat("LobbyClient OnBinaryMessageReceived {0}", player.NetworkId);
        }
    }
}
