using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/19/2018 4:03:16 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class MasterClientParameter
    {
        public string url = "http://172.16.52.101/game_operating_platform/index.php";
        public string gameId = "_self_game";
        public string channel = "default_self";

        public int      masterServerId    = 3;
        public string   masterServerIp    = "172.16.52.101";
        public ushort   masterServerPort  = 23001;

        public string username = "warserver01";
        public string password = "";

        internal ulong accountId = 0;
        internal string session = "";
    }
}
