namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 音效
    /// </summary>
    public class SkillActionSound : SkillAction
    {
        private SkillActionConfigSound actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigSound)config.config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            if (warSkill.actionUnitAgent != null)
            {
                Game.audio.PlaySoundWarSFX(actionConfig.soundPath, warSkill.actionUnitAgent.position + warSkill.actionUnitAgent.forward * 3);
            }
            End();
        }

    }
}