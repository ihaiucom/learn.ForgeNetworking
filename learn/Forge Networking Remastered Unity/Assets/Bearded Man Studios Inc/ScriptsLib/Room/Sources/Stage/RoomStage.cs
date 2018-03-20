using System;
using System.Collections.Generic;
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
        // 关卡设置
        public StageSetting Setting { get; protected set; }

        // 关卡场景
        public RoomScene Scene { get; protected set; }

        // 房间
        public NetRoomBase Room { get; protected set; }

        public void Initialize(NetRoomInfo roomInfo)
        {
            Scene = new RoomScene(this);
        }



    }
}
