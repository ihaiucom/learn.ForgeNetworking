using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 7:25:07 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public abstract class NetRoomBase
    {

        public delegate void RoomPlayerEvent(ulong roleUid, NetworkingPlayer player);
        public delegate void RoomOverEvent(NetRoomBase room);

        // 玩家 加入房间
        public event RoomPlayerEvent playerJoinRoom;
        // 玩家 离开房间
        public event RoomPlayerEvent playerLeftRoom;

        // 房间 结束
        public event RoomOverEvent roomOver;


        public ulong roomId;

        public LobbyBase lobby { get; protected set; }
        public RoomStage stage { get; protected set; }
        public RoomScene scene { get; protected set; }

        public TimeManager Time { get; set; }



        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialize(LobbyBase lobby, NetRoomInfo roomInfo)
        {
            this.lobby = lobby;
            this.roomId = roomInfo.roomUid;
            this.Time = lobby.Socket.Time;

            stage = StageFactory.Create(this, roomInfo);
            scene = stage.Scene;
        }

        /// <summary>
        /// 释放，销毁
        /// </summary>
        public virtual void Dispose()
        {
            if (stage != null)
                stage.Dispose();

            stage = null;
        }


        public virtual void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            scene.OnBinaryMessageReceived(player, frame);
        }

        // 调事件 -- 玩家加入
        protected void OnPlayerJoinRoom(ulong roleUid, NetworkingPlayer player)
        {
            if (playerJoinRoom != null)
            {
                playerJoinRoom(roleUid, player);
            }
        }

        // 调事件 -- 玩家离开
        protected void OnPlayerLeftRoom(ulong roleUid, NetworkingPlayer player)
        {
            if (playerLeftRoom != null)
            {
                playerLeftRoom(roleUid, player);
            }
        }

        // 调事件 -- 房间结束
        public void OnRoomOver()
        {
            if(roomOver != null)
            {
                roomOver(this);
            }
        }

        /// <summary>
        /// 房间结束
        /// </summary>
        public virtual void SetRoomOver()
        {
            OnRoomOver();
        }
    }
}
