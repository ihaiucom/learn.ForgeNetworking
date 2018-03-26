using BeardedManStudios;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/26/2018 3:10:55 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public interface IRoomInfo
    {
        bool IsDeserialize { get; set; }
        byte[] Metadata { get; set; }

        void MapBytes(BMSByte data);
        byte[] Serialize();
        IRoomInfo Deserialize();


        int classId { get; set; }
        ulong roomUid { get; set; }
        int stageId { get; set; }


    }
}
