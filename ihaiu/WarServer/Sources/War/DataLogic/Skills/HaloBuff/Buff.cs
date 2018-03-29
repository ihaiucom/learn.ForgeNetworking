using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 7:15:58 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{

    public class Buff : AbstractEffect
    {
        public BuffType buffType;

        /** 脉冲时间 */
        public float cd = -1;
        /** 生命 */
        public float life = -1;
        /** 是否可以叠加 */
        public bool superposition = true;

        public List<HaloBuff>               haloBuffList            = new List<HaloBuff>();
        public Dictionary<int, int>         haloBuffDic             = new Dictionary<int, int>();

        /// <summary>
        /// 当单位没有该类型BUFF时有效
        /// </summary>
        public List<AbstractEffect> firtAddEffectList = new List<AbstractEffect>();

        /// <summary>
        /// 单位添加该BUFF时生效
        /// </summary>
        public List<AbstractEffect> addEffectList = new List<AbstractEffect>();

        /// <summary>
        /// Buff每过脉冲时间执行一次
        /// </summary>
        public List<AbstractEffect> pulseEffectList = new List<AbstractEffect>();

        public BuffContainer buffContainer;
        public bool willRemove;


        public override AbstractEffect SetConfig(EffectConfig config)
        {
            BuffConfig c = (BuffConfig)config;
            buffType = c.buffType;
            cd = c.cd;
            life = c.life;
            superposition = c.superposition;

            return base.SetConfig(config);
        }


        override public void Start()
        {
            if (!superposition && addEffectList.Count > 0 && unit.buffContainer.buffDict.Count > 0)
            {
                foreach (List<Buff> itemss in unit.buffContainer.buffDict.Values)
                {
                    for (int i = 0; i < itemss.Count; i++)
                    {
                        foreach (AbstractEffect item in itemss[i].addEffectList)
                        {
                            PropStateEffectConfig items = (PropStateEffectConfig)item.config;
                            for (int ll = addEffectList.Count - 1; ll >= 0; ll--)
                            {
                                PropStateEffectConfig newItem = (PropStateEffectConfig)addEffectList[ll].config;
                                if (newItem.attachPropData.haloBuffId == items.attachPropData.haloBuffId)
                                {
                                    addEffectList.RemoveAt(ll);
                                }
                            }
                        }
                    }
                }
                if (addEffectList.Count <= 0)
                {
                    return;
                }
            }

            unit.buffContainer.AddBuff(this);
        }

        override public void Stop()
        {
            unit.buffContainer.RemoveBuff(this);
        }



        private float _cd = 0;
        private float _life = 0;
        virtual public void Update()
        {

            if (cd > 0)
            {
                _cd += Time.deltaTime;
                if (_cd >= cd)
                {
                    _cd = _cd - cd;
                    Pulse();
                }
            }

            if (life > 0)
            {
                _life += Time.deltaTime;
                if (_life >= life)
                {
                    _life = _life - life;
                    Stop();
                }
            }
            else if (life == 0)
            {
                Stop();
            }
        }

        virtual public void OnContainerAdd(BuffContainer container)
        {
            this.buffContainer = container;


            for (int i = 0; i < addEffectList.Count; i++)
            {
                addEffectList[i].Start(unit, caster);
            }
        }

        virtual public void OnContainerRemove()
        {

            for (int i = 0; i < addEffectList.Count; i++)
            {
                addEffectList[i].Stop();
            }
        }


        virtual public void FirstAdd()
        {
            for (int i = 0; i < firtAddEffectList.Count; i++)
            {
                firtAddEffectList[i].Start(unit, caster);
            }
        }

        virtual public void LastRemove()
        {
            for (int i = 0; i < firtAddEffectList.Count; i++)
            {
                firtAddEffectList[i].Stop();
            }
        }

        override public void Pulse()
        {
            for (int i = 0; i < pulseEffectList.Count; i++)
            {
                pulseEffectList[i].Pulse(unit, caster);
            }
        }

    }
}
