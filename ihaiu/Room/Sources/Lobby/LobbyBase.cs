using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 2:53:11 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public abstract class LobbyBase
    {

        public delegate void RoomEvent(int roomUid);
        public delegate void RoomFailedEvent(int roomUid, string error);


        public delegate void RoomPlayerJoinEvent(int roomUid, NetworkingPlayer player, NetJoinRoomResult ret);
        public delegate void RoomPlayerLeftEvent(int roomUid, int roleUid, NetworkingPlayer player, NetLeftRoomResult ret);


        public delegate void RoomPlayerJoinWatchEvent(int roomUid, NetworkingPlayer player, NetJoinRoomResult ret);
        public delegate void RoomPlayerLeftWatchEvent(int roomUid, NetworkingPlayer player, NetLeftRoomResult ret);


        // 创建房间成功
        public event RoomEvent createRoomSuccessed;
        // 创建房间失败
        public event RoomFailedEvent createRoomFailed;

        // 玩家 加入房间
        public event RoomPlayerJoinEvent playerJoinRoom;
        // 玩家 离开房间
        public event RoomPlayerLeftEvent playerLeftRoom;

        // 玩家 加入观看房间
        public event RoomPlayerJoinWatchEvent playerJoinWatchRoom;
        // 玩家 离开观看房间
        public event RoomPlayerLeftWatchEvent playerLeftWatchRoom;

        protected void OnCreateRoomSuccessed(int roomUid)
        {
            if (createRoomSuccessed != null)
                createRoomSuccessed(roomUid);
        }


        protected void OnCreateRoomFailed(int roomUid, string error)
        {
            if (createRoomFailed != null)
                createRoomFailed(roomUid, error);
        }

        protected void OnPlayerJoinRoom(int roomUid, NetworkingPlayer player, NetJoinRoomResult ret)
        {
            if (playerJoinRoom != null)
            {
                playerJoinRoom(roomUid, player, ret);
            }
        }

        protected void OnPlayerLeftRoom(int roomUid, int roleUid, NetworkingPlayer player, NetLeftRoomResult ret)
        {
            if (playerLeftRoom != null)
            {
                playerLeftRoom(roomUid, roleUid, player, ret);
            }
        }


        protected void OnPlayerJoinWatchRoom(int roomUid, NetworkingPlayer player, NetJoinRoomResult ret)
        {
            if (playerJoinWatchRoom != null)
            {
                playerJoinWatchRoom(roomUid, player, ret);
            }
        }


        protected void OnPlayerLeftWatchRoom(int roomUid, NetworkingPlayer player, NetLeftRoomResult ret)
        {
            if (playerLeftWatchRoom != null)
            {
                playerLeftWatchRoom(roomUid, player, ret);
            }
        }

        public NetWorker Socket { get; protected set; }



        public void Dispose()
        {
            Socket.Disconnect(true);
        }
    }
}
