using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 11:09:06 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    
    [Serializable]
    /** 战前数据--单位数据 */
    public class WarEnterUnitData
    {
        /** 名称备注 */
        public string   name;
        /** 单位ID */
        public int      unitId;
        /** 单位等级 */
        public int      unitLevel;
        /** 非默认模型id */
        public int      avatarId;

        /** 技能列表 */
        [SerializeField]
        public List<WarEnterSkillData>  skillList = new List<WarEnterSkillData>();


    }
}
