using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 7:18:49 PM
*  @Description:    光环(法术场)
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 光环
    /// </summary>
    public class Halo : AbstractEffect
    {
        public Action<Halo> sOnStopCallback = null;

        public Action<UnitData> sOnUnitEnterCallback = null;
        public Action<UnitData> sOnUnitExitCallback = null;




        public List<EffectConfig> startEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> unitEnterEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> unitExitEffectConfigList = new List<EffectConfig>();
        public List<EffectConfig> pulsetEffectConfigList = new List<EffectConfig>();



        public List<AbstractEffect> startEffectList = new List<AbstractEffect>();
        public List<AbstractEffect> unitEnterEffectList = new List<AbstractEffect>();
        public List<AbstractEffect> pulsetEffectList = new List<AbstractEffect>();

        public Dictionary<UnitData, List<AbstractEffect>> unitEnterEffectsDict = new Dictionary<UnitData, List<AbstractEffect>>();



        /** 生命 */
        public float life = -1;
        /** 区域 */
        public Area area;
        /** 除了startEffectList外，的延迟效果 */
        public float delay = 0;



        /** 势力ID */
        public int legionId = 0;
        /** 单位类型 */
        public UnitType unitType        = UnitType.None;
        /** 关系类型 */
        public RelationType relation    = RelationType.None;




        #region check
        /** 单位进入 检测时间*/
        public float checkEnterCD = 0.1f;
        /** 单位进入 一次检测数量 */
        public int checkEnterOnceCount = -1;

        /** 单位离开 检测时间*/
        public float checkExitCD = 0.1f;
        /** 单位离开 一次检测数量 */
        public int checkExitOnceCount = -1;


        /** 脉冲 检测时间*/
        public float pulseCD = 1f;
        /** 脉冲 一次检测数量 */
        public int pulseOnceCount = -1;
        /** 是否需要脉冲列表 */
        public bool isPulseItem = false;

        #endregion




        /** 是否启动 */
        protected bool isStarted;
        /** 检测时间 */
        private float _time = 0;

        /** 范围内的单位 */
        public List<UnitData> unitList = new List<UnitData>();


        public HaoleConfig haoleConfig;
        public override AbstractEffect SetConfig(EffectConfig config)
        {
            haoleConfig = (HaoleConfig)config;
            life = haoleConfig.life;
            delay = haoleConfig.delay;
            area = haoleConfig.area;
            legionId = haoleConfig.legionId;
            unitType = haoleConfig.unitType;
            relation = haoleConfig.relation;

            checkEnterCD = haoleConfig.checkEnterCD;
            checkEnterOnceCount = haoleConfig.checkEnterOnceCount;

            checkExitCD = haoleConfig.checkExitCD;
            checkExitOnceCount = haoleConfig.checkExitOnceCount;

            pulseCD = haoleConfig.pulseCD;
            pulseOnceCount = haoleConfig.pulseOnceCount;
            isPulseItem = haoleConfig.isPulseItem;

            return base.SetConfig(config);
        }



        /** 初始化 */
        virtual public void Init()
        {
        }

        /** 销毁 */
        virtual public void Destroy()
        {
            Stop();

            sOnUnitEnterCallback = null;
            sOnUnitExitCallback = null;
        }


        /** 启动 */
        override public void Start()
        {
            if (isStarted)
                return;

            isStarted = true;

            OnStart();
        }

        virtual protected void OnStart()
        {
            StartCheckEnter();
            StartCheckExit();
            StartCheckPulse();
            StartCheckLife();


            for (int i = 0; i < startEffectConfigList.Count; i++)
            {
                AbstractEffect effect = room.haoleBuff.CreateEffect(this.caster, this.caster, startEffectConfigList[i]);
                startEffectList.Add(effect);
                effect.Start();
            }

            this.caster.haoleContainer.Add(this);
        }


        /** 停止 */
        override public void Stop()
        {
            if (isStarted == false)
                return;


            if (sOnStopCallback != null)
                sOnStopCallback(this);

            for (int i = startEffectList.Count - 1; i >= 0; i--)
            {
                AbstractEffect effect = startEffectList[i];
                effect.Stop();
                startEffectList.Remove(effect);
            }


            for (int i = pulsetEffectList.Count - 1; i >= 0; i--)
            {
                AbstractEffect effect = pulsetEffectList[i];
                effect.Stop();
                pulsetEffectList.Remove(effect);
            }

            for (int i = unitEnterEffectList.Count - 1; i >= 0; i--)
            {
                AbstractEffect effect = unitEnterEffectList[i];
                effect.Stop();
                unitEnterEffectList.Remove(effect);
            }



            StopCheckEnter();
            StopCheckExit();
            StopCheckPulse();
            StopCheckLife();



            UnitData unit;
            for (int i = unitList.Count - 1; i >= 0; i--)
            {
                unit = unitList[i];
                if (unit == null)
                {
                    unitList.RemoveAt(i);
                    continue;
                }

                OnUnitExit(unit);
            }

            isStarted = false;
            unit = null;






            startEffectList.Clear();
            unitEnterEffectList.Clear();
            pulsetEffectList.Clear();
            unitEnterEffectsDict.Clear();
        }


        /// <summary>
        /// 临时用协程
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return room.drive.StartCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            room.drive.StopCoroutine(coroutine);
        }

        #region check enter
        protected Coroutine enterCoroutine;
        protected bool enterRuning;
        virtual protected void StartCheckEnter()
        {
            StopCheckEnter();
            enterRuning = true;
            enterCoroutine = StartCoroutine(OnCheckEnter());
        }

        virtual protected void StopCheckEnter()
        {
            if (enterCoroutine != null)
            {
                StopCoroutine(enterCoroutine);
                enterCoroutine = null;
            }
            enterRuning = false;
        }

        virtual protected IEnumerator OnCheckEnter()
        {

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            while (enterRuning)
            {

                if (area == null)
                    yield break;

                if (checkEnterCD > 0)
                {
                    yield return new WaitForSeconds(checkEnterCD);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }

                int loopNum = 0;

                List<UnitData> list = room.sceneData.SearchUnit(legionId, unitType,  relation);
                UnitData unit;
                for (int i = 0; i < list.Count; i++)
                {
                    unit = list[i];
                    if (unit == null || !unit.unitData.IsInSceneAndLive || unitList.Contains(unit))
                        continue;
                    if (area.Contains(unit.position, unit.unitRadius))
                    {
                        OnUnitEnter(unit);
                    }

                    loopNum++;

                    if (checkEnterOnceCount > 0 && loopNum >= checkEnterOnceCount)
                    {
                        loopNum = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }

                unit = null;
            }
        }
        #endregion




        #region check exit
        protected Coroutine exitCoroutine;
        protected bool exitRuning;
        virtual protected void StartCheckExit()
        {
            StopCheckExit();
            exitRuning = true;
            exitCoroutine = StartCoroutine(OnCheckExit());
        }

        virtual protected void StopCheckExit()
        {
            if (exitCoroutine != null)
            {
                StopCoroutine(exitCoroutine);
                exitCoroutine = null;
            }
            exitRuning = false;
        }

        virtual protected IEnumerator OnCheckExit()
        {
            while (exitRuning)
            {
                if (checkExitCD > 0)
                {
                    yield return new WaitForSeconds(checkEnterCD);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }

                int loopNum = 0;

                UnitData unit;
                for (int i = unitList.Count - 1; i >= 0; i--)
                {
                    unit = unitList[i];

                    if (unit == null)
                    {
                        unitList.RemoveAt(i);
                        continue;
                    }

                    if ( !unit.IsInSceneAndLive || !area.Contains(unit.position, unit.unitRadius))
                    {
                        OnUnitExit(unit);
                    }

                    loopNum++;

                    if (checkExitOnceCount > 0 && loopNum >= checkExitOnceCount)
                    {
                        loopNum = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }

                unit = null;
            }
        }
        #endregion




        #region check pulse
        protected Coroutine pulseCoroutine;
        protected bool pulseRuning;
        virtual protected void StartCheckPulse()
        {
            StopCheckPulse();
            pulseRuning = true;
            pulseCoroutine = StartCoroutine(OnCheckPulse());
        }

        virtual protected void StopCheckPulse()
        {
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
                pulseCoroutine = null;
            }
            pulseRuning = false;
        }

        virtual protected IEnumerator OnCheckPulse()
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            while (pulseRuning)
            {
                if (pulseCD > 0)
                {
                    yield return new WaitForSeconds(pulseCD);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }

                OnPulse();

                if (isPulseItem)
                {
                    int loopNum = 0;

                    UnitData unit;
                    for (int i = unitList.Count - 1; i >= 0; i--)
                    {
                        unit = unitList[i];

                        if (unit == null)
                        {
                            continue;
                        }

                        OnUnitPulse(unit, i);



                        loopNum++;

                        if (pulseOnceCount > 0 && loopNum >= pulseOnceCount)
                        {
                            loopNum = 0;
                            yield return new WaitForEndOfFrame();
                        }
                    }

                    unit = null;
                }
            }
        }
        #endregion


        #region check life
        protected Coroutine lifeCoroutine;
        protected bool lifeRuning;
        private float lifeTime = 0;
        virtual protected void StartCheckLife()
        {
            StopCheckLife();
            lifeRuning = true;
            lifeTime = 0;
            lifeCoroutine = StartCoroutine(OnCheckLife());
        }

        virtual protected void StopCheckLife()
        {
            if (lifeCoroutine != null)
            {
                StopCoroutine(lifeCoroutine);
                lifeCoroutine = null;
            }
            lifeRuning = false;
        }

        virtual protected IEnumerator OnCheckLife()
        {
            while (lifeRuning)
            {
                yield return new WaitForSeconds(0.5f);
                lifeTime += 0.5f;
                if (life > 0 && lifeTime >= life)
                {
                    lifeRuning = false;
                    Stop();
                    break;
                }
            }
        }
        #endregion






        /** 单位进入 */
        virtual protected void OnUnitEnter(UnitData unit)
        {
            //LLL.LL("OnUnitEnter");
            unitList.Add(unit);

            if (sOnUnitEnterCallback != null)
            {
                sOnUnitEnterCallback(unit);
            }


            for (int i = 0; i < unitEnterEffectConfigList.Count; i++)
            {
                if (unitEnterEffectConfigList[i].unitType.UContain(unit.unitType) == false)
                    continue;

                AbstractEffect effect = room.haoleBuff.CreateEffect(unit, this.caster, unitEnterEffectConfigList[i]);
                unitEnterEffectList.Add(effect);
                effect.Start();

                List<AbstractEffect> unitEffectList;
                if (!unitEnterEffectsDict.TryGetValue(unit, out unitEffectList))
                {
                    unitEffectList = new List<AbstractEffect>();
                    unitEnterEffectsDict.Add(unit, unitEffectList);
                }
                unitEffectList.Add(effect);
            }
        }


        /** 单位出去 */
        virtual protected void OnUnitExit(UnitData unit)
        {
            //LLL.LL("OnUnitExit");
            unitList.Remove(unit);

            if (sOnUnitExitCallback != null)
            {
                sOnUnitExitCallback(unit);
            }


            List<AbstractEffect> unitEffectList;
            if (unitEnterEffectsDict.TryGetValue(unit, out unitEffectList))
            {
                for (int i = unitEffectList.Count - 1; i >= 0; i--)
                {
                    AbstractEffect effect = unitEffectList[i];
                    effect.Stop();
                    unitEffectList.Remove(effect);
                    unitEnterEffectList.Remove(effect);
                }

                unitEffectList.Clear();
                unitEnterEffectsDict.Remove(unit);
            }

            if (isStarted)
            {
                if (unit != null && unit.IsInSceneAndLive)
                {
                    for (int i = 0; i < unitExitEffectConfigList.Count; i++)
                    {
                        if (unitExitEffectConfigList[i].unitType.UContain(unit.unitType) == false)
                            continue;

                        AbstractEffect effect = room.haoleBuff.CreateEffect(unit, this.caster, unitExitEffectConfigList[i]);
                        effect.Start();
                    }
                }
            }
        }


        /** 单位脉冲 */
        virtual protected void OnUnitPulse(UnitData unit, int unitIndex)
        {
           
        }


        /** 脉冲 */
        virtual protected void OnPulse()
        {
            for (int i = pulsetEffectList.Count - 1; i >= 0; i--)
            {
                AbstractEffect effect = pulsetEffectList[i];
                if (effect == null)
                    continue;

                effect.Stop();
                pulsetEffectList.Remove(effect);
            }

            pulsetEffectList.Clear();

            for (int i = 0; i < pulsetEffectConfigList.Count; i++)
            {
                if (pulsetEffectConfigList[i] == null)
                    continue;

                AbstractEffect effect = room.haoleBuff.CreateEffect(this.caster, this.caster, pulsetEffectConfigList[i]);
                if (effect == null)
                    continue;

                pulsetEffectList.Add(effect);
                effect.Pulse(unitList, caster);
            }
        }

    }
}
