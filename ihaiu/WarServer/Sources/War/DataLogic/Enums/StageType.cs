using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 4:05:33 PM
*  @Description:    关卡类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 关卡类型 */
    public enum StageType
    {
        /** 副本模式 */
        Dungeon,

        /** 多人打副本 */
        PVE,

        /** 对战模式 */
        PVP,

        /** pvp天梯模式*/
        PVPLadder,

        /** pve活动模式 */
        PVEActivity,
    }
}
