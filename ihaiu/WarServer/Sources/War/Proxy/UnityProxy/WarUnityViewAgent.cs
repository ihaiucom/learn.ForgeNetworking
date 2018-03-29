#if !NOT_USE_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 10:58:51 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 战斗视图代理--Unity
    /// </summary>
    public class WarUnityViewAgent : AbstractWarViewAgent
    {
        public WarUnityRes res
        {
            get
            {
                return room.clientRes;
            }
        }

        /** 创建战斗UI */
        public void CreateUI()
        {
            GameObject go = CreateUIWithParent(WarRes.WAR_UI, Game.uiLayer.war);
            WarUI ui = go.GetComponent<WarUI>();
            ui.SetRoom(room);
        }

        //创建单位菜单
        public void ShowUnitMenu(UnitData unitData)
        {
            if(WarUnitMenu.GetInst().IsInit())
            {
                WarUnitMenu.GetInst().Init(null, unitData);
            }
            else
            {
                GameObject go = CreateUIObjectWithParent(WarRes.UNIT_MENU_UI, Game.uiLayer.war);
                (go.transform as RectTransform).SetAsFirstSibling();
                WarUnitMenu.GetInst().Init(go, unitData);
            }
        }

		public GameObject CreateUnitStatus()
		{
			GameObject go = room.clientRes.GetGameObjectInstall(WarRes.UNIT_STATUS_UI);
			return go;
		}

        public GameObject CreateUIWithParent(string path, Transform parent)
        {
            GameObject go = room.clientRes.GetGameObjectInstall(path);
            RectTransform rectTransform = (RectTransform) go.transform;
            rectTransform.SetParent(parent, false);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            go.name = go.name.Replace("(Clone)", "");
            if (rectTransform.childCount > 0)
            {
                rectTransform = (RectTransform)rectTransform.GetChild(0);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
            }
            return go;
        }

        public  GameObject  CreateUIWithparentSimple(string path, Transform parent)
        {
            GameObject go = room.clientRes.GetGameObjectInstall(path);
            RectTransform rectTransform = (RectTransform) go.transform;
            rectTransform.SetParent(parent, false);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            go.name = go.name.Replace("(Clone)", "");
            return go;
        }

        public GameObject CreateUIObjectWithParent(string path, Transform parent)
        {
            GameObject go = room.clientRes.GetGameObjectInstall(path);
            go.transform.SetParent(parent, false);
            go.name = go.name.Replace("(Clone)", "");
            return go;
        }


        /** 添加单位 */
        public UnitAgent AddUnit(UnitData unitData)
        {
            if (unitData.avatarConfig == null)
            {
                Loger.LogErrorFormat("单位没有avatar配置 unitData={0}", unitData);
                return null;
            }

            GameObject modelGO  = res.GetGameObjectInstall(unitData.avatarConfig.model);
            GameObject unitGO   = null;
            switch(unitData.unitType)
            {
                case UnitType.Solider:
                    unitGO = res.GetGameObjectInstall(WarRes.WAR_PREFAB_UNIT_SOLIDER);
                    break;
                case UnitType.Hero:

                    unitGO = res.GetGameObjectInstall(WarRes.WAR_PREFAB_UNIT_HERO);
                    //if (unitData.clientIsOwn)
                    //{
                    //    unitGO = res.GetGameObjectInstall(WarRes.WAR_PREFAB_UNIT_PLAYER);
                    //}
                    //else
                    //{
                    //    unitGO = res.GetGameObjectInstall(WarRes.WAR_PREFAB_UNIT_HERO);
                    //}
                    break;
                case UnitType.Build:
                    unitGO = res.GetGameObjectInstall(WarRes.WAR_PREFAB_UNIT_BUILD);
                    break;
            }


            unitGO.name = unitGO.name.Replace("(Clone)", "(" + unitData.uid + ")");
            UnitAgent unitAgent = unitGO.GetComponent<UnitAgent>();
            if (unitAgent == null)
                unitAgent = unitGO.AddComponent<UnitAgent>();
            unitAgent.AddToAnchorRotation(modelGO);


            unitAgent.animatorManager = modelGO.GetComponent<AnimatorManager>();
            if (unitAgent.animatorManager == null)
            {
                unitAgent.animatorManager = modelGO.GetComponentInChildren<AnimatorManager>();
            }
            if (unitAgent.animatorManager != null && unitAgent.animatorManager.weaponPos != null)
            {
                if (unitData.unitConfig.weaponDefaultId != 0)
                {
                   AvatarConfig weaponConfig =  Game.config.avatar.GetConfig(unitData.unitConfig.weaponDefaultId);
                    if (!string.IsNullOrEmpty(weaponConfig.model))
                    {
                        unitAgent.animatorManager.weaponObj = res.GetGameObjectInstall(weaponConfig.model);
                        unitAgent.animatorManager.weaponObj.transform.SetParent(unitAgent.animatorManager.weaponPos);
                        unitAgent.animatorManager.weaponObj.transform.localPosition = Vector3.zero;
                        unitAgent.animatorManager.weaponObj.transform.localEulerAngles = Vector3.zero;
                    }
                }
            }

            unitAgent.position = unitData.position;
            unitAgent.rotation = unitData.rotation;
            unitAgent.Init(unitData);
            room.clientSceneView.AddUnit(unitAgent);

            if (unitData.unitType == UnitType.Hero)
            {
                if (unitData.clientIsOwn)
                {
                    room.clientOperationUnit.SetUnitAgent(unitAgent);
                }
                // 天梯对方英雄设置
                else if (room.stageType == StageType.PVPLadder)
                {
                    room.clientOperationUnit.SetUnitAgentPVPLadder(unitAgent);
                }
            }
            return unitAgent;
        }

        /** 移除单位 */
        public void RemoveUnit(int unitUid)
        {
            room.clientSceneView.RemoveUnit(unitUid);
        }


        /** 添加建筑格子 */
        public void AddBuildCell(BuildCellData cellData)
        {
            GameObject go = res.GetGameObjectInstall(WarRes.WAR_PREFAB_BUILDCELL);

            BuildCellAgent agent = go.GetComponent<BuildCellAgent>();
            if (agent == null)
                agent = go.AddComponent<BuildCellAgent>();
            agent.buildCellData = cellData;
            agent.SetRoom(room);
            agent.transform.position    = cellData.position;
            agent.transform.eulerAngles = cellData.rotation;
            agent.position = agent.transform.position;
            room.clientSceneView.AddBuildCell(agent);
        }

        /** 移除建筑格子 */
        public void RemoveBuildCell(int cellId)
        {
            room.clientSceneView.RemoveBuildCell(cellId);
        }
    }
}
#endif