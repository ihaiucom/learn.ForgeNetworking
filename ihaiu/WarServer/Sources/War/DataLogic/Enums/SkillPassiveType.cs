using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/17/2017 9:22:02 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 被动技能触发方式
    /// </summary>
    public enum SkillPassiveType
    {
        [Help("无")]
        None = 0,

        [Help("普攻")]
        NormalAttack = 1,

        [Help("技能攻击")]
        SkillAttack = 2,

        [Help("出生时")]
        Birth = 3,
    }
}
