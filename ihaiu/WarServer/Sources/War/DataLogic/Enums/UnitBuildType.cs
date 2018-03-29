using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:52:55 PM
*  @Description:    单位--建筑类型
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位--建筑类型
    /// </summary>
    [Flags]
    public enum UnitBuildType
    {
        /** 无 */
        None            = 0,

        /** 基地 */
        Mainbase        = 1,

        /** 机关--攻击类型 */
        Tower_Attack    = 2,

        /** 机关--防御类型 */
        Tower_Defense   = 4,

        /** 机关--能量塔 */
        Tower_Auxiliary = 8,

        /** 机关--门类型 */
        Tower_Door      = 16,

        /** 机关--开关类型 */
        Tower_Switch    = 32,

        /** 所有机关 */
        Tower           = 30,

        // 所有
        All             = 31,
    }
}
