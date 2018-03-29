using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 4:32:07 PM
*  @Description:    单位--空间类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 单位--空间类型 */
    [Flags]
    public enum UnitSpaceType
    {
        /** 地面 */
        Ground = 1,

        /** 飞行 */
        Fly = 2,

        /** 所有 */
        All = 3
    }
}
