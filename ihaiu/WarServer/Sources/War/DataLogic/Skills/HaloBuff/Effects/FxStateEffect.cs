using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:49:17 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 美术特效
    /// </summary>
    public class FxStateEffect : Effect
    {
        public FxStateEffectConfig mconfig;


        public override AbstractEffect SetConfig(EffectConfig config)
        {
            mconfig = (FxStateEffectConfig)config;

            return base.SetConfig(config);
        }


        public GameObject go;

        /** 启动 */
        protected override void OnStart()
        {
            base.OnStart();
            if (mconfig.fx == null)
            {
                Debug.LogError("FxStateEffect OnStart prefab=null");
                return;
            }

           
            go = room.clientRes.GetGameObjectInstall(mconfig.fx);
            if (go == null)
            {
                Debug.LogError("FxStateEffect OnStart go=null");
                return;
            }

            if (mconfig.parent == Space.Self)
            {
                UnitAgent unitAgent = room.clientSceneView.GetUnit(unit.uid);
                if (unitAgent != null)
                {
                    go.transform.SetParent(unitAgent.transform);
                    go.transform.localPosition = mconfig.position;
                }
            }
            else
            {
                go.transform.position = mconfig.position;
            }


            go.SetActive(true);
        }

        /** 停止 */
        protected override void OnStop()
        {
            base.OnStop();
            if (go != null && mconfig != null && mconfig.stopDestory)
                room.clientRes.Despawn(go);
        }
    }
}
