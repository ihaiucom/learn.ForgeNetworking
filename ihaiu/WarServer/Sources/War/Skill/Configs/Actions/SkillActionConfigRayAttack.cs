using FMODUnity;
using System;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 射线攻击
    /// </summary>
    [Serializable]
    public class SkillActionConfigRayAttack : SkillActionConfig
    {
        /// <summary>
        /// 多次伤害间的cd
        /// </summary>
        public float                    damageCD                    = 0.2F;
        /// <summary>
        /// 子弹伤害详情
        /// </summary>
        public SkillActionConfigDamage  skillActionConfigDamage         = new SkillActionConfigDamage();
    }
}
