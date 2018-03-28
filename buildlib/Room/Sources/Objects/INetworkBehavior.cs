using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/2/2018 9:57:16 AM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public interface INetworkBehavior
    {
        void Initialize(NetworkObject obj);
        void Initialize(RoomScene networker, byte[] metadata = null);
    }
}
