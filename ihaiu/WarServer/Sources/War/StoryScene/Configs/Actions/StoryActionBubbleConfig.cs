using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，文本气泡)
    /// </summary>
    [Serializable]
    public class StoryActionBubbleConfig : StoryActionConfig
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string msg = "Hello";
        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration = 2;

        /// <summary>
        /// 坐标类型
        /// </summary>
        public StoryBubblePositionType positionType;

        /// <summary>
        /// [positionType=World] 有效
        /// 坐标
        /// </summary>
        public Vector3 positon;

        /// <summary>
        /// [positionType=Obj] 有效
        /// 物品目标名称
        /// </summary>
        public string objName;

        /// <summary>
        /// 单位id
        /// </summary>
        public int      unitId;


    }
}