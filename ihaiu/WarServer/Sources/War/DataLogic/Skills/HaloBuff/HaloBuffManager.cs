using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 7:12:36 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class HaloBuffManager : AbstractRoomObject
    {

        public HaloBuffManager(WarRoom room)
        {
            this.room = room;
        }

        public string Identifier_Effect = "e";

        public int EID_ADD = 0;



        public string GetEffectIdentifier()
        {
            return Identifier_Effect + (EID_ADD++);
        }

        /// <summary>
        /// 创建效果
        /// </summary>
        /// <param name="unit">拥有者单位</param>
        /// <param name="caster">施法者单位</param>
        /// <param name="effectConfig">配置</param>
        /// <returns></returns>
        public AbstractEffect CreateEffect(UnitData unit, UnitData caster, EffectConfig effectConfig)
        {
            AbstractEffect effect = null;
            switch (effectConfig.effectType)
            {
                case EffectType.Haole:
                    HaoleConfig haoleConfig = (HaoleConfig)effectConfig;
                    Halo haole = new Halo();
                    for (int i = 0; i < haoleConfig.startEffectConfigList.Count; i++)
                    {
                        haole.startEffectConfigList.Add(haoleConfig.startEffectConfigList[i]);
                    }


                    for (int i = 0; i < haoleConfig.unitEnterEffectConfigList.Count; i++)
                    {
                        haole.unitEnterEffectConfigList.Add(haoleConfig.unitEnterEffectConfigList[i]);
                    }


                    for (int i = 0; i < haoleConfig.unitExitEffectConfigList.Count; i++)
                    {
                        haole.unitExitEffectConfigList.Add(haoleConfig.unitExitEffectConfigList[i]);
                    }


                    for (int i = 0; i < haoleConfig.pulsetEffectConfigList.Count; i++)
                    {
                        haole.pulsetEffectConfigList.Add(haoleConfig.pulsetEffectConfigList[i]);
                    }

                    effect = haole;
                    break;
                case EffectType.Buff:
                    BuffConfig buffConfig = (BuffConfig)effectConfig;
                    Buff buff = new Buff();

                    for (int i = 0; i < buffConfig.firtAddEffectList.Count; i++)
                    {
                        buff.firtAddEffectList.Add(CreateEffect(unit, caster, buffConfig.firtAddEffectList[i]));
                    }

                    for (int i = 0; i < buffConfig.addEffectList.Count; i++)
                    {
                        buff.addEffectList.Add(CreateEffect(unit, caster, buffConfig.addEffectList[i]));
                    }


                    for (int i = 0; i < buffConfig.pulseEffectList.Count; i++)
                    {
                        buff.pulseEffectList.Add(CreateEffect(unit, caster, buffConfig.pulseEffectList[i]));
                    }

                    HaloBuff halobuff = buff.haloBuffList.Find(m => m.HaloBuffId == buffConfig.haloBuff.HaloBuffId);
                    if (halobuff != null)
                    {
                        if (buff.haloBuffDic.ContainsKey(buffConfig.haloBuff.HaloBuffId))
                        {
                            buff.haloBuffDic[buffConfig.haloBuff.HaloBuffId] += 10;
                        }
                        else
                        {
                            buff.haloBuffDic.Add(buffConfig.haloBuff.HaloBuffId, 10);
                        }
                    }
                    else if (buffConfig.haloBuff != null)
                    {
                        buff.haloBuffList.Add(buffConfig.haloBuff);
                        buff.haloBuffDic.Add(buffConfig.haloBuff.HaloBuffId, 10);
                    }

                    effect = buff;
                    break;

                case EffectType.FxStateEffect:
                    effect = new FxStateEffect();
                    break;


                case EffectType.PropStateEffect:
                    effect = new PropStateEffect();
                    break;
                case EffectType.DamageEffect:
                    effect = new DamageEffect();
                    break;
                case EffectType.HatredEffect:
                    effect = new HatredEffect();
                    break;

            }

            effect.identifier = GetEffectIdentifier();
            effect.room = room;
            effect.unit = unit;
            effect.caster = caster;
            effect.SetConfig(effectConfig);
            return effect;
        }


        public Halo CreateHaole(HaoleConfig haoleConfig, UnitData unit = null, UnitData send = null)
        {
            if (unit == null)
            {
                // 临时，同步需要放到服务器
                unit = room.creater.CreateHaloUnit(room.HALO_UNIT_UID, 0, haoleConfig.area.position, Vector3.zero);
            }
            Halo haole = (Halo)CreateEffect(unit, send, haoleConfig);
            return haole;
        }

        #region 添加光环
        //public Halo AddHalos(int legionId, AttackRule attackRule, BuffInfo buffInfo, Vector3 InitPos, int skilllv, UnitData unit = null, UnitData parentUnit = null)
        //{
        //    //GameObject ggg = new GameObject();
        //    //ggg.transform.position = InitPos;
        //    HaoleConfig haoleConfig = new HaoleConfig(buffInfo,InitPos)
        //        .SetUnit(legionId, attackRule.unitType, attackRule.relationType)
        //        .SetCheckEnter(0.1F, -1).SetCheckExit(0.1F, -1).SetCheckPulse(buffInfo.mCD, -1, true);

        //    if (buffInfo.mEffectPath != null && buffInfo.mEffectPath.Length > 1)
        //    {
        //        // 美术特效
        //        FxStateEffectConfig fxConfig = new FxStateEffectConfig(buffInfo.mEffectPath, Space.World, (buffInfo.mEffectPos + InitPos).SetY(room.clientOperationUnit.GetUnitAgent().position.y),true);
        //        // 第一次，这个特性可以不在这里加，可以有状态特性那去处理
        //        haoleConfig.startEffectConfigList.Add(fxConfig);
        //    }
        //    if (buffInfo.buffPropList != null && buffInfo.buffPropList.Count > 0)
        //    {
        //        for (int i = buffInfo.buffPropList.Count - 1; i >= 0; i--)
        //        {
        //            if (buffInfo.buffPropList[i].aimId == 0)
        //            {
        //                buffInfo.buffPropList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                buffInfo.buffPropList[i].valValue = Game.config.skillValue.GetConfigs(buffInfo.buffPropList[i].valId, skilllv);
        //            }
        //        }
        //        if (buffInfo.buffPropList.Count > 0)
        //        {
        //            PropStateEffectConfig addPropConfig = new PropStateEffectConfig(buffInfo.buffPropList);
        //            haoleConfig.unitEnterEffectConfigList.Add(addPropConfig);
        //            haoleConfig.unitExitEffectConfigList.Add(addPropConfig);
        //        }
        //    }

        //    if (unit == null)
        //    {
        //        // 临时，同步需要放到服务器
        //        unit = room.creater.CreateHaloUnit(room.HALO_UNIT_UID, 0, haoleConfig.area.position, Vector3.zero);
        //    }
        //    // 即时伤害
        //    buffInfo.damageInfBaseCSV.skillLv = skilllv;
        //    if (buffInfo.damageInfBaseCSV.selectTarget == SelectTarget.Self)
        //    {
        //        parentUnit.GetDamageInfoVal(ref buffInfo.damageInfBaseCSV);
        //    }

        //    DamageEffectConfig pulseDamageConfig2 = new DamageEffectConfig(buffInfo.damageInfBaseCSV,buffInfo.damageType);
        //    haoleConfig.unitEnterEffectConfigList.Add(pulseDamageConfig2);
        //    // 脉冲，持续生效伤害
        //    buffInfo.pulDamageInfBaseCSV.skillLv = skilllv;
        //    if (buffInfo.pulDamageInfBaseCSV.selectTarget == SelectTarget.Self)
        //    {
        //        parentUnit.GetDamageInfoVal(ref buffInfo.pulDamageInfBaseCSV);
        //    }
        //    DamageEffectConfig pulseDamageConfig = new DamageEffectConfig(buffInfo.pulDamageInfBaseCSV,buffInfo.damageTypePul);
        //    haoleConfig.pulsetEffectConfigList.Add(pulseDamageConfig);

        //    Halo haole = (Halo)CreateHaole( haoleConfig, unit, parentUnit);
        //    haole.Start();
        //    return haole;
        //}
        public Halo AddHalos(int legionId, AttackRule attackRule, HaloBuff haloBuff, Vector3 InitPos, int skilllv, UnitData unit, UnitData parentUnit, float skillLife)
        {
            float cd = -1;
            string path = "";
            Vector3 offset = Vector3.zero;
            switch (haloBuff.buffTriggerType)
            {
                case BuffTriggerType.Disposable:
                    {
                        path = haloBuff.buffTriggerConfig.effectPath;
                        offset = haloBuff.buffTriggerConfig.effectOffset;
                    }
                    break;
                case BuffTriggerType.Periodic:
                    {
                        path = haloBuff.buffTriggerConfig.effectPath;
                        offset = haloBuff.buffTriggerConfig.effectOffset;
                        BuffTriggerConfigPeriodic config = (BuffTriggerConfigPeriodic)haloBuff.buffTriggerConfig;
                        cd = config.cd;
                    }
                    break;
            }

            HaoleConfig haoleConfig = new HaoleConfig(haloBuff,InitPos)
                .SetUnit(legionId, attackRule.unitType, attackRule.relationType)
                .SetCheckEnter(0.1F, -1).SetCheckExit(0.1F, -1).SetCheckPulse(cd, -1, true);

            if (!string.IsNullOrEmpty(path) && path.Length > 1)
            {
                // 美术特效
                FxStateEffectConfig fxConfig = new FxStateEffectConfig(path, Space.World, (offset + InitPos).SetY(room.clientOperationUnit.GetUnitAgent().position.y),true);
                // 第一次，这个特性可以不在这里加，可以有状态特性那去处理
                haoleConfig.startEffectConfigList.Add(fxConfig);
            }
            if (haloBuff.buffPropList != null && haloBuff.buffPropList.Count > 0)
            {
                for (int i = haloBuff.buffPropList.Count - 1; i >= 0; i--)
                {
                    if (haloBuff.buffPropList[i].aimId == 0)
                    {
                        haloBuff.buffPropList.RemoveAt(i);
                    }
                    else
                    {
                        haloBuff.buffPropList[i].valValue = Game.config.skillValue.GetConfigs(haloBuff.buffPropList[i].valId, skilllv);
                    }
                }
                if (haloBuff.buffPropList.Count > 0)
                {
                    PropStateEffectConfig addPropConfig = new PropStateEffectConfig(haloBuff.HaloBuffId, haloBuff.buffPropList);
                    haoleConfig.unitEnterEffectConfigList.Add(addPropConfig);
                    haoleConfig.unitExitEffectConfigList.Add(addPropConfig);
                }
            }

            if (unit == null)
            {
                // 临时，同步需要放到服务器
                unit = room.creater.CreateHaloUnit(room.HALO_UNIT_UID, 0, haoleConfig.area.position, Vector3.zero);
            }

            // 即时伤害
            if (haloBuff.immediateDamage != null && haloBuff.immediateDamage.Count > 0)
            {
                for (int i = 0; i < haloBuff.immediateDamage.Count; i++)
                {
                    DamageInfBaseCSV dibc = haloBuff.immediateDamage[i];
                    dibc.skillLv = skilllv;
                    if (dibc.selectTarget == SelectTarget.Self)
                    {
                        unit.GetDamageInfoVal(ref dibc);
                    }
                    DamageEffectConfig damageEffectConfig = new DamageEffectConfig(dibc,dibc.damageType);
                    haoleConfig.unitEnterEffectConfigList.Add(damageEffectConfig);
                }
            }
            // 脉冲，持续生效伤害
            if (haloBuff.pulseDamage != null && haloBuff.pulseDamage.Count > 0)
            {
                for (int i = 0; i < haloBuff.pulseDamage.Count; i++)
                {
                    DamageInfBaseCSV dibc = haloBuff.pulseDamage[i];
                    dibc.skillLv = skilllv;
                    if (dibc.selectTarget == SelectTarget.Self)
                    {
                        unit.GetDamageInfoVal(ref dibc);
                    }
                    DamageEffectConfig damageEffectConfig = new DamageEffectConfig(dibc,dibc.damageType);
                    haoleConfig.pulsetEffectConfigList.Add(damageEffectConfig);
                }
            }

            Halo haole = (Halo)CreateHaole( haoleConfig, unit, parentUnit);
            haole.Start();
            return haole;
        }
        #endregion

        #region 添加buff
        public Buff AddBuffForUnit(UnitData unit, HaloBuff haloBuff, int skilllv, UnitData send, float skillLife)
        {
            if (haloBuff == null || unit == null || !unit.IsInSceneAndLive || unit.isCloneUnit)
            {
                return null;
            }


                float life = 0;
            if (haloBuff.buffTriggerType != BuffTriggerType.Frequency)
            {

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
                        }
                        break;
                }
                if (skillLife > 0)
                {
                    life = skillLife;
                }
            }
            //buff 配置
            BuffConfig buffConfig = new BuffConfig(haloBuff, life);


            int aimId = 0;
            // 附加属性
            if (haloBuff.buffPropList != null && haloBuff.buffPropList.Count > 0)
            {
                for (int i = haloBuff.buffPropList.Count - 1; i >= 0; i--)
                {
                    if (haloBuff.buffPropList[i].aimId == 0)
                    {
                        haloBuff.buffPropList.RemoveAt(i);
                    }
                    else
                    {
                        haloBuff.buffPropList[i].valValue = Game.config.skillValue.GetConfigs(haloBuff.buffPropList[i].valId, skilllv);
                        aimId = haloBuff.buffPropList[i].aimId;
                    }
                }
                if (haloBuff.buffPropList.Count > 0)
                {
                    PropStateEffectConfig addPropConfig = new PropStateEffectConfig(haloBuff.HaloBuffId, haloBuff.buffPropList);
                    buffConfig.addEffectList.Add(addPropConfig);
                }
            }

            if (haloBuff.buffTriggerType != BuffTriggerType.Frequency)
            {
                switch (haloBuff.buffTriggerType)
                {
                    case BuffTriggerType.Disposable:
                        {
                            //haloBuff.buffTriggerConfig.effectPath = EditorGUILayout.TextField("特效路径", haloBuff.buffTriggerConfig.effectPath);
                            //haloBuff.buffTriggerConfig.effectOffset = EditorGUILayout.Vector3Field("特效位置偏移", haloBuff.buffTriggerConfig.effectOffset);
                            //haloBuff.buffTriggerConfig.soundPath = EditorGUILayout.TextField("初次触发音效文件", haloBuff.buffTriggerConfig.soundPath);
                            BuffTriggerConfigDisposable config = (BuffTriggerConfigDisposable)haloBuff.buffTriggerConfig;
                            life = config.life;
                        }
                        break;
                    case BuffTriggerType.Periodic:
                        {
                            //haloBuff.buffTriggerConfig.effectPath = EditorGUILayout.TextField("特效路径", haloBuff.buffTriggerConfig.effectPath);
                            //haloBuff.buffTriggerConfig.effectOffset = EditorGUILayout.Vector3Field("特效位置偏移", haloBuff.buffTriggerConfig.effectOffset);
                            //haloBuff.buffTriggerConfig.soundPath = EditorGUILayout.TextField("初次触发音效文件", haloBuff.buffTriggerConfig.soundPath);
                            BuffTriggerConfigPeriodic config = (BuffTriggerConfigPeriodic)haloBuff.buffTriggerConfig;
                            life = config.life;
                            //config.cd = EditorGUILayout.FloatField("间隔CD", config.cd);
                            //config.pulSoundPath = EditorGUILayout.TextField("每次触发音效文件", config.pulSoundPath);
                        }
                        break;
                    case BuffTriggerType.Frequency:
                        {
                            //BuffTriggerConfigFrequency config = (BuffTriggerConfigFrequency)haloBuff.buffTriggerConfig;
                            //config.pulSoundPath = EditorGUILayout.TextField("每次触发音效文件", config.pulSoundPath);
                        }
                        break;
                }
                if (skillLife > 0)
                {
                    life = skillLife;
                }
                // 首次特效
                unit.unitAgent.unitStateEffect.PutBuffEffect(haloBuff.buffTriggerConfig.effectPath, life, haloBuff.buffTriggerConfig.effectOffset, aimId);
            }

            // 仇恨值
            if (haloBuff.hatredValue != 0)
            {
                HatredEffectConfig hatredConfig = new HatredEffectConfig(haloBuff.hatredValue);
                buffConfig.addEffectList.Add(hatredConfig);
            }

            // 即时伤害
            if (haloBuff.immediateDamage != null && haloBuff.immediateDamage.Count > 0)
            {
                for (int i = 0; i < haloBuff.immediateDamage.Count; i++)
                {
                    DamageInfBaseCSV dibc = haloBuff.immediateDamage[i];
                    dibc.skillLv = skilllv;
                    if (dibc.selectTarget == SelectTarget.Self)
                    {
                        unit.GetDamageInfoVal(ref dibc);
                    }
                    DamageEffectConfig damageEffectConfig = new DamageEffectConfig(dibc,dibc.damageType);
                    buffConfig.addEffectList.Add(damageEffectConfig);
                }
            }

            // 脉冲，持续生效伤害
            if (haloBuff.pulseDamage != null && haloBuff.pulseDamage.Count > 0)
            {
                for (int i = 0; i < haloBuff.pulseDamage.Count; i++)
                {
                    DamageInfBaseCSV dibc = haloBuff.pulseDamage[i];
                    dibc.skillLv = skilllv;
                    if (dibc.selectTarget == SelectTarget.Self)
                    {
                        unit.GetDamageInfoVal(ref dibc);
                    }
                    DamageEffectConfig damageEffectConfig = new DamageEffectConfig(dibc,dibc.damageType);
                    buffConfig.pulseEffectList.Add(damageEffectConfig);
                }
            }


            // 创建和开始运行buff
            Buff buff = (Buff) room.haoleBuff.CreateEffect(unit, send, buffConfig);
            buff.Start();
            return buff;
        }


        //public Buff AddOneBuffForUnit(UnitData unit, BuffInfo buffInfo, int skilllv, UnitData send)
        //{
        //    if (buffInfo == null || unit == null)
        //    {
        //        return null;
        //    }

        //    //buff 配置
        //    BuffConfig buffConfig = new BuffConfig(buffInfo);
        //    unit.unitAgent.unitStateEffect.PutBuffEffect(buffInfo.mEffectPath, buffInfo.mLife, buffInfo.mEffectPos);
        //    if (buffInfo.buffPropList != null && buffInfo.buffPropList.Count > 0)
        //    {
        //        for (int i = buffInfo.buffPropList.Count - 1; i >= 0; i--)
        //        {
        //            if (buffInfo.buffPropList[i].aimId == 0)
        //            {
        //                buffInfo.buffPropList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                buffInfo.buffPropList[i].valValue = Game.config.skillValue.GetConfigs(buffInfo.buffPropList[i].valId, skilllv);
        //                // buff 效果
        //                //unit.unitAgent.unitStateEffect.PutStateEffect(buffInfo.buffPropList[i].aimId, buffInfo.mEffectPath, buffInfo.mLife, buffInfo.mEffectPos, buffInfo.mSuperposition);
        //            }
        //        }
        //        if (buffInfo.buffPropList.Count > 0)
        //        {
        //            PropStateEffectConfig addPropConfig = new PropStateEffectConfig(buffInfo.buffPropList);
        //            buffConfig.addEffectList.Add(addPropConfig);
        //        }
        //    }
        //    // 仇恨值
        //    if (buffInfo.hatredValue != 0)
        //    {
        //        HatredEffectConfig hatredConfig = new HatredEffectConfig(buffInfo.hatredValue);
        //        buffConfig.addEffectList.Add(hatredConfig);
        //    }


        //    // 即时伤害
        //    buffInfo.damageInfBaseCSV.skillLv = skilllv;
        //    if (buffInfo.damageInfBaseCSV.selectTarget == SelectTarget.Self)
        //    {
        //        unit.GetDamageInfoVal(ref buffInfo.damageInfBaseCSV);
        //    }
        //    DamageEffectConfig pulseDamageConfig2 = new DamageEffectConfig(buffInfo.damageInfBaseCSV,buffInfo.damageType);
        //    buffConfig.addEffectList.Add(pulseDamageConfig2);

        //    // 脉冲，持续生效伤害
        //    buffInfo.pulDamageInfBaseCSV.skillLv = skilllv;
        //    if (buffInfo.pulDamageInfBaseCSV.selectTarget == SelectTarget.Self)
        //    {
        //        unit.GetDamageInfoVal(ref buffInfo.pulDamageInfBaseCSV);
        //    }
        //    DamageEffectConfig pulseDamageConfig = new DamageEffectConfig(buffInfo.pulDamageInfBaseCSV,buffInfo.damageTypePul);
        //    buffConfig.pulseEffectList.Add(pulseDamageConfig);

        //    // 创建和开始运行buff
        //    Buff buff = (Buff) room.haoleBuff.CreateEffect(unit, send, buffConfig);
        //    buff.Start();
        //    return buff;
        //}
        #endregion

    }
}
