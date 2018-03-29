using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games.Wars
{
    /// <summary>
    /// 技能触发事件
    /// </summary>
    public class SkillTriggerEvent
    {
        public  float                   triggerTime;
        public  SkillType               skillType;
        public  WarSkillEffectType      warSkillEffectType          = WarSkillEffectType.NormalAttack;
        public  float                   skillPlayTime               = 0;
        public  string                  effectPath                  = "";
        public  string                  effectPathsuf               = "";
        public  Vector3                 pathOffset                  = Vector3.zero;
        public  bool                    followSelf                  = false;
        public  int                     activeLv                    = 1;
        public  int                     blockLv                     = 15;
        /// <summary>
        /// 是否震屏
        /// </summary>
        public  bool                    bShakeEffect                = false;
        /// <summary>
        /// 振幅
        /// </summary>
        public  float                   shakeAmplitude              = 0;
        /// <summary>
        /// 震屏时间
        /// </summary>
        public  float                   shakeTime                   = 0;
        /// <summary>
        /// 影响范围
        /// </summary>
        public  float                   shakeRange                  = 0;
        public  PassiveTargetSelect     passiveTargetSelect         = new PassiveTargetSelect();
        /// <summary>
        /// 生命周期
        /// </summary>
        public  float                   life                        = 0;
        /// <summary>
        /// 目标选择
        /// </summary>
        public  AttackRule              attackRuleList              = new AttackRule();
        /// <summary>
        /// 生效数量
        /// </summary>
        public  int                     maxTargetsSelect            = 0;
        /// <summary>
        /// 是否瞬移
        /// </summary>
        public  bool                    isBlink                     = false;
        /// <summary>
        /// 瞬移到哪个线路
        /// </summary>
        public  int                     blinkRoute                  = 0;
        /// <summary>
        /// 瞬移到线路上的哪个节点
        /// </summary>
        public  int                     blinkChild                  = 0;
        /// <summary>
        /// 位移距离
        /// </summary>
        public  float                   selfMoveDis                 = 0;
        /// <summary>
        /// 位移速度
        /// </summary>
        public  float                   selfMoveSpeed               = 1;
        /// <summary>
        /// 立即生效伤害详情
        /// </summary>
        public  DamageInfo              damageInfo                  = new DamageInfo();
        /// <summary>
        /// 二次伤害详情
        /// </summary>
        public  SecondTarget            secondTarget                = new SecondTarget();
        /// <summary>
        /// 投射物
        /// </summary>
        public  Projectile              projectile                  = new Projectile();
        /// <summary>
        /// 开启或关闭无敌状态
        /// </summary>
        public  bool                    invincible                  = false;
    }
}
