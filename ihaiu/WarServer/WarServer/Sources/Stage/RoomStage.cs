using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 11:20:25 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class RoomStage
    {
        public NetRoomServer netRoom { get; private set; }

        public RoomStage(NetRoomServer netRoom)
        {
            this.netRoom = netRoom;
        }
    }
}
