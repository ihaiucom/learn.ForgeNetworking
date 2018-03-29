using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，场景内物件操作)
    /// </summary>
    [Serializable]
    public class StoryActionSceneObjConfig : StoryActionConfig
    {
        /// <summary>
        /// 场景自定义列表序号
        /// </summary>
        public int                      objIndex;
        /// <summary>
        /// 单位id
        /// </summary>
        public int                      unitId;
        /// <summary>
        /// 动作名称
        /// </summary>
        public AnimatorState            objAnimation;
        /// <summary>
        /// 场景内物件的显示和隐藏
        /// </summary>
        public bool                     objShowOrHide       = false;
    }
}