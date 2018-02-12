using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:40:21 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public abstract class ProtoBase
    {
        protected HBaseTCP baseTcp;

        public ProtoList protoListListener;
        public ProtoList protoListSender;

        public ProtoBase(HBaseTCP baseTcp)
        {
            this.baseTcp = baseTcp;
        }


        /** 添加监听 */
        public void AddCallback<T>(Action<int, T> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnClientReceiveTwo += callback;
        }

        public void AddCallback<T>(Action<T> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnClientReceiveOnce += callback;
        }


        public void AddCallback<T>(Action<int, T, NetworkingPlayer> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnServerReceiveTwo += callback;
        }

        public void AddCallback<T>(Action<T, NetworkingPlayer> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnServerReceiveOnce += callback;
        }


        /** 移除监听 */
        public void RemoveCallback<T>(Action<int, T> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnClientReceiveTwo -= callback;
        }

        public void RemoveCallback<T>(Action<T> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnClientReceiveOnce -= callback;
        }


        public void RemoveCallback<T>(Action<int, T, NetworkingPlayer> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnServerReceiveTwo -= callback;
        }

        public void RemoveCallback<T>(Action<T, NetworkingPlayer> callback) where T : new()
        {
            Type type = typeof(T);
            ProtoItem<T> item = (ProtoItem<T>)protoListListener.GetItemByType(type);
            item.OnServerReceiveOnce -= callback;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void OnMessage(ProtoMsg msg, NetworkingPlayer player)
        {
            IProtoItem item = protoListListener.GetItemByOpcode(msg.protoId);

            if(item != null)
                Loger.LogTag("Proto", "<= " +  item);
            else
                Loger.LogTag("Proto", "<= " + msg.protoId + "没找到对应的协议");

            if (item != null && item.hasListen)
            {
                item.Handle(msg.bytes, player);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage<T>(T protoMsg)
        {
            SendMessage<T>(protoMsg, null);
        }

        public void SendMessage<T>(T protoMsg, TcpClient client)
        {

            Type type = typeof(T);
            IProtoItem item = protoListSender.GetItemByType(type);
            if(item == null)
            {
                Loger.LogTag("Proto", "-> " + protoMsg + "没找到对应的协议");
                return;
            }

            Loger.LogTag("Proto", "-> " + item);

            ProtoMsg msg = ProtoToData<T>(protoMsg);

            baseTcp.SendToPlayer(client, msg);
        }

        public ProtoMsg ProtoToData<T>(T protoMsg)
        {
            Type type = typeof(T);
            IProtoItem item = protoListSender.GetItemByType(type);
            if (item == null)
            {
                Loger.LogTag("Proto", "ProtoToData " + protoMsg + "没找到对应的协议");
                return default(ProtoMsg);
            }


            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, protoMsg);
            stream.Position = 0;

            ProtoMsg msg = new ProtoMsg();
            msg.protoId = item.opcode;
            msg.bytes = stream.ToArray();

            stream.Dispose();

            return msg;
        }


    }
}
