using Games.PB;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:35:21 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public partial class ProtoClient : ProtoBase
    {
        public HTcpClient client;


        public ProtoClient(HTcpClient client):base(client)
        {
            this.client = client;


            protoListListener = new ProtoList();
            protoListListener.ClientRegisteredListener();

            protoListSender = new ProtoList();
            protoListSender.ClientRegisteredSender();


            AddCallback<OUTER_BM2B_Ping_Req>(baseTcp.OnPingMessage);
            AddCallback<OUTER_BM2B_Pong_Resp>(baseTcp.OnPongMessage);
            AddCallback<OUTER_BM2B_Accepted_Ntf>(OnAccepted);
        }

        private void OnAccepted(OUTER_BM2B_Accepted_Ntf msg)
        {
            Loger.Log("服务器接受了当前客户端连接");
        }

        public void SendPing()
        {
            OUTER_BM2B_Ping_Req msg = new OUTER_BM2B_Ping_Req();
            msg.receivedTimestep = (ulong)DateTime.UtcNow.Ticks;
            SendMessage<OUTER_BM2B_Ping_Req>(msg);
        }

        public void SendPong(DateTime time)
        {
            OUTER_BM2B_Pong_Resp msg = new OUTER_BM2B_Pong_Resp();
            msg.receivedTimestep = (ulong)time.Ticks;
            SendMessage<OUTER_BM2B_Pong_Resp>(msg);
        }

        public void SendConnectionClose()
        {
            OUTER_BM2B_ConnectionClose_Req msg = new OUTER_BM2B_ConnectionClose_Req();
            SendMessage<OUTER_BM2B_ConnectionClose_Req>(msg);
        }


        public void SendGM(string cmd)
        {
            OUTER_BM2B_GM_Req msg = new OUTER_BM2B_GM_Req();
            msg.cmd = cmd;
            SendMessage<OUTER_BM2B_GM_Req>(msg);
        }

    }
}
