using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 3:10:37 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public partial class ProtoList
    {
        public Dictionary<int, IProtoItem> opcodeDict = new Dictionary<int, IProtoItem>();
        public Dictionary<Type, IProtoItem> typeDict = new Dictionary<Type, IProtoItem>();

        /** 添加 */
        public void AddItem(IProtoItem item)
        {
            opcodeDict.Add(item.opcode, item);
            typeDict.Add(item.protoStructType, item);
        }

        /** 获取 ProtoItem, 用 opcode */
        public IProtoItem GetItemByOpcode(int opcode)
        {
            if (!opcodeDict.ContainsKey(opcode))
            {
                Loger.LogTagErrorFormat("Proto", "ProtoBase 不存在opcode={0}的ProtoItem", opcode);
                return null;
            }
            return opcodeDict[opcode];
        }

        /** 获取 ProtoItem, 用 Type */
        public IProtoItem GetItemByType(Type type)
        {
            if (typeDict.ContainsKey(type))
            {
                return typeDict[type];
            }



            Loger.LogTagErrorFormat("Proto", "ProtoBase 不存在type={0}的ProtoItem", type);
            return null;
        }

        /** 获取 ProtoItem, 用 msg */
        public IProtoItem GetItemByMsg(object msg)
        {
            return GetItemByType(msg.GetType());
        }
    }
}
