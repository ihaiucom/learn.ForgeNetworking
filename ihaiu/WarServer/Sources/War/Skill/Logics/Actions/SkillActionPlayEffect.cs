using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 特效
    /// </summary>
    public class SkillActionPlayEffect : SkillAction
    {
        /// <summary>
        /// 展示中的特效
        /// </summary>
        private SkillBullet                         showEffects     = null;
        private SkillActionConfigEffect             actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigEffect)config.config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            // 震屏
            if (config.bShakeEffect)
            {
                Game.camera.CameraMg.OnShakeCamera(config.shakeTime, config.shakeAmplitude, config.shakeRange, warSkill.actionUnitAgent.position);
            }

            if (actionConfig.effectPath == null || actionConfig.effectPath.Length < 2)
            {
                End();
                return;
            }

            Transform parent = warSkill.actionUnitAgent.modelTform;
            if (actionConfig.followSelf)
            {
                if (actionConfig.isShotPos)
                {
                    parent = warSkill.actionUnitAgent.shotTform;
                }
            }
            showEffects = manager.GetEffectFromPool(actionConfig.effectPath, parent, actionConfig.followSelf, actionConfig.pathOffset);
            showEffects.OnStart(room, actionConfig.life);
            manager.skillBulletList.Add(showEffects);
            End();
        }

    }
}