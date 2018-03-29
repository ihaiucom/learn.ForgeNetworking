using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，相机操作)
    /// </summary>
    [Serializable]
    public class StoryActionCameraConfig : StoryActionConfig
    {
        /// <summary>
        /// 振幅
        /// </summary>
        public  float                                   shakeAmplitude              = 0;
        /// <summary>
        /// 影响范围
        /// </summary>
        public  float                                   shakeRange                  = 0;
        
        /// <summary>
        /// 旋转角度
        /// </summary>
        public  StoryRotation                           storyRotation;
        public  Vector3                                 endValue;

        /// <summary>
        /// 移动路径
        /// </summary>
        public  Vector3[]                               movePath                    = new Vector3[0];
        /// <summary>
        /// 移动总时长
        /// </summary>
        public  float                                   duration;
        /// <summary>
        /// 跟随的名称
        /// </summary>
        public  string                                  followName                  = "";
    }
}