using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/6/2018 8:31:06 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Ihaiu.Forge.Networking
{
    public class StageFactory
    {
        public static RoomStage Create(NetRoomInfo roomInfo)
        {
            RoomStage stage = new RoomStageNormal();
            stage.Initialize(roomInfo);
            return stage;
        }
    }
}
