using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 5:40:54 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 机关攻击
    /// 临时放置视图里，以后放到数据逻辑层
    /// </summary>
    public class UnitTowerBehaviour : AbstractUnitMonoBehaviour
    {
        public enum AttackState
        {
            None,
            /** 检测目标 */
            CheckTarget,
            /** 攻击 */
            Attack,
        }


        private AttackState state = AttackState.None;

        private UnitData _target;
        private UnitData target
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
            }
        }



        public SkillController skillController;
        public SkillConfig skillConfig;

        public override void Init(UnitData unitData)
        {
            base.Init(unitData);

            skillController = unitData.SkillA;
            if (skillController != null)
            {
                skillConfig = skillController.skillConfig;
            }
            state = AttackState.CheckTarget;
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            state = AttackState.None;
            target = null;
        }

        private bool enableAttack
        {
            get
            {
                if (state == AttackState.None) return false;
                if (unitData.isManualAttack) return false;
                return prop.EnableAttack;
            }
        }


        /// <summary>
        /// 检测目标是否可以攻击
        /// </summary>
        /// <returns></returns>
        virtual public bool CheckTargetEnableAttack(UnitData target)
        {
            if (!target.IsInSceneAndLive || room.sceneData.CheckUnitInSafeRegion(target) || target.isCloneUnit)
            {
                return false;
            }



            float distance = unitData.Distance(target);
            return distance <= prop.AttackRadius;
        }

        private bool isEmploying = false;

        private void Update()
        {
            if (unitAgent == null || !unitAgent.photonView.isMine) return;

            if (skillConfig == null) return;
            if (!room.IsGameing) return;

            // 天梯准备时间是否结束
            if (room.stageType == StageType.PVPLadder && !room.clientOperationUnit.IsStart) return;

            if (enableAttack)
            {
                if(unitData.IsEmploying)
                {
                    if (unitData.attackCD <= 0 && skillController.cd <= 0)
                    {
                        unitAgent.photonView.RPC("RPC_TowerAttckEmploying", PhotonTargets.Others, skillController.skillId);

                        unitAgent.ActionAttack(null, skillController.skillId);
                        unitData.attackCD = unitData.prop.AttackInterval;
                        skillController.cd = skillController.skillLevelConfig.cd;
                    }
                }
                else
                {
                    switch (state)
                    {
                        case AttackState.CheckTarget:
                            target = sceneData.SearchUnit(skillConfig, unitData);
                            if (target != null && !room.sceneData.CheckUnitInSafeRegion(target) && !target.isCloneUnit)
                            {
                                state = AttackState.Attack;
                            }
                            break;
                        case AttackState.Attack:
                            if (CheckTargetEnableAttack(target))
                            {
                                if (unitData.attackCD <= 0 && skillController.cd <= 0)
                                {
                                    unitAgent.photonView.RPC("RPC_TowerAttck", PhotonTargets.Others, target.uid, skillController.skillId);
                                    unitAgent.ActionAttack(target, skillController.skillId);
                                    unitData.attackCD = unitData.prop.AttackInterval;
                                    skillController.cd = skillController.skillLevelConfig.cd;
                                    state = AttackState.CheckTarget;
                                }
                            }
                            else
                            {
                                target = null;
                                state = AttackState.CheckTarget;
                            }
                            break;

                    }
                }
                
            }

            if (state != AttackState.None)
            {
                if (unitData.attackCD > 0)
                {
                    unitData.attackCD -= LTime.deltaTime;
                }
                else
                {
                    unitAgent.ActionShowModel();
                }

                if(isEmploying != unitData.IsEmploying)
                {
                    if(isEmploying)
                    {
                        target = null;
                        state = AttackState.CheckTarget;
                    }
                    isEmploying = unitData.IsEmploying;
                }
            }

        }


        [PunRPC]
        void RPC_TowerAttck(int targetUnitUid, int skillId)
        {
            target = room.sceneData.GetUnit(targetUnitUid);
            if (target != null && !room.sceneData.CheckUnitInSafeRegion(target) && !target.isCloneUnit)
            {
                unitAgent.ActionAttack(target, skillId);
            }
        }


        [PunRPC]
        void RPC_TowerAttckEmploying(int skillId)
        {
            unitAgent.ActionAttack(null, skillId);
        }

    }
}
