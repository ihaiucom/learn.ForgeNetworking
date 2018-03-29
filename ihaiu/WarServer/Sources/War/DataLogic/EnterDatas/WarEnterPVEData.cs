using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/30/2017 10:45:10 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// PVE进入战斗数据
    /// </summary>
    [Serializable]
    public class WarEnterPVEData
    {
        // 关卡ID
        public int                              stageId     = 0;
        // 关卡类型
        public StageType                        stageType   = StageType.Dungeon;
        // 自己角色ID
        public int                              ownRoleId   = 0;
        // 玩家列表
        [SerializeField]
        public List<WarEnterPVELegionData>      legionList  = new List<WarEnterPVELegionData>();
        // 怪物生命周期，大于0才参与计算，默认针对活动副本的怪物有效
        public float                            monsterLifeTime = 0;
        public int                              monsterAllCount = 0;
        // 活动副本类型
        public ActivityType                     activityType = ActivityType.None;
    }
}
