using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 1:54:49 PM
*  @Description:    战场数据
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class WarSceneData : AbstractRoomObject
    {


        public WarSceneData(WarRoom room)
        {
            this.room = room;
        }


        #region Hatred 仇恨值
        /// <summary>
        /// 仇恨值字典 <srcUnitUid, enemyUnitUid, hatred>
        /// </summary>
        private Dictionary2<int, int, int> hatredDict = new Dictionary2<int, int, int>();

        /// <summary>
        /// 获取仇恨值
        /// </summary>
        /// <param name="srcUnitUid">元单位</param>
        /// <param name="unitUid">敌人单位</param>
        /// <returns></returns>
        public int GetHatred(int srcUnitUid, int unitUid)
        {
            if (hatredDict.ContainsKey(srcUnitUid, unitUid))
                return hatredDict.GetValue(srcUnitUid, unitUid);

            return 0;
        }


        /// <summary>
        /// 添加仇恨值
        /// </summary>
        /// <param name="srcUnitUid">元单位</param>
        /// <param name="unitUid">敌人单位</param>
        /// <param name="unitUid">值</param>
        public void AddHatred(int srcUnitUid, int unitUid, int hatred)
        {
            hatred = GetHatred(srcUnitUid, unitUid) + hatred;
            hatredDict.Set(srcUnitUid, unitUid, hatred);
        }

        /// <summary>
        /// 移除单位的仇恨值
        /// </summary>
        /// <param name="srcUnitUid">元单位</param>
        public void RemoveHatred(int srcUnitUid)
        {
            hatredDict.Remove(srcUnitUid);
        }


        /** 仇恨值衰减--衰减间隔 */
        private float hatredReductionInterval = 1;
        /** 仇恨值衰减--衰减间隔 */
        private float hatredReductionCd = 1;

        /// <summary>
        /// 仇恨值衰减
        /// </summary>
        public void HatredReduction()
        {
            hatredReductionCd -= LTime.deltaTime;
            if (hatredReductionCd <= 0)
            {
                hatredReductionCd = hatredReductionInterval;
                foreach (var itemSrc in hatredDict.Dict)
                {
                    Dictionary<int, int> unitHatreds = itemSrc.Value;
                    List<int> keyList = new List<int>(unitHatreds.Keys);
                    foreach (var key in keyList)
                    {
                        if (unitHatreds[key] >= 0)
                        {
                            unitHatreds[key] = Mathf.FloorToInt(unitHatreds[key] - unitHatreds[key] * 0.1f);
                            if (unitHatreds[key] < 0)
                            {
                                unitHatreds[key] = 0;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        //===================================
        // 航线
        //-----------------------------------
        /** 航线列表 */
        private List<StageRouteConfig>                  routeList = new List<StageRouteConfig>();
        /** 航线字典 */
        private Dictionary<int, StageRouteConfig>       routeDict = new Dictionary<int, StageRouteConfig>();

        /** 添加航线 */
        public void AddRoute(StageRouteConfig route)
        {
            routeDict.Add(route.routeId, route);
            routeList.Add(route);
        }

        /** 获取航线 */
        public StageRouteConfig GetRoute(int routeId)
        {
            if (!routeDict.ContainsKey(routeId))
            {
                Loger.LogErrorFormat("没有找到航线routeId={0}", routeId);
                return null;
            }
            return routeDict[routeId];
        }

        /** 获取航线起点 */
        public Vector3 GetRouteBeginPoint(int routeId)
        {
            StageRouteConfig route = GetRoute(routeId);
            return route.GetBeginPoint();
        }

        /** 获取航线朝向 */
        public Vector3 GetRouteBeginDirection(int routeId)
        {
            StageRouteConfig route = GetRoute(routeId);
            return route.GetBeginDirection();
        }


        /** 获取显示航线 */
        public StageRouteConfig GetRouteParentRoot(int routeId)
        {
            StageRouteConfig route = GetRoute(routeId);
            if (route.parentId != -1)
            {
                route = GetRoute(route.parentId);
            }
            return route;
        }


        //===================================
        // 建筑格子
        //-----------------------------------
        /** 建筑格子列表 */
        private List<BuildCellData>                 buildCellList = new List<BuildCellData>();
        /** 建筑格子字典 */
        private Dictionary<int, BuildCellData>      buildCellDict = new Dictionary<int, BuildCellData>();

        /** 添加格子 */
        public void AddBuildCell(BuildCellData cell)
        {
            buildCellDict.Add(cell.uid, cell);
            buildCellList.Add(cell);
        }


        /** 添加移除 */
        public void RemoveBuildCell(BuildCellData cell)
        {
            buildCellDict.Remove(cell.uid);
            buildCellList.Remove(cell);
        }

        public void RemoveBuildCell(int cellUid)
        {
            if (buildCellDict.ContainsKey(cellUid))
                RemoveBuildCell(buildCellDict[cellUid]);
        }


        /** 获取建筑格子 */
        public BuildCellData GetBuildCell(int cellUid)
        {
            if (buildCellDict.ContainsKey(cellUid))
            {
                return buildCellDict[cellUid];
            }
            return null;
        }

        /** 获取建筑格子列表 */
        public List<BuildCellData> GetBuildCellList()
        {
            return buildCellList;
        }

        /** 设置建筑格子上当前的建筑单位UID */
        public void SetBuildCellUnit(int cellUid, UnitData unit)
        {
            BuildCellData cell = GetBuildCell(cellUid);
            if (cell != null)
            {
                cell.SetUnit(unit);
            }
        }
        /** 设置格子上没有单位 */
        public void SetBuildCellEmpty(int cellUid)
        {
            BuildCellData cell = GetBuildCell(cellUid);
            if (cell != null)
            {
                cell.SetUnitEmpty();
            }
        }

        //===================================
        // 区域
        //-----------------------------------

        /** 区域列表 */
        private List<RegionData> regionList = new List<RegionData>();
        /** 区域字典 */
        private Dictionary<int, RegionData> regionDict = new Dictionary<int, RegionData>();

        /** 添加区域 */
        public void AddRegion(RegionData regionData)
        {
            regionDict.Add(regionData.regionId, regionData);
            regionList.Add(regionData);
        }

        /** 移除区域 */
        public void RemoveRegion(RegionData regionData)
        {
            regionDict.Remove(regionData.regionId);
            regionList.Remove(regionData);
        }

        public void RemoveRegion(int regionId)
        {
            if (regionDict.ContainsKey(regionId))
            {
                RemoveRegion(regionId);
            }
        }

        /** 获取区域 */
        public RegionData GetRegion(int regionId)
        {
            if (!regionDict.ContainsKey(regionId))
                Loger.LogErrorFormat("不存在区域 regionId={0}", regionId);
            return regionDict[regionId];
        }



        //===================================
        // 安全区域
        //-----------------------------------

        /** 安全区域列表 */
        private List<RegionData>    regionListSafe  = new List<RegionData>();
        private int                 regionListCount = 0;

        /** 添加安全区域 */
        public void AddRegionSafe(RegionData regionData)
        {
            regionListSafe.Add(regionData);
            regionListCount = regionListSafe.Count;
        }

        /** 判断单位是否在安全区域内 */
        public bool CheckUnitInSafeRegion(UnitData unit)
        {
            bool result = false;
            if (regionListCount > 0 && unit != null && unit.IsInSceneAndLive)
            {
                for (int i = 0; i < regionListCount; i++)
                {
                    RegionData regionData = regionListSafe[i];
                    if (regionData.shapeType == RegionShapeType.Circle)
                    {
                        if (CheckCircle(unit, regionData.position, regionData.radius))
                        {
                            result = true;
                        }
                    }
                    else if (regionListSafe[i].shapeType == RegionShapeType.Rectangle)
                    {
                        if (CheckFanUnit(unit, regionData.position, regionData.rect))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }


        //===================================
        // 势力
        //-----------------------------------
        /** 势力列表 */
        private List<LegionData>                    legionList          = new List<LegionData>();
        /** 势力列表--真人玩家 */
        private List<LegionData>                    realityLegionList   = new List<LegionData>();
        /** 势力字典 */
        private Dictionary<int, LegionData>         legionDict          = new Dictionary<int, LegionData>();
        /** 势力组字典 */
        private Dictionary<int, LegionGroupData>    legionGroupDict     = new Dictionary<int, LegionGroupData>();
        /** 势力关系 */
        public RelationMatrixData                   relationMatrixConfig = new RelationMatrixData();

        /** 获取势力列表 */
        public List<LegionData> LegionList
        {
            get
            {
                return legionList;
            }
        }


        /** 获取势力列表--真人玩家 */
        public List<LegionData> RealityLegionList
        {
            get
            {
                return realityLegionList;
            }
        }

        /** 获取势力列表--真人再线玩家 */
        public List<LegionData> OnlineLegionList
        {
            get
            {
                List<LegionData> list = new List<LegionData>();
                foreach (LegionData legion in realityLegionList)
                {
                    if (legion.isOnline)
                    {
                        list.Add(legion);
                    }
                }
                return list;
            }
        }


        /** 获取势力列表--真人再线玩家数量 */
        public int OnlineLegionCount
        {
            get
            {
                int count = 0;
                foreach (LegionData legion in realityLegionList)
                {
                    if (legion.isOnline)
                    {
                        count++;
                    }
                }
                return count;
            }
        }




        /** 添加势力 */
        public void AddLegion(LegionData legionData)
        {
            legionList.Add(legionData);
            legionDict.Add(legionData.legionId, legionData);
            if (!legionData.isRobot)
            {
                realityLegionList.Add(legionData);
            }
        }


        /** 添加势力组 */
        public void AddLegionGroup(LegionGroupData legionGroupData)
        {
            if (legionGroupDict.ContainsKey(legionGroupData.groupId))
            {
                Loger.LogErrorFormat("已经存在legionGroupId={0}", legionGroupData.groupId);
                return;
            }
            legionGroupDict.Add(legionGroupData.groupId, legionGroupData);
        }


        /** 获取势力 */
        public LegionData GetLegion(int legionId)
        {
            return legionDict[legionId];
        }

        public LegionData GetLegionByRoleId(int roleId)
        {
            foreach (LegionData item in legionList)
            {
                if (item.roleId == roleId)
                {
                    return item;
                }
            }
            return null;
        }


        /** 获取势力组 */
        public LegionGroupData GetLegionGroup(int legionGroupId)
        {
            return legionGroupDict[legionGroupId];
        }




        //===================================
        // 单位
        //-----------------------------------

        /** 英雄列表 */
        private List<UnitData>               heroList       = new List<UnitData>();
        /** 士兵列表 */
        private List<UnitData>               soliderList    = new List<UnitData>();
        /** 主基地列表 */
        private List<UnitData>               mianbaseList   = new List<UnitData>();
        /** 机关列表 */
        private List<UnitData>              towerList       = new List<UnitData>();
        /** 单位列表 */
        private List<UnitData>               unitList       = new List<UnitData>();
        /** 单位字典 */
        private Dictionary<int, UnitData>    unitDict       = new Dictionary<int, UnitData>();
        private Dictionary<UnitType, int>    unitNumDict    = new Dictionary<UnitType, int>();
        // boss出现
        public Action OnBossShow;


        /** 添加单位 */
        public void AddUnit(UnitData unitData)
        {
            unitDict.Add(unitData.uid, unitData);
            unitList.Add(unitData);
            AddUnitNum(unitData.unitType, 1);
            unitData.isInScene = true;
            switch (unitData.unitType)
            {
                case UnitType.Solider:
                    soliderList.Add(unitData);
                    break;
                case UnitType.Hero:
                    heroList.Add(unitData);
                    break;
                case UnitType.Build:
                    if (unitData.buildType == UnitBuildType.Mainbase)
                    {
                        mianbaseList.Add(unitData);
                    }
                    else if (unitData.buildType.IsTower())
                    {
                        towerList.Add(unitData);
                    }
                    break;
            }
            if (unitData.soliderType == UnitSoliderType.Boss)
            {
                // boss出现
                if (OnBossShow != null)
                {
                    OnBossShow();
                }
            }
        }


        /** 移除单位 */
        public void RemoveUnit(UnitData unitData)
        {
            RemoveHatred(unitData.uid);
            unitDict.Remove(unitData.uid);
            unitList.Remove(unitData);
            AddUnitNum(unitData.unitType, -1);
            unitData.isInScene = false;

            switch (unitData.unitType)
            {
                case UnitType.Solider:
                    soliderList.Remove(unitData);
                    break;
                case UnitType.Hero:
                    heroList.Remove(unitData);
                    break;
                case UnitType.Build:
                    if (unitData.buildType == UnitBuildType.Mainbase)
                    {
                        mianbaseList.Remove(unitData);
                    }
                    else if (unitData.buildType.IsTower())
                    {
                        towerList.Remove(unitData);
                    }
                    break;
            }
        }

        public void RemoveUnit(int uid)
        {
            if (unitDict.ContainsKey(uid))
            {
                RemoveUnit(unitDict[uid]);
            }
        }

        /** 获取单位列表 */
        public List<UnitData> GetUnitList(bool CheckSafeRegion = true, bool removeClone = true)
        {
            List<UnitData> resultList = new List<UnitData>();
            if (CheckSafeRegion)
            {
                if (unitList.Count > 0)
                {
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (removeClone && unitList[i].isCloneUnit)
                        {
                            continue;
                        }
                        if (!room.sceneData.CheckUnitInSafeRegion(unitList[i]))
                        {
                            resultList.Add(unitList[i]);
                        }
                    }
                }
            }
            else
            {
                if (unitList.Count > 0)
                {
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (removeClone && unitList[i].isCloneUnit)
                        {
                            continue;
                        }
                        resultList.Add(unitList[i]);
                    }
                }
            }
            return resultList;
        }

        /** 获取单位 */
        public UnitData GetUnit(int unitUid)
        {
            if (!unitDict.ContainsKey(unitUid))
                return null;
            return unitDict[unitUid];
        }
        /// <summary>
        /// 依据unitId查找单位
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public List<UnitData> GetUnitWithUnitID(int unitId)
        {
            List<UnitData> result = new List<UnitData>();
            foreach (var item in unitDict)
            {
                UnitData unit = item.Value;
                if (unit.unitId == unitId)
                {
                    result.Add(unit);
                }
            }
            return result;
        }

        /** 添加单位类型数量 */
        private void AddUnitNum(UnitType unitType, int num)
        {
            if (unitNumDict.ContainsKey(unitType))
                unitNumDict[unitType] += num;
            else
                unitNumDict.Add(unitType, num);
        }

        /** 获取单位类型数量 */
        public int GetUnitNum(UnitType unitType)
        {
            if (unitNumDict.ContainsKey(unitType))
                return unitNumDict[unitType];
            return 0;
        }


        /** 获取士兵数量 */
        public int GetSoliderNum()
        {
            return GetUnitNum(UnitType.Solider);
        }

        /** 获取机关列表 */
        public List<UnitData> GetTowerList()
        {
            return towerList;
        }



        //===================================
        // 驱动
        //-----------------------------------

        private float _tmpSecondT = 0;
        private float _tmpT = 0;
        private List<UnitData> _tmpUnitList = new List<UnitData>();
        /** 更新，每帧调用 */
        public void OnSyncedUpdate()
        {
            // 仇恨值衰减
            HatredReduction();


            _tmpUnitList.AddRange(unitList);
            int count = _tmpUnitList.Count;

            UnitData unit;
            for (int i = 0; i < count; i++)
            {
                unit = _tmpUnitList[i];
                unit.OnSyncedUpdate();
            }



            // 秒更新
            _tmpSecondT += LTime.deltaTime;
            if (_tmpSecondT >= 1)
            {
                _tmpSecondT -= 1;

                // 势力
                count = legionList.Count;
                for (int i = 0; i < count; i++)
                {
                    legionList[i].OnSecond();
                }

                // 单位
                count = _tmpUnitList.Count;
                for (int i = 0; i < count; i++)
                {
                    unit = _tmpUnitList[i];
                    unit.OnSecond();
                }
            }


            _tmpUnitList.Clear();
        }


        /** 卸载 */
        public void Uninstall()
        {
            hatredDict.ClearAll();
        }


    }
}
