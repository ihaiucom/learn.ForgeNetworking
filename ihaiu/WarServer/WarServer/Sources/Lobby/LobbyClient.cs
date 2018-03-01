using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 11:22:25 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class LobbyClient : LobbyBase
    {

        /// <summary>
        /// 启动客户端
        /// </summary>
        public void StartClient(string hostAddress = "127.0.0.1", ushort port = 15959)
        {
            Socket = new UDPClient();
            Setup(hostAddress, port);
        }
    }
}
