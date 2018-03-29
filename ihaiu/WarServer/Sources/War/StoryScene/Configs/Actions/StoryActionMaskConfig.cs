using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，遮罩，显示隐藏UI组件)
    /// </summary>
    [Serializable]
    public class StoryActionMaskConfig : StoryActionConfig
    {
        /// <summary>
        /// 开启或关闭
        /// </summary>
        public bool                 isOpen;
    }
}