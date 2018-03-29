using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 10:58:43 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 士兵行为，默认士兵AI
    /// </summary>
    public partial class UnitSoliderBehaviour : AbstractUnitMonoBehaviour, IPunObservable
    {

        public enum BehaviourState
        {
            None,
            MoveRoute,
            MoveChase,
            Attack,
            Death,
            Freed,
            Birth,
        }

        public BehaviourState state;
        // 攻击目标
        public UnitData targetUnit;
        // 位置目标
        public Vector3 targetPosition;

        public float RadarRadius;
        public float AttackRadius;

        public AISoliderSkill   aiSoliderSkillA;
        public AISoliderSkill   aiSoliderSkill;
        public SkillConfig      skillConfig;
        // 士兵存活时间
        private float            aiSoliderLiveTime;
        // 士兵出生时间
        private float            aiSoliderBirthTime = 0;


        public bool IsAttackActioning
        {
            get
            {
                return unitData.disableAttack;
            }
        }


        public override void Init(UnitData unitData)
        {
            base.Init(unitData);

            if (navMeshAgent == null)
                navMeshAgent = GetComponent<UnitRVOAgent>();

            if (rvoController == null)
                rvoController = GetComponent<RVOController>();

            navMeshAgent.spaceType = unitData.spaceType;

            route = sceneData.GetRoute(unitData.routeId);
            routePointIndex = 1;
            routePointCount = route.path.Count;
            aiSoliderLiveTime = LTime.time;
            TickSelectSkill();
            MoveRoute();

            if (!unitAgent.photonView.isMine)
            {
                navMeshAgent.enabled = false;
            }
            aiSoliderBirthTime = -1;
            state = BehaviourState.Birth;
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(state);
                stream.SendNext(routePointIndex);
            }
            else
            {
                state = (BehaviourState)stream.ReceiveNext();
                routePointIndex = (int)stream.ReceiveNext();
            }
        }


        private void Update()
        {
            if (unitAgent == null || !unitAgent.photonView.isMine) return;

            //出生逻辑
            if (state == BehaviourState.Birth)
            {
                if (aiSoliderBirthTime > 0)
                {
                    if (LTime.time > aiSoliderBirthTime)
                    {
                        aniManager.Do_Run();
                        state = BehaviourState.MoveRoute;
                    }
                }
                else
                {
                    if (aniManager.aniLengthDic.ContainsKey("Birth"))
                    {
                        aiSoliderBirthTime = aniManager.aniLengthDic["Birth"] + LTime.time;
                        aniManager.Do_Birth();
                    }
                    else
                    {
                        aiSoliderBirthTime = LTime.time;
                        aniManager.Do_Idle();
                    }
                }
            }

            //活动副本调用士兵AI
            if (room != null && room.setting.soliderBehaviourType != UnitSoliderAIType.None)
            {
                UpdateActivity();
                return;
            }


            switch (state)
            {
                case BehaviourState.MoveRoute:
                    if (!unitData.disableMove)
                        TickMoveRoute();
                    break;
                case BehaviourState.MoveChase:
                    if (!unitData.disableMove)
                        TickChaseTargetUnit();
                    break;
                case BehaviourState.Attack:
                    if (!unitData.disableAttack)
                        TickAttackTargetUnit();
                    break;
            }


            switch (state)
            {
                case BehaviourState.MoveRoute:
                case BehaviourState.MoveChase:
                case BehaviourState.Attack:
                    searchTargetCD -= LTime.deltaTime;
                    if (!unitData.disableAttack)
                        TickSearchTargetAttack();
                    break;
            }

            if (moveToDelayTime > 0) moveToDelayTime -= LTime.deltaTime;
            switch (state)
            {
                case BehaviourState.MoveRoute:
                case BehaviourState.MoveChase:
                    if (moveToDelay && !IsAttackActioning && moveToDelayTime <= 0)
                    {
                        MoveTo(moveToDelayPos);
                    }
                    break;
            }


            if (state != BehaviourState.None)
            {
                if (!IsAttackActioning)
                {
                    if (selectSkillDurationCD > 0) selectSkillDurationCD = 0.0F;
                    if (moveToDelayTime > 1F) moveToDelayTime = 0.2f;

                }



                TickSelectSkill();
                if (unitData.attackCD > 0)
                {
                    unitData.attackCD -= LTime.deltaTime;
                }

                if (targetDuration > 0)
                {
                    targetDuration -= LTime.deltaTime;
                }

                RadarRadius = prop.RadarRadius;
                AttackRadius = prop.AttackRadius;

                navMeshAgent.maxSpeed = prop.MovementSpeed;
                if (prop.MovementSpeed == 0)
                {
                    navMeshAgent.enabled = false;
                }
                //else
                //{
                //    if (!navMeshAgent.enabled)
                //    {
                //        navMeshAgent.enabled = true;
                //    }
                //}



                if (preDisableMove != unitData.disableMove)
                {
                    if (unitData.disableMove)
                    {
                        if (navMeshAgent.enabled)
                        {
                            navMeshAgent.enabled = false;
                        }
                    }
                    else
                    {
                        //switch (state)
                        //{
                        //    case BehaviourState.MoveChase:
                        //    case BehaviourState.MoveRoute:
                        //        if (!navMeshAgent.enabled)
                        //        {
                        //            navMeshAgent.enabled = true;
                        //        }
                        //        break;
                        //}
                    }
                    preDisableMove = unitData.disableMove;
                }

                if (preIsStateFreezedMove != unitData.isStateFreezedMove)
                {
                    if (!unitData.isStateFreezedMove)
                    {
                        MoveTo(targetPosition);
                    }
                    preIsStateFreezedMove = unitData.isStateFreezedMove;
                }
            }


        }



#if UNITY_EDITOR
        public List<Vector3> prePoss = new List<Vector3>();
        private Vector3 prePos = Vector3.zero;
        void OnDrawGizmos()
        {
            //if (Vector3.Distance(prePos, transform.position) > 5)
            //{
            //    prePoss.Add(transform.position);
            //    prePos = transform.position;
            //}


            //PathGizmos.DrawPath(prePoss, Color.red);
        }
#endif
        /** 之前是否可以移动 */
        private bool preIsStateFreezedMove = false;
        /** 之前是否可以移动 */
        private bool preDisableMove = false;
        /** 之前是否可以攻击 */
        private bool preDisableAttack = false;


        #region 选择使用哪个技能

        /** 选择使用哪个技能频率间隔时间 */
        public float selectSkillInterval = 0.5f;
        /** 选择使用哪个技能频率间隔时间 */
        public float selectSkillCD = 0;
        public float selectSkillDuration = 6;
        public float selectSkillDurationCD = 0;
        public void TickSelectSkill()
        {
            selectSkillDurationCD -= LTime.deltaTime;

            selectSkillCD -= LTime.deltaTime;
            if (selectSkillCD <= 0)
            {
                selectSkillCD = selectSkillInterval;
                if (selectSkillDurationCD > 0) return;
                //if (unitData.attackCD > 0) return;

                float r = WarRandom.Range(0, 100);
                AISoliderSkill item = null;
                AISoliderSkill ai = null;

                float totalWeight = 0;
                for (int i = 0; i < unitData.aiSoliderSkillList.Count; i++)
                {
                    item = unitData.aiSoliderSkillList[i];
                    if (item.enableUse)
                    {
                        item.weightMinVal = totalWeight;
                        totalWeight += item.aiSoliderConfig.weight;
                        item.weightMaxVal = totalWeight;
                    }
                }

                for (int i = 0; i < unitData.aiSoliderSkillList.Count; i++)
                {
                    item = unitData.aiSoliderSkillList[i];
                    if (item.enableUse)
                    {
                        item.weightMin = item.weightMinVal / totalWeight * 100;
                        item.weightMax = item.weightMaxVal / totalWeight * 100;
                        if (r >= item.weightMin && r < item.weightMax)
                        {
                            ai = item;
                            break;
                        }
                    }
                }

                if (ai == null)
                    ai = unitData.attackAiSoliderSkill;

                if (ai != null && ai != aiSoliderSkill)
                {
                    //Loger.Log(ai.aiSoliderConfig.id + "   "+ ai.skillController.skillId + " " + ai.skillController.skillConfig.tip);
                    aiSoliderSkill = ai;
                    skillConfig = aiSoliderSkill.skillController.skillConfig;

                    if (unitData.aiSoliderSkillList.Count > 1 && targetUnit != null && targetUnit.Distance(unitData) < attackDistance * 1.2f) moveToDelayTime = unitData.attackCD;
                    targetUnit = null;
                    selectSkillDurationCD = selectSkillDuration;
                    chaseTargetCD = 0;
                    searchTargetCD = 0;
                }
            }
        }


        #endregion


        #region 搜索目标, 追击目标, 攻击目标
        /** 搜索目标频率间隔时间 */
        public float searchTargetInterval = 0.5f;
        /** 追击目标频率间隔时间 */
        public float chaseTargetInterval = 1f;

        /** 搜索目标CD */
        public float searchTargetCD = 0;
        /** 追击标CD */
        private float chaseTargetCD = 0;

        /** 当前攻击目标持续时间 */
        private float targetDurationConfig = 3;
        private float targetDuration = 0;

        /** 搜索单位距离 */
        public float radarDistance
        {
            get
            {
                return prop.RadarRadius;
            }
        }


        /** 攻击距离 */
        public float attackDistance
        {
            get
            {
                float min = unitData.rvoRadius + targetUnit.rvoRadius + 1;
                float val = aiSoliderSkill.skillController.attackRadius;

                if (aiSoliderSkill != null && val > 0)
                {
                    return Mathf.Max(min, val);
                }
                return prop.AttackRadius + unitData.rvoRadius + targetUnit.rvoRadius;
            }
        }

        /** 追击单位距离1.5倍 */
        public float chaseDistance
        {
            get
            {
                if (attackDistance > radarDistance)
                {
                    return attackDistance * 1.5f;
                }
                else
                {
                    return radarDistance * 1.5f;
                }
            }
        }


        /** 搜索目标 */
        public void TickSearchTargetAttack()
        {
            if (room.sceneData.CheckUnitInSafeRegion(unitData))
            {
                // 本身在安全区，直接返回操作
                return;
            }
            if (skillConfig == null) return;
            if (searchTargetCD <= 0)
            {
                searchTargetCD = searchTargetInterval;

                List<UnitData> enumyList = sceneData.SearchUnitList(unitData, skillConfig.targetRelation, skillConfig.targetUnitType, skillConfig.targetBuildType, skillConfig.targetSoliderType, skillConfig.targetSpaceType, skillConfig.targetProfessionType, radarDistance, true);
                int count = enumyList.Count;
                float maxScore = 0;
                UnitData maxEnumy = null;
                UnitData enumy;
                float scoreDistance = 0;
                float scoreHatred = 0;
                float scoreType = 0;
                for (int i = 0; i < enumyList.Count; i++)
                {
                    enumy = enumyList[i];
                    if (room.sceneData.CheckUnitInSafeRegion(enumy) || enumy.isCloneUnit) continue;
                    scoreDistance = unitData.Distance(enumy);
                    scoreDistance = Mathf.Clamp(1 - scoreDistance / radarDistance, 0, 2) * skillConfig.aiConfigDistance.GetVal(enumy);
                    scoreType = skillConfig.aiConfigType.GetVal(enumy);
                    scoreHatred = unitData.GetHatred(enumy.uid);

                    scoreHatred *= skillConfig.aiConfigWeight.weightHatred;
                    scoreType *= skillConfig.aiConfigWeight.weightType;
                    scoreDistance *= skillConfig.aiConfigWeight.weightDistance;


                    enumy.aiScore = scoreHatred + scoreType + scoreDistance;
                    if (enumy.aiScore > maxScore)
                    {
                        maxScore = enumy.aiScore;
                        maxEnumy = enumy;
                    }
                }


                if (maxEnumy != null)
                {
                    //                    if (targetUnit != null)
                    //                    {
                    //                        if (targetUnit != maxEnumy)
                    //                        {
                    //                            if(targetDuration > 0)
                    //                            {
                    //                                return;
                    //                            }
                    //                        }
                    //                    }
                    targetUnit = maxEnumy;
                    targetDuration = targetDurationConfig;
                    state = BehaviourState.MoveChase;

                    //TSLoger.LogFormat("士兵搜索目标 士兵={0}, skillId={1}, skillTip={2},  目标={3}", unitData.ToStringBase(),
                    //    aiSoliderSkill.skillController.skillId,
                    //    aiSoliderSkill.skillController.skillConfig.tip,
                    //    targetUnit.ToStringBase());

                    chaseTargetCD = 0;
                    TickChaseTargetUnit();
                }
            }
        }



        /** 追击目标 */
        public void TickChaseTargetUnit()
        {
            if (room.sceneData.CheckUnitInSafeRegion(unitData))
            {
                // 本身在安全区，直接返回操作
                targetUnit = null;
                MoveRoute();
                return;
            }
            chaseTargetCD -= LTime.deltaTime;
            if (chaseTargetCD <= 0)
            {
                chaseTargetCD = chaseTargetInterval;

                if (targetUnit == null || !targetUnit.IsInSceneAndLive)
                {
                    targetUnit = null;
                    MoveRoute();
                    return;
                }



                float distance = Vector3.Distance(unitAgent.position.Clone().SetY(0), targetUnit.position.Clone().SetY(0));

                if (distance < attackDistance)
                {
                    StopMove();
                    state = BehaviourState.Attack;
                    TickAttackTargetUnit();
                }
                else if (distance > chaseDistance)
                {
                    targetUnit = null;
                    MoveRoute();
                }
                else
                {
                    MoveTo(targetUnit.position);
                }

            }
        }

        /** 攻击目标单位 */
        public void TickAttackTargetUnit()
        {
            if (room.sceneData.CheckUnitInSafeRegion(unitData))
            {
                // 本身在安全区，直接返回操作
                targetUnit = null;
                MoveRoute();
                return;
            }

            if (targetUnit == null || !targetUnit.IsInSceneAndLive || room.sceneData.CheckUnitInSafeRegion(targetUnit) || targetUnit.isCloneUnit)
            {
                targetUnit = null;
                MoveRoute();
                return;
            }


            float distance = Vector3.Distance(unitAgent.position.Clone().SetY(0), targetUnit.position.Clone().SetY(0));
            if (distance > attackDistance)
            {
                state = BehaviourState.MoveChase;
                TickChaseTargetUnit();
                return;
            }
            else if (distance > chaseDistance)
            {
                targetUnit = null;
                MoveRoute();
                return;
            }


            if (!IsAttackActioning && unitData.attackCD <= 0)
            {
                //#if UNITY_EDITOR
                //                Loger.Log("士兵攻击  " + aiSoliderSkill.unit.unitAgent.name + "   " + aiSoliderSkill.skillController.skillId + " " + aiSoliderSkill.skillController.skillConfig.tip);
                //#endif


                //TSLoger.LogFormat("士兵攻击 士兵={0}, skillId={1}, skillTip={2},  目标={3}", unitData.ToStringBase(), 
                //    aiSoliderSkill.skillController.skillId , 
                //    aiSoliderSkill.skillController.skillConfig.tip,
                //    targetUnit.ToStringBase());

                unitAgent.ActionAttack(targetUnit, skillConfig.skillId);
                aiSoliderSkill.OnUse();
                selectSkillDurationCD = 0;


                unitAgent.photonView.RPC("RPC_SoliderAttck", PhotonTargets.Others, new object[] { targetUnit.uid, skillConfig.skillId });
            }

        }

        [PunRPC]
        void RPC_SoliderAttck(int targetUnitUid, int skillId)
        {
            targetUnit = room.sceneData.GetUnit(targetUnitUid);
            if (targetUnit != null && !room.sceneData.CheckUnitInSafeRegion(targetUnit) && !targetUnit.isCloneUnit)
            {
                unitAgent.ActionAttack(targetUnit, skillId);
            }
        }




        #endregion


        #region Move
        public UnitRVOAgent navMeshAgent;
        public RVOController rvoController;

        public float moveToDelayTime = 0;
        public bool moveToDelay = false;
        public Vector3 moveToDelayPos;
        public void MoveTo(Vector3 pos)
        {
            if (IsAttackActioning || moveToDelayTime > 0)
            {
                moveToDelay = true;
                moveToDelayPos = pos;
                return;
            }
            moveToDelay = false;
            targetPosition = unitAgent.GetNearest(pos);
            if (unitData.prop.MovementSpeed > 0)
            {
                aniManager.Do_Run();
                unitAgent.photonView.RPC("RPC_SoliderMove", PhotonTargets.Others);
            }

            unitAgent.RegisterRVO();
            if (unitAgent.photonView.isMine)
            {
                if (!navMeshAgent.enabled) navMeshAgent.enabled = true;
                navMeshAgent.SetTarget(targetPosition);
            }
        }




        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMove()
        {
            if (!IsAttackActioning) aniManager.Do_Idle();
            navMeshAgent.enabled = false;
            unitAgent.photonView.RPC("RPC_SoliderStopMove", PhotonTargets.Others);

        }

        [PunRPC]
        void RPC_SoliderStopMove()
        {
            if (aniManager != null)
            {
                aniManager.Do_Idle();
            }

            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }
        }


        [PunRPC]
        void RPC_SoliderMove()
        {
            aniManager.Do_Run();
        }

        #endregion


        #region MoveRoute
        public StageRouteConfig route;
        public int routePointIndex = 0;
        public int routePointCount = 0;
        public float arriveDistance = 1f;
        /** 行走航线 */
        public void MoveRoute()
        {
            if (routePointIndex >= routePointCount)
            {
                if (room.stageType == StageType.PVEActivity)
                {
                    DestorySolider();
                    return;
                }

                routePointIndex = routePointCount - 1;
                if (routePointIndex < routePointCount)
                    routePointIndex = 0;
            }

            if (routePointIndex <= routePointCount)
            {
                arriveDistance = 1f;
                if (arriveDistance == routePointCount - 1)
                {
                    arriveDistance = 8f;
                }
                state = BehaviourState.MoveRoute;

                if (routePointIndex < routePointCount - 1)
                {
                    Vector3 dirCurr = unitAgent.position - route.path[routePointIndex];
                    Vector3 dirNext = unitAgent.position - route.path[routePointIndex + 1];
                    dirCurr = dirCurr.normalized;
                    dirNext = dirNext.normalized;
                    float dirDot = Vector3.Dot(dirCurr, dirNext);
                    if (dirDot < 0)
                    {
                        routePointIndex++;
                    }
                }

                MoveTo(route.path[routePointIndex]);
            }
            else
            {
                MoveRouteComplete();
            }
        }

        /** 检测航线节点是否走完 */
        public void TickMoveRoute()
        {
            if (prop.MovementSpeed > 0)
            {
                if (!navMeshAgent.enabled)
                {
                    navMeshAgent.enabled = true;
                }
            }
            float distance = Vector3.Distance(unitAgent.position.Clone().SetY(0), targetPosition.Clone().SetY(0));
            if (distance < arriveDistance)
            {
                routePointIndex++;
                MoveRoute();
            }
        }

        /** 行走航线所有节点走完 */
        public void MoveRouteComplete()
        {
            // state = BehaviourState.None;
            if (room.stageType == StageType.PVEActivity)
            {
                DestorySolider();
            }
            else
            {
                StopMove();
            }
        }
        #endregion

        /// <summary>
        /// 死亡
        /// </summary>
        public void Death()
        {
            this.enabled = false;
            navMeshAgent.enabled = false;
            state = BehaviourState.Death;
        }


        /// <summary>
        /// 复活
        /// </summary>
        public void RestLive()
        {
            this.enabled = true;
            state = BehaviourState.None;
            MoveRoute();
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public override void OnGameOver()
        {
            base.OnGameOver();
            this.enabled = false;
            navMeshAgent.enabled = false;
            state = BehaviourState.None;
        }

    }
}
