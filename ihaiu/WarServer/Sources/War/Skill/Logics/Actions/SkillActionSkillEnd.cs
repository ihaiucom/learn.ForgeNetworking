using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 结束技能
    /// </summary>
    public class SkillActionSkillEnd : SkillAction
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
            if (warSkill.actionUnitAgent != null)
            {
                warSkill.actionUnitAgent.unitData.isSkillAttack = false;
                room.clientOperationUnit.OnSkillEnterCD();
            }
            End();
        }
        protected override void OnEnd()
        {
            // 关闭普通特效，进入idle动作
            //if (warSkill.actionUnitAgent != null && warSkill.actionUnitAgent.aniManager != null)
            //{
            //    //warSkill.actionUnitAgent.aniManager.Do_Idle();
            //}
        }
    }
}