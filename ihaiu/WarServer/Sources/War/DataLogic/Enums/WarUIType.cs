using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:48:58 PM
*  @Description:    单位类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 单位类型 */
    [System.Flags]
    public enum WarUIType
    {
        /// <summary>
        /// 回收机关按钮
        /// </summary>
        Recycle = 0,

        /// <summary>
        /// 右侧建造列表第一个
        /// </summary>
        Build1 = 11,
        /// <summary>
        /// 右侧建造列表第二个
        /// </summary>
        Build2 = 12,
        /// <summary>
        /// 右侧建造列表第三个
        /// </summary>
        Build3 = 13,
        /// <summary>
        /// 右侧建造列表第四个
        /// </summary>
        Build4 = 14,

        /** 所有 */
        All = 15,
    }

    [System.Flags]
    public enum WarSkillEffectType
    {
        NormalAttack = 0,
        LeftAttack,
        RightAttack,
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Skill5,
        Skill6,
        Skill7,
        Attack1,
    }
}
