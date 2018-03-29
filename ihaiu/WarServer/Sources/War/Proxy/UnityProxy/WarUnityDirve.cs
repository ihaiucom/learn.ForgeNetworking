#if !NOT_USE_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/12/2017 7:30:14 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars 
{
    /// <summary>
    /// Unity 驱动
    /// </summary>
    public class WarUnityDirve : MonoBehaviour, IWarDrive
    {
        /** 房间--门面 */
        public WarRoom room { get; set; }



        /** 设置房间 */
        public void SetRoom(WarRoom room)
        {
            this.room = room;
        }

        /** 销毁 */
        public void DestoryDirve()
        {
            DestroyImmediate(gameObject);
        }


        /** 更新 */
        private void FixedUpdate()
        {
            if(room != null)
            {
                room.Update();
                //room.clientUnitcontrol.Update();
            }
        }



    }
}
#endif