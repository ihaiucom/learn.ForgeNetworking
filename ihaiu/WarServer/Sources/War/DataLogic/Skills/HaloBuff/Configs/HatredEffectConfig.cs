using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 8:16:34 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 仇恨效果配置
    /// </summary>
    public class HatredEffectConfig : EffectConfig
    {
        public int hatred = 100;
        public HatredEffectConfig()
        {

            effectType = EffectType.HatredEffect;
        }

        public HatredEffectConfig(int hatred)
        {
            effectType = EffectType.HatredEffect;
            this.hatred = hatred;
        }

    }
}
