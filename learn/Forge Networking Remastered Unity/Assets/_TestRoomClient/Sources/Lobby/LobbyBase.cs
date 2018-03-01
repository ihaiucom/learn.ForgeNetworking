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
namespace Games
{
    public abstract class LobbyBase
    {
        public NetWorker Socket { get; protected set; }


        public void Dispose()
        {
            Socket.Disconnect(true);
        }
    }
}
