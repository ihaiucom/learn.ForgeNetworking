using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为结束方式
    /// </summary>
    public enum StoryActionEndType 
    {
        None = 0,
        // 下一帧
        NextFrame,
        // 持续一段时间
        DurationTime,
        // 自己Call End
        Call,
    }
}