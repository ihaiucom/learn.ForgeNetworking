using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 被动判断条件
    /// </summary>
    public enum JudgmentTargger
    {
        None,
        /// <summary>
        /// 场上同类数量
        /// </summary>
        SameTypeCount,
        /// <summary>
        /// 目标类型
        /// </summary>
        TargetType,
        /// <summary>
        /// 距离目标距离
        /// </summary>
        TargetDis,
        /// <summary>
        /// 随机概率<X%
        /// </summary>
        RandRangeCount,
        /// <summary>
        /// 自身buff层数
        /// </summary>
        SelfBuffCount,
        /// <summary>
        /// 当前怪物波数
        /// </summary>
        MonsterWaves,
        /// <summary>
        /// 自身属性临界点
        /// </summary>
        SelfProperties,
        /// <summary>
        /// 技能携带buff
        /// </summary>
        SkillAddBuff,
        /// <summary>
        /// 更改普攻技能调用信息
        /// </summary>
        ChangeNormalSkill,
    }
}