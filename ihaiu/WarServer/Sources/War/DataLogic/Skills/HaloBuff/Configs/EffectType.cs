using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:22:21 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 效果类型
    /// </summary>
    public enum EffectType
    {

        [HelpAttribute("光环效果")]
        Haole,

        [HelpAttribute("Buff效果")]
        Buff,

        [HelpAttribute("状态--美术效果")]
        FxStateEffect,

        [HelpAttribute("状态--属性效果")]
        PropStateEffect,

        [HelpAttribute("伤害效果")]
        DamageEffect,

        [HelpAttribute("仇恨效果")]
        HatredEffect,
    }
}
