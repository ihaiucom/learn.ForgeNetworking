using FMODUnity;
using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 造成伤害
    /// </summary>
    [Serializable]
    public class SkillActionConfigDamageSecond : SkillActionConfig
    {
        /// <summary>
        /// 二次伤害规则
        /// </summary>
        public WarSkillRule             warSkillRule                = WarSkillRule.RandomRange;
        /// <summary>
        /// 二次伤害目标数量
        /// </summary>
        public int                      maxSceondTargets            = 1;
        /// <summary>
        /// 是否启用被动监听
        /// </summary>
        public bool                     passiveJudgment             = false;
        /// <summary>
        /// 攻击到目标时的音效文件路径
        /// </summary>
        [SoundKey]
        public string                   attackSoundPath             = "";
        /// <summary>
        /// 特效路径
        /// </summary>
        public string                   secondEffectPath            = "";
        public string                   secondEffectPathsuf         = "";
        /// <summary>
        /// 生命周期
        /// </summary>
        public float                    life                        = 1;
        /// <summary>
        /// 攻击到目标才有二次伤害
        /// </summary>
        public bool                     hitActiveSecond             = false;
        /// <summary>
        /// 二次伤害半径
        /// </summary>
        public float                    secondaryRadius             = 10;
        /// <summary>
        /// 伤害计算
        /// </summary>
        public DamageInfBaseCSV         damageInfBaseCSV            = new DamageInfBaseCSV();
        /// <summary>
        /// 附加到受击者的buff
        /// </summary>
        public int                      buffid                      = 0;
        /// <summary>
        /// buff生命周期
        /// </summary>
        public float                    bufflife                    = 0;
    }
}
