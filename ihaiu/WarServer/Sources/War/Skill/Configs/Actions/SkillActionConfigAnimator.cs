using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 动作
    /// </summary>
    [Serializable]
    public class SkillActionConfigAnimator : SkillActionConfig
    {
        /// <summary>
        /// 播放哪个动作
        /// </summary>
        public AnimatorState        warSkillEffectType          = AnimatorState.Idle;
        /// <summary>
        /// 动作时长
        /// </summary>
        //public float                skillPlayTime               = 0;
    }
}
