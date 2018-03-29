using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情状态
    /// </summary>
    public enum StoryStageStatus
    {

        // 没处理
        None,
        // 触发器检测中
        Tiggering,
        // 已经触发，正在执行行为
        Actioning,
        // 已经完成
        Finish,
    }
}