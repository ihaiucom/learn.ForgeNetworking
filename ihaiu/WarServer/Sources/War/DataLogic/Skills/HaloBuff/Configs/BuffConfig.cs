using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:31:38 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// Buff效果
    /// </summary>
    public class BuffConfig : EffectConfig
    {
        public HaloBuff haloBuff        = null;

        public BuffType buffType;

        /** 脉冲时间 */
        public float cd = -1;
        /** 生命 */
        public float life = -1;
        /** 是否可以叠加 */
        public bool superposition = true;

        public List<EffectConfig> firtAddEffectList = new List<EffectConfig>();
        public List<EffectConfig> addEffectList = new List<EffectConfig>();
        public List<EffectConfig> pulseEffectList = new List<EffectConfig>();


        public BuffConfig()
        {
            effectType = EffectType.Buff;
        }

        //public BuffConfig(BuffInfo buffInfo)
        //{
        //    effectType = EffectType.Buff;
        //    buffType = BuffType.PropState;
        //    life = buffInfo.mLife;
        //    cd = buffInfo.mCD;
        //    superposition = buffInfo.mSuperposition;
        //}

        public BuffConfig(HaloBuff haloBuff, float life)
        {
            effectType = EffectType.Buff;
            buffType = BuffType.PropState;
            switch (haloBuff.buffTriggerType)
            {
                case BuffTriggerType.Disposable:
                    {
                        BuffTriggerConfigDisposable config = (BuffTriggerConfigDisposable)haloBuff.buffTriggerConfig;
                        this.life = life;
                    }
                    break;
                case BuffTriggerType.Periodic:
                    {
                        BuffTriggerConfigPeriodic config = (BuffTriggerConfigPeriodic)haloBuff.buffTriggerConfig;
                        this.life = life;
                        cd = config.cd;
                    }
                    break;
                case BuffTriggerType.Frequency:
                    {
                        this.haloBuff = haloBuff;
                    }
                    break;
            }
            superposition = haloBuff.buffTriggerConfig.superposition;
        }


        public BuffConfig(BuffType buffType, float life = -1, float cd = -1, bool superposition = true)
        {
            effectType = EffectType.Buff;
            this.buffType = buffType;
            this.cd = cd;
            this.life = life;
            this.superposition = superposition;
        }

    }

}
