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
        public int ClassId { get { return 0; } }
        protected bool IsDeserialize { get; set; }
        protected byte[] Metadata { get; set; }

        public ulong roomUid { get; set; }
        public int stageClassId { get; set; }

        private static Dictionary<int, Type> classDict = new Dictionary<int, Type>();
        public static void RegistereClasss<T>(int classId) where T : IRoomInfo
        {
            classDict[classId] = typeof(T);
        }

        public static IRoomInfo Read(BMSByte StreamData)
        {

            int classId = StreamData.GetBasicType<int>();
            IRoomInfo info = null;

            if (classDict.ContainsKey(classId) && classDict[classId] != null)
            {
                info = (IRoomInfo)Activator.CreateInstance(classDict[classId]);
            }

            if (info == null)
            {
                info = new NetRoomInfo();
            }

            info.ReadBytes(StreamData);
            return info;
        }


        public virtual void MapBytes(BMSByte data)
        {
            ObjectMapper.Instance.MapBytes(data, ClassId);
            ObjectMapper.Instance.MapBytes(data, roomUid);
            ObjectMapper.Instance.MapBytes(data, stageClassId);

            OnMapBytes(data);

            byte[] metadata = SerializeMetadata();
            //如果对象具有元数据，则写入
            ObjectMapper.Instance.MapBytes(data, metadata != null);
            if (metadata != null)
                ObjectMapper.Instance.MapBytes(data, metadata);
        }

        public virtual void ReadBytes(BMSByte StreamData)
        {
            roomUid = StreamData.GetBasicType<ulong>();
            stageClassId = StreamData.GetBasicType<int>();

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
            //ObjectMapper.Instance.MapBytes(metadata, roomUid);
            //ObjectMapper.Instance.MapBytes(metadata, stageId);
            //return metadata.CompressBytes();
            return null;
        }

        protected virtual IRoomInfo DeserializeMetadata()
        {
            IsDeserialize = true;
            return this;
        }
    }
}
