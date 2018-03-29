using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 射线攻击
    /// </summary>
    public class SkillActionRayAttack : SkillAction
    {
        public SkillActionConfigRayAttack actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigRayAttack)config.config;

            if (warSkill.skillInfoConfig.activation == Activation.User)
            {
                SkillTriggerUse skillTriggerUse = (SkillTriggerUse)warSkill.skillInfoConfig.skillTriggerConfig;
                if (skillTriggerUse.targetLocation == TargetLocation.LinearWave)
                {
                    SkillTriggerUseLinearWave skillTriggerUseLinearWave = (SkillTriggerUseLinearWave)skillTriggerUse.skillTriggerLocation;
                    maxDis = skillTriggerUseLinearWave.targetFanRadius;
                }
            }
            endType = StoryActionEndType.Call;
        }

        float maxDis = 0;

        protected override void OnStart()
        {
            // 播放攻击音效
            Game.audio.PlaySoundWarSFX(actionConfig.skillActionConfigDamage.attackSoundPath, warSkill.actionUnitAgent.position);

            if (warSkill.attackUnit == null)
            {
                // 选择攻击目标
                List<UnitData> list = manager.SelectAttackTarget(1,warSkill.skillInfoConfig,warSkill.actionUnitAgent,warSkill.attackUnit,warSkill.attackPos, actionConfig.attackRuleList);
                if (list.Count == 1)
                {
                    warSkill.attackUnit = list[0];
                }
            }

            // 震屏
            if (config.bShakeEffect)
            {
                Game.camera.CameraMg.OnShakeCamera(config.shakeTime, config.shakeAmplitude, config.shakeRange, warSkill.actionUnitAgent.position);
            }

            if (warSkill.actionUnitAgent.currentRayAttackEffect == null)
            {
                Transform parent = warSkill.actionUnitAgent.shotTform;
                warSkill.actionUnitAgent.currentRayAttackEffect = manager.GetEffectFromPool(actionConfig.skillActionConfigDamage.attackEffectPath, parent, true, actionConfig.skillActionConfigDamage.pathOffset);
                warSkill.actionUnitAgent.currentRayAttackEffect.fxLineSegment = warSkill.actionUnitAgent.currentRayAttackEffect.gObject.GetComponent<FxLineSegment>();
                warSkill.actionUnitAgent.currentRayAttackEffect.fxLineSegment.maxDis = maxDis;
            }
            if (warSkill.attackUnit != null)
            {
                warSkill.actionUnitAgent.currentRayAttackEffect.fxLineSegment.endNode = warSkill.attackUnit.AnchorAttackbyTform;
            }
            else
            {
                float endDis = maxDis;
                warSkill.attackPos = warSkill.actionUnitAgent.shotTform.position + warSkill.actionUnitAgent.shotTform.forward * endDis;
                warSkill.actionUnitAgent.currentRayAttackEffect.fxLineSegment.SetValue(warSkill.attackPos);
            }

            int skillLv = 1;
            SkillController skillController = warSkill.actionUnitAgent.unitData.GetSkill(warSkill.skillInfoConfig.skillId);
            if (skillController != null) skillLv = skillController.skillLevel;

            warSkill.actionUnitAgent.currentRayAttackEffect.OnStart(room, actionConfig.skillActionConfigDamage, warSkill.actionUnitAgent.unitData, warSkill.attackUnit, actionConfig.attackRuleList, skillLv, warSkill.skillInfoConfig.skillId, actionConfig.damageCD, actionConfig.skillActionConfigDamage.life, warSkill.attackPos, maxDis);
            manager.skillBulletList.Add(warSkill.actionUnitAgent.currentRayAttackEffect);
            
            // 结束
            End();

        }
    }
}