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

        public byte[] Serialize()
        {
            BMSByte metadata = new BMSByte();
            ObjectMapper.Instance.MapBytes(metadata, uid);
            ObjectMapper.Instance.MapBytes(metadata, name);
            return metadata.CompressBytes();
        }

        public IRoleInfo Deserialize()
        {
            IsDeserialize = true;
            return this;
        }


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
