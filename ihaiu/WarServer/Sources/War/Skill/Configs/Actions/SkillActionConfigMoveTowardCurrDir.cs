using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 位移
    /// </summary>
    [Serializable]
    public class SkillActionConfigMoveTowardCurrDir : SkillActionConfig
    {
        /// <summary>
        /// 是否瞬移
        /// </summary>
        public bool                    isBlink                     = false;
        /// <summary>
        /// 瞬移到哪个线路
        /// </summary>
        public int                     blinkRoute                  = 0;
        /// <summary>
        /// 瞬移到线路上的哪个节点
        /// </summary>
        public int                     blinkChild                  = 0;
        /// <summary>
        /// 位移距离
        /// </summary>
        public float                   selfMoveDis                 = 0;
        /// <summary>
        /// 位移速度
        /// </summary>
        public float                   selfMoveSpeed               = 1;
    }
}
