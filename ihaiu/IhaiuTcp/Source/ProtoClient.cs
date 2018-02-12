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
        }


        public void SendConnectionClose()
        {

        }

        public void SendPing()
        {

        }

        public void SendPong(DateTime time)
        {

        }
    }
}
