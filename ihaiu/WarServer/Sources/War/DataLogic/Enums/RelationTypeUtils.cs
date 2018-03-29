using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 5:19:33 PM
*  @Description:    关系 辅助类
* ==============================================================================
*/
namespace Games.Wars
{
    public static class RelationTypeUtils
    {
        public static bool ROwn(this RelationType relation)
        {
            return (int)(RelationType.Own & relation) != 0;
        }

        public static bool RFriendly(this RelationType relation)
        {
            return (int)(RelationType.Friendly & relation) != 0;
        }


        public static bool REnemy(this RelationType relation)
        {
            return (int)(RelationType.Enemy & relation) != 0;
        }

        public static bool RContain(this RelationType relation, RelationType item)
        {
            return (int)(item & relation) != 0;
        }
    }
}
