﻿/*-----------------------------+-------------------------------\
|                                                              |
|                         !!!NOTICE!!!                         |
|                                                              |
|  These libraries are under heavy development so they are     |
|  subject to make many changes as development continues.      |
|  For this reason, the libraries may not be well commented.   |
|  THANK YOU for supporting forge with all your feedback       |
|  suggestions, bug reports and comments!                      |
|                                                              |
|                              - The Forge Team                |
|                                Bearded Man Studios, Inc.     |
|                                                              |
|  This source code, project files, and associated files are   |
|  copyrighted by Bearded Man Studios, Inc. (2012-2017) and    |
|  may not be redistributed without written permission.        |
|                                                              |
\------------------------------+------------------------------*/

using System;
using System.Net.Sockets;

namespace BeardedManStudios.Forge.Networking
{
    public abstract class BaseTCP : NetWorker
    {
        public BaseTCP() : base() { }
        public BaseTCP(int maxConnections) : base(maxConnections) { }

        /// <summary>
        /// Reads the current client stream and pulls the next set of data from it
        /// </summary>
        /// <param name="playerClient">The client that is to be read from</param>
        /// <param name="usingMask">Changes the algorithm to look for a mask in the bytes to be used</param>
        /// <returns>The bytes that are read for this frame</returns>
        /// <summary>
                ///读取当前的客户端流，并从中取出下一组数据
                /// </ summary>
                /// <param name =“playerClient”>要从</ param>中读取的客户端
                /// <param name =“usingMask”>更改算法以查找要使用的字节中的掩码</ param>
                /// <returns>为这个帧读取的字节</ returns>
        protected byte[] GetNextBytes(NetworkStream stream, int available, bool usingMask)
        {
            //将缓冲区设置为现在有可用字节的长度
            // Setup the buffer to have the length of the available bytes for now
            byte[] bytes = new byte[available];

            //读取前2个字节，第一个字节是控制帧id，
            //第二个是初始数据长度检查
            // Read the first 2 bytes, the first byte being the control fram id,
            // and the second being the initial data length check
            stream.Read(bytes, 0, 2);

            //确定当前长度是否是第二个字节或更多的值
            // Determine if the current length is the value of the second byte or more
            int dataLength = bytes[1] & 127;
            int payloadOffset = 2;

            // TODO：BRENTT !!! 为什么这使它工作？！
            //TODO: BRENTT!!! WHY DOES THIS MAKE IT WORK?!
            bool maskedMessage = false;
            /// END WHY

            //如果按位返回126，则有4个字节要读取，否则127是10个字节
            // If the bitwise & returns 126 there are 4 bytes to be read, otherwise 127 is 10 bytes
            if (dataLength == 126)
            {
                payloadOffset = 4;
                // TODO：BRENTT !!! 为什么这使它工作？！
                //TODO: BRENTT!!! WHY DOES THIS MAKE IT WORK?!
                maskedMessage = true;
                /// END WHY
            }
            else if (dataLength == 127)
                payloadOffset = 10;

            //将数据长度初始化为数据长度
            // Initialize the length to the data length
            int length = dataLength;

            //如果bitwize＆第二个字节产生了其他126或127的东西
            //从指定的字节范围读取长度
            // If the bitwize & on the second byte produced something other that 126 or 127 then
            // read the length from the specifid byte range
            if (payloadOffset != 2)
            {
                //获取要用于确定有效负载长度的下一组字节
                // Get the next set of bytes that are to be used to determine the payload length
                stream.Read(bytes, 2, payloadOffset - 2);

                //需要扭转末端秩序
                // Need to reverse the endien order
                if (payloadOffset == 4)
                {
                    if (usingMask)
                        length = BitConverter.ToUInt16(bytes, 2);
                    else
                        length = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                }
                else
                {
                    if (usingMask)
                        length = (int)BitConverter.ToUInt32(bytes, 2);
                    else
                        length = BitConverter.ToInt32(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2] }, 0);
                }
            }

            if (length == 0)
                return null;

            //Array.Resize<byte>(ref bytes, length + 4);
            //TODO: BRENTT!!! WHY DOES THIS MAKE IT WORK?!
            if (maskedMessage)
                length = available - payloadOffset;
            /// END WHY

            // Read the mask
            if (usingMask)
            {
                // Pop the mask off
                stream.Read(bytes, payloadOffset, 4);

                //TODO: BRENTT!!! WHY DOES THIS MAKE IT WORK?!
                if (maskedMessage)
                    length -= 4;
                /// END WHY

                // Pop off the rest of the payload other than the mask
                stream.Read(bytes, payloadOffset + 4, length);
            }
            else
                stream.Read(bytes, payloadOffset, length);

            return bytes;
        }
    }
}