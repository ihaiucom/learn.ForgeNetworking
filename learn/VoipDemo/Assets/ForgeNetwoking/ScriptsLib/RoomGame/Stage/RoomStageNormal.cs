using Rooms.Forge.Networking.Generated;
using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 3:31:31 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{

    /// <summary>
    /// 关卡 -- 普通
    /// </summary>
    public class RoomStageNormal : RoomStage
    {
        // 网络对象工厂
        public RoomNetworkObjectFactory Factory { get; protected set; }

        UnitNetworkObject hero;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize(NetRoomBase room, IRoomInfo roomInfo)
        {
            base.Initialize(room, roomInfo);

            Factory = new RoomNetworkObjectFactory(Scene);
            Scene.Factory = Factory;

            if (room is NetRoomClient)
            {
                hero = Factory.InstantiateHero();
            }
        }

        /// <summary>
        /// 释放，销毁
        /// </summary>
        public override void Dispose()
        {

            if (hero != null)
                hero.Destroy();

            base.Dispose();
        }

        /// <summary>
        /// 玩家离开房间
        /// </summary>
        protected override void OnPlayerLeftRoom(ulong roleUid, NetworkingPlayer player)
        {
            base.OnPlayerLeftRoom(roleUid, player);
            Scene.DestoryPlayerNetworkObjects(player);
        }
    }
}
