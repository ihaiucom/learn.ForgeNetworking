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

            // 离开房间
            NetLeftRoomResult ret = LeftRoom(player, true);

            if(ret == NetLeftRoomResult.Failed_RoomNoPlayer)
            {
                // 如果房间没有这玩家，就离开观看房间
                LeftWatchRoom(player, true);
            }
        }


        // 发送消息给指定玩家
        public void Send(NetworkingPlayer player, FrameStream frame, bool reliable = false)
        {
            ((UDPServer)Socket).Send(player, frame, true);
        }

        // 接收二进制数据
        private void OnBinaryMessageReceived (NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            Loger.LogFormat("LobbyServer OnBinaryMessageReceived {0}", player.NetworkId);

            if(frame.RoomId != 0)
            {

                if (roomDict.ContainsKey((int)frame.RoomId))
                {
                    roomDict[(int)frame.RoomId].OnBinaryMessageReceived(player, frame, sender);
                }
                return;
            }

            if (frame.GroupId == MessageGroupIds.Lobby)
            {
                byte routerId = frame.RouterId;

                switch(routerId)
                {
                    case RouterIds.LOBBY_CREATE_ROOM:
                        CreateRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_JOIN_ROOM:
                        JoinRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_LEFT_ROOM:
                        LeftRoom(player, frame);
                        break;


                    case RouterIds.LOBBY_JOIN_WATCH_ROOM:
                        JoinWatchRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_LEFT_WATCH_ROOM:
                        LeftWatchRoom(player, frame);
                        break;
                }
            }
            else if(frame.GroupId == MessageGroupIds.ROOM)
            {
                int roomUid = frame.StreamData.GetBasicType<int>();
                if(roomDict.ContainsKey(roomUid))
                {
                    roomDict[roomUid].OnBinaryMessageReceived(player, frame, sender);
                }
            }

        }



        // 房间字典
        public Dictionary<int, NetRoomServer> roomDict = new Dictionary<int, NetRoomServer>();


        /// <summary>
        /// 创建房间
        /// </summary>
        private void CreateRoom(NetworkingPlayer player, Binary frame)
        {
            NetRoomInfo roomInfo = new NetRoomInfo();
            roomInfo.roomUid = frame.StreamData.GetBasicType<int>();
            roomInfo.stageId = frame.StreamData.GetBasicType<int>();
            CreateRoom(roomInfo, player);
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        public bool CreateRoom(NetRoomInfo roomInfo, NetworkingPlayer player = null)
        {
            if(roomDict.ContainsKey(roomInfo.roomUid))
            {
                // 创建房间失败， 房间 已经存在
                string error = string.Format("创建房间失败， 房间ID={0}, 已经存在", roomInfo.roomUid);

                OnCreateRoomFailed(roomInfo.roomUid, error);


                if (player != null)
                {
                    BMSByte data = ObjectMapper.BMSByte(false, roomInfo.roomUid, error);
                    Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_CREATE_ROOM);
                    Send(player, frame, true);
                }
                return false;
            }

            // 创建房间成功
            NetRoomServer room = new NetRoomServer(this, roomInfo);
            roomDict.Add(room.uid, room);

            OnCreateRoomSuccessed(room.uid);


            if (player != null)
            {
                BMSByte data = ObjectMapper.BMSByte(true, roomInfo.roomUid, string.Empty);
                Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_CREATE_ROOM);
                Send(player, frame, true);
            }
            return true;
        }

        

        /// <summary>
        /// 加入房间
        /// </summary>
        private void JoinRoom(NetworkingPlayer player, Binary frame)
        {
            // 房间UID
            int roomUid = frame.StreamData.GetBasicType<int>();
            // 角色UID
            int roleUid = frame.StreamData.GetBasicType<int>();

            NetJoinRoomResult ret;

            NetRoomServer room;
            if(!roomDict.TryGetValue(roomUid, out room))
            {
                // 失败 不存在该房间 
                ret = NetJoinRoomResult.Failed_NoRoom;
            }
            else
            {
                ret = room.JoinRoom(roleUid, player, frame);

                player.lastRoomUid = roomUid;
                player.lastRoleUid = roleUid;
            }

            OnPlayerJoinRoom(roomUid, player, ret);


            BMSByte data = ObjectMapper.BMSByte(roomUid, (int)ret);
            Binary sendframe = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_ROOM);
            Send(player, sendframe, true);
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        private NetLeftRoomResult LeftRoom(NetworkingPlayer player, Binary frame)
        {
            // 房间UID
            int roomUid = frame.StreamData.GetBasicType<int>();
            // 角色UID
            int roleUid = frame.StreamData.GetBasicType<int>();

            return LeftRoom(roomUid, roleUid, player);
        }

        private NetLeftRoomResult LeftRoom(NetworkingPlayer player, bool isDisconnected = false)
        {
            return LeftRoom(player.lastRoomUid, player.lastRoleUid, player, isDisconnected);
        }

        public NetLeftRoomResult LeftRoom(int roomUid, int roleUid, NetworkingPlayer player = null, bool isDisconnected = false)
        {
            NetLeftRoomResult ret;

            NetRoomServer room;
            if (roomDict.TryGetValue(roomUid, out room))
            {
                ret = room.LeftRoom(roleUid, player);
            }
            else
            {
                ret = NetLeftRoomResult.Failed_NoRoom;
            }

            OnPlayerLeftRoom(roomUid, roleUid, player, ret);


            if(isDisconnected && player != null)
            {
                BMSByte data = ObjectMapper.BMSByte(roomUid, (int)ret);
                Binary sendframe = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_ROOM);
                Send(player, sendframe, true);
            }

            return ret;
        }


        /// <summary>
        /// 加入观看房间
        /// </summary>
        private void JoinWatchRoom(NetworkingPlayer player, Binary frame)
        {
            // 房间UID
            int roomUid = frame.StreamData.GetBasicType<int>();

            NetJoinRoomResult ret;

            NetRoomServer room;
            if (!roomDict.TryGetValue(roomUid, out room))
            {
                // 失败 不存在该房间 
                ret = NetJoinRoomResult.Failed_NoRoom;
            }
            else
            {
                room.JoinWatchRoom(player);
                ret = NetJoinRoomResult.Successed;
                player.lastRoomUid = roomUid;
                player.lastRoleUid = -1;
            }

            OnPlayerJoinWatchRoom(roomUid, player, ret);


            BMSByte data = ObjectMapper.BMSByte(roomUid, (int)ret);
            Binary sendframe = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_WATCH_ROOM);
            Send(player, sendframe, true);
        }


        /// <summary>
        /// 离开观看房间
        /// </summary>
        private void LeftWatchRoom(NetworkingPlayer player, Binary frame)
        {
            // 房间UID
            int roomUid = frame.StreamData.GetBasicType<int>();

            LeftWatchRoom(roomUid, player);
        }

        private void LeftWatchRoom(NetworkingPlayer player, bool isDisconnected = false)
        {
            LeftWatchRoom(player.lastRoomUid,  player, isDisconnected);
        }

        public void LeftWatchRoom(int roomUid,  NetworkingPlayer player = null, bool isDisconnected = false)
        {
            NetLeftRoomResult ret;

            NetRoomServer room;
            if (roomDict.TryGetValue(roomUid, out room))
            {
                room.LeftWatchRoom(player);
                ret = NetLeftRoomResult.Successed;
            }
            else
            {
                ret = NetLeftRoomResult.Failed_NoRoom;
            }


            OnPlayerLeftWatchRoom(roomUid, player, ret);


            if (isDisconnected && player != null)
            {
                BMSByte data = ObjectMapper.BMSByte(roomUid, (int)ret);
                Binary sendframe = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_LEFT_WATCH_ROOM);
                Send(player, sendframe, true);
            }

        }


    }
}
