using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 创建buff
    /// </summary>
    [Serializable]
    public class SkillActionConfigBuffCreate : SkillActionConfig
    {
        /// <summary>
        /// 创建buffId，仅仅创建buff列表，不生效，附加在主体身上，等待条件触发
        /// </summary>
        public  int                     buffId                  = 0;
        public  float                   buffLife                = 0;
    }
}
