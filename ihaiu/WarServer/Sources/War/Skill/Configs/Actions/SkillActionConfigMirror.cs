using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 镜像分身
    /// </summary>
    [Serializable]
    public class SkillActionConfigMirror : SkillActionConfig
    {
        /// <summary>
        /// 镜像的属性与本体相同，无需单独属性，此属性不再使用
        /// </summary>
        public bool                 isUseOntology               = false;
        /// <summary>
        /// AvatarId
        /// </summary>
        public int                  unitId                      = 0;
        /// <summary>
        /// 武器id
        /// </summary>
        public int                  weaponId                    = 0;
        /// <summary>
        /// 是否随本体一起死亡
        /// </summary>
        public bool                 isDeathWithMain             = false;
        /// <summary>
        /// 出生距离
        /// </summary>
        public float                disFromSelf                 = 10;
        /// <summary>
        /// 出生移动速度
        /// </summary>
        //public float                moveSpeed                   = 1;
        /// <summary>
        /// 存活周期
        /// </summary>
        public float                life                        = 10;
        /// <summary>
        /// 输出百分比
        /// </summary>
        public float                attackPer                   = 0.5F;
        /// <summary>
        /// 防御百分比(受伤)
        /// </summary>
        public float                hitPer                      = 1.5F;
    }
}
