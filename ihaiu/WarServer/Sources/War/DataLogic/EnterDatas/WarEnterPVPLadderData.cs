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
    /// WarEnterPVPLadderData进入战斗数据
    /// </summary>
    [Serializable]
    public class WarEnterPVPLadderData
    {
        // 关卡ID
        public int                              stageId             = 0;
        // 关卡类型
        public StageType                        stageType           = StageType.PVPLadder;
        // 自己角色ID
        public int                              ownRoleId           = 0;
        // 对方角色ID
        public int                              otherRoleId         = 0;
        // 自己角色数据列表
        [SerializeField]
        public List<WarEnterPVELegionData>      legionList          = new List<WarEnterPVELegionData>();
        // 对方角色数据列表
        [SerializeField]
        public List<WarEnterPVELegionData>      ohterLegionList     = new List<WarEnterPVELegionData>();
    }
}
