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
using System.Collections.Generic;
using System.Linq;

namespace BeardedManStudios.Forge.Networking.Frame
{
    /// <summary>
    /// 消息解析器
    /// </summary>
	public static class Factory
	{
        /// <summary>
        /// 解析完整消息的包头
        /// </summary>
        /// <param name="bytes">消息数据</param>
        /// <param name="useMask"></param>
        /// <param name="indexFirstMask"></param>
        /// <returns></returns>
		private static byte[] DecodeHead(byte[] bytes, bool useMask, out int indexFirstMask)
		{
			int dataLength = bytes[1] & 127;
			indexFirstMask = 2;
			if (dataLength == 126)
				indexFirstMask = 4;
			else if (dataLength == 127)
				indexFirstMask = 10;

            //如果我们从数据中拉出一个掩码，找到掩码，它是长度字节之后的下一个4个字节
            // Find the mask if we are pulling one from the data, it is the next 4 bytes after the length bytes
            IEnumerable<byte> keys = null;
			if (useMask)
			{
				keys = bytes.Skip(indexFirstMask).Take(4);
				indexFirstMask += 4;
			}

            //通过并解码字节，如果没有提供掩码，则复制剩余字节的长度
            // Go through and decode the bytes, if no mask is supplied then copy the remaining bytes after length
            byte[] decoded = new byte[bytes.Length - indexFirstMask];

			if (useMask)
				for (int i = indexFirstMask, j = 0; i < bytes.Length; i++, j++)
				{
					decoded[j] = (byte)(bytes[i] ^ keys.ElementAt(j % 4));
				}
			else
				Buffer.BlockCopy(bytes, indexFirstMask, decoded, 0, decoded.Length);

			return decoded;
		}

        /// <summary>
        /// Decode the message that was receieved over the network, this is after it has been fully
        /// parsed into a complete byte array
        /// </summary>
        /// <param name="bytes">The bytes that are to be decoded</param>
        /// <param name="useMask">Determines if a mask was used and if it should find the mask</param>
        /// <returns>The frame that was found during decoding</returns>
        /// <summary>
        ///解码通过网络接收到的消息，这是完全的
        ///解析成一个完整的字节数组
        /// </ summary>
        /// <param name =“bytes”>要解码的字节</ param>
        /// <param name =“useMask”>确定是否使用了蒙版，是否应该找到蒙版</ param>
        /// <returns>在解码期间找到的帧</ returns>
        public static FrameStream DecodeMessage(byte[] bytes, bool useMask, int groupId, NetworkingPlayer sender, byte receivers = 255)
		{
			int indexFirstMask = 0;
			byte[] decoded = DecodeHead(bytes, useMask, out indexFirstMask);
			return ReadFrameStream(bytes[0], decoded, indexFirstMask, groupId, sender, receivers);
		}

		public static FrameStream ReadFrameStream(byte firstByte, byte[] decoded, int indexFirstMask, int groupId, NetworkingPlayer sender, byte receivers = 255)
		{
            // TCP sends as a stream, so we didn't get the group id from the packet compilation
            // In this case we need to read it as the first set of bytes after the mask
            // TCP作为流发送，所以我们没有从包编译得到组ID
            //在这种情况下，我们需要将它作为掩码之后的第一组字节读取
            if (groupId == MessageGroupIds.TCP_FIND_GROUP_ID)
			{
				groupId = BitConverter.ToInt32(decoded, 0);
				indexFirstMask = 4;
			}
			else
				indexFirstMask = 0;

            // 找出这是什么样的frame，并构建适当的阅读frame
            // Find out what kind of frame this is and construct the appropriate frame for reading
            switch (firstByte)
			{
				case Binary.CONTROL_BYTE:
					return new Binary(decoded, indexFirstMask, groupId, sender, receivers);
				case ConnectionClose.CONTROL_BYTE:
					return new ConnectionClose(decoded, indexFirstMask, groupId, sender, receivers);
				case Continuation.CONTROL_BYTE:
					return new Continuation(decoded, indexFirstMask, groupId, sender, receivers);
				case Error.CONTROL_BYTE:
					return new Error(decoded, indexFirstMask, groupId, sender, receivers);
				case Ping.CONTROL_BYTE:
					return new Ping(decoded, indexFirstMask, groupId, sender, receivers);
				case Pong.CONTROL_BYTE:
					return new Pong(decoded, indexFirstMask, groupId, sender, receivers);
				case Text.CONTROL_BYTE:
					return new Text(decoded, indexFirstMask, groupId, sender, receivers);
				default:
					throw new BaseNetworkException("Message received but header doesn't define correct frame type.");
			}
		}
	}
}