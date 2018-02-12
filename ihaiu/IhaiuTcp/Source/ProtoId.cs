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
        // ping
        public const int PING = 3001;

        // 反馈ping
        public const int PONG = 3002;


        // 错误
        public const int Error = 3003;
        // 断开连接
        public const int ConnectionClose = 3004;
        // 发送接受连接
        public const int Accepted = 3005;
        // GM
        public const int GM = 3006;


        // 创建房间
        public const int CreateRoom = 3021;

        // 删除房间
        public const int RemoveRoom = 3022;

        // 通知 房间战斗结果
        public const int RoomOver = 3023;



    }
}
