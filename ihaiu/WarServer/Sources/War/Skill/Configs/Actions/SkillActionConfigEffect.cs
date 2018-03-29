using System;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 特效
    /// </summary>
    [Serializable]
    public class SkillActionConfigEffect : SkillActionConfig
    {
        /// <summary>
        /// 特效路径
        /// </summary>
        public  string                  effectPath                  = "";
        /// <summary>
        /// 特效后缀，编辑器使用
        /// </summary>
        public  string                  effectPathsuf               = "";
        /// <summary>
        /// 特效从发射点出生
        /// </summary>
        public  bool                    isShotPos                   = true;
        /// <summary>
        /// 偏移坐标
        /// </summary>
        public  Vector3                 pathOffset                  = Vector3.zero;
        /// <summary>
        /// 是否附身特效
        /// </summary>
        public  bool                    followSelf                  = false;
        /// <summary>
        /// 特效生命周期
        /// </summary>
        public  float                   life                        = 1;
    }
}
