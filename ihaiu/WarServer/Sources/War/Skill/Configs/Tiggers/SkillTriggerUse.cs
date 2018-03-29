using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public class SkillTriggerUse : SkillTriggerConfig
    {
        /// <summary>
        /// 是否无敌状态
        /// </summary>
        public bool                 invincible              = false;
        /// <summary>
        /// 是否打断其他技能
        /// </summary>
        public bool                 breakOther              = false;
        /// <summary>
        /// 技能执行的方向和位置
        /// </summary>
        public TargetLocation       targetLocation;
        /// <summary>
        /// 技能执行方向及参数
        /// </summary>
        public SkillTriggerLocation skillTriggerLocation    = null;
        
    }
}