using System.Collections;
using System;

using ProtoBuf;
using System.IO;
using BeardedManStudios.Forge.Networking;

namespace ihaiu
{
    public interface IProtoItem
    {
        bool hasListen { get; }
        Type protoStructType { get; set; }
        int opcode { get; set; }
        void Handle(Stream stream, NetworkingPlayer player);
        void Handle(byte[] bytes, NetworkingPlayer player);
    }

    public class ProtoItem<T> : IProtoItem where T : new()
    {

        /** 协议号 */
        public int opcode { get; set; }

        /** 结构体Type */
        public Type protoStructType { get; set; }

        /** 结构体名称 */
        public string protoStructName;


        /** 协议文件 */
        public string protoFilename;

        /** 协议文件 */
        public int[] opcodeMapping;

        /** 描述 */
        public string note;



        public Action<T> OnClientReceiveOnce;
        public Action<int, T> OnClientReceiveTwo;

        public Action<T, NetworkingPlayer> OnServerReceiveOnce;
        public Action<int, T, NetworkingPlayer> OnServerReceiveTwo;

        public bool hasListen
        {
            get
            {
                return OnServerReceiveOnce != null || OnServerReceiveTwo != null || OnClientReceiveOnce != null || OnClientReceiveTwo != null;
            }
        }


        public void Handle(byte[] bytes, NetworkingPlayer player)
        {
            if(bytes == null || bytes.Length == 0)
            {
                T msg = new T();

                // server
                if (OnServerReceiveOnce != null)
                {
                    OnServerReceiveOnce(msg, player);
                }

                if (OnServerReceiveTwo != null)
                {
                    OnServerReceiveTwo(opcode, msg, player);
                }

                // client
                if (OnClientReceiveOnce != null)
                {
                    OnClientReceiveOnce(msg);
                }

                if (OnClientReceiveTwo != null)
                {
                    OnClientReceiveTwo(opcode, msg);
                }
            }
            else
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    Handle(stream, player);
                }
            }
        }

        public void Handle(Stream stream, NetworkingPlayer player)
        {
            stream.Seek(0, SeekOrigin.Begin);
            T msg = Serializer.Deserialize<T>(stream);

            // server
            if (OnServerReceiveOnce != null)
            {
                OnServerReceiveOnce(msg, player);
            }

            if (OnServerReceiveTwo != null)
            {
                OnServerReceiveTwo(opcode, msg, player);
            }

            // client
            if (OnClientReceiveOnce != null)
            {
                OnClientReceiveOnce(msg);
            }

            if (OnClientReceiveTwo != null)
            {
                OnClientReceiveTwo(opcode, msg);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}]", opcode, protoStructName, protoFilename, note);
        }

    }

}