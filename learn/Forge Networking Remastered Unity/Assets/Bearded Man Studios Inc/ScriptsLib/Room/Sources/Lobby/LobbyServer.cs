using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
using BeardedManStudios.Threading;
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
namespace Rooms.Forge.Networking
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

            Loger.LogFormat("LobbyServer OnBinaryMessageReceived player={0} RoomId={1}", player.NetworkId, frame.RoomId);

            if (frame.RoomId != 0)
            {

                if (roomDict.ContainsKey(frame.RoomId))
                {
                    try
                    {
                        roomDict[frame.RoomId].OnBinaryMessageReceived(player, frame, sender);
                    }
                    catch(Exception e)
                    {
                        Loger.LogError(e.ToString());
                    }
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

        }



        // 房间字典
        public Dictionary<ulong, NetRoomServer> roomDict = new Dictionary<ulong, NetRoomServer>();


        /// <summary>
        /// 获取房间列表
        /// </summary>
        public List<NetRoomServer> GetRoomList()
        {
            List<NetRoomServer> list = new List<NetRoomServer>(roomDict.Values);
            return list;
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        public NetRoomServer GetRoom(ulong roomId)
        {
            if(roomDict.ContainsKey(roomId))
            {
                return roomDict[roomId];
            }
            return null;
        }


        /// <summary>
        /// 创建房间
        /// </summary>
        private void CreateRoom(NetworkingPlayer player, Binary frame)
        {
            NetRoomInfo roomInfo = new NetRoomInfo();
            roomInfo.roomUid = frame.StreamData.GetBasicType<ulong>();
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
            roomDict.Add(room.roomId, room);

            OnCreateRoomSuccessed(room.roomId);


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
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            // 角色UID
            ulong roleUid = frame.StreamData.GetBasicType<ulong>();


            Action<NetJoinRoomResult> CallResult = (NetJoinRoomResult result) =>
            {
                BMSByte data = ObjectMapper.BMSByte(roomUid, (int)result);
                Binary sendframe = new Binary(Socket.Time.Timestep, false, data, Receivers.Target, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_ROOM);
                Send(player, sendframe, true);
            };

            NetJoinRoomResult ret;

            NetRoomServer room;
            if(!roomDict.TryGetValue(roomUid, out room))
            {
                // 失败 不存在该房间 
                ret = NetJoinRoomResult.Failed_NoRoom;
                CallResult(ret);
            }
            else
            {
                ret = room.JoinRoom(roleUid, player, frame, CallResult);
                player.lastRoomUid = roomUid;
            }

            OnPlayerJoinRoom(roomUid, player, ret);

        }

        /// <summary>
        /// 离开房间
        /// </summary>
        private NetLeftRoomResult LeftRoom(NetworkingPlayer player, Binary frame)
        {
            // 房间UID
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            // 角色UID
            ulong roleUid = frame.StreamData.GetBasicType<ulong>();

            return LeftRoom(roomUid, roleUid, player);
        }

        private NetLeftRoomResult LeftRoom(NetworkingPlayer player, bool isDisconnected = false)
        {
            return LeftRoom(player.lastRoomUid, player.lastRoleUid, player, isDisconnected);
        }

        public NetLeftRoomResult LeftRoom(ulong roomUid, ulong roleUid, NetworkingPlayer player = null, bool isDisconnected = false)
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
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();

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
                player.lastRoleUid = 0;
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
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();

            LeftWatchRoom(roomUid, player);
        }

        private void LeftWatchRoom(NetworkingPlayer player, bool isDisconnected = false)
        {
            LeftWatchRoom(player.lastRoomUid,  player, isDisconnected);
        }

        public void LeftWatchRoom(ulong roomUid,  NetworkingPlayer player = null, bool isDisconnected = false)
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


        /// <summary>
        /// 释放，销毁
        /// </summary>
        public override void Dispose()
        {

            foreach(var kvp in roomDict)
            {
                kvp.Value.Dispose();
            }
            roomDict.Clear();

            base.Dispose();
        }


    }
}
