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
    /// 场景视图 -- 建筑格子
    /// </summary>
    public partial class WarUnitySceneView 
    {
        //===================================
        // Root 根节点
        //-----------------------------------
        private Transform _rootBuildCell;
        public Transform RootBuildCell
        {
            get
            {
                if(_rootBuildCell == null)
                {
                    _rootBuildCell = new GameObject("WarBuildCellList").transform;
                    GameObject.DontDestroyOnLoad(_rootBuildCell.gameObject);
                }
                return _rootBuildCell;
            }
        }


        public void AddToRoot(BuildCellAgent cell)
        {
            cell.transform.SetParent(RootBuildCell, false);
        }



        //===================================
        // 建筑格子
        //-----------------------------------

        /** 建筑格子列表 */
        private List<BuildCellAgent> buildCellList = new List<BuildCellAgent>();
        /** 建筑格子字典 */
        private Dictionary<int, BuildCellAgent> buildCellDict = new Dictionary<int, BuildCellAgent>();


        /** 添加建筑格子 */
        public void AddBuildCell(BuildCellAgent cell)
        {
            buildCellDict.Add(cell.uid, cell);
            buildCellList.Add(cell);
            AddToRoot(cell);
        }


        /** 移除建筑格子 */
        public void RemoveBuildCell(BuildCellAgent cell)
        {
            buildCellDict.Remove(cell.uid);
            buildCellList.Remove(cell);
        }

        public void RemoveBuildCell(int uid)
        {
            if (buildCellDict.ContainsKey(uid))
            {
                RemoveBuildCell(buildCellDict[uid]);
            }
        }

        /** 获取建筑格子列表 */
        public List<BuildCellAgent> GetBuildCellList()
        {
            return buildCellList;
        }

        /** 获取建筑格子 */
        public BuildCellAgent GetBuildCell(int cellUid)
        {
            if (!buildCellDict.ContainsKey(cellUid))
                return null;
            return buildCellDict[cellUid];
        }



        //===================================
        // 驱动
        //-----------------------------------

        /** 高亮的建筑格子 */
        private BuildCellAgent   buildCellHighlight;
        /** 是否是销毁选择模式 */
        private bool             buildCellSelectModel = false;
        private bool            _buildCellSelectModelPre = false;
        /** 玩家上一次位置 */
        private Vector3         _playerPositionPre = Vector3.zero;
        /** 玩家是否移动了 */
        private bool            _playerIsMove = false;    
        
        /// <summary>
        /// 获取选中的建筑格子
        /// </summary>
        /// <returns></returns>
        public BuildCellAgent GetSelectBuildCell()
        {
            return buildCellHighlight;
        }


        /// <summary>
        /// 建筑格子 设置为选择模式
        /// </summary>
        public void SetBuildSelectSelectModel()
        {
            buildCellSelectModel = true;
        }


        /// <summary>
        /// 建筑格子 取消为选择模式
        /// </summary>
        public void CancelBuildSelectSelectModel()
        {
            buildCellSelectModel = false;
        }



        /** 更新，每帧调用 */
        public void UpdateBuildCell()
        {
            int count = buildCellList.Count;
            BuildCellAgent cell;

            float distance;
            float minDistance = 10;
            BuildCellAgent minCell = null;
            Vector3 playerPosition = room.clientOperationUnit.position;
            _playerIsMove = Vector3.Distance(playerPosition, _playerPositionPre) > 0.2f;
            _playerPositionPre = playerPosition;


            for (int i = 0; i < count; i++)
            {
                cell = buildCellList[i];
                if (cell.buildCellData.hasUnit)
                {
                    if (!cell.isUsed)
                    {
                        cell.SetUsed();
                    }
                }
                else
                {
                    if (cell.isUsed)
                    {
                        cell.SetUnUsed();
                    }
                }

                // 检测离玩家最近的建筑
                if (!cell.buildCellData.hasUnit)
                {
                    distance = Vector3.Distance(cell.position, playerPosition);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minCell = cell;
                    }
                }

            }

            // 切换高亮建筑格子
            if (buildCellHighlight != minCell)
            {
                if (buildCellHighlight != null)
                {
                    buildCellHighlight.SetDefault();
                }

                buildCellHighlight = minCell;

                if (buildCellHighlight != null)
                {
                    buildCellHighlight.SetHighlight();
                }
            }

            if (buildCellHighlight != null)
            {
                buildCellHighlight.playerIsMove = _playerIsMove;
                if(_playerIsMove && !buildCellHighlight.isHighlight)
                {
                    buildCellHighlight.SetHighlight();
                }
            }

            // 选择模式
            if (buildCellSelectModel)
            {
                for (int i = 0; i < count; i++)
                {
                    cell = buildCellList[i];
                    if (cell.buildCellData.hasUnit && cell.buildCellData.clientIsOwn)
                    {
                        cell.SetSelectModel();
                    }
                    else
                    {
                        cell.SetUnSelectModel();
                    }
                }
            }
            else if(_buildCellSelectModelPre)
            {
                for (int i = 0; i < count; i++)
                {
                    cell = buildCellList[i];
                    cell.SetUnSelectModel();
                }
            }
            _buildCellSelectModelPre = buildCellSelectModel;
        }

    }
}
#endif
