namespace Games.Wars
{
    /// <summary>
    /// 技能事件类型
    /// </summary>
    public enum SKillActionType
    {
        /// <summary>
        /// 播放动作
        /// </summary>
        PlayAnimator,
        /// <summary>
        /// 播放特效
        /// </summary>
        PlayEffect,
        /// <summary>
        /// 播放音效
        /// </summary>
        PlayMusic,
        /// <summary>
        /// 进行攻击
        /// </summary>
        TriggerDamage,
        /// <summary>
        /// 移除buff
        /// </summary>
        RemoveBuff,
        /// <summary>
        /// 发射子弹
        /// </summary>
        ShotBullet,
        /// <summary>
        /// 自身朝前方发生位移
        /// </summary>
        MoveTowardCurrDir,
        /// <summary>
        /// 后摇开始
        /// </summary>
        AfterShaking,
        /// <summary>
        /// 技能结束
        /// </summary>
        SkillEnd,
        /// <summary>
        /// 创建镜像分身
        /// </summary>
        CreateMirror,
        /// <summary>
        /// buff生效
        /// </summary>
        BuffEffective,
        /// <summary>
        /// 射线攻击
        /// </summary>
        RayAttack,
    }
}
