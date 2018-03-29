using FMODUnity;
using System;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 造成伤害
    /// </summary>
    [Serializable]
    public class SkillActionConfigDamage : SkillActionConfig
    {
        /// <summary>
        /// 是否仅仅存在二次伤害
        /// </summary>
        public bool                     onlySecondDamage            = false;
        /// <summary>
        /// 伤害次数
        /// </summary>
        public int                      damageTimes                 = 1;
        /// <summary>
        /// 多次伤害间的cd
        /// </summary>
        public float                    damageCD                    = 0;
        /// <summary>
        /// 目标数量
        /// </summary>
        public int                      maxTargetsSelect            = 1;
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
        public string                   attackEffectPath            = "";
        public string                   attackEffectPathsuf         = "";
        /// <summary>
        /// 特效路径2，非常规子弹
        /// </summary>
        public string                   attackEffectPath2           = "";
        public string                   attackEffectPathsuf2        = "";
        /// <summary>
        /// 偏移坐标
        /// </summary>
        public  Vector3                 pathOffset                  = Vector3.zero;
        /// <summary>
        /// 特效生命周期
        /// </summary>
        public  float                   life                        = 1;
        /// <summary>
        /// 被动选择规则
        /// </summary>
        public  PassiveTargetSelect     passiveTargetSelect         = null;
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
        /// <summary>
        /// 是否消耗当前所有子弹
        /// </summary>
        public bool                     costBullet                  = false;
        /// <summary>
        /// 当前点生成光环
        /// </summary>
        public int                      haloid                      = 0;
        /// <summary>
        /// 光环生命周期
        /// </summary>
        public float                    halolife                    = 0;
        /// <summary>
        /// 是否存在二次伤害
        /// </summary>
        public bool                     haveDamageSecond            = false;
        /// <summary>
        /// 二次伤害详情
        /// </summary>
        public SkillActionConfigDamageSecond damageSecond           = null;
    }
}
