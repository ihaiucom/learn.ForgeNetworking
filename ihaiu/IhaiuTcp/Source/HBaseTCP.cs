﻿using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 9:19:35 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    /*
# 服务器内部通讯协议格式说明

## 1.	数据包结构
数据包包含包头和包体及包尾三部分

- 包头

包头模式: 小端模式(little endian)
包头结构: 前四个字节表示数据包长度, 随后两字节表示消息类型

    int32 :总长度, 包含包头及包尾长度
    int16 :消息类型

- 包体

二进制字节流

- 包尾
包尾结构: 1个字节表示包尾类型, 随后8个字符表示类型参数

|包尾类型枚举值|  说明    			 | 参数含义           |
|-------------|---------------------|--------------------|
|1       	  | 消息来自客户端 	  | 客户端在网关上的编号 |
|2            | 消息发往客户端       | 客户端在网关上的编号 |
|3            | 消息发往所有客户端   | 必须填0			   |
|4            | 转发客户端的消息  	 | 客户端在网关上的编号 |
|5            | 转发服务器的消息     | 目标服务器标识	    |
|6            | 消息转发标识 	       | 来源服务器标识	  |

- 示意图

```
|----------包头--------------|---包体------|--------包尾---------|
|4字节消息长度|2字节消息类型   |  二进制流   |1字符类型|8字节类型参数|
|-----------------|----------|------------|---------------------| 
```
## 2.	必要的协议

|消息类型|包体 |  说明    |
|--------|----|---------|
|1       | 空 |  心跳协议|
*/

    public abstract class HBaseTCP : NetWorker
    {

        public HBaseTCP() : base() { }
        public HBaseTCP(int maxConnections) : base(maxConnections) { }

        /// <summary>
        /// 读取当前的客户端流，并从中取出下一组数据
        /// </summary>
        /// <param name =“playerClient”>要从</ param>中读取的客户端
        /// <param name =“usingMask”>更改算法以查找要使用的字节中的掩码</ param>
        /// <returns>为这个帧读取的字节</ returns>
        protected ProtoMsg GetNextBytes(NetworkStream stream, int available, bool usingMask)
        {
            ProtoMsg msg = new ProtoMsg();

            //将缓冲区设置为现在有可用字节的长度
            byte[] bytes = new byte[9];

            // 包头结构: 前四个字节表示数据包长度, 随后两字节表示消息类型
            // int32 :总长度, 包含包头及包尾长度
            // int16 :消息类型
            stream.Read(bytes, 0, 6);
            int length = BitConverter.ToInt32(bytes, 0);
            int propId = BitConverter.ToInt16(bytes, 4);
            length -= 6 - 9;

            // 包体
            byte[] body = null;
            if(length > 0)
            {
                body = new byte[length];
                stream.Read(body, 6, length);
            }
           

            // 包尾 包尾结构: 1个字节表示包尾类型, 随后8个字符表示类型参数
            stream.Read(bytes, 6 + length, 9);
            byte fromType = bytes[1];
            long fromId = BitConverter.ToInt64(bytes, 1);

            msg.protoId     = propId;
            msg.bytes       = body;
            msg.fromType    = fromType;
            msg.fromId      = fromId;

            return msg;
        }


        public void OnMessage(ProtoMsg msg, NetworkingPlayer player)
        {
            if (msg.protoId == ProtoId.PING || msg.protoId == ProtoId.PONG)
            {
                // 发送ping时的 发送者时间
                long receivedTimestep = 0;
                DateTime received = new DateTime(receivedTimestep);

                // 现在接收到反馈ping的时间 - 自己发送ping是的时间
                TimeSpan ms = DateTime.UtcNow - received;

                if (msg.protoId == ProtoId.PING)
                    // 反馈ping, 将发送ping时的 发送者时间 一起发给他
                    Pong(player, received);
                else
                    // 接收都ping的反馈
                    OnPingRecieved(ms.TotalMilliseconds, player);

                return;
            }

        }


        public override void FireRead(FrameStream frame, NetworkingPlayer currentPlayer)
        {
        }
    }
}
