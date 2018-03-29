using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能--触发器 主动技能
    /// </summary>
    [Serializable]
    public class SkillTriggerUser : SkillTrigger
    {
        public SkillTriggerUse tiggerConfig;
        public override void SetConfig(SkillTriggerConfig config)
        {
            base.SetConfig(config);
            tiggerConfig = (SkillTriggerUse)config;
        }

        public void GetDir(AttackRule attackRule)
        {
            if (warSkill.actionUnitAgent != null && tiggerConfig.targetLocation != TargetLocation.Self && tiggerConfig.targetLocation != TargetLocation.CircularShockwave)
            {

                if (warSkill.attackUnit != null && warSkill.attackUnit.IsInSceneAndLive)
                {
                    if (room.sceneData.CheckUnitInSafeRegion(warSkill.attackUnit) || warSkill.attackUnit.isCloneUnit)
                    {
                        warSkill.attackUnit = null;
                    }
                    else
                    {
                        // 有目标，朝向目标
                        warSkill.actionUnitAgent.LookAt(warSkill.attackUnit.AnchorAttackbyPos);
                    }
                }
                else
                {
                    warSkill.attackUnit = null;
                }

                if (warSkill.attackUnit == null)
                {
                    if (warSkill.auto)
                    {
                        List<UnitData> list = room.sceneData.GetUnitList().FindAll(m => attackRule.unitType.UContain(m.unitType) && attackRule.unitSpaceType.UContain(m.spaceType) && attackRule.relationType.RContain(m.unitData.GetRelationType(warSkill.actionUnitAgent.unitData.LegionId)));
                        if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
                        {
                            list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
                        }
                        if (list.Count > 0)
                        {
                            for (int i = list.Count - 1; i >= 0; i--)
                            {
                                if (room.sceneData.CheckUnitInSafeRegion(list[i]) || list[i].isCloneUnit)
                                {
                                    list.RemoveAt(i);
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            Vector3 pos = warSkill.actionUnitAgent.position;
                            list.Sort((UnitData a, UnitData b) =>
                            {
                                float aa = Vector3.Distance(pos, a.position);
                                float bb = Vector3.Distance(pos, b.position);
                                if (aa > bb)
                                {
                                    return 1;
                                }
                                else if (aa < bb)
                                {
                                    return -1;
                                }
                                return 0;
                            });
                            // 攻击目标
                            warSkill.actionUnitAgent.LookAt(list[0].AnchorAttackbyPos);
                            float dis = Vector3.Distance(warSkill.actionUnitAgent.position.SetY(0), list[0].AnchorAttackbyPos.SetY(0));
                            SkillTriggerUse skillTriggerUse = (SkillTriggerUse)warSkill.skillInfoConfig.skillTriggerConfig;
                            switch (skillTriggerUse.targetLocation)
                            {
                                case TargetLocation.FanWave:
                                    {
                                        if (dis <= ((SkillTriggerUseFanWave)skillTriggerUse.skillTriggerLocation).targetFanRadius)
                                        {
                                            warSkill.attackUnit = list[0];
                                        }
                                    }
                                    break;
                                case TargetLocation.LinearWave:
                                    {
                                        if (dis <= ((SkillTriggerUseLinearWave)skillTriggerUse.skillTriggerLocation).targetFanRadius)
                                        {
                                            warSkill.attackUnit = list[0];
                                        }
                                    }
                                    break;
                                case TargetLocation.TargetCircleArea:
                                    {
                                        if (dis <= ((SkillTriggerUseTargetCircleArea)skillTriggerUse.skillTriggerLocation).targetCircleAreaDis)
                                        {
                                            warSkill.attackUnit = list[0];
                                        }
                                    }
                                    break;
                            }
                        }
                        if (warSkill.attackUnit != null)
                        {
                            warSkill.actionUnitAgent.LookAt(warSkill.attackUnit.AnchorAttackbyPos);
                        }
                    }
                    else
                    {
                        // 朝向施法方向
                        warSkill.actionUnitAgent.rotationQuaternion = warSkill.quaternion;
                    }
                }
            }
        }

        public override void Start()
        {
            if (warSkill.actionUnitAgent != null)
            {
                warSkill.actionUnitAgent.unitData.invincible = tiggerConfig.invincible;
                warSkill.actionUnitAgent.unitData.isSkillAttack = tiggerConfig.forceStop;
                room.clientOperationUnit.OnSkillEnterCD();
            }
            OnTirgger();
        }

    }
}
