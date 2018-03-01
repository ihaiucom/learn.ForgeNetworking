using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 4:05:58 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class NetRoomServer : NetRoomBase
    {

        // 玩家字典
        public Dictionary<int, NetRoomPlayer>   playerDict = new Dictionary<int, NetRoomPlayer>();

        // 观战玩家列表
        public List<NetworkingPlayer>           watchPlayers = new List<NetworkingPlayer>();

        public NetRoomServer(LobbyServer lobby, NetRoomInfo roomInfo)
        {
            this.lobby      = lobby;
            this.uid        = roomInfo.roomUid;
            this.stageId    = roomInfo.stageId;
        }

        /// <summary>
        /// 玩家, 加入房间
        /// </summary>
        public NetJoinRoomResult JoinRoom(int roleUid, NetworkingPlayer networkingPlayer, Binary frame)
        {
            NetRoomPlayer player;
            if (playerDict.TryGetValue(roleUid, out player))
            {
                player.networkingPlayer = networkingPlayer;
                return NetJoinRoomResult.Existed;
            }
            else
            {
                player = new NetRoomPlayer(roleUid, networkingPlayer);
                playerDict.Add(roleUid, player);
                return NetJoinRoomResult.Successed;
            }
        }

        /// <summary>
        /// 玩家, 离开房间
        /// </summary>
        public NetLeftRoomResult LeftRoom(int roleUid, NetworkingPlayer networkingPlayer)
        {
            NetRoomPlayer player;
            if (playerDict.TryGetValue(roleUid, out player))
            {
                if (networkingPlayer != null && networkingPlayer != player.networkingPlayer)
                {
                    // 不是同一个终端
                    return NetLeftRoomResult.Failed_NoSameSocket;
                }

                playerDict.Remove(roleUid);
                return NetLeftRoomResult.Successed;
            }

            // 不存在该玩家
            return NetLeftRoomResult.Failed_RoomNoPlayer;
        }



        /// <summary>
        /// 玩家, 加入观看房间
        /// </summary>
        public void JoinWatchRoom(NetworkingPlayer networkingPlayer)
        {
            if (!watchPlayers.Contains(networkingPlayer))
            {
                watchPlayers.Add(networkingPlayer);
            }
        }


        /// <summary>
        /// 玩家, 离开观看房间
        /// </summary>
        public void LeftWatchRoom(NetworkingPlayer networkingPlayer)
        {
            if (watchPlayers.Contains(networkingPlayer))
                watchPlayers.Remove(networkingPlayer);
        }


        // 接收二进制数据
        public void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {

        }
    }
}
