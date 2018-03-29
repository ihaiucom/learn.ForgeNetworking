using Assets.Scripts.Common;
using Games.PB;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 1:22:44 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗产兵控制器 -- 汇总玩家交互 */
    public partial class WarSpawnSoliderController 
    {

        /** 跳过准备的玩家列表 */
        public Dictionary<int, bool>   skipLegionDict = new Dictionary<int, bool>();
        private List<int>               skipLegionList = new List<int>();


        /** 已经点击跳过的玩家数量 */
        public int skipNum = 0;
        /** 真人玩家总数量 */
        public int skipTotal = 1;

        /** 设置势力点击 */
        public void OnSetSkip(int legionId)
        {
            Loger.Log("OnSetSkip legionId=" + legionId);
            if (!skipLegionDict.ContainsKey(legionId))
            {
                skipLegionDict.Add(legionId, true);
                skipLegionList.Add(legionId);
            }
            CheckSkip();
        }



        /** 检测在线的所有真人都投了跳过票 */
        private void CheckSkip()
        {
            if (state != WarSpawnWaveState.ReadyTime || readyTime <= 0)
            {
                return;
            }

            List<LegionData> list = room.sceneData.OnlineLegionList;
            skipTotal = list.Count;
            // TODO 帧同步需要 修改玩家上线离线 同步消息
            skipTotal = PhotonNetwork.playerList.Length;
            skipNum = 0;
            foreach (LegionData legion in list)
            {
                if (skipLegionDict.ContainsKey(legion.legionId))
                {
                    skipNum++;
                }
            }

            if (OnSkipChangeEvent != null)
            {
                OnSkipChangeEvent();
            }

            if (skipNum >= skipTotal)
            {
                if(photonView.isMine)
                {
                    Skip();
                }
            }

            Loger.LogFormat("CheckSkip 经跳过的势力列表 {0} skipCount={1}, totalCount={2}", skipLegionList.ToStr<int>(","), skipNum, skipTotal);

        }


        /** 设置跳过 */
        private void Skip()
        {
            List<LegionData> list = room.sceneData.RealityLegionList;
            for (int i = 0; i < list.Count; i++)
            {
                LegionData legionData = list[i];
                float engeryCost = legionData.EnergyRecover * readyTime;
                List<UnitData> buildList = room.sceneData.GetTowerList();
                int energyTowerNum = 0;
                UnitData energyTowerUnit = null;
                foreach (UnitData unit in buildList)
                {
                    if (unit.legionId == legionData.legionId)
                    {
                        if(unit.buildType == UnitBuildType.Tower_Auxiliary)
                        {
                            energyTowerNum++;
                            energyTowerUnit = unit; 
                        }
                    }
                }

                if(energyTowerUnit != null)
                {
                    float attackInterval = energyTowerUnit.prop.AttackInterval;
                    float energyVal = Game.config.skillValue.GetConfig(30050001).GetVal(energyTowerUnit.unitLevel);
                    if (attackInterval <= 0) attackInterval = 1;
                    engeryCost += readyTime * energyTowerNum * energyVal / attackInterval;
                }

                legionData.Energy += engeryCost;
                room.punScene.RPC_LegionEnergy(legionData.legionId, legionData.Energy);
            }
            SetSpawn();
        }

    }


}
