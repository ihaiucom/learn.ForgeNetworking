using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 被动触发条件
    /// </summary>
    public enum Triggercondition
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 创建时
        /// </summary>
        OnCreated,
        /// <summary>
        /// 受到伤害
        /// </summary>
        OnHitDamage,
        /// <summary>
        /// 建造同类型机关
        /// </summary>
        OnCreateSameType,
        /// <summary>
        /// 自身死亡
        /// </summary>
        OnDie,
        /// <summary>
        /// 攻击命中
        /// </summary>
        OnAttackTarget,
        /// <summary>
        /// 怪物刷新
        /// </summary>
        OnMonsterRefrush,
        /// <summary>
        /// 开始攻击
        /// </summary>
        OnStartAttack,
    }
}