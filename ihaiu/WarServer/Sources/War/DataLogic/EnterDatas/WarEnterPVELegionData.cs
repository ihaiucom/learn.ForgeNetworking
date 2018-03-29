using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/30/2017 10:45:31 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// PVE进入战斗数据--玩家数据
    /// </summary>
    [Serializable]
    public class WarEnterPVELegionData
    {
        // 基本信息
        [SerializeField]
        public RoleInfo                     roleInfo            = new RoleInfo();
        // 英雄单位
        [SerializeField]
        public WarEnterUnitData             hero                = new WarEnterUnitData();
        // 主基地单位
        [SerializeField]
        public WarEnterUnitData             mianbaseUnit        = new WarEnterUnitData();
        // 机关单位列表
        [SerializeField]
        public List<WarEnterUnitData>       towerList           = new List<WarEnterUnitData>();
    }
}
