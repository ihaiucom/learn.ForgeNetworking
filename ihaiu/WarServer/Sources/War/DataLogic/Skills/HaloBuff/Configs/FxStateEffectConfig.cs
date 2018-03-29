using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:25:08 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 状态--美术效果
    /// </summary>
    public class FxStateEffectConfig : EffectConfig
    {
        public string       fx = "";
        public Space        parent = Space.Self;
        public Vector3      position = Vector3.zero;
        public float        radius = -1;
        public bool         stopDestory = true;



        public FxStateEffectConfig()
        {
            effectType = EffectType.FxStateEffect;
        }



        public FxStateEffectConfig(string fx, Space parent, Vector3 position,  bool stopDestory = true)
        {
            effectType = EffectType.FxStateEffect;
            this.fx = fx;
            this.parent = parent;
            this.position = position;
            this.stopDestory = stopDestory;
        }

        public FxStateEffectConfig SetUnitType(UnitType unitType)
        {
            this.unitType = unitType;
            return this;
        }
    }
}
