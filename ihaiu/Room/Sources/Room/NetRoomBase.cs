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
namespace Games
{
    public abstract class NetRoomBase
    {
        public int uid;
        public int stageId;

        public LobbyBase lobby { get; protected set; }
    }
}
