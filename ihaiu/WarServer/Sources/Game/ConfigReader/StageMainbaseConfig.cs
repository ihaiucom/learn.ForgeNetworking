using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/22/2017 1:28:20 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡主基地配置 */
    public class StageMainbaseConfig
    {
        /** 势力ID */
        public int                  legionId;
        /** 单位配置 */
        public StageUnitConfig      unit = new StageUnitConfig();
        /** 位置 */
        [SerializeField]
        public Vector3                position;
        /** 方向 */
        [SerializeField]
        public Vector3                rotation = Vector3.zero;

        public StageMainbaseConfig Clone()
        {
            StageMainbaseConfig config = new StageMainbaseConfig();
            config.legionId = legionId;
            config.unit = unit.Clone();
            config.position = position;
            config.rotation = rotation;
            return config;
        }

        public override string ToString()
        {
            return string.Format("主基地{0} ({0})", legionId, unit);
        }
    }
}
