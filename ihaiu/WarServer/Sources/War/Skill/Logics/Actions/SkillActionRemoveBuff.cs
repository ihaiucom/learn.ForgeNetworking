using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 移除buff
    /// </summary>
    public class SkillActionRemoveBuff : SkillAction
    {
        private SkillActionConfig actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            End();
        }

    }
}