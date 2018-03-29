#if !NOT_USE_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 5:55:53 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 场景视图
    /// </summary>
    public partial class WarUnitySceneView : AbstractWarScene
    {
        public void Exit()
        {
            if (_rootUnit != null)
            {
                _rootUnit.gameObject.SetActive(false);
                GameObject.DestroyImmediate(_rootUnit.gameObject);
            }

            if (_rootBuildCell != null)
            {
                _rootBuildCell.gameObject.SetActive(false);
                GameObject.DestroyImmediate(_rootBuildCell.gameObject);
            }
            
        }

        //===================================
        // Root 根节点
        //-----------------------------------
        private Transform _rootUnit;
        public Transform RootUnit
        {
            get
            {
                if(_rootUnit == null)
                {
                    _rootUnit = new GameObject("WarUnitList").transform;
                    GameObject.DontDestroyOnLoad(_rootUnit.gameObject);
                }
                return _rootUnit;
            }
        }

        private Transform _rootHeroUnit;
        public Transform RootHeroUnit
        {
            get
            {
                if (_rootHeroUnit == null)
                {
                    _rootHeroUnit = new GameObject("HeroList").transform;
                    _rootHeroUnit.SetParent(RootUnit, false);
                }

                return _rootHeroUnit;
            }
        }

        private Transform _rootSoliderUnit;
        public Transform RootSoliderUnit
        {
            get
            {
                if (_rootSoliderUnit == null)
                {
                    _rootSoliderUnit = new GameObject("SoliderList").transform;
                    _rootSoliderUnit.SetParent(RootUnit, false);
                }

                return _rootSoliderUnit;
            }
        }

        private Transform _rootMainbaseUnit;
        public Transform RootMainbaseUnit
        {
            get
            {
                if (_rootMainbaseUnit == null)
                {
                    _rootMainbaseUnit = new GameObject("MainbaseList").transform;
                    _rootMainbaseUnit.SetParent(RootUnit, false);
                }

                return _rootMainbaseUnit;
            }
        }

        private Transform _rootTowerUnit;
        public Transform RootTowerUnit
        {
            get
            {
                if (_rootTowerUnit == null)
                {
                    _rootTowerUnit = new GameObject("TowerList").transform;
                    _rootTowerUnit.SetParent(RootUnit, false);
                }

                return _rootTowerUnit;
            }
        }


        public Transform GetUnitRoot(UnitType unitType, UnitBuildType buildType)
        {
            return RootUnit;
            switch (unitType)
            {
                case UnitType.Hero:
                    return RootHeroUnit;
                case UnitType.Solider:
                    return RootSoliderUnit;
                case UnitType.Build:
                    switch(buildType)
                    {
                        case UnitBuildType.Tower:
                            return RootTowerUnit;
                        case UnitBuildType.Mainbase:
                            return RootMainbaseUnit;
                    }
                    break;
            }
            return RootUnit;
        }


        public void AddToRoot(UnitAgent unit)
        {
            AddToRoot(unit.unitType, unit.buildType, unit.transform);
        }

        public void AddToRoot(UnitType unitType, UnitBuildType buildType, Transform node)
        {
            Transform root = GetUnitRoot(unitType, buildType);
            node.SetParent(root, false);
        }



        //===================================
        // 单位
        //-----------------------------------

        /** 单位列表 */
        private List<UnitAgent> unitList = new List<UnitAgent>();
        /** 单位字典 */
        private Dictionary<int, UnitAgent> unitDict = new Dictionary<int, UnitAgent>();


        /** 添加单位 */
        public void AddUnit(UnitAgent unit)
        {
            unitDict.Add(unit.uid, unit);
            unitList.Add(unit);
            AddToRoot(unit);
        }


        /** 移除单位 */
        public void RemoveUnit(UnitAgent unit)
        {
            unitDict.Remove(unit.uid);
            unitList.Remove(unit);
        }

        public void RemoveUnit(int uid)
        {
            if (unitDict.ContainsKey(uid))
            {
                RemoveUnit(unitDict[uid]);
            }
        }

        /** 获取单位列表 */
        public List<UnitAgent> GetUnitList()
        {
            return unitList;
        }

        /** 获取单位 */
        public UnitAgent GetUnit(int unitUid)
        {
            if (!unitDict.ContainsKey(unitUid))
                return null;
            return unitDict[unitUid];
        }



        //===================================
        // 驱动
        //-----------------------------------

        /** 更新，每帧调用 */
        public void OnSyncedUpdate()
        {
            UpdateBuildCell();

            UpdateDataPosition();
        }

        /// <summary>
        /// 更新单位位置
        /// 临时
        /// </summary>
        public void UpdateDataPosition()
        {
            for(int i = 0; i < unitList.Count; i ++)
            {
                if (unitList[i].unitData.isLive)
                {
                    unitList[i].unitData.position = unitList[i].position;
                    unitList[i].unitData.rotation = unitList[i].rotation;
                }
            }
        }



        /** 游戏--游戏结束 */
        public void OnGameOver()
        {

            int count = unitList.Count;
            for (int i = 0; i < count; i++)
            {
                unitList[i].OnGameOver();
            }
        }

    }
}
#endif
