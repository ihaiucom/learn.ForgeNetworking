using Games.PB;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 3:09:05 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    /// <summary>
    /// 服务器注册 监听协议
    /// </summary>
    public partial class ProtoList
    {
        public void ServerRegisteredListener()
        {

            // =======================
            // 框架
            // -- --------------------

            // ping
            AddItem(new ProtoItem<OUTER_BM2B_Ping_Req>() { opcode = 3001, protoStructType = typeof(OUTER_BM2B_Ping_Req),  protoStructName = "OUTER_BM2B_Ping_Req",  protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "Ping" });

            // pong
            AddItem(new ProtoItem<OUTER_BM2B_Pong_Resp>() { opcode = 3002, protoStructType = typeof(OUTER_BM2B_Pong_Resp), protoStructName = "OUTER_BM2B_Pong_Resp", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "pong ping反馈" });

            // 请求断开连接
            AddItem(new ProtoItem<OUTER_BM2B_ConnectionClose_Req>() { opcode = 3004, protoStructType = typeof(OUTER_BM2B_ConnectionClose_Req), protoStructName = "OUTER_BM2B_ConnectionClose_Req", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "请求断开连接" });

            // 请求执行GM
            AddItem(new ProtoItem<OUTER_BM2B_GM_Req>() { opcode = 3006, protoStructType = typeof(OUTER_BM2B_GM_Req), protoStructName = "OUTER_BM2B_GM_Req", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "请求执行GM" });


            // ====================
            // 房间
            // -- --------------------

            // 请求 创建房间
            AddItem(new ProtoItem<OUTER_BM2B_CreateRoom_Req>() { opcode = 3021, protoStructType = typeof(OUTER_BM2B_CreateRoom_Req), protoStructName = "OUTER_BM2B_CreateRoom_Req", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "请求 创建房间" });

            // 请求 删除房间
            AddItem(new ProtoItem<OUTER_BM2B_RemoveRoom_Req>() { opcode = 3022, protoStructType = typeof(OUTER_BM2B_RemoveRoom_Req), protoStructName = "OUTER_BM2B_RemoveRoom_Req", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "请求 删除房间" });

        }
    }
}
