using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 过程动画中添加的物件
    /// </summary>
    public class StorySceneUnit
    {
        public  GameObject          gObject;
        public  Transform           tForm;
        public  AnimatorManager     animatorManager;
        public  string              objName                     = "";
    }
}