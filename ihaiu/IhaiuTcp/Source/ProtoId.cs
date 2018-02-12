using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:37:20 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class ProtoId
    {
        // 错误
        public const int Error = 1;
        // 断开连接
        public const int ConnectionClose = 2;
        // 发送接受连接
        public const int Accepted = 3;

        // ping
        public const int PING = 4;

        // 反馈ping
        public const int PONG = 5;

    }
}
