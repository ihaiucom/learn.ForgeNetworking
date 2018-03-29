using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 4:44:05 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位--职业类型
    /// </summary>
    [Flags]
    public enum UnitProfessionType
    {
        /** 未知 */
        None = 0,

        /** 坦克 */
        Tank = 1,

        /** 射手 */
        Shooter = 2,

        /** 法师 */
        Master = 4,

        /** 术士 */
        Warlock = 8,

        /** 所有 */
        All = 15,
    }
}
