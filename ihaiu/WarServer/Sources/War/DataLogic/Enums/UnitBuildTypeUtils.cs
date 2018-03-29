using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 10:14:56 AM
*  @Description:    建筑类型 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 建筑类型 辅助类
    /// </summary>
    public static class UnitBuildTypeUtils
    {
        /** 是否是机关类型 */
        public static bool IsTower(this UnitBuildType value)
        {
            return value.UContain(UnitBuildType.Tower_Attack)
                || value.UContain(UnitBuildType.Tower_Defense)
                || value.UContain(UnitBuildType.Tower_Auxiliary)
                || value.UContain(UnitBuildType.Tower_Door);
        }

        public static bool UContain(this UnitBuildType value, UnitBuildType item)
        {
            return (int)(item & value) != 0;
        }


        public static UnitBuildType UAdd(this UnitBuildType value, UnitBuildType item)
        {
            return value | item;
        }
    }
}
