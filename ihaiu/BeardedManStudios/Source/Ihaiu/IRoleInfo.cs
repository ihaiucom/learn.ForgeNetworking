using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/26/2018 3:12:27 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public interface IRoleInfo
    {
        bool IsDeserialize { get; set; }
        byte[] Metadata { get; set; }
        byte[] Serialize();
        IRoleInfo Deserialize();

        ulong uid { get; set; }
        string name { get; set; }

        
    }
}
