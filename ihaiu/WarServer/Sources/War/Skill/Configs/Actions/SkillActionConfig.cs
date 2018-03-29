using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件
    /// </summary>
    [Serializable]
    public class SkillActionConfig
    {
        /// <summary>
        /// 类型
        /// </summary>
        public SKillActionType          sKillActionType;
        /// <summary>
        /// 开始时间
        /// </summary>
        public float                    triggerTime          = 0;
        /// <summary>
        /// 是否震屏
        /// </summary>
        public  bool                    bShakeEffect                = false;
        /// <summary>
        /// 振幅
        /// </summary>
        public  float                   shakeAmplitude              = 0;
        /// <summary>
        /// 震屏时间
        /// </summary>
        public  float                   shakeTime                   = 0;
        /// <summary>
        /// 影响范围
        /// </summary>
        public  float                   shakeRange                  = 0;
        /// <summary>
        /// 启用等级
        /// </summary>
        public int                      activeLv                    = 1;
        /// <summary>
        /// 停用等级
        /// </summary>
        public int                      blockLv                     = 15;
        /// <summary>
        /// 目标选择
        /// </summary>
        public AttackRule               attackRuleList              = new AttackRule();

        public SkillActionConfig config { set; get; }

        public void SetConfig()
        {
            switch (sKillActionType)
            {
                case SKillActionType.PlayAnimator:
                    config = new SkillActionConfigAnimator();
                    break;
                case SKillActionType.PlayEffect:
                    config = new SkillActionConfigEffect();
                    break;
                case SKillActionType.PlayMusic:
                    config = new SkillActionConfigSound();
                    break;
                case SKillActionType.TriggerDamage:
                    config = new SkillActionConfigDamage();
                    break;
                case SKillActionType.ShotBullet:
                    config = new SkillActionConfigProjectile();
                    break;
                case SKillActionType.MoveTowardCurrDir:
                    config = new SkillActionConfigMoveTowardCurrDir();
                    break;
                case SKillActionType.CreateMirror:
                    config = new SkillActionConfigMirror();
                    break;
                case SKillActionType.BuffEffective:
                    config = new SkillActionConfigBuffCreate();
                    break;
                case SKillActionType.RayAttack:
                    config = new SkillActionConfigRayAttack();
                    break;
                default:
                    config = new SkillActionConfig();
                    break;
            }
            config.sKillActionType = sKillActionType;
        }
    }
}
