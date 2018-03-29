using Games.PB;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:42:13 PM
*  @Description:    战斗门面类
* ==============================================================================
*/
namespace Games.Wars
{
    public class War
    {
        /** 战斗版本 */
        public static string        version = "1.0.0";

        /** 是否调试模式 */
        public static bool          isDebugModel = false;

        /** 当前战斗房间 */
        public static WarRoom       currentRoom;

        private static WarEnterPVEData pveData;

        public static bool heroInvincible = false;
        public static bool soliderInvincible = false;


        /// <summary>
        /// 进入PVE副本(单人、多人)
        /// </summary>
        public static void EnterPVE(WarEnterPVEData pveData)
        {
            //if (pveData.stageId == 10101)
            //{
            //    foreach (WarEnterPVELegionData legion in pveData.legionList)
            //    {
            //        WarEnterSkillData skill = new WarEnterSkillData();
            //        skill.skillId = 300100;
            //        skill.skillLevel = 1;
            //        WarEnterUnitData tower = new WarEnterUnitData();
            //        tower.unitId = 3001;
            //        tower.unitLevel = 1;
            //        tower.skillList.Add(skill);
            //        legion.towerList.Add(tower);

            //    }
            //}
            War.pveData = pveData;
            WarEnterData enterData = WarEnterDataUtil.GeneratePVE(pveData);
            War.Enter(enterData);
        }

        /** 进入副本 */
        public static void Enter(int stageId, RoleInfo roleInfo, WarEnterUnitData hero, WarEnterUnitData mianbaseUnit, List<WarEnterUnitData> towerList)
        {
            WarEnterData enterData = WarEnterDataUtil.GenerateDungeon(stageId, roleInfo, hero, mianbaseUnit, towerList);
            War.Enter(enterData);
        }

        /// <summary>
        /// 进入pvp天梯关卡
        /// </summary>
        /// <param name="pvpLadderData"></param>
        public static void Enter(WarEnterPVPLadderData pvpLadderData)
        {
            WarEnterData enterData = WarEnterDataUtil.GeneratePVPLadder(pvpLadderData);
            War.Enter(enterData);
        }

        /** 启动战斗 */
        public static void Enter(WarEnterData enterData)
        {
            //if (currentRoom != null
            //    && currentRoom.clientProcessState != WarProcessState.Exit
            //    && currentRoom.clientProcessState != WarProcessState.None)
            //{
            //    return;
            //}

            if (currentRoom != null)
            {
                Loger.Log("有战斗正在进行，请先结算之前的战斗");

                currentRoom.Exit();
            }

            currentRoom = new WarRoom();
            currentRoom.Enter(enterData);
        }


        /** 退出战斗 */
        public static void Exit()
        {
            if(currentRoom != null)
            {
				currentRoom.Exit();
				LuaCallCsharpFun.LoadMainScene(false);
            }

            currentRoom = null;
        }


        /** 重新开始副本战斗 */
        public static void Restart()
        {
            if (pveData == null)
            {
                return;
            }

            CsharpCallLuaFun.CloseAllMenu();
            if (currentRoom != null)
            {
                currentRoom.Exit();
                currentRoom = null;
            }
            EnterPVE(pveData);
        }

        public static void GM_Win()
        {
            if(currentRoom != null)
            {
                WarOverData over = new WarOverData();
                over.stageId = currentRoom.stageConfig.stageId;
                over.stageType = currentRoom.stageConfig.stageType;
                over.overType = WarOverType.Win;
                over.starNum = 3;
                over.killNum = WarUI.Instance.activityMaxMonsterCount;
                over.isManualExit = true;
                if (!currentRoom.isNetModel)
                {
                    currentRoom.Over(over);
                }
                else
                {
                    //currentRoom.serverNetBroadcast.GameOver(over);
                }
            }
        }


        public static void GM_Fail()
        {
            if (currentRoom != null)
            {
                WarOverData over = new WarOverData();
                over.stageId = currentRoom.stageConfig.stageId;
                over.stageType = currentRoom.stageConfig.stageType;
                over.overType = WarOverType.Fail;
                over.starNum = 0;
                over.killNum = 0;
                over.isManualExit = true;
                if (!currentRoom.isNetModel)
                {
                    currentRoom.Over(over);
                }
                else
                {

                    WarSyncOver action = new WarSyncOver();
                    action.overType = (uint)over.overType;
                    action.star = (uint)over.starNum;
                    BattleSyncData msg = WarProtoUtil.CreateBattleSyncMsg<WarSyncOver>(WarSyncActionType.Over, action);
                    msg.proxie = (uint)WarProxieModel.ClientAndServer;
                    //currentRoom.serverNet.C_BattleSync(msg);
                }
            }
        }
    }
}
