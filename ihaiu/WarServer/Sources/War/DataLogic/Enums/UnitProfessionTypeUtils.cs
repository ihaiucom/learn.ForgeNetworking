using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/16/2017 6:12:27 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public static class UnitProfessionTypeUtils
    {
        public static bool UContain(this UnitProfessionType value, UnitProfessionType item)
        {
            return (int)(item & value) != 0;
        }

        public static UnitProfessionType UAdd(this UnitProfessionType value, UnitProfessionType item)
        {
            return value | item;
        }
    }
}
