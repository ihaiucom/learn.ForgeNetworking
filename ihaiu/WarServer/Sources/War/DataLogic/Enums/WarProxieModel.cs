using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 9:21:41 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗代理模式 */
    public enum WarProxieModel
    {
        /** 客户端 */
        Client = 1,

        /** 服务器 */
        Server = 2,

        /** 客户端和服务器 */
        ClientAndServer = 3,
    }

}
