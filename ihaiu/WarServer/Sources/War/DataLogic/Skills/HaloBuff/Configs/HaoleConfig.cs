using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:36:28 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 光环
    /// </summary>
    public class HaoleConfig : EffectConfig
    {
        public HaloBuff     haloBuff        = null;
        /** 生命 */
        public float        life = -1;
        /** 区域 */
        public Area         area;
        /** 除了startEffectList外，的延迟效果 */
        public float        delay = 0;


        /** 势力ID */
        public int              legionId = 0;
        /** 关系类型 */
        public RelationType     relation = RelationType.All;

        public List<EffectConfig> startEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> unitEnterEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> unitExitEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> pulsetEffectConfigList = new List<EffectConfig>();



        #region check
        /** 单位进入 检测时间*/
        public float    checkEnterCD = 0.1f;
        /** 单位进入 一次检测数量 */
        public int      checkEnterOnceCount = -1;

        /** 单位离开 检测时间*/
        public float    checkExitCD = 0.1f;
        /** 单位离开 一次检测数量 */
        public int      checkExitOnceCount = -1;


        /** 脉冲 检测时间*/
        public float    pulseCD = 1f;
        /** 脉冲 一次检测数量 */
        public int      pulseOnceCount = -1;
        /** 是否需要脉冲列表 */
        public bool     isPulseItem = false;

        #endregion

        //public HaoleConfig(BuffInfo buffInfo,Vector3 InitPos)
        //{
        //    effectType = EffectType.Haole;
        //    life = buffInfo.mLife;
        //    delay = buffInfo.mCD;
        //    area = new CircleArea(InitPos, buffInfo.mRadius);
        //}

        public HaoleConfig(HaloBuff haloBuff,Vector3 InitPos)
        {
            effectType = EffectType.Haole;

            switch (haloBuff.buffTriggerType)
            {
                case BuffTriggerType.Disposable:
                    {
                        BuffTriggerConfigDisposable config = (BuffTriggerConfigDisposable)haloBuff.buffTriggerConfig;
                        life = config.life;
                    }
                    break;
                case BuffTriggerType.Periodic:
                    {
                        BuffTriggerConfigPeriodic config = (BuffTriggerConfigPeriodic)haloBuff.buffTriggerConfig;
                        life = config.life;
                        delay = config.cd;
                    }
                    break;
                case BuffTriggerType.Frequency:
                    {
                        this.haloBuff = haloBuff;
                    }
                    break;
            }
            area = new CircleArea(InitPos, haloBuff.buffTriggerConfig.radius);
        }

        public HaoleConfig SetLife(float life)
        {
            this.life = life;
            return this;
        }

        public HaoleConfig SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        public HaoleConfig SetArea(Vector3 pos, float radius)
        {
            area = new CircleArea(pos, radius);
            return this;
        }

        public HaoleConfig SetUnit(int legionId, UnitType unitType, RelationType relation)
        {
            this.legionId = legionId;
            this.unitType = unitType;
            this.relation = relation;
            return this;
        }

        public HaoleConfig SetCheckEnter(float checkEnterCD, int checkEnterOnceCount = -1)
        {
            this.checkEnterCD = checkEnterCD;
            this.checkEnterOnceCount = checkEnterOnceCount;
            return this;
        }


        public HaoleConfig SetCheckExit(float checkExitCD, int checkExitOnceCount = -1)
        {
            this.checkExitCD        = checkExitCD;
            this.checkExitOnceCount = checkExitOnceCount;
            return this;
        }


        public HaoleConfig SetCheckPulse(float pulseCD, int pulseOnceCount = -1, bool isPulseItem = false)
        {
            this.pulseCD                = pulseCD;
            this.pulseOnceCount         = pulseOnceCount;
            this.isPulseItem            = isPulseItem;
            return this;
        }



    }

}
