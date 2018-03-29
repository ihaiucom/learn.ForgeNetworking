using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 技能 json存储
    /// </summary>
    public class SkillInfoConfig
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public int                              skillId                     = 0;
        /// <summary>
        /// 前置技能id
        /// </summary>
        public int                              preSkillId                  = 0;
        /// <summary>
        /// 后续技能id(理论上都是触发被动效果的，加载技能时，后续技能需全部加载)
        /// </summary>
        public string                           followSkillIdList           = "";
        /// <summary>
        /// 技能名称
        /// </summary>
        public string                           skillName                   = "";
        /// <summary>
        /// 技能描述
        /// </summary>
        public string                           skillDes                    = "";
        /// <summary>
        /// 技能优先级
        /// </summary>
        public int                              skillPriority               = 0;
        /// <summary>
        /// 激活类型
        /// </summary>
        public Activation                       activation;
        /// <summary>
        /// 激活配置
        /// </summary>
        public SkillTriggerConfig               skillTriggerConfig          = null;
        /// <summary>
        /// 技能事件列表
        /// </summary>
        public List<SkillActionConfig>          SkillActionConfigList       = new List<SkillActionConfig>();
    }
}