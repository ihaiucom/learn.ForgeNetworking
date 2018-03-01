using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 7:09:00 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class NetRoomPlayer
    {
        // 角色UID
        public int roleUid;

        public NetworkingPlayer networkingPlayer;

        public NetRoomPlayer(int roleUid, NetworkingPlayer networkingPlayer)
        {
            this.roleUid = roleUid;
            this.networkingPlayer = networkingPlayer;
        }
    }
}
