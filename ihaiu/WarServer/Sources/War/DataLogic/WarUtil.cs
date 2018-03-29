using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 9:58:15 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 战斗辅助工具类
    /// </summary>
    public static class WarUtil
    {
        /// <summary>
        /// 单位列表排序，按距离
        /// </summary>
        /// <param name="unitList">单位列表</param>
        /// <param name="point">参照单位</param>
        public static void SortByDistance(this List<UnitData> unitList, UnitData unit)
        {
            SortByDistance(unitList, unit.position);
        }

        /// <summary>
        /// 单位列表排序，按距离，结果为从小到大排列
        /// </summary>
        /// <param name="unitList">单位列表</param>
        /// <param name="point">参照坐标</param>
        public static void SortByDistance(this List<UnitData> unitList, Vector3 point)
        {
            unitList.Sort((UnitData a, UnitData b) => {
                float aa = Vector3.Distance(point, a.position);
                float bb = Vector3.Distance(point, b.position);
                if(aa > bb)
                {
                    return -1;
                }
                else if(aa < bb)
                {
                    return 1;
                }
                return 0;
            });
        }

        /// <summary>
        /// 获取from点到to点方向距离from点distance的点
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector3 Point(this UnitData from, UnitData to, float distance)
        {
            return HMath.Point(from.position, to.position, distance);
        }

        /// <summary>
        /// 获取两个单位之间的距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Distance(this UnitData from, UnitData to)
        {
            return Vector3.Distance(from.position, to.position);
        }

    }
}
