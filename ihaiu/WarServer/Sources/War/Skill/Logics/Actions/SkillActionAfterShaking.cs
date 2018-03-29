using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 后摇
    /// </summary>
    public class SkillActionAfterShaking : SkillAction
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
            warSkill.actionUnitAgent.unitData.isSkillAttack = false;
            room.clientOperationUnit.OnSkillEnterCD();
            End();
        }

    }
}