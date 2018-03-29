using Pathfinding;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 10:14:56 AM
*  @Description:    单位空间类型 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位空间类型 辅助类
    /// </summary>
    public static class UnitSpaceTypeUtils
    {

        /** 是否是 地面单位 */
        public static bool IsGround(this UnitSpaceType value)
        {
            return value.UContain(UnitSpaceType.Ground);
        }

        /** 是否是 飞行单位 */
        public static bool IsFly(this UnitSpaceType value)
        {
            return value.UContain(UnitSpaceType.Fly);
        }



        public static bool UContain(this UnitSpaceType value, UnitSpaceType item)
        {
            return (int)(item & value) != 0;
        }


        public static UnitSpaceType UAdd(this UnitSpaceType value, UnitSpaceType item)
        {
            return value | item;
        }


        public static int GetPathGraphMask(this UnitSpaceType value)
        {
            return UnityPathGraphSetting.GetPathGraphMask(value);
            //if (AstarPath.active.graphs.Length <= 1)
            //    return -1;

            //int val = 0;
            //if (value.IsGround())
            //{
            //    val |= 1 << 0;
            //}

            //if (value.IsFly())
            //{
            //    val |= 1 << 1;
            //}
            //return val;
        }


        public static NNConstraint GetNNConstraint(this UnitSpaceType value)
        {
            
            NNConstraint item = NNConstraint.None;
            item.graphMask = value.GetPathGraphMask();
            return item;
        }


		public static NNConstraint GetNNConstraint(this UnitData value)
		{

			NNConstraint item = NNConstraint.None;
			item.graphMask = value.GetPathGraphMask();
			return item;
		}
    }
}
