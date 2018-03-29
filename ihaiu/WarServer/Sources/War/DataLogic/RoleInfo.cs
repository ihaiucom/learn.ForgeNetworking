using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 5:59:46 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    [Serializable]
    /** 角色信息 */
    public class RoleInfo
    {
        /** 角色ID */
        public int      roleId;

        /** 角色名称 */
        public string   roleName;

        /** 等级 */
        public int      roleLevel;

        /** 公会ID */
        public int      clanId;

        /** 公会名称 */
        public string   clanName;
    }
}
