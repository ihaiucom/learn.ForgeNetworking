using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 3:06:41 PM
*  @Description:    战斗模式
* ==============================================================================
*/
namespace Games.Wars
{
    /** 攻击规则 */
    public enum WarSkillRule
    {

        /// <summary>
        /// 随机
        /// </summary>
        RandomRange,

        /// <summary>
        /// 血量从高到低
        /// </summary>
        HpHighToLower,

        /// <summary>
        /// 血量从低到高
        /// </summary>
        HpLowerToHigh,

        /// <summary>
        /// 优先英雄
        /// </summary>
        HeroToBuild,
        /// <summary>
        /// 优先距离近的
        /// </summary>
        FromNearToFar,
        /// <summary>
        /// 优先距离远的
        /// </summary>
        FromFarToNear,
        /// <summary>
        /// boss优先
        /// </summary>
        BossToSolider,
        /// <summary>
        /// 小兵优先
        /// </summary>
        SoliderToBoss,
    }
    /// <summary>
    /// 特效位置，前摇，攻击，后摇
    /// </summary>
    public enum EffectPos
    {
        Before,
        Attack,
        Later,
    }
    /// <summary>
    /// 技能枚举类型
    /// </summary>
    public enum SkillType
    {
        /// <summary>
        /// 播放特效
        /// </summary>
        PlayEffect,
        /// <summary>
        /// 播放音效
        /// </summary>
        PlayMusic,
        /// <summary>
        /// 造成伤害
        /// </summary>
        TriggerDamage,
        /// <summary>
        /// 造成治疗
        /// </summary>
        TriggerCure,
        /// <summary>
        /// 扣除能量
        /// </summary>
        CustEngery,
        /// <summary>
        /// 回复能量
        /// </summary>
        RevertEngery,
        /// <summary>
        /// 减少CD
        /// </summary>
        ReduceCD,
        /// <summary>
        /// 施加buff
        /// </summary>
        CreateBuff,
        /// <summary>
        /// 移除buff
        /// </summary>
        RemoveBuff,
        /// <summary>
        /// 创建光环
        /// </summary>
        CreateHalo,
        /// <summary>
        /// 发射子弹
        /// </summary>
        ShotBullet,
        /// <summary>
        /// 自身朝前方发生位移
        /// </summary>
        MoveTowardCurrDir,
        /// <summary>
        /// 震屏
        /// </summary>
        ShakeCameraXXX,
        /// <summary>
        /// 技能结束
        /// </summary>
        SkillEnd,
        /// <summary>
        /// 播放动作
        /// </summary>
        PlayAnimator,
        /// <summary>
        /// 后摇开始
        /// </summary>
        AfterShaking,
    }

}
