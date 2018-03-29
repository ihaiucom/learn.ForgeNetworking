using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 4:47:53 PM
*  @Description:    关系类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 关系类型 */
    [System.Flags]
    public enum RelationType
    {
        /** 未知 */
        None = 0,

        /** 仅自己 */
        Own             = 1,

        /** 仅队友 */
        Friendly        = 2,

        /** 仅敌人 */
        Enemy           = 4,




        // 队友和自己
        // Own | Friendly 
        OwnAndFriendly  = 3,

        // 队友和自己
        // Own | Friendly | Enemy
        All             = 7,
    }
}
