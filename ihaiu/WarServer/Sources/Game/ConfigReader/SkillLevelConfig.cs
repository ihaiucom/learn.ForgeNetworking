using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 5:49:45 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 技能等级配置 */
    public class SkillLevelConfig
    {
        /** 技能等级ID */
        public int      levelId;
        /** 名称 */
        public string   name;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public float    cd;
        /// <summary>
        /// 充能上限
        /// </summary>
        public int      maxCharge;
        /// <summary>
        /// 装弹CD
        /// </summary>
        public float    bulletCD;
        /// <summary>
        /// 弹匣上限
        /// </summary>
        public int      bulletMax;
        /// <summary>
        /// 能量消耗
        /// </summary>
        public int      energyCost;
        /** 技能等级 */
        public int      level;
        /// <summary>
        /// 解锁条件单位
        /// </summary>
        public int      unlockUnitId;
        /// <summary>
        /// 解锁单位等级
        /// </summary>
        public int      unlockUnitLevel;
        /// <summary>
        /// 效果类型：1伤害2治疗
        /// </summary>
        public int      effect;
        /// <summary>
        /// 效果数值
        /// </summary>
        public int      effectValue;
        /// <summary>
        /// 施加buff
        /// </summary>
        public int      buff;
        /// <summary>
        /// buff时长
        /// </summary>
        public int      buffLife;
        /// <summary>
        /// 附加属性 AttributePack
        /// </summary>
        public int      attributePack;
        /// <summary>
        /// 描述文字
        /// </summary>
        public string   tip;
    }
}
