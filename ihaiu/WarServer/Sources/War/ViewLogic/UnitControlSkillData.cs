//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Games.Wars
//{
    /// <summary>
    /// 单位操控
    /// </summary>
    //public partial class UnitControl : AbstractUnitMonoBehaviour
    //{
        //--使用技能 发送数据

        #region 更新技能状态
        /// <summary>
        /// 开启位移
        /// </summary>
        //private bool            bMoveUnitAgent      = false;
        /// <summary>
        /// 是否开启后摇
        /// </summary>
        //private bool            afterShaking        = false;
        /// <summary>
        /// 技能开启 - 主动
        /// </summary>
        //[HideInInspector]
        //public bool            startSkill           = false;
        /// <summary>
        /// 更新技能状态
        /// </summary>
        /// 
        //private void Update()
        //{
        //    if (!room.IsGameing)
        //    {
        //        return;
        //    }

        //    #region 被动技能监听
        //    if (PassiveList.Count > 0)
        //    {
        //        for (int i = PassiveList.Count - 1; i >= 0; i--)
        //        {
        //            bool bTargger   = false;
        //            bool remove     = false;
        //            UnitData  origin = null;
        //            UnitData  target = null;
        //            #region 是否触发
        //            // 列举触发条件，判断是否触发
        //            if (PassiveList[i].passiveTriggerList[0].triggercondition == Triggercondition.OnCreated)
        //            {
        //                // "创建时";自身初始化时，也就是第一次进入判断时，然后可以清除
        //                bTargger = true;
        //                remove = true;
        //                TriggEvent TE = TriggerconditionDic.Find(m => m.Td == PassiveList[i].passiveTriggerList[0].triggercondition);
        //                if (TE != null)
        //                {
        //                    origin = TE.origin;
        //                    target = TE.target;
        //                    bTargger = true;
        //                    TriggerconditionDic.Remove(TE);
        //                }
        //            }
        //            else
        //            {
        //                TriggEvent TE = TriggerconditionDic.Find(m => m.Td == PassiveList[i].passiveTriggerList[0].triggercondition);
        //                if (TE != null)
        //                {
        //                    origin = TE.origin;
        //                    target = TE.target;
        //                    bTargger = true;
        //                    TriggerconditionDic.Remove(TE);
        //                }
        //            }
        //            #endregion

        //            #region 触发后的判断条件是否满足
        //            if (bTargger)
        //            {
        //                bool andor = PassiveList[i].passiveTriggerList[0].andor;
        //                for (int l = 0; l < PassiveList[i].passiveTriggerList[0].judgmentInfoList.Count; l++)
        //                {
        //                    bool res = OnJudgmentTargger(PassiveList[i].passiveTriggerList[0].judgmentInfoList[l]);
        //                    if (l == 0)
        //                    {
        //                        bTargger = res;
        //                    }
        //                    else
        //                    {
        //                        if (andor)
        //                        {
        //                            bTargger = bTargger && res;
        //                        }
        //                        else
        //                        {
        //                            bTargger = bTargger || res;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region 满足以上两者后，进入技能事件运行
        //            if (bTargger)
        //            {
        //                //PassiveTriggerEventList.Add(InitTriggerEvent(PassiveList[i].actionEvent.actionEventList, PassiveList[i].skillId, origin, target, false, PassiveList[i].legionId));
        //                passiveTriggerInfList.Add(getskillTriggerInfList(PassiveList[i].actionEvent.actionEventList, PassiveList[i].skillId, false, origin, target, PassiveList[i].legionId));
        //            }
        //            #endregion
        //            if (remove)
        //            {
        //                PassiveList.RemoveAt(i);
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 被动技能事件
        //    if (passiveTriggerInfList.Count > 0)
        //    {
        //        for (int k = passiveTriggerInfList.Count - 1; k >= 0; k--)
        //        {
        //            List<SkillTriggerInf> listEvent = passiveTriggerInfList[k];
        //            for (int i = listEvent.Count - 1; i >= 0; i--)
        //            {
        //                if (listEvent.Count <= 0)
        //                {
        //                    unitData.invincible = false;
        //                    continue;
        //                }
        //                SkillTriggerInf targEvent = listEvent[i];
        //                if (targEvent.bLife && targEvent.skillTriggerEvent.triggerTime + targEvent.skillTriggerEvent.life + targEvent.startTime <= LTime.time)
        //                {
        //                    // 事件到达生命周期，结束
        //                    targEvent.bLife = false;
        //                    unitData.invincible = false;
        //                    listEvent.RemoveAt(i);
        //                    continue;
        //                }
        //                if (!targEvent.bLife && targEvent.skillTriggerEvent.triggerTime + targEvent.startTime <= LTime.time)
        //                {
        //                    // 开始执行事件
        //                    targEvent.bLife = true;
        //                    unitData.invincible = targEvent.skillTriggerEvent.invincible;
        //                    if (targEvent.skillTriggerEvent.bShakeEffect)
        //                    {
        //                        Game.camera.CameraMg.OnShakeCamera(targEvent.skillTriggerEvent.shakeTime, targEvent.skillTriggerEvent.shakeAmplitude, targEvent.skillTriggerEvent.shakeRange, unitAgent.position);
        //                    }
        //                    switch (targEvent.skillTriggerEvent.skillType)
        //                    {
        //                        #region 播放攻击动作
        //                        case SkillType.PlayAnimator:
        //                            {
        //                                // 播放动作
        //                                switch (targEvent.skillTriggerEvent.warSkillEffectType)
        //                                {
        //                                    case WarSkillEffectType.NormalAttack:
        //                                        {
        //                                            unitAgent.aniManager.Do_Attack();
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.LeftAttack:
        //                                    case WarSkillEffectType.RightAttack:
        //                                        {
        //                                            unitAgent.aniManager.Do_AttackL();
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill1:
        //                                        {
        //                                            unitAgent.aniManager.Do_Skill1();
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill2:
        //                                        {
        //                                            unitAgent.aniManager.Do_Skill2();
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill3:
        //                                        {
        //                                            unitAgent.aniManager.Do_Skill3();
        //                                        }
        //                                        break;
        //                                }
        //                            }
        //                            break;
        //                        #endregion
        //                        case SkillType.PlayEffect:
        //                            {
        //                                // "播放特效";
        //                                WarInstantiationEffect.Instance.InitEffect(null, targEvent.skillTriggerEvent.effectPath, targEvent.skillTriggerEvent.life, unitAgent.position, null, unitData.LegionId);
        //                            }
        //                            break;
        //                        case SkillType.PlayMusic:
        //                            {
        //                                // "播放音效";targEvent.skillTriggerEvent.effectPath
        //                                Game.audio.PlayWarSFX(targEvent.skillTriggerEvent.effectPath, unitAgent.position);
        //                            }
        //                            break;
        //                        case SkillType.TriggerDamage:
        //                        case SkillType.CreateHalo:
        //                        case SkillType.CreateBuff:
        //                        case SkillType.TriggerCure:
        //                            {
        //                                // "造成伤害";
        //                                #region 造成伤害
        //                                if (targEvent.skillTriggerEvent.maxTargetsSelect > 0)
        //                                {
        //                                    int count = 0;
        //                                    List<UnitData> list = new List<UnitData>();
        //                                    int count2 = 0;
        //                                    List<UnitData> list2 = new List<UnitData>();
        //                                    if (targEvent.skillTriggerEvent.passiveTargetSelect.bOrigin && targEvent.origin != null)
        //                                    {
        //                                        list = room.sceneData.SearchMinDistanceUnit(targEvent.origin, targEvent.skillTriggerEvent.attackRuleList, targEvent.skillTriggerEvent.passiveTargetSelect.minRadius, targEvent.skillTriggerEvent.passiveTargetSelect.maxRadius, targEvent.skillTriggerEvent.maxTargetsSelect, out count);
        //                                        count = list.Count;
        //                                    }
        //                                    if (targEvent.skillTriggerEvent.passiveTargetSelect.bTarget && targEvent.target != null)
        //                                    {
        //                                        list2 = room.sceneData.SearchMinDistanceUnit(targEvent.target, targEvent.skillTriggerEvent.attackRuleList, targEvent.skillTriggerEvent.passiveTargetSelect.minRadius, targEvent.skillTriggerEvent.passiveTargetSelect.maxRadius, targEvent.skillTriggerEvent.maxTargetsSelect, out count2);
        //                                        count2 = list2.Count;
        //                                    }
        //                                    UnitData listOrigin = list.Find(m => m.uid == targEvent.origin.uid);
        //                                    UnitData listTarget = list2.Find(m => m.uid == targEvent.target.uid);
        //                                    if (targEvent.skillTriggerEvent.passiveTargetSelect.bOriginSelf)
        //                                    {
        //                                        if (listOrigin == null)
        //                                        {
        //                                            list.Add(targEvent.origin);
        //                                            count++;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (listOrigin != null)
        //                                        {
        //                                            list.Remove(targEvent.origin);
        //                                            count--;
        //                                        }
        //                                    }
        //                                    if (targEvent.skillTriggerEvent.passiveTargetSelect.bTargetSelf)
        //                                    {
        //                                        if (listTarget == null)
        //                                        {
        //                                            list2.Add(targEvent.target);
        //                                            count2++;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (listTarget != null)
        //                                        {
        //                                            list2.Remove(targEvent.target);
        //                                            count2--;
        //                                        }
        //                                    }
        //                                    if (count > 0)
        //                                    {
        //                                        Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                        for (int l = 0; l < count; l++)
        //                                        {
        //                                            if (list[l] != null)
        //                                            {
        //                                                AttackTartget(unitAgent, list[l], targEvent.skillTriggerEvent.damageInfo, targEvent.skillTriggerEvent.secondTarget, list[l].position, targEvent.skilllv, targEvent.skillTriggerEvent.attackRuleList, ref dic, null, targEvent.legionId);
        //                                            }
        //                                        }
        //                                    }
        //                                    if (count2 > 0)
        //                                    {
        //                                        Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                        for (int l = 0; l < count2; l++)
        //                                        {
        //                                            if (list2[l] != null)
        //                                            {
        //                                                AttackTartget(unitAgent, list2[l], targEvent.skillTriggerEvent.damageInfo, targEvent.skillTriggerEvent.secondTarget, list2[l].position, targEvent.skilllv, targEvent.skillTriggerEvent.attackRuleList, ref dic, null, targEvent.legionId);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                            break;
        //                        case SkillType.MoveTowardCurrDir:
        //                            {
        //                                if (targEvent.skillTriggerEvent.isBlink)
        //                                {
        //                                    StageRouteConfig srcf = sceneData.GetRoute(targEvent.skillTriggerEvent.blinkRoute);
        //                                    if (srcf != null && targEvent.skillTriggerEvent.blinkChild < srcf.path.Count)
        //                                    {
        //                                        skillDate.unitAgentSend.position = srcf.GetBeginPoint(targEvent.skillTriggerEvent.blinkChild);
        //                                        skillDate.unitAgentSend.rotation = srcf.GetBeginDirection(targEvent.skillTriggerEvent.blinkChild);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    skillDate.unitAgentSend.unitControl.MoveUnitFromOther(targEvent.skillTriggerEvent.selfMoveDis, targEvent.skillTriggerEvent.selfMoveSpeed);
        //                                }
        //                            }
        //                            break;
        //                        case SkillType.CustEngery:
        //                            {
        //                                // "扣除能量";
        //                            }
        //                            break;
        //                        case SkillType.RevertEngery:
        //                            {
        //                                // "回复能量";
        //                            }
        //                            break;
        //                        case SkillType.ReduceCD:
        //                            {
        //                                // "减少CD";
        //                            }
        //                            break;
        //                        case SkillType.RemoveBuff:
        //                            {
        //                                // "移除buff";
        //                            }
        //                            break;
        //                        case SkillType.ShotBullet:
        //                            {
        //                                // "发射投射物";
        //                                //room.clientInstantiationEffect.InitEffectList(null, targEvent, targEvent.projectile,unitAgent);
        //                            }
        //                            break;
        //                        case SkillType.ShakeCameraXXX:
        //                            {
        //                                // "震屏";
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 主动技能
        //    if (startSkill && skillDate.unitAgentSend.unitData.IsInSceneAndLive) // 技能开启
        //    {
        //        if (skillDate.skillBass.skillActivation.activation == Activation.User) // 主动技能
        //        {
        //            if (skillTriggerInfList.Count <= 0)
        //            {
        //                startSkill = false;
        //            }
        //            if (!skillDate.skillBass.skillActivation.forceStop && attackRule != null && skillDate.unitAgentSend.unitType == UnitType.Hero)
        //            {
        //                GetDir(attackRule, true);
        //            }
        //            for (int i = skillTriggerInfList.Count - 1; i >= 0; i--)
        //            {
        //                if (skillTriggerInfList.Count <= 0)
        //                {
        //                    skillDate.unitAgentSend.unitData.isSkillAttack = false;
        //                    unitData.invincible = false;
        //                    continue;
        //                }
        //                SkillTriggerInf skillTI = skillTriggerInfList[i];
        //                if (skillTI.bLife && skillTI.skillTriggerEvent.triggerTime + skillTI.skillTriggerEvent.life + skillTI.startTime <= LTime.time)
        //                {
        //                    // 事件到达生命周期，结束
        //                    skillTI.bLife = false;
        //                    skillTriggerInfList.RemoveAt(i);
        //                    if (skillTriggerInfList.Count <= 0)
        //                    {
        //                        skillDate.unitAgentSend.unitData.isSkillAttack = false;
        //                    }
        //                    continue;
        //                }
        //                if (!skillTI.bLife && skillTI.skillTriggerEvent.triggerTime + skillTI.startTime <= LTime.time)
        //                {
        //                    // 开始执行事件
        //                    skillTI.bLife = true;
        //                    unitData.invincible = skillTI.skillTriggerEvent.invincible;
        //                    #region 判断是否震屏
        //                    if (skillTI.skillTriggerEvent.bShakeEffect)
        //                    {
        //                        Game.camera.CameraMg.OnShakeCamera(skillTI.skillTriggerEvent.shakeTime, skillTI.skillTriggerEvent.shakeAmplitude, skillTI.skillTriggerEvent.shakeRange, unitAgent.position);
        //                    }
        //                    #endregion


        //                    switch (skillTI.skillTriggerEvent.skillType)
        //                    {
        //                        #region 播放攻击动作
        //                        case SkillType.PlayAnimator:
        //                            {
        //                                // 是否强制停止移动
        //                                skillDate.unitAgentSend.unitData.isSkillAttack = skillDate.skillBass.skillActivation.forceStop;
        //                                #region 确定单位朝向，已经存在目标的，朝向目标，否则自动寻找目标
        //                                if (skillDate.unitDataBy != null && skillDate.unitDataBy.IsInSceneAndLive && skillDate.skillAttackPoint.x > -100000)
        //                                {
        //                                    skillDate.unitAgentSend.LookAt(skillDate.skillAttackPoint);
        //                                }
        //                                else
        //                                {
        //                                    attackRule = skillTI.skillTriggerEvent.attackRuleList;
        //                                    GetDir(attackRule, skillDate.bAuToSearchTarget);
        //                                }
        //                                #endregion
        //                                // 播放动作
        //                                float giveLength = skillTI.skillTriggerEvent.skillPlayTime <= 0 ? 1 : skillTI.skillTriggerEvent.skillPlayTime;
        //                                switch (skillTI.skillTriggerEvent.warSkillEffectType)
        //                                {
        //                                    case WarSkillEffectType.Attack1:
        //                                        {
        //                                            OnPassiveEvent(Triggercondition.OnStartAttack, skillDate.unitAgentSend.unitData);
        //                                            skillDate.unitAgentSend.warUnitcontrol.bDoAttack = true;
        //                                            skillDate.unitAgentSend.warUnitcontrol.animatorState = AnimatorState.Attack;
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.NormalAttack:
        //                                        {
        //                                            //OnPassiveEvent(Triggercondition.OnStartAttack, skillDate.unitAgentSend.unitData);
        //                                            skillDate.unitAgentSend.aniManager.Do_Attack(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.LeftAttack:
        //                                    case WarSkillEffectType.RightAttack:
        //                                        {
        //                                            //OnPassiveEvent(Triggercondition.OnStartAttack, skillDate.unitAgentSend.unitData);
        //                                            skillDate.unitAgentSend.aniManager.Do_AttackL(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill1:
        //                                        {
        //                                            skillDate.unitAgentSend.aniManager.Do_Skill1(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill2:
        //                                        {
        //                                            skillDate.unitAgentSend.aniManager.Do_Skill2(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill3:
        //                                        {
        //                                            skillDate.unitAgentSend.aniManager.Do_Skill3(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill4:
        //                                        {
        //                                            skillDate.unitAgentSend.aniManager.Do_Skill4(giveLength);
        //                                        }
        //                                        break;
        //                                    case WarSkillEffectType.Skill5:
        //                                        {
        //                                            skillDate.unitAgentSend.aniManager.Do_Skill5(giveLength);
        //                                        }
        //                                        break;
        //                                }
        //                            }
        //                            break;
        //                        #endregion
        //                        case SkillType.PlayEffect:
        //                            {
        //                                // "播放特效";
        //                                WarInstantiationEffect.Instance.InitEffect(skillDate, skillTI.skillTriggerEvent, skillDate.unitAgentSend.position, skillTI.skillTriggerEvent.followSelf);
        //                            }
        //                            break;
        //                        case SkillType.PlayMusic:
        //                            {
        //                                // "播放音效";
        //                                Game.audio.PlayWarSFX(skillTI.skillTriggerEvent.effectPath, skillDate.unitAgentSend.position + skillDate.unitAgentSend.forward * 3);
        //                            }
        //                            break;
        //                        case SkillType.TriggerDamage:
        //                        case SkillType.CreateHalo:
        //                        case SkillType.TriggerCure:
        //                        case SkillType.CreateBuff:
        //                            {
        //                                // "造成伤害";
        //                                // "播放音效";
        //                                Game.audio.PlayWarSFX(skillTI.skillTriggerEvent.damageInfo.DamageMusic, skillDate.unitAgentSend.position);
        //                                #region 造成伤害
        //                                if (skillTI.skillTriggerEvent.maxTargetsSelect > 0)
        //                                {
        //                                    switch (skillDate.skillBass.aimDirectionList[0].targetLocation)
        //                                    {
        //                                        case TargetLocation.CircularShockwave:
        //                                            {
        //                                                // 对自身周围圆形区域造成伤害
        //                                                int count = 0;
        //                                                List < UnitData > list = room.sceneData.SearchMinDistanceUnit(skillDate.unitAgentSend.unitData, skillTI.skillTriggerEvent.attackRuleList, skillDate.skillBass.aimDirectionList[0].targetMinRadius, skillDate.skillBass.aimDirectionList[0].targetFanRadius, skillTI.skillTriggerEvent.maxTargetsSelect, out count);
        //                                                count = list.Count;
        //                                                if (count > 0)
        //                                                {
        //                                                    Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                                    for (int l = 0; l < count; l++)
        //                                                    {
        //                                                        AttackTartget(skillDate.unitAgentSend, list[l], skillTI.skillTriggerEvent.damageInfo, skillTI.skillTriggerEvent.secondTarget, list[l].position, skillDate.skillBass.skillLv, skillTI.skillTriggerEvent.attackRuleList, ref dic, skillDate);
        //                                                    }
        //                                                }
        //                                            }
        //                                            break;
        //                                        case TargetLocation.FanWave:
        //                                            {
        //                                                // 对扇形区域造成伤害
        //                                                int count = 0;
        //                                                List < UnitData > list = room.sceneData.SearchFanUnit(skillDate.unitAgentSend.rotationQuaternion,skillDate.unitAgentSend.position,skillDate.skillBass.aimDirectionList[0].targetFanAngle,skillDate.unitAgentSend.unitData.LegionId,skillDate.unitAgentSend.position, skillTI.skillTriggerEvent.attackRuleList, skillDate.skillBass.aimDirectionList[0].targetFanRadius, skillTI.skillTriggerEvent.maxTargetsSelect, out count);
        //                                                count = list.Count;
        //                                                if (count > 0)
        //                                                {
        //                                                    Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                                    for (int l = 0; l < count; l++)
        //                                                    {
        //                                                        AttackTartget(skillDate.unitAgentSend, list[l], skillTI.skillTriggerEvent.damageInfo, skillTI.skillTriggerEvent.secondTarget, list[l].position, skillDate.skillBass.skillLv, skillTI.skillTriggerEvent.attackRuleList, ref dic, skillDate);
        //                                                    }
        //                                                }
        //                                            }
        //                                            break;
        //                                        case TargetLocation.LinearWave:
        //                                            {
        //                                                // 对矩形区域造成伤害
        //                                                int count = 0;
        //                                                List < UnitData > list = room.sceneData.SearchFanUnit(skillDate.unitAgentSend.rotationNode.TransformDirection(Vector3.forward),skillDate.unitAgentSend.rotationQuaternion,skillDate.unitAgentSend.position,skillDate.skillBass.aimDirectionList[0].targetLocationWidth,skillDate.skillBass.aimDirectionList[0].targetMinRadius,skillDate.skillBass.aimDirectionList[0].targetFanRadius,skillDate.unitAgentSend.unitData.LegionId,skillDate.unitAgentSend.position, skillTI.skillTriggerEvent.attackRuleList, skillTI.skillTriggerEvent.maxTargetsSelect, out count);
        //                                                count = list.Count;
        //                                                if (count > 0)
        //                                                {
        //                                                    Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                                    for (int l = 0; l < count; l++)
        //                                                    {
        //                                                        AttackTartget(skillDate.unitAgentSend, list[l], skillTI.skillTriggerEvent.damageInfo, skillTI.skillTriggerEvent.secondTarget, list[l].position, skillDate.skillBass.skillLv, skillTI.skillTriggerEvent.attackRuleList, ref dic, skillDate);
        //                                                    }
        //                                                }
        //                                            }
        //                                            break;
        //                                        case TargetLocation.Self:
        //                                            {
        //                                                // 对自身造成伤害
        //                                                Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                                AttackTartget(skillDate.unitAgentSend, skillDate.unitAgentSend.unitData, skillTI.skillTriggerEvent.damageInfo, skillTI.skillTriggerEvent.secondTarget, skillDate.unitAgentSend.position, skillDate.skillBass.skillLv, skillTI.skillTriggerEvent.attackRuleList, ref dic, skillDate);
        //                                            }
        //                                            break;
        //                                        case TargetLocation.TargetCircleArea:
        //                                            {
        //                                                // 对目标圆形区域造成伤害
        //                                                if (skillDate.unitDataBy != null)
        //                                                {
        //                                                    int count = 0;
        //                                                    List < UnitData > list = room.sceneData.SearchMinDistanceUnit(skillDate.unitAgentSend.unitData.LegionId,skillDate.unitDataBy.position, skillTI.skillTriggerEvent.attackRuleList, skillDate.skillBass.aimDirectionList[0].targetFanRadius, skillTI.skillTriggerEvent.maxTargetsSelect, out count);
        //                                                    count = list.Count;
        //                                                    if (count > 0)
        //                                                    {
        //                                                        Dictionary<int, int> dic = new Dictionary<int, int>();
        //                                                        for (int l = 0; l < count; l++)
        //                                                        {
        //                                                            AttackTartget(skillDate.unitAgentSend, list[l], skillTI.skillTriggerEvent.damageInfo, skillTI.skillTriggerEvent.secondTarget, list[l].position, skillDate.skillBass.skillLv, skillTI.skillTriggerEvent.attackRuleList, ref dic, skillDate);
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                            break;
        //                        case SkillType.CustEngery:
        //                            {
        //                                // "扣除能量";
        //                            }
        //                            break;
        //                        case SkillType.RevertEngery:
        //                            {
        //                                // "回复能量";
        //                            }
        //                            break;
        //                        case SkillType.ReduceCD:
        //                            {
        //                                // "减少CD";
        //                            }
        //                            break;
        //                        case SkillType.RemoveBuff:
        //                            {
        //                                // "移除buff";
        //                            }
        //                            break;
        //                        case SkillType.ShotBullet:
        //                            {
        //                                // "发射投射物";
        //                                WarInstantiationEffect.Instance.InitEffectList(skillDate, skillTI.skillTriggerEvent, skillTI.skillTriggerEvent.projectile, skillDate.unitAgentSend);
        //                            }
        //                            break;
        //                        case SkillType.MoveTowardCurrDir:
        //                            {
        //                                // "发生瞬移&位移";
        //                                if (skillTI.skillTriggerEvent.isBlink)
        //                                {
        //                                    StageRouteConfig srcf = sceneData.GetRoute(skillTI.skillTriggerEvent.blinkRoute);
        //                                    if (srcf != null && skillTI.skillTriggerEvent.blinkChild < srcf.path.Count)
        //                                    {
        //                                        skillDate.unitAgentSend.position = srcf.GetBeginPoint(skillTI.skillTriggerEvent.blinkChild);
        //                                        skillDate.unitAgentSend.rotation = srcf.GetBeginDirection(skillTI.skillTriggerEvent.blinkChild);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    skillDate.unitAgentSend.unitControl.MoveUnitFromOther(skillTI.skillTriggerEvent.selfMoveDis, skillTI.skillTriggerEvent.selfMoveSpeed);
        //                                }
        //                            }
        //                            break;
        //                        case SkillType.ShakeCameraXXX:
        //                            {
        //                                // "震屏";
        //                            }
        //                            break;
        //                        case SkillType.SkillEnd:
        //                            {
        //                                // "技能结束";
        //                                //Stop(skillDate);
        //                                skillTriggerInfList.Clear();
        //                                attackRule = null;
        //                                startSkill = false;
        //                                skillDate.unitAgentSend.unitData.isSkillAttack = false;
        //                            }
        //                            break;
        //                        case SkillType.AfterShaking:
        //                            {
        //                                // 开启后摇
        //                                afterShaking = true;
        //                                skillDate.unitAgentSend.unitData.isSkillAttack = false;
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 位移技能移动位置
        //    if (bMoveUnitAgent)
        //    {
        //        Quaternion _temQuaternion = Quaternion.LookRotation(unitAgent.forward);
        //        unitAgent.Move(_temQuaternion, 12 * moveSpeed, false);
        //        if (LTime.time > moveAllTime || Vector3.Distance(unitAgent.position, StartPos) >= moveEndDis)
        //        {
        //            bMoveUnitAgent = false;
        //        }
        //    }
        //    #endregion
        //}

        #endregion

        #region 位移技能赋值
        /// <summary>
        /// 位移距离
        /// </summary>
        //private float       moveEndDis     = 0;
        /// <summary>
        /// 位移速度
        /// </summary>
        //private float       moveSpeed      = 0.5F;
        /// <summary>
        /// 启动位置
        /// </summary>
        //private Vector3     StartPos;
        //private float       moveAllTime    = 0;
        //void MoveUnitFromOther(float EndDis, float Speed)
        //{
        //    StartPos = unitAgent.position;
        //    moveEndDis = EndDis;
        //    moveSpeed = Speed * 0.1F;
        //    moveAllTime = moveEndDis / moveSpeed / 12 + LTime.time;
        //    bMoveUnitAgent = true;
        //}
        #endregion

        #region 被动判断条件
        //bool OnJudgmentTargger(JudgmentInfo judgmentInfo)
        //{
        //    bool res = false;
        //    JudgmentInfo ji = judgmentInfoEvent.Find(m => m.judgmentTargger == judgmentInfo.judgmentTargger);
        //    switch (judgmentInfo.judgmentTargger)
        //    {
        //        case JudgmentTargger.None:
        //            {
        //                res = true;
        //            }
        //            break;
        //        case JudgmentTargger.TargetType:
        //            {
        //                // "目标类型";
        //                if (ji != null && ji.unitType == judgmentInfo.unitType)
        //                {
        //                    res = true;
        //                }
        //            }
        //            break;
        //        case JudgmentTargger.TargetDis:
        //            {
        //                // "距离目标距离";普攻攻击到对方  DamageInfo.passiveJudgment = true,对方和自己的距离
        //                if (ji != null && Mathf.Abs(ji.disTarget - judgmentInfo.disTarget) < 0.01F)
        //                {
        //                    res = true;
        //                }
        //            }
        //            break;
        //        case JudgmentTargger.RandRangeCount:
        //            {
        //                // "随机概率<X%"; 请求服务器生成随机数，分发客户端
        //                if (ji != null && WarRandom.Range(0, 100) < judgmentInfo.randomRangeCount)
        //                {
        //                    res = true;
        //                }
        //            }
        //            break;
        //        case JudgmentTargger.MonsterWaves:
        //            {
        //                // "当前怪物波数"; wave
        //                if (ji != null && ji.monsterCount == judgmentInfo.monsterCount)
        //                {
        //                    res = true;
        //                }
        //            }
        //            break;
        //        case JudgmentTargger.SelfProperties:
        //            {
        //                // 属性达到临界点
        //                if (ji != null)
        //                {
        //                    float xx = ji.prop.HpRate;
        //                    if (judgmentInfo.propId == PropId.Energy)
        //                    {
        //                        xx = ji.prop.GetProp(judgmentInfo.propId) / ji.prop.GetProp(PropId.EnergyMax);
        //                    }
        //                    if (xx < judgmentInfo.propVal)
        //                    {
        //                        res = true;
        //                    }
        //                }
        //            }
        //            break;
        //        case JudgmentTargger.SameTypeCount:
        //            {
        //                // "场上同类数量"; 暂时不做
        //            }
        //            break;
        //        case JudgmentTargger.SelfBuffCount:
        //            {
        //                // "自身buff层数"; 转移到buff
        //            }
        //            break;
        //    }
        //    judgmentInfoEvent.Remove(ji);
        //    return res;
        //}
        #endregion

        #region 被动回调
        /// <summary>
        /// 被动触发条件
        /// </summary>
        /// <param name="triggercondition"></param>
        //public void OnPassiveEvent(Triggercondition triggercondition, UnitData origin = null, UnitData target = null)
        //{
        //    TriggEvent TE = new TriggEvent();
        //    TE.Td = triggercondition;
        //    TE.origin = origin;
        //    TE.target = target;
        //    TriggerconditionDic.Add(TE);
        //}
        //public void OnPassiveJudgEvent(JudgmentInfo judgmentInfo)
        //{
        //    judgmentInfoEvent.Add(judgmentInfo);
        //}
        #endregion

        #region 确定朝向
        //void GetDir(AttackRule attackRuleList, bool auto)
        //{
        //    if (skillDate.skillBass.aimDirectionList[0].targetLocation != TargetLocation.CircularShockwave && skillDate.skillBass.aimDirectionList[0].targetLocation != TargetLocation.Self)
        //    {
        //        if (skillDate.unitDataBy != null && skillDate.unitDataBy.IsInSceneAndLive)
        //        {
        //            // 有目标，朝向目标
        //            skillDate.unitAgentSend.LookAt(skillDate.unitDataBy.AnchorAttackbyPos);
        //        }
        //        else
        //        {
        //            if (auto)
        //            {
        //                // 自动寻怪
        //                List<UnitData> list = room.sceneData.GetUnitList().FindAll(m => attackRule.unitType.UContain(m.unitType) && attackRule.unitSpaceType.UContain(m.spaceType) && attackRule.relationType.RContain(m.unitData.GetRelationType(skillDate.unitAgentSend.legionData.legionId)));
        //                if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
        //                {
        //                    list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
        //                }
        //                Vector3 pos = skillDate.unitAgentSend.position;
        //                if (list.Count > 0)
        //                {
        //                    list.Sort((UnitData a, UnitData b) =>
        //                    {
        //                        float aa = Vector3.Distance(pos, a.position);
        //                        float bb = Vector3.Distance(pos, b.position);
        //                        if (aa > bb)
        //                        {
        //                            return 1;
        //                        }
        //                        else if (aa < bb)
        //                        {
        //                            return -1;
        //                        }
        //                        return 0;
        //                    });
        //                    // 攻击目标
        //                    skillDate.unitAgentSend.LookAt(list[0].AnchorAttackbyPos);
        //                    float dis = Vector3.Distance(skillDate.unitAgentSend.position , list[0].AnchorAttackbyPos);
        //                    switch (skillDate.skillBass.aimDirectionList[0].targetLocation)
        //                    {
        //                        case TargetLocation.FanWave:
        //                            {
        //                                if (dis <= skillDate.skillBass.aimDirectionList[0].targetFanRadius)
        //                                {
        //                                    skillDate.unitDataBy = list[0];
        //                                }
        //                            }
        //                            break;
        //                        case TargetLocation.LinearWave:
        //                            {
        //                                if (dis <= skillDate.skillBass.aimDirectionList[0].targetFanRadius)
        //                                {
        //                                    skillDate.unitDataBy = list[0];
        //                                }
        //                            }
        //                            break;
        //                        case TargetLocation.TargetCircleArea:
        //                            {
        //                                if (dis <= skillDate.skillBass.aimDirectionList[0].targetCircleAreaDis)
        //                                {
        //                                    skillDate.unitDataBy = list[0];
        //                                }
        //                            }
        //                            break;
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                // 朝向施法方向
        //                skillDate.unitAgentSend.rotationQuaternion = skillDate.quaternion;
        //            }
        //        }
        //    }
        //}
        #endregion

        #region 攻击目标
        /// <summary>
        /// 攻击目标，施加buff、光环
        /// </summary>
        /// <param name="unitAgent"></param>
        /// <param name="unitData"></param>
        /// <param name="damageInfo"></param>
        /// <param name="secondTarget"></param>
        /// <param name="pos"></param>
        /// <param name="skilllv"></param>
        /// <param name="attackRule"></param>
        /// <param name="dic"></param>
        //void AttackTartget(UnitAgent unitAgent, UnitData unitData, DamageInfo damageInfo, SecondTarget secondTarget, Vector3 pos, int skilllv, AttackRule attackRule, ref Dictionary<int, int> dic, SkillDate skilldate = null, int legionId = -1)
        //{
        //    if (dic == null)
        //    {
        //        dic = new Dictionary<int, int>();
        //    }
        //    if (dic.Count > 0 && unitData != null)
        //    {
        //        if (dic.ContainsKey(unitData.uid))
        //        {
        //            unitData = null;
        //        }
        //    }
        //    if (unitAgent != null && unitData != null)
        //    {
        //        //if (damageInfo.passiveJudgment)
        //        //{
        //        //    unitAgent.unitControl.OnPassiveEvent(Triggercondition.OnAttackTarget, unitAgent.unitData, unitData);
        //        //}
        //        //JudgmentInfo ji = new JudgmentInfo();
        //        //ji.judgmentTargger = JudgmentTargger.TargetType;
        //        //ji.unitType = unitData.unitType;
        //        //unitAgent.unitControl.OnPassiveJudgEvent(ji);
        //        //JudgmentInfo ji2 = new JudgmentInfo();
        //        //ji2.judgmentTargger = JudgmentTargger.TargetDis;
        //        //ji2.disTarget = Vector3.Distance(unitAgent.position, unitData.position);
        //        //unitAgent.unitControl.OnPassiveJudgEvent(ji2);

        //        DamageData damage = new DamageData();
        //        damage.AttackSend = unitAgent.unitData;
        //        damage.damageInfBaseCSV = damageInfo.damageInfBaseCSV;
        //        damage.damageInfBaseCSV.skillLv = skilllv;
        //        if (damage.damageInfBaseCSV.selectTarget == SelectTarget.Self)
        //        {
        //            unitAgent.unitData.GetDamageInfoVal(ref damage.damageInfBaseCSV);
        //        }
        //        unitData.OnTakeDamage(damage);
        //        if (!dic.ContainsKey(unitData.uid))
        //        {
        //            dic.Add(unitData.uid, unitData.uid);
        //        }
        //    }
        //    if (damageInfo.bShakeEffect)
        //    {
        //        Game.camera.CameraMg.OnShakeCamera(damageInfo.shakeTime, damageInfo.shakeAmplitude, damageInfo.shakeRange, pos);
        //    }
        //    if (damageInfo.buffid > 0)
        //    {
        //        // 生成buff
        //        BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(damageInfo.buffid);
        //        if (damageInfo.bufflife > 0)
        //        {
        //            buffInfo.mLife = damageInfo.bufflife;
        //        }
        //        room.haoleBuff.AddOneBuffForUnit(unitData, buffInfo, skilllv, unitAgent.unitData);
        //    }

        //    if (damageInfo.haloid > 0)
        //    {
        //        // 生成定位光环
        //        BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(damageInfo.haloid);
        //        if (damageInfo.bufflife > 0)
        //        {
        //            buffInfo.mLife = damageInfo.bufflife;
        //        }
        //        room.haoleBuff.AddHalos(unitAgent.unitData.LegionId, attackRule, buffInfo, pos, skilllv, unitData, unitAgent.unitData);
        //    }
        //    if (unitData == null && secondTarget.hitActiveSecond)
        //    {
        //        return;
        //    }
        //    // 是否有爆点，二次伤害效果
        //    if (secondTarget.maxSceondTargets > 0)
        //    {
        //        #region 爆炸特效
        //        WarInstantiationEffect.Instance.InitEffect(skilldate, secondTarget.secondEffectPath, secondTarget.life, pos, null, legionId);
        //        #endregion

        //        // "播放音效";
        //        Game.audio.PlayWarSFX(secondTarget.secondaryMusic, pos);
        //        if (secondTarget.damageInfo.bShakeEffect)
        //        {
        //            Game.camera.CameraMg.OnShakeCamera(secondTarget.damageInfo.shakeTime, secondTarget.damageInfo.shakeAmplitude, secondTarget.damageInfo.shakeRange, pos);
        //        }
        //        #region 伤害计算
        //        if (secondTarget.secondaryRadius > 0/* && secondTarget.damageInfo.damageVal != 0*/)
        //        {
        //            int _count = 0;
        //            List<UnitData> TemUnitDataList = room.sceneData.SearchUnitListInRule(unitAgent.unitData, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, secondTarget.secondaryRadius,pos,out _count,true,secondTarget.warSkillRule,secondTarget.maxSceondTargets);
        //            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
        //            {
        //                TemUnitDataList = TemUnitDataList.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
        //            }
        //            if (dic != null && dic.Count > 0)
        //            {
        //                for (int j = TemUnitDataList.Count - 1; j >= 0; j--)
        //                {
        //                    if (dic.ContainsKey(TemUnitDataList[j].unitData.uid))
        //                    {
        //                        TemUnitDataList.RemoveAt(j);
        //                    }
        //                }
        //            }
        //            _count = TemUnitDataList.Count;
        //            if (_count > 0)
        //            {
        //                BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(secondTarget.damageInfo.buffid);
        //                if (secondTarget.damageInfo.bufflife > 0)
        //                {
        //                    buffInfo.mLife = secondTarget.damageInfo.bufflife;
        //                }
        //                for (int l = 0; l < _count; l++)
        //                {
        //                    room.haoleBuff.AddOneBuffForUnit(TemUnitDataList[l], buffInfo, skilllv, unitAgent.unitData);
        //                    DamageData damage = new DamageData();
        //                    damage.AttackSend = unitAgent.unitData;
        //                    //damage.AttakcVal = secondTarget.damageInfo.damageVal;
        //                    damage.damageInfBaseCSV = secondTarget.damageInfo.damageInfBaseCSV;
        //                    damage.damageInfBaseCSV.skillLv = skilllv;
        //                    if (damage.damageInfBaseCSV.selectTarget == SelectTarget.Self)
        //                    {
        //                        unitAgent.unitData.GetDamageInfoVal(ref damage.damageInfBaseCSV);
        //                    }
        //                    TemUnitDataList[l].unitData.OnTakeDamage(damage);
        //                    // "播放音效";
        //                    Game.audio.PlayWarSFX(secondTarget.damageInfo.DamageMusic, TemUnitDataList[l].position);
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //}
        #endregion

        #region 参数配置

        //private AttackRule  attackRule      = null;
        //private SkillDate _skillData        = null;

        //public SkillDate skillDate
        //{
        //    set
        //    {
        //        // 保持主动技能唯一性，当前技能未进入后摇不接受下一个指令
        //        if (!startSkill || _skillData == null || (startSkill && afterShaking) || (startSkill && _skillData.isNormalSkill))
        //        {
        //            if (startSkill && afterShaking && _skillData.isNormalSkill)
        //            {
        //                if (_skillData.skillBass.skillId == value.skillBass.skillId)
        //                {
        //                    return;
        //                }
        //            }
        //            attackRule = null;
        //            _skillData = value;
        //            startSkill = true;
        //            afterShaking = false;
        //            //if (_skillData.unitDataBy != null && _skillData.unitDataBy.IsInSceneAndLive)
        //            //{
        //            //    _skillData.lookAtPos = _skillData.unitDataBy.AnchorAttackbyPos;
        //            //}
        //            //else
        //            //{
        //            //    _skillData.lookAtPos = new Vector3(-100001, 0);
        //            //}
        //            skillTriggerInfList = getskillTriggerInfList(skillDate.skillBass.actionEvent.actionEventList, skillDate.skillBass.skillId, true);
        //        }

        //    }
        //    get
        //    {
        //        return _skillData;
        //    }
        //}

        #endregion
        #region 获取技能事件流程
        //private List<SkillTriggerInf>   skillTriggerInfList = new List<SkillTriggerInf>();
        //List<SkillTriggerInf> getskillTriggerInfList(List<SkillTriggerEvent> list, int skillid, bool use = false, UnitData origin = null, UnitData target = null, int legionId = -1)
        //{
        //    List<SkillTriggerInf> result = new List<SkillTriggerInf>();
        //    int skilllv = 1;
        //    SkillController skillController = unitData.GetSkill(skillid);
        //    if (skillController != null)
        //    {
        //        skilllv = skillController.skillLevel;
        //    }
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        SkillTriggerInf sti = new SkillTriggerInf();
        //        sti.skillTriggerEvent = list[i];
        //        if (sti.skillTriggerEvent.activeLv <= skilllv && sti.skillTriggerEvent.blockLv >= skilllv)
        //        {
        //            sti.bLife = false;
        //            sti.startTime = LTime.time;
        //            sti.origin = origin;
        //            sti.target = target;
        //            sti.skilllv = skilllv;
        //            sti.legionId = legionId;
        //            result.Add(sti);
        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region 使用技能
        //public void Use(SkillDate skillDate)
        //{
        //    if (skillDate != null)
        //    {
        //        this.skillDate = skillDate;
        //    }
        //}
        #endregion
        #region 使用被动技能
        //public void Use(UnitAgent unitAgent, int skillid)
        //{
        //    SkillController skillController = unitAgent.unitData.GetSkill(skillid);
        //    SkillBass skillBass = skillController.skillBass;
        //    skillBass.legionId = unitAgent.unitData.LegionId;
        //    PassiveList.Add(skillBass);
        //}
        #endregion
        #region 被动技能参数配置
        /// <summary>
        /// 队列中的被动技能
        /// </summary>
        //private List<SkillBass>                 PassiveList             = new List<SkillBass>();
        /// <summary>
        /// 被动技能事件
        /// </summary>
        //private List<List<SkillTriggerEvent>>   PassiveTriggerEventList = new List<List<SkillTriggerEvent>>();
        //private List<List<SkillTriggerInf>>     passiveTriggerInfList   = new List<List<SkillTriggerInf>>();
        //private List<TriggEvent>                TriggerconditionDic     = new List<TriggEvent>();
        //private List<JudgmentInfo>              judgmentInfoEvent       = new List<JudgmentInfo>();

        #endregion

        //public bool StopSkill()
        //{
        //    if (startSkill && afterShaking && _skillData.isNormalSkill)
        //    {
        //        Stop(skillDate, false);
        //        startSkill = false;
        //    }
        //    return !startSkill;
        //}

        #region 针对英雄的关闭普攻特效
        //public bool StopSkill()
        //{
        //    if (startSkill && afterShaking && _skillData.isNormalSkill)
        //    {
        //        Stop(skillDate);
        //        startSkill = false;
        //    }
        //    return !startSkill;
        //}
        //public void Stop(SkillDate skillDate)
        //{
        //    for (int i = 0; i < skillDate.skillBass.actionEvent.actionEventList.Count; i++)
        //    {
        //        SkillTriggerEvent targEvent = skillDate.skillBass.actionEvent.actionEventList[i];
        //        if (targEvent.skillType == SkillType.PlayEffect)
        //        {
        //            WarInstantiationEffect.Instance.OnStopEffect(skillDate.unitAgentSend.uid, targEvent.effectPath);
        //        }
        //    }
        //    if (skillDate.unitAgentSend.unitType == UnitType.Hero || skillDate.unitAgentSend.unitType == UnitType.Player)
        //    {
        //        skillDate.unitAgentSend.aniManager.Do_Idle();
        //    }
        //    attackRule = null;
        //}
        #endregion


    //}
//}
