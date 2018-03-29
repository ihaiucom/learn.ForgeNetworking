using Games.PB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 8:24:46 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗操作单位 Ai控制的天梯对方英雄
     * 英雄控制----移动  释放技能
     *  */
    public partial class WarOperationUnit
    {

        /** 操作的UnitData */
        private UnitData unitDataPVPLadder;
        /** 操作的UnitAgent */
        private UnitAgent unitAgentPVPLadder;

        public LegionData legionDataPVPLadder
        {
            get
            {
                if (unitDataPVPLadder == null) return null;
                return unitDataPVPLadder.legionData;
            }
        }

        public bool IsLivePVPLadder
        {
            get
            {
                if (unitDataPVPLadder == null) return true;
                return unitDataPVPLadder.isLive;
            }
        }

        private bool _isStart = false;
        /** 是否开始pvp天梯战斗 */
        public bool IsStart
        {
            get
            {
                return _isStart;
            }
            set
            {
                _isStart = value;
            }
        }

        /** 设置操作的单位 */
        public void SetUnitAgentPVPLadder(UnitAgent unitAgent)
        {
            if(this.unitAgentPVPLadder != null)
            {
                Loger.LogWarning("WarOperationUnitPvpLadder:SetUnitAgentPVPLadder 设置操作的单位 已经设置过了");
                return;
            }
            this.unitAgentPVPLadder = unitAgent;
            this.unitDataPVPLadder = unitAgent.unitData;
        }

        /** 获取操作的单位 */
        public UnitAgent GetUnitAgentPVPLadder()
        {
            return unitAgentPVPLadder;
        }

        public UnitData GetUnitDataPVPLadder()
        {
            return unitDataPVPLadder;
        }

        /** 获取操作的单位 */
        public int unitUidPVPLadder
        {
            get
            {
                return unitDataPVPLadder.uid;
            }
        }

        /** 获取操作的势力ID */
        public int legionIdPVPLadder
        {
            get
            {
                return unitDataPVPLadder.legionId;
            }
        }

        /** 获取单位位置 */
        public Vector3 positionPVPLadder
        {
            get
            {
                if (unitAgentPVPLadder != null)
                {
                    return unitAgentPVPLadder.position;
                }
                return Vector3.zero;
            }
        }
    }
}
