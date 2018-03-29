using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能--触发器 被动技能
    /// </summary>
    [Serializable]
    public class SkillTriggerPas : SkillTrigger
    {
        SkillTriggerPassive tiggerConfig;
        public override void SetConfig(SkillTriggerConfig config)
        {
            base.SetConfig(config);
            tiggerConfig = (SkillTriggerPassive)config;
        }

        public override void Start()
        {
            switch (tiggerConfig.triggercondition)
            {
                case Triggercondition.None:
                    {
                        End();
                    }
                    break;
                case Triggercondition.OnCreated:
                    {
                        OnReady(tiggerConfig.triggercondition, warSkill.actionUnitAgent.unitData, null);
                    }
                    break;
                default:
                    {
                        // 被动调用一次，所以此次无需移除回调
                        UIHandler.OnHandPassive += OnReady;
                    }
                    break;
            }
        }

        private void OnReady(Triggercondition triggercondition, UnitData origin, UnitData target)
        {
            if (tiggerConfig.triggercondition == triggercondition)
            {
                switch (triggercondition)
                {
                    case Triggercondition.OnDie:
                    case Triggercondition.OnHitDamage:
                        {
                            if (target.uid != warSkill.actionUnitId)
                            {
                                return;
                            }
                        }
                        break;
                    case Triggercondition.OnAttackTarget:
                    case Triggercondition.OnStartAttack:
                        {
                            if (origin.uid != warSkill.actionUnitId)
                            {
                                return;
                            }
                        }
                        break;
                }
                
                if (OnPassiveJudgment(tiggerConfig.passiveJudgmentList, origin, target))
                {
                    OnTirgger();
                }
            }
        }

        //public override void End()
        //{
        //    UIHandler.OnHandPassive -= OnReady;
        //}

        public bool OnPassiveJudgment(List<PassiveJudgment> judgmentList, UnitData origin, UnitData target)
        {
            bool result = false;
            for (int i = judgmentList.Count - 1; i >= 0; i--)
            {
                bool configbool = false;
                switch (judgmentList[i].config.judgmentTargger)
                {
                    case JudgmentTargger.SameTypeCount:
                        {
                            PassiveJudgmentSameTypeCount passConfig = (PassiveJudgmentSameTypeCount)judgmentList[i].config;
                        }
                        break;
                    case JudgmentTargger.TargetType:
                        {
                            PassiveJudgmentTargetType passConfig = (PassiveJudgmentTargetType)judgmentList[i].config;
                            configbool = passConfig.unitType == target.unitType;
                        }
                        break;
                    case JudgmentTargger.TargetDis:
                        {
                            PassiveJudgmentTargetDis passConfig = (PassiveJudgmentTargetDis)judgmentList[i].config;
                            configbool = Vector3.Distance(origin.position, target.position) <= passConfig.disTarget;
                        }
                        break;
                    case JudgmentTargger.RandRangeCount:
                        {
                            PassiveJudgmentRandRangeCount passConfig = (PassiveJudgmentRandRangeCount)judgmentList[i].config;
                            configbool = WarRandom.Range(0, 100) <= passConfig.randomRangeCount;
                        }
                        break;
                    case JudgmentTargger.SelfBuffCount:
                        {
                            //PassiveJudgmentSelfBuffCount passConfig = (PassiveJudgmentSelfBuffCount)judgmentList[i].config;
                            //if (origin.bulletIdCountDic.ContainsKey(passConfig.buffId))
                            //{
                            //    int layyer = origin.bulletIdCountDic[passConfig.buffId];
                            //    switch (passConfig.compare)
                            //    {
                            //        case CompareType.Equal:
                            //            {
                            //                configbool = layyer == passConfig.buffCount;
                            //            }
                            //            break;
                            //        case CompareType.MoreThanThe:
                            //            {
                            //                configbool = layyer > passConfig.buffCount;
                            //            }
                            //            break;
                            //        case CompareType.LessThan:
                            //            {
                            //                configbool = layyer < passConfig.buffCount;
                            //            }
                            //            break;
                            //        case CompareType.GreaterOrEqual:
                            //            {
                            //                configbool = layyer >= passConfig.buffCount;
                            //            }
                            //            break;
                            //        case CompareType.LessThanOrEqual:
                            //            {
                            //                configbool = layyer <= passConfig.buffCount;
                            //            }
                            //            break;
                            //    }
                            //    if (configbool)
                            //    {
                            //        if (!origin.useBullet.Contains(passConfig.buffId))
                            //        {
                            //            origin.useBullet.Add(passConfig.buffId);
                            //        }
                            //    }
                            //}
                        }
                        break;
                    case JudgmentTargger.MonsterWaves:
                        {
                            PassiveJudgmentMonsterWaves passConfig = (PassiveJudgmentMonsterWaves)judgmentList[i].config;
                            configbool = room.spawnSolider.waveIndex + 1 == passConfig.monsterCount;
                        }
                        break;
                    case JudgmentTargger.SelfProperties:
                        {
                            PassiveJudgmentSelfProperties passConfig = (PassiveJudgmentSelfProperties)judgmentList[i].config;
                            float xx = origin.prop.HpRate;
                            if (passConfig.propId == PropId.Energy)
                            {
                                xx = origin.prop.GetProp(PropId.Energy) / origin.prop.GetProp(PropId.EnergyMax);
                            }
                            configbool = xx < passConfig.propVal;
                        }
                        break;
                    case JudgmentTargger.SkillAddBuff:
                        {
                            PassiveJudgmentTrigerOnSkill triggerConfig = (PassiveJudgmentTrigerOnSkill)judgmentList[i].config;
                            if (triggerConfig.skillIdList != null && triggerConfig.skillIdList.Count > 0)
                            {
                                if (!origin.skillAddBuffDic.ContainsKey(warSkill.skillInfoConfig.skillId))
                                {
                                    origin.skillAddBuffDic.Add(warSkill.skillInfoConfig.skillId, new Dictionary<int, SkillActionConfigBuffCreate>());
                                }
                                Dictionary <int, SkillActionConfigBuffCreate> dicInfo = origin.skillAddBuffDic[warSkill.skillInfoConfig.skillId];
                                for (int kkk = 0; kkk < triggerConfig.skillIdList.Count; kkk++)
                                {
                                    if (!dicInfo.ContainsKey(triggerConfig.skillIdList[kkk]))
                                    {
                                        dicInfo.Add(triggerConfig.skillIdList[kkk], new SkillActionConfigBuffCreate());
                                    }
                                }
                                configbool = true;
                            }
                        }
                        break;
                    case JudgmentTargger.ChangeNormalSkill:
                        {
                            PassiveJudgmentChangeNormalSkill triggerConfig = (PassiveJudgmentChangeNormalSkill)judgmentList[i].config;
                            origin.ChangeNormalSkillInf(triggerConfig.skillIdNew);
                            configbool = true;
                        }
                        break;
                    default:
                        configbool = true;
                        break;
                }
                if (i == 0)
                {
                    result = configbool;
                }
                else
                {
                    if (tiggerConfig.andor)
                    {
                        result = configbool && result;
                    }
                    else
                    {
                        result = configbool || result;
                    }
                }
            }
            return result;
        }

    }
}
