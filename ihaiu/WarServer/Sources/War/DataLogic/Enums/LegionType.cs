using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 4:30:02 PM
*  @Description:    势力类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 势力类型 */
    [Flags]
    public enum LegionType
    {
        /** 没有 */
        None            = 0,

        /** 中立 */
        Neutral         = 1,

        /** [副本，PVE] 怪物 */
        Monster         = 2,

        /** 玩家 */
        Player          = 4,



        // 玩家和怪物
        PlayerAndMonster = 6,

        // 所有
        All              = 7,
    }
}
