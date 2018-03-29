#if NOT_USE_UNITY
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 11:16:44 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class WarProxyUtil
    {
        public static WarConsoleDirve CreateDirve(WarRoom room)
        {
            WarConsoleDirve dirve = new WarConsoleDirve();
            dirve.SetRoom(room);
            return dirve;
        } 
        
        
        /** 创建资源管理器 */
        public static WarConsoleRes CreateRes(WarRoom room)
        {
            return new WarConsoleRes(room);
        }

        /** 创建视图代理 */
        public static WarConsoleViewAgent CreateViewAgent(WarRoom room)
        {
            WarConsoleViewAgent view = new WarConsoleViewAgent();
            view.room = room;
            return view;
        }

    }
}
#endif