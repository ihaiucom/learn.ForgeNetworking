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
        public int      masterServerId    = 3;
        public string   masterServerIp    = "172.16.52.101";
        public ushort   masterServerPort  = 23001;

        public string localServerIp     = "127.0.0.1";
        public ushort localServerPort   = 16000;


    }
}
