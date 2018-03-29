using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 位移
    /// </summary>
    public class SkillActionMoveToward : SkillAction
    {
        private SkillActionConfigMoveTowardCurrDir actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigMoveTowardCurrDir)config.config;
            endType = StoryActionEndType.Call;
        }

        /// <summary>
        /// 位移距离
        /// </summary>
        private float       moveEndDis     = 0;
        /// <summary>
        /// 位移速度
        /// </summary>
        private float       moveSpeed      = 0.5F;
        /// <summary>
        /// 启动位置
        /// </summary>
        private Vector3     StartPos;
        private float       moveAllTime    = 0;

        protected override void OnStart()
        {
            if (actionConfig.isBlink)
            {
                // 瞬移
                StageRouteConfig srcf = room.sceneData.GetRoute(actionConfig.blinkRoute);
                if (srcf != null && actionConfig.blinkChild < srcf.path.Count)
                {
                    warSkill.actionUnitAgent.position = srcf.GetBeginPoint(actionConfig.blinkChild);
                    warSkill.actionUnitAgent.rotation = srcf.GetBeginDirection(actionConfig.blinkChild);
                }
                End();
            }
            else
            {
                // 记录初始位置
                StartPos = warSkill.actionUnitAgent.position;
                moveEndDis = actionConfig.selfMoveDis;
                moveSpeed = actionConfig.selfMoveSpeed * 0.1F;
                moveAllTime = moveEndDis / moveSpeed / warSkill.actionUnitAgent.unitData.prop.MovementSpeed + room.LTime.time;
            }
        }

        protected override void OnTick()
        {
            // 移动中
            Quaternion _temQuaternion = Quaternion.LookRotation(warSkill.actionUnitAgent.forward);
            warSkill.actionUnitAgent.Move(_temQuaternion, warSkill.actionUnitAgent.unitData.prop.MovementSpeed * moveSpeed, false);
            if (room.LTime.time > moveAllTime || Vector3.Distance(warSkill.actionUnitAgent.position, StartPos) >= moveEndDis)
            {
                End();
            }
        }

    }
}