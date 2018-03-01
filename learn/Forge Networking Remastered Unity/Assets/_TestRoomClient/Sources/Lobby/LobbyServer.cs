using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 2:52:10 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class LobbyServer : LobbyBase
    {
        public LobbyServer(int connections, string hostAddress = "0.0.0.0", ushort port = 16000)
        {
            Socket = new UDPServer(connections);

            Socket.binaryMessageReceived    += OnBinaryMessageReceived;

            Socket.playerConnected          += OnPlayerConnected;
            Socket.playerAccepted           += OnPlayerAccepted;
            Socket.playerRejected           += OnPlayerRejected;
            Socket.playerDisconnected       += OnPlayerDisconnected;

            ((UDPServer)Socket).Connect(hostAddress, port);
        }

        // 客户端连接上
        private void OnPlayerConnected(NetworkingPlayer player, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnPlayerConnected {0}", player.NetworkId);
        }

        // 客户端验证通过
        private void OnPlayerAccepted(NetworkingPlayer player, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnPlayerAccepted {0}", player.NetworkId);
        }

        // 客户端验证没通过
        private void OnPlayerRejected(NetworkingPlayer player, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnPlayerRejected {0}", player.NetworkId);
        }

        // 客户端断开连接
        private void OnPlayerDisconnected(NetworkingPlayer player, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnPlayerDisconnected {0}", player.NetworkId);
        }

        // 接收二进制数据
        private void OnBinaryMessageReceived (NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnBinaryMessageReceived {0}", player.NetworkId);
        }

    }
}
