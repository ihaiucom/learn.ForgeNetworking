using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 2:11:14 PM
*  @Description:    战斗视图时间
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗视图时间 */
    public class WarTime
    {
        public  float timeScale
        {
            get
            {
                return Time.timeScale;
            }

            set
            {
                Time.timeScale = value;
            }
        }


        public  float deltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }


        public  int frameCount
        {
            get
            {
                return Time.frameCount;
            }
        }


        public  float time
        {
            get
            {
                return Time.time;
            }
        }




        public  float fixedDeltaTime
        {
            get
            {
                return Time.fixedDeltaTime;
            }
        }

        public  float fixedTime
        {
            get
            {
                return Time.fixedTime;
            }
        }

        public  float realtimeSinceStartup
        {
            get
            {
                return Time.realtimeSinceStartup;
            }
        }

        public  float smoothDeltaTime
        {
            get
            {
                return Time.smoothDeltaTime;
            }
        }

        public  float unscaledDeltaTime
        {
            get
            {
                return Time.unscaledDeltaTime;
            }
        }


        public  float unscaledTime
        {
            get
            {
                return Time.unscaledTime;
            }
        }


    }
}
