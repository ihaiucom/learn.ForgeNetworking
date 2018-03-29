using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 2:29:59 PM
*  @Description:    战前数据
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarEnterData
    {
        /** 战斗模式 */
        public WarVSMode                vs = WarVSMode.One;
        /** 关卡ID */
        public int                      stageId;
        /** 组列表 */
        public List<WarEnterGroupData>      groupList = new List<WarEnterGroupData>();
        /** 主基地列表 */
        public List<WarEnterMainbaseData>   mainbaseList = new List<WarEnterMainbaseData>();
        /** pvp玩家初始化携带的机关列表 */
        public List<WarEnterUnitData>       initUnitList = new List<WarEnterUnitData>();
        /** pvp对方初始化携带的机关列表 */
        public List<WarEnterUnitData>       initOhterUnitList = new List<WarEnterUnitData>();

        // 活动副本类型
        public ActivityType                 activityType = ActivityType.None;
        /// <summary>
        /// 怪物生命周期
        /// 0表示不做限制，大于0，生命周期到，自动销毁怪物
        /// </summary>
        public float                        monsterLiveTime = 0;
        /// <summary>
        /// 活动副本怪物总数量
        /// </summary>
        public int                          monsterAllCount = 0;

        /// <summary>
        /// 【客户端有效】
        /// 自己势力ID
        /// </summary>
        public int ownLegionId;

        /// <summary>
        /// [临时]
        /// 主机势力ID
        /// </summary>
        public int hostLegionId;


        /// <summary>
        /// 【客户端有效】
        /// 对方势力ID
        /// </summary>
        public int otherLegionId;

        private int _deathGrading = 0;
        /// <summary>
        /// 死亡分级，1默认，2血腥
        /// </summary>
        public int  deathGrading
        {
            get
            {
                if (_deathGrading == 0)
                {
                    _deathGrading = Game.config.globalConfig.GetGlobalConfigs((int)GlobalKey.DeathGrading);
                }
                return _deathGrading;
            }
        }

        #region 扩展逻辑

        /** 玩家字典 */
        private Dictionary<int, WarEnterLegionData> legionDict          = new Dictionary<int, WarEnterLegionData>();
        private List<WarEnterLegionData>            legionRealityList = new List<WarEnterLegionData>();
        /** 真人玩家数量 */
        private int                                 legionRealityCount  = 0;

        public List<WarEnterUnitData> ownTowerList;
        private bool isInit = false;

        public WarEnterLegionData ownLegion
        {
            get
            {
                return legionDict[ownLegionId];
            }
        }

        internal string PhotonRoomName
        {
            get
            {
                List<int> roleIds = new List<int>();


                // 初始化 玩家字典
                foreach (WarEnterGroupData group in groupList)
                {
                    foreach (WarEnterLegionData legion in group.legionList)
                    {
                        if (!legion.isRobot)
                        {
                            roleIds.Add(legion.roleInfo.roleId);
                        }
                    }
                }

                roleIds.Sort();
                return "Room-" + stageId + "-" + roleIds.ToStr<int>("_", "", "");
            }
        }

        /** 初始化 */
        internal void Init()
        {
            if (isInit)
                return;
            isInit = true;


            // 初始化 玩家字典
            foreach (WarEnterGroupData group in groupList)
            {
                foreach (WarEnterLegionData legion in group.legionList)
                {
                    legionDict.Add(legion.legionId, legion);
                    if (legion.isRobot)
                    {
                        // 初始化机器人玩家状态
                        legion.SetProcessState(WarProcessState.Gameing);
                        legion.SetLoadProgress(100);
                    }
                    else
                    {
                        legionRealityList.Add(legion);
                        legionRealityCount++;
                        // 初始化真人玩家状态
                        legion.SetProcessState(WarProcessState.Ready_WaitConnect);
                        legion.SetLoadProgress(0);

                        if (War.isDebugModel)
                        {
                            if (legion.legionId != ownLegionId)
                            {
                                legion.SetProcessState(WarProcessState.Ready_WaitEnter);
                                legion.SetLoadProgress(100);
                            }
                        }
                    }

                    if(legion.legionId == ownLegionId)
                    {
                        ownTowerList = legion.towerList;
                    }

                }
            }

        }


        /** 获取玩家数量 */
        internal int LegionCount
        {
            get
            {
                return legionDict.Count;
            }
        }


        /** 获取真人玩家数量 */
        internal int LegionRealityCount
        {
            get
            {
                return legionRealityCount;
            }
        }

        /** 获取连接的玩家数量 */
        internal int LegionConnectCount
        {
            get
            {
                int count = 0;
                foreach (var kvp in legionDict)
                {
                    if (kvp.Value.GetProcessState() > WarProcessState.Ready_WaitConnect)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /** 获取等待进入游戏的玩家数量 */
        internal int LegionWaitEnterCount
        {
            get
            {
                int count = 0;
                foreach (var kvp in legionDict)
                {
                    if (kvp.Value.GetProcessState() >= WarProcessState.Ready_WaitEnter)
                    {
                        count++;
                    }
                }
                return count;
            }
        }


        /** 获取等待进入游戏的真人玩家数量 */
        internal int LegionRealityWaitEnterCount
        {
            get
            {
                int count = 0;
                foreach (WarEnterLegionData legion in legionRealityList)
                {
                    if (legion.GetProcessState() >= WarProcessState.Ready_WaitEnter)
                    {
                        count++;
                    }
                }
                return count;
            }
        }


        /** 获取势力 */
        internal WarEnterLegionData GetLegion(int legionId)
        {
            return legionDict[legionId];
        }

        /** 设置势力状态和加载进度 */
        internal void SetLegionStateAndProgress(int legionId, WarProcessState processState, int progress)
        {
            WarEnterLegionData legion = GetLegion(legionId);
            legion.SetProcessState(processState);
            legion.SetLoadProgress(progress);
        }

        /** 获取机关单位技能列表 */
        public WarEnterUnitData GetLegionUnit(int legionId, int unitId)
        {
            WarEnterLegionData legion = GetLegion(legionId);
            if(legion == null)
            {
                return null;
            }
            return legion.GetUnit(unitId);
        }
        #endregion


    }
}
