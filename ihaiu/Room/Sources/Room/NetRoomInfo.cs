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


        public static IRoomInfo Read(BMSByte StreamData)
        {
            IRoomInfo roomInfo = new NetRoomInfo();
            roomInfo.roomUid = StreamData.GetBasicType<ulong>();
            roomInfo.stageId = StreamData.GetBasicType<int>();
            if (StreamData.GetBasicType<bool>())
                roomInfo.Metadata = ObjectMapper.Instance.Map<byte[]>(StreamData);

            roomInfo = roomInfo.Deserialize();
            return roomInfo;
        }

        public virtual void MapBytes(BMSByte data)
        {
            ObjectMapper.Instance.MapBytes(data, roomUid);
            ObjectMapper.Instance.MapBytes(data, stageId);

            byte[] metadata = Serialize();

            //如果对象具有元数据，则写入
            ObjectMapper.Instance.MapBytes(data, metadata != null);
            if (metadata != null)
                ObjectMapper.Instance.MapBytes(data, metadata);
        }

        public byte[] Serialize()
        {
            //BMSByte metadata = new BMSByte();
            //ObjectMapper.Instance.MapBytes(metadata, roomUid);
            //ObjectMapper.Instance.MapBytes(metadata, stageId);
            //return metadata.CompressBytes();
            return null;
        }

        public IRoomInfo Deserialize()
        {
            IsDeserialize = true;
            return this;
        }
    }
}
