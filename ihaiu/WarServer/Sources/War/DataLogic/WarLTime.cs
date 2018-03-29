using System;
using System.Collections.Generic;
using UnityEngine;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 7:55:48 PM
*  @Description:    战斗逻辑时间, 和服务器同步
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗逻辑时间, 和服务器同步 */
    public class WarLTime
    {
        //public float    deltaTime = 0.02f;
        //public int      frameCount = 0;
        //public float    time = 0;
        public float deltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }


        public int frameCount
        {
            get
            {
                return Time.frameCount;
            }
        }


        public float time
        {
            get
            {
                return Time.time;
            }
        }
    }
}
