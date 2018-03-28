using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 8:26:05 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /** 关卡--建筑格子 */
    [System.Serializable]
    public class StageBuildCellConfig
    {
        /** 格子Uid */
        public int                      uid;
        /** 航线ID */
        public int                      routeId     = -1;
        /** 坐标 */
        [SerializeField]
        public Vector3                    position    = Vector3.zero;
        /** 方向 */
        [SerializeField]
        public Vector3                    rotation    = Vector3.zero;
        /** 大小 */
        [SerializeField]
        public Vector2                    size        = new Vector2(6, 6);
        /** 初始建筑 */
        public StageBuildCellUnitConfig initUnit    = new StageBuildCellUnitConfig();


        public StageBuildCellConfig Clone()
        {
            StageBuildCellConfig config = new StageBuildCellConfig();
            config.uid = uid;
            config.routeId = routeId;
            config.position = position;
            config.rotation = rotation;
            config.size = size;
            config.initUnit = initUnit.Clone();
            return config;
        }

        public override string ToString()
        {
            if (initUnit.HasSetting)
            {
                return string.Format("建筑格子{0} ({1})", uid, initUnit);
            }
            else
            {
                return string.Format("建筑格子{0}", uid);
            }
        }

        public int GetLegionId()
        {
            if (initUnit.HasSetting)
                return initUnit.legionId;
            return -1;
        }
    }
}
