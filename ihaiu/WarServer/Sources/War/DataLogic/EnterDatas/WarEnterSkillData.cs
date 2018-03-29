using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 11:13:01 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战前数据--技能数据 */
    [Serializable]
    public struct WarEnterSkillData
    {
        /** 技能ID */
        public int skillId;
        /** 技能等级 */
        public int skillLevel;

        public WarEnterSkillData Clone()
        {
            WarEnterSkillData item = new WarEnterSkillData();
            item.skillId        = skillId;
            item.skillLevel     = skillLevel;
            return item;
        }
    }
}
