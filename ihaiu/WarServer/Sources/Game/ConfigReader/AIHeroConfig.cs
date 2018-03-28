using System.Collections.Generic;

namespace Games
{
    public class AIHeroConfig
    {
        /** 编号 */
        public int                                  id;
        /** 名称 */
        public string                               name;
        /// <summary>
        /// 描述文字
        /// </summary>
        public string                               tip;
        /// <summary>
        /// AI刷新间隔
        /// </summary>
        public float                                timeCD;
        /// <summary>
        /// 默认状态
        /// </summary>
        public Wars.UnitHeroAI.HeroAiEnum           defaultState;
        /// <summary>
        /// 距离列表
        /// </summary>
        public Dictionary<int, int>                distanceList;
        /// <summary>
        /// 距离事件列表
        /// </summary>
        public Dictionary<int,Dictionary<int,Wars.UnitHeroAI.HeroAiEnum>> eventDic;

    }
}
