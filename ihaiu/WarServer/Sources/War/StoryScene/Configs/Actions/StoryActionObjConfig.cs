using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，物件操作)
    /// </summary>
    [Serializable]
    public class StoryActionObjConfig : StoryActionConfig
    {
        /// <summary>
        /// 物件名称标签 
        /// </summary>
        public string                   objName                 = "新物件";
        public string                   objPath;
        public string                   objPathsuf;
        public Vector3                  objPos                  = Vector3.zero;
        public Vector3                  objAngle                = Vector3.zero;
        public Vector3                  objScale                = Vector3.one;

        // 非添加物件时，对哪个物件进行操作
        public string                   currentObjName;

        // 动作
        public AnimatorState            objAnimation;

        // 移动
        public Vector3[]                objMovePath             = new Vector3[0];
        // 移动总时长
        public float                    objMoveDuration;

    }
}