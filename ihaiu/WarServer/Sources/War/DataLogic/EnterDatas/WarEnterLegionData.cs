using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 11:03:53 AM
*  @Description:    战前数据--势力数据
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战前数据--势力数据 */
    public class WarEnterLegionData
    {
        /** 角色信息 */
        public RoleInfo roleInfo = new RoleInfo();
        /** 势力类型 */
        public LegionType legionType;
        /** 中立势力建筑关系类型 */
        public LegionBuildChangType neutralBuildChangeType;
        /** 势力ID */
        public int legionId;
        /** 是否是机器人 */
        public bool isRobot = false;
        /** 机器人ID */
        public int robotId;
        /** 英雄数据 */
        public WarEnterUnitData hero = new WarEnterUnitData();
        /** 机关列表 */
        public List<WarEnterUnitData> towerList = new List<WarEnterUnitData>();

        /** 出生区域 */
        public int regionId = -1;



        #region 扩展逻辑

        /** 获取单位 */
        public WarEnterUnitData GetUnit(int unitId)
        {
            if (unitId == hero.unitId) return hero;
            foreach(WarEnterUnitData item in towerList)
            {
                if (unitId == item.unitId) return item;
            }
            return null;
        }

        /** 玩家进程状态 */
        private WarProcessState processState = WarProcessState.None;
        /** 玩家加载进度 */
        private int             loadProgress = 0;

        /** 设置玩家进程状态 */
        public void SetProcessState(WarProcessState val)
        {
            processState = val;
        }

        /** 获取玩家进程状态 */
        public WarProcessState GetProcessState()
        {
            return processState;
        }


        /** 设置玩家加载进度 */
        public void SetLoadProgress(int val)
        {
            loadProgress = val;
        }

        /** 获取玩家加载进度 */
        public int GetLoadProgress()
        {
            return loadProgress;
        }
        #endregion
    }
}
