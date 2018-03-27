﻿using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 3:24:35 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    /// <summary>
    /// 关卡
    /// </summary>
    public class RoomStage
    {
        public const int UPDATE_INTERVAL = 50;

        // 关卡设置
        public StageSetting Setting { get; protected set; }

        // 关卡场景
        public RoomScene Scene { get; protected set; }

        // 房间
        public NetRoomBase Room { get; protected set; }


        // 房间是否启动
        public bool IsRuning { get; protected set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize(NetRoomBase room, IRoomInfo roomInfo)
        {
            Room = room;
            Scene = new RoomScene(this);

            IsRuning = true;
            StarFixedUpdate();

            Room.playerLeftRoom += OnPlayerLeftRoom;
        }


        /// <summary>
        /// 释放，销毁
        /// </summary>
        public virtual void Dispose()
        {
            Room.playerLeftRoom -= OnPlayerLeftRoom;
            IsRuning = false;
        }


        /// <summary>
        /// 启动固定更新
        /// </summary>
        private void StarFixedUpdate()
        {
            Loger.Log("RoomStage Star FixedUpdate");
            Task.Queue(() =>
            {
                Loger.Log("RoomStage Begin FixedUpdate");
                while (IsRuning && Room.lobby.Socket.IsBound)
                {
                    FixedUpdate();
                    Thread.Sleep(UPDATE_INTERVAL);
                }

                Loger.Log("RoomStage End FixedUpdate");
            }, 0);
        }


        /// <summary>
        /// 固定更新
        /// </summary>
        public virtual void FixedUpdate()
        {
            Scene.FixedUpdate();
        }

        /// <summary>
        /// 玩家离开房间
        /// </summary>
        protected virtual void OnPlayerLeftRoom(ulong roleUid, NetworkingPlayer player)
        {
            Loger.Log("RoomStage OnPlayerLeftRoom" + player.NetworkId);
        }




    }
}
