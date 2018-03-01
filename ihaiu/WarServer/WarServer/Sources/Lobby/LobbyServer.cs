using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 11:22:14 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class LobbyServer : LobbyBase
    {

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="connections">可以接收的连接数量</param>
        public void StartServer(int connections, string hostAddress = "0.0.0.0", ushort port = 15959)
        {
            Socket = new UDPServer(connections);
            Setup(hostAddress, port);
        }
    }
}
