namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 动作
    /// </summary>
    public class SkillActionPlayAnimator : SkillAction
    {
        private SkillActionConfigAnimator actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigAnimator)config.config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            // 确定朝向
            if (warSkill.skillInfoConfig.activation == Activation.User)
            {
                SkillTriggerUser stu = (SkillTriggerUser)warSkill.tigger;
                stu.GetDir(actionConfig.attackRuleList);
            }
            // 播放动作
            if (actionConfig.warSkillEffectType == AnimatorState.Attack1)
            {
                warSkill.actionUnitAgent.warUnitcontrol.bDoAttack = true;
                warSkill.actionUnitAgent.warUnitcontrol.animatorState = AnimatorState.Attack;
            }
            else
            {
                warSkill.actionUnitAgent.aniManager.ActionAni(actionConfig.warSkillEffectType);
                if (warSkill.attackUnit != null)
                {
                    warSkill.actionUnitAgent.aniManager.SetAttackPos(warSkill.attackUnit.AnchorAttackbyTform);
                }
            }
            if (actionConfig.warSkillEffectType == AnimatorState.Attack || actionConfig.warSkillEffectType == AnimatorState.Attack1 || actionConfig.warSkillEffectType == AnimatorState.AttackL)
            {
                UIHandler.OnHandPassiveed(Triggercondition.OnStartAttack, warSkill.actionUnitAgent.unitData);
            }
            End();
        }

    }
}