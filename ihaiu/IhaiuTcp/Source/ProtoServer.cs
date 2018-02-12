using BeardedManStudios.Forge.Networking;
using Games.PB;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:34:10 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public partial class ProtoServer : ProtoBase
    {
        public HTcpServer server;

        public ProtoServer(HTcpServer server):base(server)
        {
            this.server = server;

            protoListListener = new ProtoList();
            protoListListener.ServerRegisteredListener();

            protoListSender = new ProtoList();
            protoListSender.ServerRegisteredSender();

            AddCallback<OUTER_BM2B_Ping_Req>(baseTcp.OnPingMessage);
            AddCallback<OUTER_BM2B_Pong_Resp>(baseTcp.OnPongMessage);
        }




        public void SendPong(NetworkingPlayer playerRequesting, DateTime time)
        {
            OUTER_BM2B_Pong_Resp msg = new OUTER_BM2B_Pong_Resp();
            msg.receivedTimestep = (ulong)time.Ticks;
            SendMessage<OUTER_BM2B_Pong_Resp>(msg, playerRequesting.TcpClientHandle);
        }

        public void SendError(TcpClient client, ProtoErrorId errorId, string erroContent)
        {
            OUTER_BM2B_Error_Ntf msg = new OUTER_BM2B_Error_Ntf();
            msg.errorId = (UInt32)errorId;
            msg.erroContent = erroContent;
            SendMessage<OUTER_BM2B_Error_Ntf>(msg, client);
        }

        public void SendConnectionClose(TcpClient client)
        {
            OUTER_BM2B_ConnectionClose_Ntf msg = new OUTER_BM2B_ConnectionClose_Ntf();
            SendMessage<OUTER_BM2B_ConnectionClose_Ntf>(msg, client);
        }


        public void SendAccepted(TcpClient client)
        {
            OUTER_BM2B_Accepted_Ntf msg = new OUTER_BM2B_Accepted_Ntf();
            SendMessage<OUTER_BM2B_Accepted_Ntf>(msg, client);
        }






    }
}
