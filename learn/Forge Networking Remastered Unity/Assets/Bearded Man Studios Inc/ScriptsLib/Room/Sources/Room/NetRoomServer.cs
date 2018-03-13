using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
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
namespace Rooms.Ihaiu.Forge.Networking
{
    public class NetRoomServer : NetRoomBase
    {
        // 服务器Lobby
        internal LobbyServer serverLobby;

        // 玩家字典
        public Dictionary<ulong, NetworkingPlayer>      playerDict = new Dictionary<ulong, NetworkingPlayer>();

        // 观战玩家列表
        public List<NetworkingPlayer>                   watchPlayers = new List<NetworkingPlayer>();



        public NetRoomServer(LobbyServer lobby, NetRoomInfo roomInfo)
        {
            this.serverLobby = lobby;

            Initialize(lobby, roomInfo);
        }

        /// <summary>
        /// 玩家, 加入房间
        /// </summary>
        public NetJoinRoomResult JoinRoom(ulong roleUid, NetworkingPlayer networkingPlayer, Binary frame)
        {
            NetJoinRoomResult ret;
            if (playerDict.ContainsKey(roleUid))
            {
                playerDict.Remove(roleUid);
                playerDict.Add(roleUid, networkingPlayer);
                ret = NetJoinRoomResult.Existed;
            }
            else
            {
                playerDict.Add(roleUid, networkingPlayer);
                ret = NetJoinRoomResult.Successed;
            }

            networkingPlayer.lastRoleUid = roleUid;

            // 广播玩家加入
            BMSByte data = ObjectMapper.BMSByte(roleUid);
            Binary sendframe = new Binary(Time.Timestep, false, data, Receivers.Target, MessageGroupIds.ROOM, false, RouterIds.ROOM_JOIN_ROOM, roomId);
            Send(sendframe, true);

            OnPlayerJoinRoom(roleUid, networkingPlayer);
            OnPlayerJoined(networkingPlayer);
            return ret;
        }


        /// <summary>
        /// 玩家, 离开房间
        /// </summary>
        public NetLeftRoomResult LeftRoom(ulong roleUid, NetworkingPlayer networkingPlayer)
        {
            NetworkingPlayer player;
            if (playerDict.TryGetValue(roleUid, out player))
            {
                if (networkingPlayer != null && networkingPlayer.NetworkId != player.NetworkId)
                {
                    // 不是同一个终端
                    return NetLeftRoomResult.Failed_NoSameSocket;
                }

                playerDict.Remove(roleUid);


                // 广播玩家离开
                BMSByte data = ObjectMapper.BMSByte(roleUid);
                Binary sendframe = new Binary(Time.Timestep, false, data, Receivers.Target, MessageGroupIds.ROOM, false, RouterIds.ROOM_LEFT_ROOM, roomId);
                Send(sendframe, true);

                OnPlayerLeftRoom(roleUid, networkingPlayer);
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


        private void OnPlayerJoined(NetworkingPlayer networkingPlayer)
        {
            SendBuffer(networkingPlayer);
            scene.OnPlayerAccepted(networkingPlayer);
        }


        // 接收二进制数据
        public void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            byte routerId = frame.RouterId;

        }

        // 发送消息, 给指定玩家
        public void Send(NetworkingPlayer player, FrameStream frame, bool reliable = false)
        {
            serverLobby.Send(player, frame, reliable);
        }

        // 发送消息，给所有玩家
        public void Send(FrameStream frame, bool reliable = false)
        {
            Send(frame, reliable);
        }

        protected List<FrameStream> bufferedMessages = new List<FrameStream>();
        public void Send(FrameStream frame, bool reliable = false, NetworkingPlayer skipPlayer = null)
        {
            if (frame.Receivers == Receivers.AllBuffered || frame.Receivers == Receivers.OthersBuffered)
                bufferedMessages.Add(frame);

            lock (playerDict)
            {
                foreach (var kvp in playerDict)
                {
                    if (!PlayerIsReceiver(kvp.Value, frame, skipPlayer))
                        continue;

                    Send(kvp.Value, frame, reliable);
                }
            }


            lock (playerDict)
            {
                foreach (NetworkingPlayer player in watchPlayers)
                {
                    if (!PlayerIsReceiver(player, frame, skipPlayer))
                        continue;

                    Send(player, frame, reliable);
                }
            }
        }


        /// <summary>
        /// 发送缓存消息给这个玩家
        /// </summary>
        /// <param name="player"></param>
        private void SendBuffer(NetworkingPlayer player)
        {
            foreach (FrameStream frame in bufferedMessages)
                Send(player, frame, true);
        }

        // 检测玩家是否接收该消息
        public bool PlayerIsReceiver(NetworkingPlayer player, FrameStream frame, NetworkingPlayer skipPlayer = null)
        {
            // 不要将消息发送给尚未被服务器接受的播放器
            // Don't send messages to a player who has not been accepted by the server yet
            if ((!player.Accepted && !player.PendingAccepted) || player == skipPlayer)
                return false;

            if (player == frame.Sender)
            {
                //如果发送给其他人，则不要发送消息给发送方
                // Don't send a message to the sending player if it was meant for others
                if (frame.Receivers == Receivers.Others || frame.Receivers == Receivers.OthersBuffered || frame.Receivers == Receivers.OthersProximity)
                    return false;
            }
            

            return true;
        }
    }
}
