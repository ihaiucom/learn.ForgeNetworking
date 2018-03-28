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
        public int ClassId { get { return 0; } }
        protected bool IsDeserialize { get; set; }
        protected byte[] Metadata { get; set; }

        private static Dictionary<int, Type> classDict = new Dictionary<int, Type>();
        public static void RegistereClasss<T>(int classId) where T : IRoleInfo
        {
            classDict[classId] = typeof(T);
        }

        public static IRoleInfo Read(BMSByte StreamData)
        {

            int classId = StreamData.GetBasicType<int>();
            IRoleInfo info = null;

            if(classDict.ContainsKey(classId) && classDict[classId] != null)
            {
                info = (IRoleInfo) Activator.CreateInstance(classDict[classId]);
            }

            if(info == null)
            {
                info = new NetRoleInfo();
            }

            info.ReadBytes(StreamData);
            return info;
        }

        public virtual void MapBytes(BMSByte data)
        {
            ObjectMapper.Instance.MapBytes(data, ClassId);
            ObjectMapper.Instance.MapBytes(data, uid);
            ObjectMapper.Instance.MapBytes(data, name);

            OnMapBytes(data);


            byte[] metadata = SerializeMetadata();
            //如果对象具有元数据，则写入
            ObjectMapper.Instance.MapBytes(data, metadata != null);
            if (metadata != null)
                ObjectMapper.Instance.MapBytes(data, metadata);
        }

        public virtual void ReadBytes(BMSByte StreamData)
        {
            uid = StreamData.GetBasicType<ulong>();
            name = StreamData.GetBasicType<string>();

            OnReadBytes(StreamData);

            if (StreamData.GetBasicType<bool>())
                Metadata = ObjectMapper.Instance.Map<byte[]>(StreamData);

            DeserializeMetadata();
        }


        protected virtual void OnMapBytes(BMSByte data)
        {

        }

        protected virtual void OnReadBytes(BMSByte StreamData)
        {

        }

        protected virtual byte[] SerializeMetadata()
        {
            //BMSByte metadata = new BMSByte();
            //ObjectMapper.Instance.MapBytes(metadata, uid);
            //ObjectMapper.Instance.MapBytes(metadata, name);
            //return metadata.CompressBytes();
            return null;
        }

        protected virtual IRoleInfo DeserializeMetadata()
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
