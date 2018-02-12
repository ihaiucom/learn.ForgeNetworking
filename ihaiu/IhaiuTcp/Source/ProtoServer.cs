using BeardedManStudios.Forge.Networking;
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
        }

        public void SendError(TcpClient client, ProtoErrorId errorId, string error)
        {

        }

        public void SendConnectionClose(TcpClient client)
        {

        }


        public void SendAccepted(TcpClient client)
        {

        }

        public void SendPong(NetworkingPlayer playerRequesting, DateTime time)
        {

        }


    }
}
