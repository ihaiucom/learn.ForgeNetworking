using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 过场动画基本类，保存为json
    /// </summary>
    public class StoryStageConfig
    {
        /// <summary>
        /// id
        /// </summary>
        public  int                                     storyScenesId;
        /// <summary>
        /// 名称
        /// </summary>
        public  string                                  storySceneName;

        /// <summary>
        /// 触发条件
        /// </summary>
        public  TirggerType                             tirggerType;
        /// <summary>
        /// 触发配置
        /// </summary>
        public  StoryTriggerConfig                      storyTrigger             = null;
        /// <summary>
        /// 剧情信息
        /// </summary>
        public Dictionary<int,List<StoryActionConfig>>  storyActionConfigDic     = new Dictionary<int, List<StoryActionConfig>>();

        /// <summary>
        /// 自增长id，编辑器适用，其他地方不用
        /// </summary>
        public  int                                     actionAddID              = 0;
    }
}