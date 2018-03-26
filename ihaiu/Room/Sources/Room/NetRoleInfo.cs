using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 10:20:22 AM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class NetRoleInfo : IRoleInfo
    {
        public bool IsDeserialize { get; set; }
        public byte[] Metadata { get; set; }

        public static IRoleInfo Read(BMSByte StreamData)
        {
            IRoleInfo roleInfo = new NetRoleInfo();
            roleInfo.uid = StreamData.GetBasicType<ulong>();
            roleInfo.name = StreamData.GetBasicType<string>();
            if (StreamData.GetBasicType<bool>())
                roleInfo.Metadata = ObjectMapper.Instance.Map<byte[]>(StreamData);

            roleInfo = roleInfo.Deserialize();
            return roleInfo;
        }

        public virtual void MapBytes(BMSByte data)
        {
            ObjectMapper.Instance.MapBytes(data, uid);
            ObjectMapper.Instance.MapBytes(data, name);

            byte[] metadata = Serialize();

            //如果对象具有元数据，则写入
            ObjectMapper.Instance.MapBytes(data, metadata != null);
            if (metadata != null)
                ObjectMapper.Instance.MapBytes(data, metadata);
        }

        public virtual byte[] Serialize()
        {
            //BMSByte metadata = new BMSByte();
            //ObjectMapper.Instance.MapBytes(metadata, uid);
            //ObjectMapper.Instance.MapBytes(metadata, name);
            //return metadata.CompressBytes();
            return null;
        }

        public virtual IRoleInfo Deserialize()
        {
            IsDeserialize = true;
            return this;
        }


        public int classId { get; set; }
        public ulong uid { get; set; }
        public string name { get; set; }

        // 玩家加入
        internal void OnPlayerJoined()
        {

        }

        // 玩家离开
        internal void OnPlayerLeft()
        {

        }
    }
}
