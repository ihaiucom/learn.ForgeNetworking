using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 10:14:56 AM
*  @Description:    士兵类型 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 士兵类型 辅助类
    /// </summary>
    public static class UnitSoliderTypeUtiles
    {
        /** 是否是 小怪或者精英怪 */
        public static bool IsGeneralOrElite(this UnitSoliderType value)
        {
            return value.UContain(UnitSoliderType.General)
                || value.UContain(UnitSoliderType.Elite);
        }


        /** 是否是 Boss */
        public static bool IsBoss(this UnitSoliderType value)
        {
            return value.UContain(UnitSoliderType.Boss);
        }


        public static bool UContain(this UnitSoliderType value, UnitSoliderType item)
        {
            return (int)(item & value) != 0;
        }


        public static UnitSoliderType UAdd(this UnitSoliderType value, UnitSoliderType item)
        {
            return value | item;
        }



    }
}
