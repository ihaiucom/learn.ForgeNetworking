using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// pvpladder英雄AI行为
    /// </summary>
    public partial class UnitHeroAI
    {
        private void Update()
        {
            if (room == null) return;

            if (!room.IsGameing) return;

            if (room.stageType != StageType.PVPLadder && !unitData.isCloneUnit) return;

            if (unitAgent == null) return;

            if (!unitData.isCloneUnit)
            {
                if (!WarUI.Instance.isPVPLadderAuto && unitData.LegionId == room.clientOperationUnit.legionId) return;
            }
            if (aiRefrushCD > 0) aiRefrushCD -= LTime.deltaTime;
            if (searchTargetCD > 0) searchTargetCD -= LTime.deltaTime;

            if (!unitData.isLive) return;

            if (unitData.disableAttack) return;

            // 始终朝向目标
            if (targetUnit != null)
            {
                unitAgent.LookAt(targetUnit, true);
            }
            //判断状态
            switch (state)
            {
                case HeroAiEnum.None:
                    {
                        // 待机中,查找目标，朝向目标，进入idle
                        if (targetUnit == null)
                        {
                            if (attackDistance > 0)
                            {
                                aniManager.Do_Idle();
                                state = HeroAiEnum.Idle;
                            }
                        }
                    }
                    break;
                case HeroAiEnum.Idle:
                    {
                        if (room.clientOperationUnit.IsStart || unitData.isCloneUnit)
                        {
                            if (unitData.isCloneUnit)
                            {
                                if (targetUnit == null || targetUnit.unitAgent == null)
                                {
                                    targetUnit = null;
                                    if (skillIdDic.ContainsKey(HeroAiEnum.Attack))
                                    {
                                        //停止普攻
                                        room.skillManager.OnStopSkill(unitAgent, skillIdDic[HeroAiEnum.Attack]);
                                    }
                                }
                            }
                            TickState();
                        }
                    }
                    break;
                case HeroAiEnum.Default:
                    {
                        TickIdleState();
                    }
                    break;
                case HeroAiEnum.Move:
                    {
                        // 移动到目标点
                        //Loger.Log(unitData.name + "  HeroAiEnum.Move");
                        state = HeroAiEnum.Idle;
                    }
                    break;
                case HeroAiEnum.MoveFoward:
                case HeroAiEnum.MoveBack:
                case HeroAiEnum.MoveLeft:
                case HeroAiEnum.MoveRight:
                    {
                        // 前后左右移动
                        TickState(true);
                        aniManager.Do_Run();
                        unitAgent.Move(state, moveSpeed);
                    }
                    break;
                case HeroAiEnum.Attack:
                    {
                        // 普攻
                        unitAgent.ActionAttack(targetUnit, skillIdDic[state]);
                        state = HeroAiEnum.Idle;
                    }
                    break;
                case HeroAiEnum.AttackBack:
                    {
                        TickState(true);
                        aniManager.Do_RunD();
                        unitAgent.Move(state, moveSpeed * 0.5F);
                        if (!currentAttackStart)
                        {
                            currentAttackStart = true;
                            unitAgent.ActionAttack(targetUnit, skillIdDic[HeroAiEnum.Attack]);
                        }
                    }
                    break;
                case HeroAiEnum.AttackFoward:
                    {
                        TickState(true);
                        aniManager.Do_RunU();
                        unitAgent.Move(state, moveSpeed * 0.5F);
                        if (!currentAttackStart)
                        {
                            currentAttackStart = true;
                            unitAgent.ActionAttack(targetUnit, skillIdDic[HeroAiEnum.Attack]);
                        }
                    }
                    break;
                case HeroAiEnum.AttackLeft:
                    {
                        TickState(true);
                        aniManager.Do_RunL();
                        unitAgent.Move(state, moveSpeed * 0.5F);
                        if (!currentAttackStart)
                        {
                            currentAttackStart = true;
                            unitAgent.ActionAttack(targetUnit, skillIdDic[HeroAiEnum.Attack]);
                        }
                    }
                    break;
                case HeroAiEnum.AttackRight:
                    {
                        TickState(true);
                        aniManager.Do_RunR();
                        unitAgent.Move(state, moveSpeed * 0.5F);
                        if (!currentAttackStart)
                        {
                            currentAttackStart = true;
                            unitAgent.ActionAttack(targetUnit, skillIdDic[HeroAiEnum.Attack]);
                        }
                    }
                    break;
                case HeroAiEnum.Skill1:
                case HeroAiEnum.Skill2:
                case HeroAiEnum.Skill3:
                case HeroAiEnum.Skill4:
                    {
                        // 释放技能1
                        unitAgent.ActionAttack(targetUnit, skillIdDic[state]);
                        state = HeroAiEnum.Default;
                    }
                    break;
            }
        }
    }
}