using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/11/2018 11:16:24 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// 房间服
    /// </summary>
    public class NetRoomServer
    {
        public LobbyBase net { get; private set; }

        public NetRoomServer(LobbyBase net)
        {
            this.net = net;
        }


    }
}
