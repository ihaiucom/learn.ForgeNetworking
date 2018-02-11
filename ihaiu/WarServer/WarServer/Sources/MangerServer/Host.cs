using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/11/2018 2:53:53 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public struct Host
    {
        public string Name;
        public string Address;
        public ushort Port;
        public int PlayerCount;
        public int MaxPlayers;
        public string Comment;
        public string Id;
        public string Type;
        public string Mode;
        public string Protocol;
        public int Elo;
        public bool UseElo;
        public NetworkingPlayer Player;
    }
}
