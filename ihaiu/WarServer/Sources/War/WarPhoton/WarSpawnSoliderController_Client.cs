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
    /** 战斗产兵控制器 -- 玩家交互 */
    public partial class WarSpawnSoliderController 
    {

        /// <summary>
        /// 事件--准备
        /// </summary>
        public Action OnReadyEvent;
        /// <summary>
        /// 事件--跳过投票改变
        /// </summary>
        public Action OnSkipChangeEvent;
        /// <summary>
        /// 事件--开始出怪
        /// </summary>
        public Action OnSpawnEvent;


        /// <summary>
        /// 自己是否已经设置跳过
        /// </summary>
        public bool IsOwnSetSkip
        {
            get
            {
                return skipLegionDict.ContainsKey(room.clientOwnLegionId);
            }
        }

        /// <summary>
        /// 获取当前波次怪物信息
        /// </summary>
        public List<StageWaveUnitConfig> UnitList
        {
            get
            {
                return waveConfig.unitList;
            }
        }

        /// <summary>
        /// 需显示在提示UI上的航线
        /// </summary>
        public List<StageRouteConfig> RouteList
        {
            get
            {
                List<StageRouteConfig> list = new List<StageRouteConfig>();
                foreach (StageWaveUnitConfig unit in UnitList)
                {
                    StageRouteConfig route = room.sceneData.GetRouteParentRoot(unit.routeId);
                    if (!list.Contains(route))
                        list.Add(route);
                }
                return list;
            }
        }


        /// <summary>
        /// 发送跳过
        /// </summary>
        public void SendSkip()
        {
            if (state != WarSpawnWaveState.ReadyTime) return;

            if (!IsOwnSetSkip)
                room.clientOperationUnit.BallotSpawnWaveSkip();
        }
    }


}
