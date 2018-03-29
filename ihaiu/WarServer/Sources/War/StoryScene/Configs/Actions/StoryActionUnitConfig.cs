using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能，单位操作)
    /// </summary>
    [Serializable]
    public class StoryActionUnitConfig : StoryActionConfig
    {
        // 添加到场景内
        //public  int                 addUnitId;
        //public  int                 addUnitLv               = 1;
        //public  int                 addLegionId;
        //public  int                 addRouteId;
        //public  int                 addRouteIndex           = 1;
        //public  string              addUnitName;

        // 非添加时，对哪个单位进行操作
        //public  string              currentUnitName;
        // 动作
        public  AnimatorState       animatorState;

        // 属性
        public  int                 propId;
        public  PropType            propType                = PropType.Add;
        public  int                 propValue               = 0;
        
        // 位置
        public Vector3              position                = Vector3.zero;
        public Vector3              angle                   = Vector3.zero;

        // 移动
        public Vector3[]            movePath                = new Vector3[0];
        public float                moveDuration;
    }
}