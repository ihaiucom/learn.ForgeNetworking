using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 6:46:23 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /** 关卡--航线配置 */
    [Serializable]
    public class StageRouteConfig
    {

        /** 航线名称备注 */
        public string           name;
        /** 航线ID */
        public int              routeId;
        /** 航线颜色ID */
        public int              colorIndex;
        /** 路径点 */
        [SerializeField]
        public List<Vector3>      path = new List<Vector3>();
        /// <summary>
        /// 父航线
        /// </summary>
        public int              parentId = -1;

        /** 获取航线起点 */
        public Vector3 GetBeginPoint()
        {
            return path[0];
        }


        /** 获取航线起点朝向 */
        public Vector3 GetBeginDirection()
        {
            if(path.Count > 1)
            {
                return HMath.AngleBetweenVector(path[0], path[1]).ToAngleVInt3();
            }
            return Vector3.zero;
        }

        public Vector3 GetBeginPoint(int count)
        {
            return path[count];
        }


        /** 获取航线起点朝向 */
        public Vector3 GetBeginDirection(int count)
        {
            if (path.Count > count + 1)
            {
                return HMath.AngleBetweenVector(path[count], path[count + 1]).ToAngleVInt3();
            }
            return Vector3.zero;
        }


        public StageRouteConfig Clone()
        {
            StageRouteConfig config = new StageRouteConfig();
            config.name = name;
            config.routeId = routeId;
            config.colorIndex = colorIndex;

            foreach(Vector3 point in path)
            {
                config.path.Add(point);
            }
            return config;
        }



    }
}
