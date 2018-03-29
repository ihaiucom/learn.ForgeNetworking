using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/16/2017 9:03:28 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    ////
    /// 主动施法方式: 0无效1直线 2扇形3身边圆形区域4目标圆形区域5对自己6方向瞄准
    public enum SkillCastType
    {
        [Help("无效")]
        None = 0,

        [Help("直线")]
        Line = 1,

        [Help("扇形")]
        Fan = 2,

        [Help("身边圆形区域")]
        CircleSelf = 3,

        [Help("身边圆形区域")]
        CircleTarget = 4,

        [Help("自己")]
        Self = 5,
    }
}
