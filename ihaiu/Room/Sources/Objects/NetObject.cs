using BeardedManStudios;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 4:08:16 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// 网络对象
    /// </summary>
    public class NetObject
    {
        /// <summary>
        ///传递一个NetworkObject源的事件通用委托
        /// </ summary>
        /// <param name =“netObject”>这个事件的对象来源</ param>
        public delegate void NetObjectEvent(NetObject netObject);


        /// <summary>
        ///用于创建需要BMSByte数据的事件
        /// </ summary>
        /// <param name =“data”>读取的数据</ param>
        public delegate void BinaryDataEvent(BMSByte data);

        /// <summary>
        ///在网络上创建对象时使用
        /// </ summary>
        /// <param name =“identity”>标识用于知道这是什么类型的网络对象</ param>
        /// <param name =“hash”>哈希ID（如果发送）匹配客户端创建的对象与服务器将异步响应的ID </ param>
        /// <param name =“id”>这个网络对象的id </ param>
        /// <param name =“frame”>该对象创建的帧数据（默认值）</ param>
        public delegate void CreateEvent(int identity, int hash, uint id, FrameStream frame);


        /// 在网络上接收到此对象的二进制消息并需要读取时发生
        public event BinaryDataEvent readBinary;


    }
}
