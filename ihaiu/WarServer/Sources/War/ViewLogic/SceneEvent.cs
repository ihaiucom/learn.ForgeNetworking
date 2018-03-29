using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/28/2017 3:48:49 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class SceneEvent : MonoBehaviour
    {
        public static Action<string> OnLoadEvent;
        public static Action<string> OnDestroyEvent;
        [HideInInspector]
        public string sceneName;
        private void Start()
        {
            sceneName = SceneManager.GetActiveScene().name;
            Loger.LogFormat("SceneEvent OnLoad {0}", sceneName);
            if(OnLoadEvent != null)
            {
                OnLoadEvent(sceneName);
            }
        }

        private void OnDestroy()
        {
            Loger.LogFormat("SceneEvent OnDestroy {0}", sceneName);
            if (OnDestroyEvent != null)
            {
                OnDestroyEvent(sceneName);
            }
        }
    }
}
