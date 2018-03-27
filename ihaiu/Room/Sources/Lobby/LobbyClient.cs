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
*  @CreateTime      2/28/2018 2:52:19 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public class LobbyClient : LobbyBase
    {
        public IRoleInfo roleInfo;
        public IRoomInfo roomInfo;
        public NetRoomClient room;

        public LobbyClient()
        {
            Socket = new UDPClient();

            Socket.binaryMessageReceived += OnBinaryMessageReceived;

            Socket.serverAccepted += OnServerAccepted;
            Socket.disconnected += OnDisconnected;
        }

        public void Connect(string hostAddress = "127.0.0.1", ushort port = 16000)
        {
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
            Socket.serverAccepted -= OnServerAccepted;

            Loger.LogFormat("LobbyClient OnServerAccepted {0}", sender);
        }


        // 发送消息
        public void Send(FrameStream frame, bool reliable = false)
        {
            ((UDPClient)Socket).Send(frame, reliable);
        }

        // 接收文本数据
        private void OnTextMessageReceived(NetworkingPlayer player, Text frame, NetWorker sender)
        {
            if (room != null && room.roomId == frame.RoomId)
            {
                room.OnTextMessageReceived(player, frame, sender);
            }
        }

        // 接收二进制数据
        private void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            Loger.LogFormat("LobbyClient OnBinaryMessageReceived player={0} RoomId={1}", player.NetworkId, frame.RoomId);
            if (frame.GroupId == MessageGroupIds.Lobby)
            {
                byte routerId = frame.RouterId;

                Loger.LogFormat("LobbyClient Lobby OnBinaryMessageReceived player={0}, groupId={1}, routerId={2} ", player.NetworkId, frame.GroupId, routerId);

                switch (routerId)
                {
                    case RouterIds.LOBBY_CREATE_ROOM:
                        OnCreateRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_JOIN_ROOM:
                        OnJoinRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_LEFT_ROOM:
                        OnLeftRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_JOIN_WATCH_ROOM:
                        OnJoinWatchRoom(player, frame);
                        break;

                    case RouterIds.LOBBY_LEFT_WATCH_ROOM:
                        OnLeftWatchRoom(player, frame);
                        break;
                }
            }
            else
            {
                Loger.LogFormat("LobbyClient Room OnBinaryMessageReceived player={0}, groupId={1}, roomUid={2} ", player.NetworkId, frame.GroupId, frame.RoomId);

                if (room != null && room.roomId == frame.RoomId)
                {
                    room.OnBinaryMessageReceived(player, frame, sender);
                }
            }
        }

        /// <summary>
        /// 测试创建房间
        /// </summary>
        public void TestCreateRoom(IRoomInfo roomInfo)
        {
            room = new NetRoomClient(this, roomInfo);
        }

        /// <summary>
        /// 创建并加入房间
        /// </summary>
        public void CreateAndJoinRoom()
        {
            BMSByte data = new BMSByte();
            // RoomInfo
            roomInfo.MapBytes(data);
            // RoleInfo
            roleInfo.MapBytes(data);

            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_CREATE_ROOM);
            Send(frame, true);
        }


        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateRoom()
        {
            BMSByte data = new BMSByte();
            roomInfo.MapBytes(data);

            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_CREATE_ROOM);
            Send(frame, true);
        }

        // 服务器反馈, 创建房间
        private void OnCreateRoom(NetworkingPlayer player, Binary frame)
        {
            bool ret = frame.StreamData.GetBasicType<bool>();
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            string error = frame.StreamData.GetBasicType<string>();

            if(ret)
            {
                OnCreateRoomSuccessed(roomUid);
            }
            else
            {
                OnCreateRoomFailed(roomUid, error);
            }
        }

        /// <summary>
        /// 获取玩家列表
        /// </summary>
        public void GetRoomPlayerList()
        {
            BMSByte data = ObjectMapper.BMSByte(roomInfo.roomUid);
            roleInfo.MapBytes(data);

            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.ROOM, false, RouterIds.ROOM_GET_PLAYERLIST);
            Send(frame, true);
        }


        /// <summary>
        /// 加入房间
        /// </summary>
        public void JoinRoom()
        {
            BMSByte data = ObjectMapper.BMSByte(roomInfo.roomUid);
            roleInfo.MapBytes(data);

            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_ROOM);
            Send(frame, true);
        }


        // 服务器反馈, 加入房间
        private void OnJoinRoom(NetworkingPlayer player, Binary frame)
        {
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            NetJoinRoomResult ret = (NetJoinRoomResult)frame.StreamData.GetBasicType<int>();

            if (ret != NetJoinRoomResult.Failed_NoRoom)
            {
                room = new NetRoomClient(this, roomInfo);
            }

            OnPlayerJoinRoom(roomUid, player, ret);
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        public void LeftRoom()
        {
            BMSByte data = ObjectMapper.BMSByte(roomInfo.roomUid, roleInfo.uid);
            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_LEFT_ROOM);
            Send(frame, true);
        }

        // 服务器反馈, 离开房间
        private void OnLeftRoom(NetworkingPlayer player, Binary frame)
        {
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            NetLeftRoomResult ret = (NetLeftRoomResult)frame.StreamData.GetBasicType<int>();
            if (room != null)
            {
                room.OnLeftRoom();
                room = null;
            }
            OnPlayerLeftRoom(roomUid, roleInfo.uid, player, ret);
        }


        /// <summary>
        /// 加入观看房间
        /// </summary>
        public void JoinWatchRoom(int roomUid)
        {
            BMSByte data = ObjectMapper.BMSByte(roomUid);
            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_JOIN_WATCH_ROOM);
            Send(frame, true);

        }

        // 服务器反馈, 加入观看房间
        private void OnJoinWatchRoom(NetworkingPlayer player, Binary frame)
        {
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            NetJoinRoomResult ret = (NetJoinRoomResult)frame.StreamData.GetBasicType<int>();

            if(ret != NetJoinRoomResult.Failed_NoRoom)
            {
                room = new NetRoomClient(this, roomInfo);
            }
            OnPlayerJoinWatchRoom(roomUid, player, ret);
        }


        /// <summary>
        /// 离开观看房间
        /// </summary>
        public void LeftWatchRoom(int roomUid)
        {
            BMSByte data = ObjectMapper.BMSByte(roomUid);
            Binary frame = new Binary(Socket.Time.Timestep, false, data, Receivers.Server, MessageGroupIds.Lobby, false, RouterIds.LOBBY_LEFT_WATCH_ROOM);
            Send(frame, true);
        }

        // 服务器反馈, 离开房间
        private void OnLeftWatchRoom(NetworkingPlayer player, Binary frame)
        {
            ulong roomUid = frame.StreamData.GetBasicType<ulong>();
            NetLeftRoomResult ret = (NetLeftRoomResult)frame.StreamData.GetBasicType<int>();
            if(room != null)
            {
                room.OnLeftRoom();
                room = null;
            }
            OnPlayerLeftWatchRoom(roomUid, player, ret);
        }

        /// <summary>
        /// 释放，销毁
        /// </summary>
        public override void Dispose()
        {
            if(room != null)
                room.Dispose();

            room = null;

            base.Dispose();
        }

    }
}
