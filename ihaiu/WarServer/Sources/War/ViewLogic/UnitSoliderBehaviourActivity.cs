using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 士兵行为，活动副本AI
    /// </summary>
    public partial class UnitSoliderBehaviour
    {


        private void UpdateActivity()
        {
            switch (room.setting.soliderBehaviourType)
            {
                /** 依照路径移动，到达终点后自动销毁 */
                case UnitSoliderAIType.PathOver:
                    {
                        if (state == BehaviourState.MoveRoute)
                        {
                            if (!unitData.disableMove)
                                TickMoveRoute();
                        }
                    }
                    break;
                /** 无移动，生命周期结束后自动销毁 */
                case UnitSoliderAIType.LiveOver:
                    {
                        if (state == BehaviourState.MoveRoute)
                        {
                            state = BehaviourState.None;
                            aniManager.Do_Idle();
                        }
                        if (aiSoliderLiveTime + room.gameUnitLiveTime <= LTime.time)
                        {
                            DestorySolider();
                        }
                    }
                    break;
                /** 依照路径移动，到达终点或生命周期结束后自动销毁 */
                case UnitSoliderAIType.PathOrLiveOver:
                    {
                        if (aiSoliderLiveTime + room.gameUnitLiveTime <= LTime.time)
                        {
                            DestorySolider();
                        }
                        else if (state == BehaviourState.MoveRoute)
                        {
                            if (!unitData.disableMove)
                                TickMoveRoute();
                        }
                    }
                    break;
            }
            


            if (state != BehaviourState.None)
            {
                navMeshAgent.maxSpeed = prop.MovementSpeed;
                if(prop.MovementSpeed == 0)
                {
                    navMeshAgent.enabled = false;
                }
                
                if(preDisableMove != unitData.disableMove)
                {
                    if(unitData.disableMove)
                    {
                        if (navMeshAgent.enabled)
                        {
                            navMeshAgent.enabled = false;
                        }
                    }
                    preDisableMove = unitData.disableMove;
                }
                
            }


        }

        private void DestorySolider()
        {
            unitAgent.punUnit.Death(-1);
        }

    }
}
