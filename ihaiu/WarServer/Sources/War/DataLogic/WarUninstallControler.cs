using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 3:12:15 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗卸载控制器 */
    public class WarUninstallControler : AbstractRoomObject
    {
        public WarUninstallControler(WarRoom room)
        {
            this.room = room;
        }

        /** 启动 */
        public void Start()
        {
            // 释放对象
        }

        /** 释放对象池 */
        private void ReleasePool()
        {
            ClassObjPool<LegionGroupData>.ReleaseAll();
            ClassObjPool<LegionData>.ReleaseAll();
        }
    }
}
