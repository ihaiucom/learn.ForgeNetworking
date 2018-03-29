using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 9:25:27 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public static class WarProxieModelUtils
    {
        public static bool PContain(this WarProxieModel value, WarProxieModel item)
        {
            return (int)(item & value) != 0;
        }

        public static bool PClient(this WarProxieModel val)
        {
            return (int)(WarProxieModel.Client & val) != 0;
        }

        public static bool PServer(this WarProxieModel val)
        {
            return (int)(WarProxieModel.Server & val) != 0;
        }


    }
}
