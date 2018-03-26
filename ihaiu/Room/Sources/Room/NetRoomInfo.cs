using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 4:30:02 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public class NetRoomInfo : IRoomInfo
    {
        public bool IsDeserialize { get; set; }
        public byte[] Metadata { get; set; }

        public ulong roomUid { get; set; }
        public int stageId { get; set; }

        public byte[] Serialize()
        {
            BMSByte metadata = new BMSByte();
            ObjectMapper.Instance.MapBytes(metadata, roomUid);
            ObjectMapper.Instance.MapBytes(metadata, stageId);
            return metadata.CompressBytes();
        }

        public IRoomInfo Deserialize()
        {
            IsDeserialize = true;
            return this;
        }
    }
}
