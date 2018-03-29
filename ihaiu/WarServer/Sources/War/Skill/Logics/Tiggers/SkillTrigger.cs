using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能--触发器基类触发
    /// </summary>
    [Serializable]
    public abstract class SkillTrigger
    {
        public WarRoom                  room;
        public WarSkill                 warSkill;
        public WarRoomSkillManager      manager;
        public static SkillTrigger CreateTirgger(SkillTriggerConfig config, WarSkill warSkill)
        {
            SkillTrigger trigger = null;
            switch (config.activation)
            {
                case Activation.User:
                    trigger = new SkillTriggerUser();
                    break;
                case Activation.Passive:
                    trigger = new SkillTriggerPas();
                    break;
            }

            trigger.warSkill = warSkill;
            trigger.room = warSkill.room;
            trigger.manager = warSkill.manager;
            trigger.SetConfig(config);
            return trigger;
        }

        public SkillTriggerConfig config
        {
            set;
            get;
        }

        virtual public void SetConfig(SkillTriggerConfig config)
        {
            this.config = config;
        }

        virtual public void Start()
        {

        }


        /// <summary>
        /// 检测
        /// </summary>
        virtual public void Tick()
        {

        }

        /// <summary>
        /// 结束
        /// </summary>
        virtual public void End()
        {

        }

        /// <summary>
        /// 触发
        /// </summary>
        virtual public void OnTirgger()
        {
            warSkill.OnTigger();
        }
    }
}
