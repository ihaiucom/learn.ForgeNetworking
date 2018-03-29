#if !NOT_USE_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 11:16:44 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class WarProxyUtil
    {
        /** 创建驱动 */
        public static WarUnityDirve CreateDirve(WarRoom room)
        {
            GameObject go = GameObject.Find("WarManager");
            if (go == null)
            {
                go = new GameObject("WarManager");
                GameObject.DontDestroyOnLoad(go);
            }

            WarUnityDirve drive = go.GetComponent<WarUnityDirve>();
            if (drive == null)
            {
                drive = go.AddComponent<WarUnityDirve>();
            }
            drive.SetRoom(room);

            return drive;
        }

        /** 创建资源管理器 */
        public static WarUnityRes CreateRes(WarRoom room)
        {
            return new WarUnityRes(room);
        }

        /** 创建视图代理 */
        public static WarUnityViewAgent CreateViewAgent(WarRoom room)
        {
            WarUnityViewAgent view = new WarUnityViewAgent();
            view.room = room;
            return view;
        }

        /** 创建场景视图 */
        public static WarUnitySceneView CreateSceneView(WarRoom room)
        {
            WarUnitySceneView view = new WarUnitySceneView();
            view.room = room;
            return view;
        }
    }
}
#endif