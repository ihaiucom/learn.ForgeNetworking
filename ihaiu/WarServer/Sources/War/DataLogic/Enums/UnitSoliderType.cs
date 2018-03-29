using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/16/2017 6:14:30 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位--士兵类型
    /// </summary>
    [Flags]
    public enum UnitSoliderType
    {
        /** 无 */
        None = 0,

        /** 小怪 */
        General = 1,

        /** 精英 */
        Elite = 2,

        /** Boss */
        Boss = 4,

        /** 小怪和精英 */
        GeneralAndElite = 3,

        /** 所有 */
        All = 7,
    }
}
