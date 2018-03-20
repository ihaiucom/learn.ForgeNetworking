using Games.PB;
using Rooms.Ihaiu.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/16/2018 4:12:00 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class ProtoMaster
    {
        private string TagName = "ProtoMaster";
        private MasterClient    client;
        private HTcpClient      tcpClient;


        public ProtoMaster(MasterClient client)
        {
            this.client = client;
            this.tcpClient = client.tcpClient;

            Register();
            AddListener();
        }


        /// <summary>
        /// 注册
        /// </summary>
        public void Register()
        {
            Action< IProtoItem > AddItem= tcpClient.protoClient.protoListListener.AddItem;

            // ====================
            // authorization Listener
            // -- --------------------


            // Ping
            AddItem(new ProtoItem<OUTER_B2BM_Ping>() { opcode = 5001, protoStructType = typeof(OUTER_B2BM_Ping), protoStructName = "OUTER_B2BM_Ping", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "Master服 Ping" });

            // 注册战斗服
            AddItem(new ProtoItem<OUTER_B2BM_RegNewBattleServer_Resp>() { opcode = 5003, protoStructType = typeof(OUTER_B2BM_RegNewBattleServer_Resp), protoStructName = "OUTER_B2BM_RegNewBattleServer_Resp", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "Master服反馈 注册战斗服" });

            // 创建房间
            AddItem(new ProtoItem<OUTER_BM2B_MPVE_CreateRoom_Req>() { opcode = 5004, protoStructType = typeof(OUTER_BM2B_MPVE_CreateRoom_Req), protoStructName = "OUTER_BM2B_MPVE_CreateRoom_Req", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "Master服请求 创建房间" });

            // 房间结束
            AddItem(new ProtoItem<OUTER_B2BM_RoomEnd_Resp>() { opcode = 5007, protoStructType = typeof(OUTER_B2BM_RoomEnd_Resp), protoStructName = "OUTER_B2BM_RoomEnd_Resp", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "Master服反馈接收 房间结果" });





            AddItem = tcpClient.protoClient.protoListSender.AddItem;

            // ====================
            // authorization Sender
            // -- --------------------

            // Ping
            AddItem(new ProtoItem<OUTER_B2BM_Ping>() { opcode = 5001, protoStructType = typeof(OUTER_B2BM_Ping), protoStructName = "OUTER_B2BM_Ping", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "战斗服发送 Ping" });

            // 注册战斗服
            AddItem(new ProtoItem<OUTER_B2BM_RegNewBattleServer_Req>() { opcode = 5002, protoStructType = typeof(OUTER_B2BM_RegNewBattleServer_Req), protoStructName = "OUTER_B2BM_RegNewBattleServer_Req", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "战斗服发送 注册战斗服" });

            // 创建房间
            AddItem(new ProtoItem<OUTER_BM2B_MPVE_CreateRoom_Resp>() { opcode = 5005, protoStructType = typeof(OUTER_BM2B_MPVE_CreateRoom_Resp), protoStructName = "OUTER_B2BM_RegNewBattleServer_Req", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "战斗服发送 创建房间的ID" });

            // 房间结束
            AddItem(new ProtoItem<OUTER_B2BM_RoomEnd_Req>() { opcode = 5006, protoStructType = typeof(OUTER_B2BM_RoomEnd_Req), protoStructName = "OUTER_B2BM_RoomEnd_Req", protoFilename = "battle_inner", opcodeMapping = new int[] { }, note = "战斗服发送 房间结果" });

        }



        /** 添加监听 */
        public void AddCallback<T>(Action<T> callback) where T : new()
        {
            tcpClient.protoClient.AddCallback<T>(callback);
        }

        /** 发送消息 */
        public void SendMessage<T>(T msg)
        {
            tcpClient.protoClient.SendMessage<T>(msg);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        private void AddListener()
        {
            // Ping
            AddCallback<OUTER_B2BM_Ping>(S_Ping);

            // 注册战斗服
            AddCallback<OUTER_B2BM_RegNewBattleServer_Resp>(S_RegNewBattleServer);

            // 创建房间
            AddCallback<OUTER_BM2B_MPVE_CreateRoom_Req>(S_MPVE_CreateRoom);

            // 房间结束
            AddCallback<OUTER_B2BM_RoomEnd_Resp>(S_RoomEnd);
        }

        /// <summary>
        /// 发送 Ping
        /// </summary>
        public void C_Ping()
        {
            OUTER_B2BM_Ping msg = new OUTER_B2BM_Ping();
            msg.timestamp = (ulong)DateTime.UtcNow.Ticks;
            SendMessage<OUTER_B2BM_Ping>(msg);
        }

        /// 接收 Ping
        private void S_Ping(OUTER_B2BM_Ping msg)
        {
            //Loger.LogTagFormat(TagName, "S_Ping_1 timestamp={0}", msg.timestamp);
        }


        /// <summary>
        ///  发送 注册战斗服
        /// </summary>
        public void C_RegNewBattleServer(string ip, UInt32 port)
        {
            OUTER_B2BM_RegNewBattleServer_Req msg = new OUTER_B2BM_RegNewBattleServer_Req();
            msg.ip = ip;
            msg.port = port;
            SendMessage<OUTER_B2BM_RegNewBattleServer_Req>(msg);
        }

        /// 接收 注册战斗服反馈
        private void S_RegNewBattleServer(OUTER_B2BM_RegNewBattleServer_Resp msg)
        {
            Loger.LogTagFormat(TagName, "S_RegNewBattleServer result={0}", msg.result == 0 ? "成功" : "失败");
            
        }

        /// <summary>
        /// 接收 创建房间
        /// </summary>
        private void S_MPVE_CreateRoom(OUTER_BM2B_MPVE_CreateRoom_Req msg)
        {
            NetRoomInfo roomInfo = new NetRoomInfo();
            roomInfo.roomUid = msg.room_id;
            roomInfo.stageId = (int)msg.copy_id;
            bool result = client.lobbyServer.CreateRoom(roomInfo);
            C_MPVE_CreateRoom(result);
        }

        // 反馈 创建房间ID
        public void C_MPVE_CreateRoom(bool result)
        {
            OUTER_BM2B_MPVE_CreateRoom_Resp msg = new OUTER_BM2B_MPVE_CreateRoom_Resp();
            msg.result = result ? 0u : 1u;
            SendMessage<OUTER_BM2B_MPVE_CreateRoom_Resp>(msg);
        }

        /// <summary>
        /// 发送 房间结束 结果
        /// </summary>
        public void C_RoomEnd(ulong roomId)
        {
            OUTER_B2BM_RoomEnd_Req msg = new OUTER_B2BM_RoomEnd_Req();
            msg.room_id = (UInt32) roomId;
            SendMessage<OUTER_B2BM_RoomEnd_Req>(msg);
        }

        /// 接收 房间结束反馈
        private void S_RoomEnd(OUTER_B2BM_RoomEnd_Resp msg)
        {
            Loger.LogTagFormat(TagName, "S_RoomEnd result={0}", msg.result == 0 ? "成功" : "失败");
        }
    }
}
