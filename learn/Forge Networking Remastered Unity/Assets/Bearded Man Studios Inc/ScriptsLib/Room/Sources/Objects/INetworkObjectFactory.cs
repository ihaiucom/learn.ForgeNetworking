using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/2/2018 9:42:33 AM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public interface INetworkObjectFactory
    {
        /// <summary>
        ///在网络上有一个请求来创建一个NetworkObject类型
        ///是该消息的入口点
        /// </ summary>
        /// <param name =“networker”>即将创建NetworkObject的控制器的NetWorker </ param>
        /// <param name =“identity”> NetworkObject的标识为int，以便该工厂知道要创建哪种NetworkObject </ param>
        /// <param name =“id”>服务器分配了这个新的NetworkObject（如果客户端）的id </ param>
        /// <param name =“frame”>在网络上接收到的关于这个创建的数据</ param>
        /// <param name =“callback”>成功创建的网络对象的回调</ param>
        /// <returns> </ returns>
        void NetworkCreateObject(RoomScene networker, int identity, uint id, FrameStream frame, System.Action<NetworkObject> callback);

    }
}
