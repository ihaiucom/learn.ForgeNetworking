using Games.PB;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 3:09:20 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    /// <summary>
    /// 服务器注册 发送协议
    /// </summary>
    public partial class ProtoList
    {
        public void ServerRegisteredSender()
        {

            // =======================
            // 框架
            // -- --------------------

            // ping
            AddItem(new ProtoItem<OUTER_BM2B_Ping_Req>() { opcode = 3001, protoStructType = typeof(OUTER_BM2B_Ping_Req), protoStructName = "OUTER_BM2B_Ping_Req", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "Ping" });

            // pong
            AddItem(new ProtoItem<OUTER_BM2B_Pong_Resp>() { opcode = 3002, protoStructType = typeof(OUTER_BM2B_Pong_Resp), protoStructName = "OUTER_BM2B_Pong_Resp", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "pong ping反馈" });

            // 通知错误
            AddItem(new ProtoItem<OUTER_BM2B_Error_Ntf>() { opcode = 3003, protoStructType = typeof(OUTER_BM2B_Error_Ntf), protoStructName = "OUTER_BM2B_Error_Ntf", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "错误通知" });

            // 通知断开连接
            AddItem(new ProtoItem<OUTER_BM2B_ConnectionClose_Ntf>() { opcode = 3004, protoStructType = typeof(OUTER_BM2B_ConnectionClose_Ntf), protoStructName = "OUTER_BM2B_ConnectionClose_Ntf", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "通知断开连接" });

            // 通知接受该客户端
            AddItem(new ProtoItem<OUTER_BM2B_Accepted_Ntf>() { opcode = 3005, protoStructType = typeof(OUTER_BM2B_Accepted_Ntf), protoStructName = "OUTER_BM2B_Accepted_Ntf", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "通知接受该客户端" });

            // 响应执行GM结果
            AddItem(new ProtoItem<OUTER_BM2B_GM_Resp>() { opcode = 3006, protoStructType = typeof(OUTER_BM2B_GM_Resp), protoStructName = "OUTER_BM2B_GM_Resp", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "响应执行GM结果" });


            // ====================
            // 房间
            // -- --------------------

            // 响应 创建房间
            AddItem(new ProtoItem<OUTER_BM2B_CreateRoom_Resp>() { opcode = 3021, protoStructType = typeof(OUTER_BM2B_CreateRoom_Resp), protoStructName = "OUTER_BM2B_CreateRoom_Resp", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "响应 创建房间" });

            // 响应 删除房间
            AddItem(new ProtoItem<OUTER_BM2B_RoomOver_Ntf>() { opcode = 3022, protoStructType = typeof(OUTER_BM2B_RoomOver_Ntf), protoStructName = "OUTER_BM2B_RoomOver_Ntf", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "响应 删除房间" });

            // 通知 房间战斗结果
            AddItem(new ProtoItem<OUTER_BM2B_RoomOver_Ntf>() { opcode = 3023, protoStructType = typeof(OUTER_BM2B_RoomOver_Ntf), protoStructName = "OUTER_BM2B_RoomOver_Ntf", protoFilename = "battle_server", opcodeMapping = new int[] { }, note = "通知 房间战斗结果" });

        }
    }
}
