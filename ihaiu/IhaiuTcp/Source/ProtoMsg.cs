using BeardedManStudios;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:42:08 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public struct ProtoMsg
    {
        public int protoId;
        public byte[] bytes;
        public byte fromType;
        public long fromId;

        public byte[] GetData()
        {
            //| ----------包头-------------- | ---包体------ | --------包尾-------- -|
            //| 4字节消息长度 | 2字节消息类型 | 二进制流 | 1字符类型 | 8字节类型参数 |
            // | -----------------| ----------| ------------| ---------------------|

            int length = 6 + bytes.Length  + 9;
            byte[] data = new byte[length];
            BitConverter.GetBytes(length);

            // length
            int index = 0;
            int count = sizeof(Int32);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, data, index, count);

            // protoId
            index += count;
            count = sizeof(Int16);
            Buffer.BlockCopy(BitConverter.GetBytes((Int16)protoId), 0, data, index, count);

            // body
            index += count;
            count = bytes.Length;
            Buffer.BlockCopy(bytes, 0, data, index, count);


            // formType
            index += count;
            count = sizeof(byte);
            data[index] = fromType;


            // fromId
            index += count;
            count = sizeof(long);
            Buffer.BlockCopy(BitConverter.GetBytes(fromId), 0, data, index, count);


            return data;
        }
    }
}
