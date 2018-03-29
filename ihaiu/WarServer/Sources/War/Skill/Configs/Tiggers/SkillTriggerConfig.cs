using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    [Serializable]
    public class SkillTriggerConfig
    {
        /// <summary>
        /// 主动或被动技能
        /// </summary>
        public Activation           activation;
        /// <summary>
        /// 目标选择
        /// </summary>
        //public AttackRule           attackRuleList          = new AttackRule();
        /// <summary>
        /// 强制停止释放技能
        /// </summary>
        public bool                 forceStop               = true;

        public  static SkillTriggerConfig CreateConfig(Activation activation)
        {
            SkillTriggerConfig config = null;
            switch (activation)
            {
                case Activation.User:
                    config = new SkillTriggerUse();
                    break;
                case Activation.Passive:
                    config = new SkillTriggerPassive();
                    break;
            }
            config.activation = activation;
            return config;
        }

    }
}