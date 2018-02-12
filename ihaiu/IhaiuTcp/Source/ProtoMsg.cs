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
        public int fromType;
        public long fromId;



        public byte[] GetData()
        {
            return new byte[0];
        }
    }
}
