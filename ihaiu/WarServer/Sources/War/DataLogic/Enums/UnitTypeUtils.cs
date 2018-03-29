using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 10:08:07 AM
*  @Description:    单位类型 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    public static class UnitTypeUtils
    {
        public static bool UContain(this UnitType value, UnitType item)
        {
            return (int)(item & value) != 0;
        }

        public static UnitType UAdd(this UnitType value, UnitType item)
        {
            return value | item;
        }
    }
}
