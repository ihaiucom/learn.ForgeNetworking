using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:48:58 PM
*  @Description:    单位类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 单位类型 */
    [System.Flags]
    public enum UnitType
    {
        /** 没有 */
        None    = 0,

        /** 英雄 */
        Hero    = 1,

        /** 士兵 */
        Solider = 2,

        /** 建筑 */
        Build   = 4,

        /** 玩家 */
        Player = 8,

        /** 光环 */
        Halo = 16,

        /** 所有 */
        All     = 15,
        /** 建筑和玩家组合 **/
        BuildAndPlayer = 5,
    }

    public enum UnitTypeId
    {
        /** 没有 */
        None = 0,

        /** 玩家 */
        Player = 8,

        /** 英雄 */
        Hero = 1,

        /** 士兵 */
        Solider = 2,

        /** 其他物体 */
        Object = 16,

        /** 建筑--主基地 */
        Build_Mainbase = 16,

        /** 建筑--机关--攻击类型 */
        Build_Tower_Attack = 16,

        /** 建筑--机关--防御类型 */
        Build_Tower_Defense = 16,

        /** 建筑--机关--辅助类型 */
        Build_Tower_Auxiliary = 16,

        /** 所有机关 */
        Build_Tower_All,

        /** 所有建筑 */
        Build_All,

        All,

    }
}
