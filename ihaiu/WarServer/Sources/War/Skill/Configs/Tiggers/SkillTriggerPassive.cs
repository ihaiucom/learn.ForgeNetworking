using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 被动技能
    /// </summary>
    public class SkillTriggerPassive : SkillTriggerConfig
    {
        /// <summary>
        /// 被动触发条件
        /// </summary>
        public Triggercondition         triggercondition            = Triggercondition.None;
        /// <summary>
        /// 同时满足条件或仅满足一个条件就触发
        /// </summary>
        public bool                     andor                       = false;
        /// <summary>
        /// 判断条件
        /// </summary>
        public List<PassiveJudgment>    passiveJudgmentList         = new List<PassiveJudgment>();
    }
}