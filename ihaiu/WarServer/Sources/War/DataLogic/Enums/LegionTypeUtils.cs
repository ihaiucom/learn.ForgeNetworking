using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 10:14:56 AM
*  @Description:    势力类型 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    public static class LegionTypeUtils
    {
        public static bool LContain(this LegionType value, LegionType item)
        {
            return (int)(item & value) != 0;
        }
    }
}
