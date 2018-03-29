using Games.PB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 战斗房间 安装控制器
    /// </summary>
    public class WarRoomInstallController : AbstractRoomObject
    {

        public WarRoomInstallController(WarRoom room) 
        {
            this.room = room;
        }


        /** 战斗进入数据 */
        protected WarEnterData enterData
        {
            get
            {
                return room.enterData;
            }
        }



        /** 获取真人玩家数量 */
        private int LegionCount
        {
            get
            {
                return enterData.LegionRealityCount;
            }
        }

        /** 获取等待进入游戏的玩家数量 */
        private int WaitEnterCount
        {
            get
            {
                return enterData.LegionRealityWaitEnterCount;
            }
        }


        // 启动
        public void Start()
        {

            room.clientProcessState = WarProcessState.Ready_Loading;

            // 生成预加载资源列表
            room.clientRes.GenerateList();

            // 加载资源
            room.clientRes.Load(OnLoadComplete);

            room.mainThread.updateEvent += Update;
        }



        /** 资源加载完成 */
        private void OnLoadComplete()
        {
        }


        private float _preLoadTime = 0;
        public void Update()
        {
            _preLoadTime += LTime.deltaTime;
            if (_preLoadTime >= 1f)
            {
                _preLoadTime = 0;
                room.clientNetC.SetRoomState(room.clientOwnLegionId, room.clientProcessState, room.clientLoadProgress);
            }

            switch (room.clientProcessState)
            {
                #region Ready_Loading
                // [房间进程状态] 加载中
                case WarProcessState.Ready_Loading:
                    {
                        // [房间加载状态]
                        switch (room.clientLoadState)
                        {
                            // [房间加载状态] 还没开始加载
                            case WarLoadState.None:
                                room.clientLoadState = WarLoadState.Loading;
                                // 生成预加载资源列表
                                room.clientRes.GenerateList();
                                // 加载资源
                                room.clientRes.Load(null);
                                break;
                            // [房间加载状态] 加载中
                            case WarLoadState.Loading:
                                if (room.clientLoadProgress >= 100)
                                {
                                    room.clientLoadState = WarLoadState.LoadComplete;
                                }
                                break;
                            // [房间加载状态] 加载完成
                            case WarLoadState.LoadComplete:

                                // 生成势力组
                                GenerateLegionGroupList();
                                // 生成势力数据
                                GenerateLegionList();
                                // 生成势力关系
                                GenerateLegionMatrixConfig();
                                // 生成地图信息
                                GenerateMapInfo();

                                room.clientProcessState = WarProcessState.Ready_WaitEnter;
                                room.clientNetC.SetRoomState(room.clientOwnLegionId, room.clientProcessState, room.clientLoadProgress);
                                break;
                        }
                    }
                    break;
                #endregion


                #region Ready_WaitEnter
                // [房间进程状态] 加载中
                case WarProcessState.Ready_WaitEnter:
                    {
                        if (WaitEnterCount >= LegionCount || WarPhotonRoom.GetRoomIsGameing())
                        {
                            //WarPhotonRoom.CreateTrueSync();

                            // 创建玩家
                            //WarPhotonRoom.CreatePlayer(room.clientOwnLegionId);


                            // 创建初始建筑
                            WarPhotonRoom.GenerateBuildCellInitUnit();

                            // 创建主基地
                            WarPhotonRoom.CreateUnitMainbase();

                            // 创建自己英雄
                            WarPhotonRoom.CreateUnitHero(room.clientOwnLegionId);

                            // 创建对方英雄
                            if (room.stageType == StageType.PVPLadder)
                            {
                                WarPhotonRoom.CreateUnitHero(room.enterData.otherLegionId);
                            }

                            // 移除更新
                            room.mainThread.updateEvent -= Update;

                            // 创建UI
                            room.clientViewAgent.CreateUI();
                            // 初始化UI
                            WarUI.Instance.Init();
                            // 初始化操作
                            room.clientOperationUnit.SetEasyTouch();

                            room.clientOperationUnit.Start();
                            room.clientRes.CloseLoadPanel();

                            // 创建场景控制器
                            WarPhotonRoom.CreateUnitScne();

                            // 关闭加载面板
                            room.clientRes.CloseLoadPanel();
                            // 设置游戏开始
                            room.clientProcessState = WarProcessState.Gameing;
                            WarPhotonRoom.SetRoomState(TSRoomState.Gameing);
                        }
                    }
                    break;
                #endregion


                #region 游戏开始
                // [房间进程状态] 游戏开始
                case WarProcessState.Gameing:
                    {
                        //GenerateHero();
                        //room.clientViewAgent.CreateUI();
                        //WarUI.Instance.Init();
                        //room.clientOperationUnit.SetEasyTouch();
                        //room.clientRes.CloseLoadPanel();
                        //room.mainThread.updateEvent -= Update;
                    }
                    break;
                #endregion
            }

        }


        #region 生成英雄
        /** 生成英雄 */
        private void GenerateHero()
        {
            foreach (WarEnterGroupData group in enterData.groupList)
            {
                foreach (WarEnterLegionData legion in group.legionList)
                {
                    if (legion.hero == null || legion.hero.unitId <= 0) continue;
                    Vector3 position = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerPosition();
                    Vector3 rotation = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerRotation();
                    WarEnterUnitData item = legion.hero;
                    List<WarSyncCreateSkill> skills = WarProtoUtil.GenerateSyncSkillList(room, item);

                    WarSyncCreateUnit msg = WarProtoUtil.GenerateUnit(room.UNIT_UID, item.unitId, item.avatarId, item.unitLevel, legion.legionId, position, rotation, skills);
                    CreateUnit(msg);
                }
            }
        }


        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="msg"></param>
        public void CreateUnit(WarSyncCreateUnit msg)
        {
            UnitData unit = CreateUnitData(msg);
            UnitAgent unitAgent = CreateUnitAgent(unit);
            OnCreateUnit(unit, unitAgent);
        }


        /** 创建单位数据 */
        public UnitData CreateUnitData(WarSyncCreateUnit msg)
        {
            UnitData unit = room.creater.CreateUnit(msg);
            unit.room = room;
            unit.unitData = unit;
            room.sceneData.AddUnit(unit);
            switch (unit.unitType)
            {
                case UnitType.Hero:
                    // 势力 主单位
                    unit.legionData.mainUnit = unit;
                    // TODO 临时 给初始能量
                    unit.prop.AddProp(PropId.Energy, PropType.Base, room.stageConfig.energy);
                    unit.prop.AddProp(PropId.EnergyMax, PropType.Base, room.stageConfig.energyMax);
                    break;
                case UnitType.Build:
                    switch (unit.buildType)
                    {
                        case UnitBuildType.Mainbase:
                            // 势力 主基地
                            //unit.legionData.mainbaseUnit = unit;
                            if (unit.legionData.group != null)
                            {
                                foreach (LegionData legion in unit.legionData.group.list)
                                {
                                    legion.mainbaseUnit = unit;
                                }
                            }
                            break;
                    }
                    break;
            }

            unit.prop.InitNonrevertFinals();
            unit.prop.Calculate();
            return unit;
        }


        /** 创建单位数据 */
        public UnitData CreateUnitData(int legionId, WarEnterUnitData enterUnitData, Vector3 position, Vector3 rotation)
        {

            UnitData unit = room.creater.CreateUnit(room.UNIT_UID, legionId, enterUnitData, position, rotation);
            unit.room = room;
            unit.unitData = unit;
            room.sceneData.AddUnit(unit);
            switch (unit.unitType)
            {
                case UnitType.Hero:
                    // 势力 主单位
                    unit.legionData.mainUnit = unit;
                    // TODO 临时 给初始能量
                    unit.prop.AddProp(PropId.Energy, PropType.Base, room.stageConfig.energy);
                    unit.prop.AddProp(PropId.EnergyMax, PropType.Base, room.stageConfig.energyMax);
                    break;
                case UnitType.Build:
                    switch (unit.buildType)
                    {
                        case UnitBuildType.Mainbase:
                            // 势力 主基地
                            //unit.legionData.mainbaseUnit = unit;
                            if (unit.legionData.group != null)
                            {
                                foreach (LegionData legion in unit.legionData.group.list)
                                {
                                    legion.mainbaseUnit = unit;
                                }
                            }
                            break;
                    }
                    break;
            }

            unit.prop.InitNonrevertFinals();
            unit.prop.Calculate();
            return unit;
        }


        /** 创建单位视图 */
        public UnitAgent CreateUnitAgent(UnitData unitData)
        {
            UnitAgent unitAgent = room.clientViewAgent.AddUnit(unitData);
            return unitAgent;
        }


        private void OnCreateUnit(UnitData unitData, UnitAgent unitAgent)
        {
            unitData.UsePassiveSkillBirth();
        }
        #endregion


        #region 生成势力
        /** 生成势力组 */
        private void GenerateLegionGroupList()
        {
            foreach (WarEnterGroupData enterGroup in enterData.groupList)
            {
                LegionGroupData group = room.creater.CreateLegionGroup(enterGroup.groupId);
                room.sceneData.AddLegionGroup(group);
            }
        }



        /** 生成势力数据 */
        private void GenerateLegionList()
        {
            foreach (WarEnterGroupData enterGroup in enterData.groupList)
            {
                foreach (WarEnterLegionData enterLegion in enterGroup.legionList)
                {
                    LegionData legion = room.creater.CreateLegion(enterLegion);
                    LegionGroupData group = room.sceneData.GetLegionGroup(enterGroup.groupId);
                    group.AddLegion(legion);
                    room.sceneData.AddLegion(legion);
                }
            }
        }

        /** 生成势力关系 */
        private void GenerateLegionMatrixConfig()
        {
            RelationMatrixData relationMatrix = null;
            if (room.stageType != StageType.PVP)
            {
                relationMatrix = room.stageConfig.releationMatrix;
            }
            else
            {
                relationMatrix = new RelationMatrixData();
                List<int> legionList = new List<int>();
                foreach (WarEnterGroupData group in enterData.groupList)
                {
                    foreach (WarEnterLegionData legion in group.legionList)
                    {
                        legionList.Add(legion.legionId);
                    }
                }
                relationMatrix.Generate(legionList);

                foreach (WarEnterGroupData group in enterData.groupList)
                {
                    foreach (WarEnterLegionData legionA in group.legionList)
                    {
                        foreach (WarEnterLegionData legionB in group.legionList)
                        {
                            if (legionA.legionId == legionB.legionId)
                                continue;

                            relationMatrix.SetValue(legionA.legionId, legionB.legionId, 1);
                        }
                    }
                }
            }


            room.sceneData.relationMatrixConfig.Generate(relationMatrix.LegionIdList, relationMatrix.DataToList());
        }
        #endregion

        #region 生成地图环境信息
        // 生成地图环境信息
        private void GenerateMapInfo()
        {
            // 生成玩家出生区域
            GenerateSpawnPlayerRegion();
            // 生成怪物航线
            GenerateRoute();
            // 生成建筑格子
            GenerateBuildCell();
        }


        /** 生成玩家出生区域 */
        public void GenerateSpawnPlayerRegion()
        {
            foreach (RegionData item in room.stageConfig.spawnPlayerRegionList) 
            {
                room.sceneData.AddRegion(item.Clone());
            }
            foreach (RegionData item in room.stageConfig.spawnRegionListSafe) 
            {
                room.sceneData.AddRegionSafe(item.Clone());
            }
        }


        /** 生成航线 */
        public void GenerateRoute()
        {
            foreach (StageRouteConfig item in room.stageConfig.routeList)
            {
                room.sceneData.AddRoute(item);
            }
        }


        /** 生成建筑格子 */
        public void GenerateBuildCell()
        {
            int count = room.stageConfig.buildCellList.Count;

            for (int i = 0; i < count; i++)
            {

                StageBuildCellConfig item = room.stageConfig.buildCellList[i];
                BuildCellData cell = new BuildCellData(room);
                cell.uid = item.uid;
                cell.routeId = item.routeId;
                cell.position = item.position;
                cell.rotation = item.rotation;
                cell.size = item.size;
                cell.switchBuildCellList.AddRange(item.initUnit.switchBuildCellList);

                room.sceneData.AddBuildCell(cell);
                room.clientViewAgent.AddBuildCell(cell);
            }
        }
        #endregion



    }

}