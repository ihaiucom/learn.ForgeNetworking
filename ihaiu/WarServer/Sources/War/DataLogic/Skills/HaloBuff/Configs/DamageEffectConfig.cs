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
    /// 伤害效果配置
    /// </summary>
    public class DamageEffectConfig : EffectConfig
    {
        //public float damage = 10;
        //public float damageper = 0;
        public DamageType damageType;
        public DamageInfBaseCSV damageInfBaseCSV;

        public DamageEffectConfig()
        {

            effectType = EffectType.DamageEffect;
        }

        /// <summary>
        /// 伤害或百分比伤害
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageper"></param>
        public DamageEffectConfig(float damage, DamageType damageType)
        {
            effectType = EffectType.DamageEffect;
            //this.damage = damage;
            this.damageType = damageType;
        }
        public DamageEffectConfig(DamageInfBaseCSV damageInfBaseCSV, DamageType damageType)
        {
            effectType = EffectType.DamageEffect;
            this.damageInfBaseCSV = damageInfBaseCSV;
            this.damageType = damageType;
        }

    }
}
