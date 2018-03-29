using FMODUnity;
using System;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 子弹
    /// </summary>
    [Serializable]
    public class SkillActionConfigProjectile : SkillActionConfig
    {
        /// <summary>
        /// 子弹音效文件路径
        /// </summary>
        [SoundKey]
        public string                   projectSound                    = "";
        /// <summary>
        /// 子弹路径
        /// </summary>
        //public string                   projectPath                     = "";
        //public string                   projectPathsuf                  = "";
        /// <summary>
        /// 偏移量
        /// </summary>
        //public Vector3                  pathOffset                      = Vector3.zero;
        /// <summary>
        /// 投射物的方向和移动方法
        /// </summary>
        public ProjectileMoveMethod     projectileMoveMethod;
        /// <summary>
        /// 最大投射物的创建数量
        /// </summary>
        public int                      maxProjectiles                  = 1;
        /// <summary>
        /// cd间隔
        /// </summary>
        public float                    createCD                        = 0;
        /// <summary>
        /// 多重箭角度
        /// </summary>
        public float                    angleFromEach                   = 45;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float                    moveSpeed                       = 1;
        /// <summary>
        /// 撞检测宽度
        /// </summary>
        public float                    collisionRayWidth               = 0.02F;
        /// <summary>
        /// 最大飞行距离
        /// </summary>
        public float                    maxMoveDistance                 = 1;
        /// <summary>
        /// 最大穿透数量
        /// </summary>
        public int                      CrossCount                      = 1;
        /// <summary>
        /// 是否抛物线
        /// </summary>
        public bool                     bRotation                       = false;
        /// <summary>
        /// 抛物线弧线中的夹角
        /// </summary>
        public float                    rotationAngle                  = 45;
        /// <summary>
        /// 子弹伤害详情
        /// </summary>
        public SkillActionConfigDamage  skillActionConfigDamage         = new SkillActionConfigDamage();
    }
}
