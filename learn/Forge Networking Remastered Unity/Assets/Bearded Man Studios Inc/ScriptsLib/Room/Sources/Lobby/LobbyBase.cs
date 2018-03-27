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
namespace Rooms.Forge.Networking
{
    public abstract class LobbyBase
    {
        //===================================================
        // 工厂类
        //---------------------------------------------------

        /// <summary>
        /// 场景工厂
        /// </summary>
        public IStageFactory StageFactory { get; set; }


        //===================================================
        // 事件
        //---------------------------------------------------
        public delegate void RoomEvent(ulong roomUid);
        public delegate void RoomFailedEvent(ulong roomUid, string error);


        public delegate void RoomPlayerJoinEvent(ulong roomUid, NetworkingPlayer player, NetJoinRoomResult ret);
        public delegate void RoomPlayerLeftEvent(ulong roomUid, ulong roleUid, NetworkingPlayer player, NetLeftRoomResult ret);


        public delegate void RoomPlayerJoinWatchEvent(ulong roomUid, NetworkingPlayer player, NetJoinRoomResult ret);
        public delegate void RoomPlayerLeftWatchEvent(ulong roomUid, NetworkingPlayer player, NetLeftRoomResult ret);


        // 创建房间成功
        public event RoomEvent createRoomSuccessed;
        // 创建房间失败
        public event RoomFailedEvent createRoomFailed;


        // 房间结束
        public event NetRoomBase.RoomOverEvent roomOver;

        // 玩家 加入房间
        public event RoomPlayerJoinEvent playerJoinRoom;
        // 玩家 离开房间
        public event RoomPlayerLeftEvent playerLeftRoom;

        // 玩家 加入观看房间
        public event RoomPlayerJoinWatchEvent playerJoinWatchRoom;
        // 玩家 离开观看房间
        public event RoomPlayerLeftWatchEvent playerLeftWatchRoom;

        protected void OnCreateRoomSuccessed(ulong roomUid)
        {
            if (createRoomSuccessed != null)
                createRoomSuccessed(roomUid);
        }


        protected void OnCreateRoomFailed(ulong roomUid, string error)
        {
            if (createRoomFailed != null)
                createRoomFailed(roomUid, error);
        }


        internal void OnRoomOver(NetRoomBase room)
        {
            if (roomOver != null)
            {
                roomOver(room);
            }
        }

        protected void OnPlayerJoinRoom(ulong roomUid, NetworkingPlayer player, NetJoinRoomResult ret)
        {
            if (playerJoinRoom != null)
            {
                playerJoinRoom(roomUid, player, ret);
            }
        }

        protected void OnPlayerLeftRoom(ulong roomUid, ulong roleUid, NetworkingPlayer player, NetLeftRoomResult ret)
        {
            if (playerLeftRoom != null)
            {
                playerLeftRoom(roomUid, roleUid, player, ret);
            }
        }


        protected void OnPlayerJoinWatchRoom(ulong roomUid, NetworkingPlayer player, NetJoinRoomResult ret)
        {
            if (playerJoinWatchRoom != null)
            {
                playerJoinWatchRoom(roomUid, player, ret);
            }
        }


        protected void OnPlayerLeftWatchRoom(ulong roomUid, NetworkingPlayer player, NetLeftRoomResult ret)
        {
            if (playerLeftWatchRoom != null)
            {
                playerLeftWatchRoom(roomUid, player, ret);
            }
        }

        public NetWorker Socket { get; protected set; }


        /// <summary>
        /// 释放，销毁
        /// </summary>
        public virtual void Dispose()
        {
            Socket.Disconnect(true);
        }
    }
}
