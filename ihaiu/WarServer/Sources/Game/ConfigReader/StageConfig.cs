using System;
using System.Collections.Generic;
using Games.Wars;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 1:37:50 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡配置 */
    public partial class StageConfig
    {
        /** 关卡名称 */
        public string                       stageName;
        /** 关卡ID */
        public int                          stageId;
        /** 关卡类型 */
        public StageType                    stageType               = StageType.Dungeon;
        /** 场景名称 */
        public string                       sceneName;
        /** 初始能量 */
        public float                        energy = 100;
        /** 初始能量上限 */
        public float                        energyMax = 100;
        /** 游戏时间 */
        public int                          timeLimit = -1;
        /** 主基地列表 */
        [SerializeField]
        public List<StageMainbaseConfig>    mainbaseList            = new List<StageMainbaseConfig>();
        /** 玩家出生区域列表 */
        [SerializeField]
        public List<RegionData>             spawnPlayerRegionList   = new List<RegionData>();
        /** 安全区域，仅针对个人关卡 */
        [SerializeField]
        public List<RegionData>             spawnRegionListSafe    = new List<RegionData>();
        /** 怪物出生区域列表 */
        //public List<RegionData>             spawnMonsterRegionList  = new List<RegionData>();
        /** 航线列表 */
        [SerializeField]
        public List<StageRouteConfig>       routeList               = new List<StageRouteConfig>();
        /** 建筑格子列表 */
        [SerializeField]
        public List<StageBuildCellConfig>   buildCellList           = new List<StageBuildCellConfig>();
        /** 怪物波次 */
        [SerializeField]
        public List<StageWaveConfig>        waveList                = new List<StageWaveConfig>();
        /** 势力配置 */
        [SerializeField]
        public List<StageLegionConfig>      legionList              = new List<StageLegionConfig>();
        /** 势力关系矩阵 */
        [SerializeField]
        public RelationMatrixData           releationMatrix = new RelationMatrixData();



        public StageWaveConfig GetWave(int index)
        {
            // 适应无怪物波次
            if (waveList.Count > 0)
            {
                return waveList[index];
            }
            else
            {
                return null;
            }
        }

        public int GetRouteColor(int routeId)
        {
            foreach(StageRouteConfig item in routeList)
            {
                if(item.routeId == routeId)
                {
                    return item.colorIndex;
                }
            }
            return -1;
        }


        public int GetLegionColor(int legionId)
        {
            if (legionId == -1) return -1;

            foreach (StageLegionConfig item in legionList)
            {
                if (item.legionId == legionId)
                {
                    return item.colorIndex;
                }
            }
            return -1;
        }

    }
}
