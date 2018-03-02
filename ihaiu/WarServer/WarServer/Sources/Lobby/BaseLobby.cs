using BeardedManStudios.Forge.Logging;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 9:45:08 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public abstract class LobbyBase
    {
        public NetWorker Socket { get; protected set; }



        private void Setup(string hostAddress, ushort port)
        {
            Socket.binaryMessageReceived += Read;

            if (Socket.IsServer)
            {
                ((UDPServer)Socket).Connect(hostAddress, port);
                Socket.playerConnected += (player, sender) => { BMSLog.Log("PLAYER CONNECTED " + player.IPEndPointHandle.Address); };
                Socket.playerAccepted += (player, sender) => { BMSLog.Log("PLAYER ACCEPTED " + player.IPEndPointHandle.Address); };
                Socket.playerRejected += (player, sender) => { BMSLog.Log("PLAYER REJECTED " + player.IPEndPointHandle.Address); };
                Socket.playerDisconnected += (player, sender) => { BMSLog.Log("PLAYER DISCONNECTED " + player.IPEndPointHandle.Address); };
                StartWar(Socket);
            }
            else
            {
                Socket.serverAccepted += StartWar;
                ((UDPClient)Socket).Connect(hostAddress, port);
            }
        }


        private void StartWar(NetWorker sender)
        {
            sender.serverAccepted -= StartWar;
        }


        private void Read(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            if (frame.GroupId != MessageGroupIds.Lobby)
                return;

        }

        public void Send(FrameStream frame, bool reliable = false)
        {
            ((BaseUDP)Socket).Send(frame, reliable);
        }




        /// <summary>
        /// 连接上大厅
        /// </summary>
        public void OnJoinedLoby()
        {

        }

        /// <summary>
        /// 离开大厅
        /// </summary>
        public void OnLeftLobby()
        {

        }

        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateRoom()
        {

        }

        /// <summary>
        /// 加入房间
        /// </summary>
        public void JoinRoom()
        {

        }

        /// <summary>
        /// 加入房间失败
        /// </summary>
        public void OnJoinRoomFailed()
        {

        }

        /// <summary>
        /// 加入到房间
        /// </summary>
        public void OnJoinedRoom()
        {

        }


        /// <summary>
        /// 离开房间
        /// </summary>
        public void OnLeftRoom()
        {

        }

        /// <summary>
        /// 玩家连接
        /// </summary>
        public void OnPlayerConnected()
        {

        }

        /// <summary>
        /// 玩家离开
        /// </summary>
        public void OnPlayerDisconnected()
        {

        }



    }
}
