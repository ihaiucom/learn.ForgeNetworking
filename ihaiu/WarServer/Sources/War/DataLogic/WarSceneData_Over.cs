using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 7:14:43 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 战斗结束检测
    /// </summary>
    public partial class WarSceneData
    {
        /// <summary>
        /// 检测战斗结束
        /// </summary>
        public void TickGameOver()
        {
            switch (room.setting.billingType)
            {
                // 正常结算
                case BillingType.None:
                    {
                        if (room.spawnSolider.state == WarSpawnWaveState.Final)
                        {
                            if (GetSoliderNum() <= 0)
                            {
                                GameStageOver(WarOverType.Win);
                                return;
                            }
                        }


                        // 检测主机地 和 英雄是否活着
                        bool mainbaseIsLive = false;
                        for (int i = 0; i < mianbaseList.Count; i++)
                        {
                            if (mianbaseList[i].isLive)
                            {
                                mainbaseIsLive = true;
                            }
                        }


                        bool playerIsLive = false;
                        for (int i = 0; i < heroList.Count; i++)
                        {
                            if (heroList[i].isLive && !heroList[i].isCloneUnit)
                            {
                                playerIsLive = true;
                            }
                        }

                        if (mainbaseIsLive == false || playerIsLive == false)
                        {
                            GameStageOver(WarOverType.Fail);
                        }
                        else if (room.isGameTimeLimit)
                        {
                            if (room.gameTimeleft <= 0)
                            {
                                GameStageOver(WarOverType.Fail);
                            }
                        }
                    }
                    break;
                    //pvp天梯结算
                case BillingType.DeathHero:
                    {
                        if (room.isGameTimeLimit && room.gameTimeleft <= 0)
                        {
                            float hp1 = 0;
                            float hp2 = 0;
                            for (int i = 0; i < heroList.Count; i++)
                            {
                                if (heroList[i].LegionId == room.clientOwnLegionId)
                                {
                                    hp1 = heroList[i].prop.HpRate;
                                }
                                else
                                {
                                    hp2 = heroList[i].prop.HpRate;
                                }
                            }
                            if (hp1 > hp2)
                            {
                                GameStageOver(WarOverType.Win);
                            }
                            else
                            {
                                GameStageOver(WarOverType.Fail);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < heroList.Count; i++)
                            {
                                if (!heroList[i].isLive)
                                {
                                    if (heroList[i].LegionId == room.clientOwnLegionId)
                                    {
                                        GameStageOver(WarOverType.Fail);
                                    }
                                    else
                                    {
                                        GameStageOver(WarOverType.Win);
                                    }
                                }
                            }
                        }
                    }
                    break;
                    // 活动副本
                case BillingType.SoliderFinal:
                    {
                        if (room.isGameTimeLimit && room.gameTimeleft <= 0)
                        {
                            GameStageOver(WarOverType.Win);
                        }
                        if (room.spawnSolider.state == WarSpawnWaveState.Final)
                        {
                            if (GetSoliderNum() <= 0)
                            {
                                GameStageOver(WarOverType.Win);
                            }
                        }
                    }
                    break;
            }




            //// 副本结算
            //if (room.stageType == StageType.Dungeon || room.stageType == StageType.PVE)
            //{
            //    if (room.spawnSolider.state == WarSpawnWaveState.Final)
            //    {
            //        if (GetSoliderNum() <= 0)
            //        {
            //            GameStageOver(WarOverType.Win);
            //            return;
            //        }
            //    }


            //    // 检测主机地 和 英雄是否活着
            //    bool mainbaseIsLive = false;
            //    for (int i = 0; i < mianbaseList.Count; i++)
            //    {
            //        if (mianbaseList[i].isLive)
            //        {
            //            mainbaseIsLive = true;
            //        }
            //    }


            //    bool playerIsLive = false;
            //    for (int i = 0; i < heroList.Count; i++)
            //    {
            //        if (heroList[i].isLive)
            //        {
            //            playerIsLive = true;
            //        }
            //    }

            //    if (mainbaseIsLive == false || playerIsLive == false)
            //    {
            //        GameStageOver(WarOverType.Fail);
            //    }
            //    else if (room.isGameTimeLimit)
            //    {
            //        if (room.gameTimeleft <= 0)
            //        {
            //            GameStageOver(WarOverType.Fail);
            //        }
            //    }
            //}
            //// pvp天梯结算
            //else if (room.stageType == StageType.PVPLadder)
            //{
            //    if (room.isGameTimeLimit && room.gameTimeleft <= 0)
            //    {
            //        float hp1 = 0;
            //        float hp2 = 0;
            //        for (int i = 0; i < heroList.Count; i++)
            //        {
            //            if (heroList[i].LegionId == room.clientOwnLegionId)
            //            {
            //                hp1 = heroList[i].prop.HpRate;
            //            }
            //            else
            //            {
            //                hp2 = heroList[i].prop.HpRate;
            //            }
            //        }
            //        if (hp1 > hp2)
            //        {
            //            GameStageOver(WarOverType.Win);
            //        }
            //        else
            //        {
            //            GameStageOver(WarOverType.Fail);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < heroList.Count; i++)
            //        {
            //            if (!heroList[i].isLive)
            //            {
            //                if (heroList[i].LegionId == room.clientOwnLegionId)
            //                {
            //                    GameStageOver(WarOverType.Fail);
            //                }
            //                else
            //                {
            //                    GameStageOver(WarOverType.Win);
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (room.stageType == StageType.PVEActivity)
            //{
            //    if (room.isGameTimeLimit && room.gameTimeleft <= 0)
            //    {
            //        GameStageOver(WarOverType.Win);
            //    }
            //    if (room.spawnSolider.state == WarSpawnWaveState.Final)
            //    {
            //        if (GetSoliderNum() <= 0)
            //        {
            //            GameStageOver(WarOverType.Win);
            //        }
            //    }
            //}
        }

        public void GameStageOver(WarOverType overType)
        {
            room.punScene.GameOverPVE(overType);
        }

        public void RPC_GameOverPVE(WarOverType overType)
        {
            WarOverData data = new WarOverData();
            data.stageId = room.stageConfig.stageId;
            data.stageType = room.stageType;
            data.overType = overType;
            data.starNum = overType == WarOverType.Fail ? 0 : UnityEngine.Random.Range(1, 3);
            data.killNum = WarUI.Instance.activityKillNum;
            room.Over(data);
        }
    }
}
