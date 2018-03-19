using Games.PB;
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
            AddItem(new ProtoItem<S_Ping_1>() { opcode = 1, protoStructType = typeof(S_Ping_1), protoStructName = "S_Ping_1", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端接收Ping" });

            




            AddItem = tcpClient.protoClient.protoListSender.AddItem;

            // ====================
            // authorization Sender
            // -- --------------------

            // Ping
            AddItem(new ProtoItem<C_Ping_1>() { opcode = 1, protoStructType = typeof(C_Ping_1), protoStructName = "C_Ping_1", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端发送Ping" });

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
            /**  服务器：Ping */
            AddCallback<S_Ping_1>(S_Ping_1);
            
        }


        /** 服务器：Ping */
        public void C_Ping_1()
        {
            C_Ping_1 msg = new C_Ping_1();
            SendMessage<C_Ping_1>(msg);
        }

        /** 服务器：Ping */
        private void S_Ping_1(S_Ping_1 msg)
        {
            Loger.LogTagFormat("ProtoLogin", "S_Ping_1");
        }

    }
}
