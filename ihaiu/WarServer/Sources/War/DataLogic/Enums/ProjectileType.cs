using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 7:22:12 PM
*  @Description:    投射(弹道) 类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 投射(弹道) 类型 */
    public enum ProjectileType
    {
        /** 直线 */
        Beeline,
        /** 抛物线 */
        Parabola,
        /** 追击(直接命中/锁定目标) */
        Chase,
    }
}
