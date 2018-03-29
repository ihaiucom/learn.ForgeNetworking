using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 剧情--暂停
    /// </summary>
    public class StoryActionPause : StoryAction
    {
        override protected void OnStart()
        {
            stage.Puase(true);
            End();
        }
    }
}
