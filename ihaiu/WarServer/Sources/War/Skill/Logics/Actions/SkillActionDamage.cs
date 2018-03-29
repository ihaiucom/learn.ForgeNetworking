using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 伤害
    /// </summary>
    public class SkillActionDamage : SkillAction
    {
        public SkillActionConfigDamage actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigDamage)config.config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            // 播放攻击音效
            Game.audio.PlaySoundWarSFX(actionConfig.attackSoundPath, warSkill.actionUnitAgent.position);
            // 选择攻击目标
            List<UnitData> list = manager.SelectAttackTarget(actionConfig.maxTargetsSelect,warSkill.skillInfoConfig,warSkill.actionUnitAgent,warSkill.attackUnit,warSkill.attackPos, actionConfig.attackRuleList,actionConfig);
            // 对目标造成伤害
            int skillLv = 1;
            SkillController skillController = warSkill.actionUnitAgent.unitData.GetSkill(warSkill.skillInfoConfig.skillId);
            if (skillController != null) skillLv = skillController.skillLevel;
            // 已经攻击过的列表
            Dictionary<int, int> dic = new Dictionary<int, int>();


            // 震屏
            if (config.bShakeEffect)
            {
                Game.camera.CameraMg.OnShakeCamera(config.shakeTime, config.shakeAmplitude, config.shakeRange, warSkill.actionUnitAgent.position);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (!room.sceneData.CheckUnitInSafeRegion(list[i]) && !list[i].isCloneUnit)
                {
                    manager.OnDamage(warSkill.actionUnitAgent, list[i], actionConfig.attackRuleList, actionConfig, list[i].AnchorAttackbyPos, skillLv, warSkill.skillInfoConfig.skillId, ref dic);
                }
            }
            // 结束
            End();
        }
    }
}