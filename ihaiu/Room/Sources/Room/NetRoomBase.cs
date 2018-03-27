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

        public delegate void RoomPlayerInfoListEvent(List<IRoleInfo> players);
        public delegate void RoomPlayerInfoEvent(IRoleInfo roleInfo, NetworkingPlayer player);
        public delegate void RoomPlayerEvent(ulong roleUid, NetworkingPlayer player);
        public delegate void RoomOverEvent(NetRoomBase room);

        // 给外界扩展文本消息
        public event NetWorker.TextFrameEvent textMessageReceived;
        // 给外界扩展二进制消息
        public event NetWorker.BinaryFrameEvent binaryMessageReceived;
        // 玩家 获取玩家列表
        public event RoomPlayerInfoListEvent playerListEvent;
        // 玩家 加入房间
        public event RoomPlayerInfoEvent playerJoinRoom;
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
        protected virtual void Initialize(LobbyBase lobby, IRoomInfo roomInfo)
        {
            this.lobby = lobby;
            this.Time = lobby.Socket.Time;
            this.roomId = roomInfo.roomUid;


            stage = lobby.StageFactory.Create(this, roomInfo);
            if(stage != null)
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

        public virtual void OnTextMessageReceived(NetworkingPlayer player, Text frame, NetWorker sender)
        {
            if (textMessageReceived != null)
                textMessageReceived(player, frame, sender);
        }

        public virtual void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            if (scene != null)
                scene.OnBinaryMessageReceived(player, frame, sender);
            else
                OnBinaryMessageEvent(player, frame, sender);
        }

        internal virtual void OnBinaryMessageEvent(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            if (binaryMessageReceived != null)
                binaryMessageReceived(player, frame, sender);
        }

        // 调事件 -- 获取玩家列表
        protected void OnPlayerListEvent(List<IRoleInfo> playerList)
        {
            if (playerListEvent != null)
            {
                playerListEvent(playerList);
            }
        }

        // 调事件 -- 玩家加入
        protected void OnPlayerJoinRoom(IRoleInfo roleInfo, NetworkingPlayer player)
        {
            if (playerJoinRoom != null)
            {
                playerJoinRoom(roleInfo, player);
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
