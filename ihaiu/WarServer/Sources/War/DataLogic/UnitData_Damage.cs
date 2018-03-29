using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 4:49:35 PM
*  @Description:    单位数据--伤害
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {
        #region 受到攻击


        /// <summary>
        /// 受到攻击
        /// </summary>
        /// <param name="unitDataSend">攻击者</param>
        /// <param name="attackPower">攻击力</param>
        /// <param name="bCrit">是否暴击</param>
        public void OnTakeDamage(DamageData Dd, List<HaloBuff> haloBuff = null)
        {
            if (!room.IsGameing || unitAgent == null) return;
            if (Dd.damageInfBaseCSV.selectTarget == SelectTarget.Target)
            {
                GetDamageInfoVal(ref Dd.damageInfBaseCSV);
            }
            if (!Dd.bSkillDamage && Dd.damageType == DamageType.EngeryChange)
            {
                float energy = room.clientOperationUnit.Energy + Dd.damageInfBaseCSV.resultVal;
                if (energy > room.clientOperationUnit.EnergyMax)
                {
                    room.clientOperationUnit.Energy = room.clientOperationUnit.EnergyMax;
                }
                else if (energy < 0)
                {
                    room.clientOperationUnit.Energy = 0;
                }
                else
                {
                    room.clientOperationUnit.Energy = energy;
                }
                int resval = (int)Dd.damageInfBaseCSV.resultVal;
                if (resval != 0)
                {
                    WarUI.Instance.OnBloodDame(BloodStartPos, (int)Dd.damageInfBaseCSV.resultVal, false, PropId.Energy);
                }
                return;
            }

            if (prop.Hp <= 0)
            {
                if (room.sceneData.GetUnit(uid) == null)
                {
                    if (unitType == UnitType.Build && unitAgent != null && unitAgent.animatorManager != null)
                    {
                        unitAgent.ActionDeath();
                    }
                }
                return;
            }
            float val = Dd.damageInfBaseCSV.resultVal;
            if (isCloneUnit)
            {
                val *= hitPer;
            }
            if (Dd.AttackSend.isCloneUnit)
            {
                val *= Dd.AttackSend.attackPer;
            }
            if ((int)val == 0)
            {
                // 0，无伤害，强制去除
                return;
            }
            // 当前护盾值
            float shield = prop.GetProp(PropId.Shield, PropType.Final);
            if (val >= 0)
            {
                if (War.heroInvincible)
                {
                    if (unitType == UnitType.Hero)
                    {
                        val = 0;
                    }
                }
                if (War.soliderInvincible)
                {
                    if (unitType != UnitType.Hero)
                    {
                        val = 0;
                    }
                }

                if (invincible)
                {
                    // 无敌状态，不掉血
                    val = 0;
                }

                Game.audio.PlaySoundWarSFX(avatarConfig.audioHit, position);

                UIHandler.OnHandPassiveed(Triggercondition.OnHitDamage, Dd.AttackSend, unitData);
            }
            // 计算辅助buff伤害的特殊属性
            if (haloBuff != null && haloBuff.Count > 0)
            {
                for (int i = 0; i < haloBuff.Count; i++)
                {
                    HaloBuff hb = haloBuff[i];
                    switch (hb.specialFun)
                    {
                        case SpecialFunctionType.Crit:
                            {
                                SpecialFunConfigCrit config = (SpecialFunConfigCrit)hb.specialFunConfig;
                                val *= config.specialVal;
                                Dd.bCrit = true;
                            }
                            break;
                    }
                }
            }
            // 受伤害
            if (val > 0)
            {
                prop.Hp -= Math.Max(0, val - shield);
            }
            else
            {
                // 分身无法被治疗
                if (!isCloneUnit)
                {
                    prop.Hp -= val;
                }
            }

            if (prop.Hp < prop.HpMax || val > 0)
            {
                if ((int)val != 0)
                {
                    WarUI.Instance.OnBloodDame(BloodStartPos, (int)val, Dd.bCrit, PropId.Hp);
                }
            }

            // 去护盾值
            if (shield > 0 && val > 0)
            {
                float xx = Math.Max(0, shield - val);
                prop.SetProp(PropId.Shield, PropType.Final, xx);
            }
            if (prop.Hp <= 0)
            {
                prop.Hp = 0;
                unitAgent.punUnit.Death(Dd.AttackSend != null ? Dd.AttackSend.uid : -1);
                return;
            }
            else
            {
                if (/*(unitType == UnitType.Build && buildType != UnitBuildType.Mainbase) || */unitType == UnitType.Solider)
                {
                    unitAgent.OnHitBy();
                    //if (unitAgent != null && unitAgent.animatorManager != null && unitAgent.animatorManager.GetAnimatorState(0) == AnimatorState.Idle)
                    //{
                    //    unitAgent.animatorManager.Do_Hit();
                    //}
                }
                else if (unitType == UnitType.Build && buildType != UnitBuildType.Mainbase && val > 0)
                {
                    if (unitAgent != null && unitAgent.animatorManager != null && unitAgent.animatorManager.GetAnimatorState(0) == AnimatorState.Idle)
                    {
                        unitAgent.animatorManager.Do_Hit();
                    }
                }

            }

            if (prop.Hp >= prop.HpMax)
            {
                prop.Hp = prop.HpMax;
            }
            else
            {
                showBloodTime = LTime.time;
            }

            if (Dd.skillConfig != null && Dd.AttackSend != null)
            {
                AddHatred(Dd.AttackSend.uid, (int)Dd.skillConfig.aiConfigHatred.GetVal(this));
            }
        }

        public void RPC_Death(int attackUid)
        {
            Game.audio.PlaySoundWarSFX(avatarConfig.audioDead, position);
            UnitData attackUnit = room.sceneData.GetUnit(attackUid);
            //unitAgent.unitControl.OnPassiveEvent(Triggercondition.OnDie, attackUnit, unitData);
            UIHandler.OnHandPassiveed(Triggercondition.OnDie, attackUnit, unitData);
            switch (unitType)
            {
                case UnitType.Solider:
                    if (attackUnit != null)
                    {
                        room.clientNetS.LegionEnergyAdd(attackUnit.legionId, deathCost, position);
                    }
                    room.clientNetS.RemoveUnit(uid, attackUid);
                    break;
                case UnitType.Hero:
                    if (isCloneUnit)
                    {
                        //分身死亡，从主体分身列表移除
                        CloneDeath(attackUid);
                    }
                    else
                    {
                        //主体死亡，移除分身列表
                        if (cloneChilds.Count > 0)
                        {
                            for (int i = 0; i < cloneChilds.Count; i++)
                            {
                                UnitAgent childUnitAgent = room.clientSceneView.GetUnit(cloneChilds[i]);
                                if (childUnitAgent != null && childUnitAgent.unitData.isDeathWithMain)
                                {
                                    room.clientNetS.RemoveUnit(childUnitAgent.uid, -1, true);
                                }
                            }
                            cloneChilds.Clear();
                        }
                        room.clientNetS.SetUnitIsLive(uid, false);
                    }
                    break;
                default:
                    room.clientNetS.RemoveUnit(uid);
                    break;
            }
        }
        #endregion

        #region 获取伤害数值
        public void GetDamageInfoVal(ref DamageInfBaseCSV dibc)
        {
            float Damageval = prop.GetProp(dibc.refId, dibc.refRpopType) * dibc.refRatio;
            dibc.resultVal = Game.config.skillValue.GetConfigs(dibc.valid, dibc.skillLv) + Damageval;
        }
        #endregion

        #region 分身死亡
        /// <summary>
        /// 分身死亡，-1表示时间到或主体死亡的消失
        /// </summary>
        /// <param name="attackUid"></param>
        public void CloneDeath(int attackUid)
        {
            UnitAgent mainUnitAgent = room.clientSceneView.GetUnit(cloneUnitMainUid);
            if (mainUnitAgent != null)
            {
                int index = mainUnitAgent.unitData.cloneChilds.FindIndex(m => m == uid);
                if (index >= 0)
                {
                    mainUnitAgent.unitData.cloneChilds.RemoveAt(index);
                }
            }
            room.clientNetS.RemoveUnit(uid, attackUid, true);
        }
        #endregion

    }


}
