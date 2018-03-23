using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 3:28:19 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    /// <summary>
    /// 场景
    /// </summary>
    public class RoomScene
    {

        /// <summary>
        /// 任何一种网络事件的基本代理
        /// </summary>
        public delegate void BaseNetworkEvent(RoomScene sender);

        /// <summary>
        /// 服务器接受这个客户端时发生
        /// </summary>
        public event BaseNetworkEvent serverAccepted;


        /// <summary>
        /// 当客户端从服务器异步获取属于此NetworkObject的ID时发生
        /// Occurs when a client get's an id from the server asynchronously that belongs to this NetworkObject
        /// </summary>
        public event NetworkObject.CreateEvent objectCreateAttach;

        /// <summary>
        /// 当网络对象已在网络上创建时发生。
        /// Occurs when a network object has been created on the network
        /// </summary>
        public event NetworkObject.NetworkObjectEvent objectCreated;

        /// <summary>
        /// TODO: COMMENT
        /// </summary>
        public event NetworkObject.CreateRequestEvent objectCreateRequested;

        /// <summary>
        /// TODO: COMMENT
        /// </summary>
        public event NetworkObject.NetworkObjectEvent factoryObjectCreated;


        internal bool ObjectCreatedRegistered { get { return objectCreated != null; } }

        /// <summary>
        /// 用于创建网络对象的工厂，只有1个工厂
        /// 任何网络应用程序都需要 （共享代码库）
        /// </summary>
        public INetworkObjectFactory Factory { get; set; }




        /// <summary>
        /// Socket
        /// </summary>
        public NetWorker Networker { get; protected set; }

        /// <summary>
        /// RoomStage
        /// </summary>
        public RoomStage RoomStage { get; protected set; }


        /// <summary>
        /// NetRoomBase
        /// </summary>
        public NetRoomBase Room { get; protected set; }

        /// <summary>
        /// 房间ID
        /// </summary>
        public ulong RoomUid { get; protected set; }


        /// <summary>
        /// 当前机器的玩家引用
        /// A player reference to the current machine
        /// </summary>
        public NetworkingPlayer Me { get { return Networker.Me; } }

        /// <summary>
        ///这个对象用来跟踪这个也是已知的联网者的时间
        ///作为这个系统中的“时间步骤”
        /// </ summary>
        public TimeManager Time { get { return Networker.Time; } }

        /// <summary>
        /// 是否是 服务器
        /// </summary>
        public bool IsServer { get; protected set; }

        /// <summary>
        /// 是否是 客户端
        /// </summary>
        public bool IsClient { get; protected set; }

        /// <summary>
        /// 是否是 TCPClient
        /// </summary>
        public bool IsTCPClient { get; protected set; }


        /// <summary>
        /// 是否是 TCP
        /// </summary>
        public bool IsTcp { get; protected set; }


        /// <summary>
        /// 是否是 UDPClient
        /// </summary>
        public bool IsUDPClient { get; protected set; }

        /// <summary>
        /// 是否是 UDPServer
        /// </summary>
        public bool IsUDPServer { get; protected set; }


        /// <summary>
        /// 用于确定这样的网络已被放弃
        /// </summary>
        public bool IsBound { get { return Networker.IsBound; } }

        /// <summary>
        /// 用于确定此NetWorker是否已被释放以避免重新连接
        /// </summary>
        public bool Disposed { get { return Networker.Disposed; } }


        /// <summary>
        /// 允许新创建的网络对象排队进行刷新调用
        /// Allows the newly created network object to be queued for the flush call
        /// </summary>
        public bool PendCreates { get; set; }

        internal int GlobalHash { get; set; }


        // 缺少对象缓冲区
        public Dictionary<uint, List<Action<NetworkObject>>> missingObjectBuffer = new Dictionary<uint, List<Action<NetworkObject>>>();


        /// <summary>
        /// 用于跟踪在网络上创建的所有NetworkObjects的静态列表
        /// A static list for tracking all of the NetworkObjects that have been created on the network
        /// </summary>
        private List<NetworkObject> networkObjects = new List<NetworkObject>();
        public  List<NetworkObject> NetworkObjects { get { return networkObjects; } }

        internal  List<NetworkObject> pendingCreates = new List<NetworkObject>();

        /// <summary>
        /// 由它的id索引的所有网络对象的字典
        /// </summary>
        public Dictionary<uint, NetworkObject> NetworkObjectDict { get; private set; }

        /// <summary>
        /// 所有网络对象的列表
        /// A list of all of the network objects
        /// </summary>
        public List<NetworkObject> NetworkObjectList { get; private set; }

        /// <summary>
        /// 用于为每个添加的网络对象赋予一个唯一的ID
        /// Used to give a unique id to each of the network objects that are added
        /// </summary>
        private uint currentNetworkObjectId = 0;


        public RoomScene(RoomStage roomStage)
        {
            this.RoomStage = roomStage;
            this.Networker = roomStage.Room.lobby.Socket;
            this.Room = roomStage.Room;
            this.RoomUid = Room.roomId;

            IsServer = Networker is IServer;
            IsClient = Networker is IClient;
            IsTcp = Networker is BaseTCP;
            IsTCPClient = Networker is TCPClient;
            IsUDPClient = Networker is UDPClient;
            IsUDPServer = Networker is UDPServer;


            NetworkObjectDict = new Dictionary<uint, NetworkObject>();
            NetworkObjectList = new List<NetworkObject>();

            NetworkInitialize();
        }

        public void Dispance()
        {

        }

        /// <summary>
        /// 客户端被服务器接受
        /// 从子类调用serverAccepted事件的包装器
        /// A wrapper around calling the serverAccepted event from child classes
        /// </summary>
        protected void OnServerAccepted()
        {
            if (serverAccepted != null)
                serverAccepted(this);
        }

        public virtual void Send(FrameStream frame, bool reliable = false)
        {
            if (Room is NetRoomServer)
                ((NetRoomServer)Room).Send(frame, reliable);
            else
                ((NetRoomClient)Room).Send(frame, reliable);
        }


        public virtual void Send(FrameStream frame, bool reliable = false, NetworkingPlayer skipPlayer = null)
        {
            if (Room is NetRoomServer)
                ((NetRoomServer)Room).Send(frame, reliable, skipPlayer);
            else
                ((NetRoomClient)Room).Send(frame, reliable);
        }

        public virtual void ClientSend(FrameStream frame, bool reliable = false)
        {
            ((NetRoomClient)Room).Send(frame, reliable);
        }


        public virtual void ServerSend(FrameStream clientFrame, FrameStream frame, bool reliable = false)
        {
            ((NetRoomServer)Room).Send(clientFrame.Sender, frame, reliable);
        }


        public virtual void ServerSend(NetworkingPlayer player, FrameStream frame, bool reliable = false)
        {
            ((NetRoomServer)Room).Send(player, frame, reliable);
        }


        public virtual void ServerSendAll(FrameStream frame, bool reliable = false, NetworkingPlayer skipPlayer = null)
        {
            ((NetRoomServer)Room).Send(frame, reliable, skipPlayer);
        }



        internal void OnObjectCreated(NetworkObject target)
        {
            if (objectCreated != null)
                objectCreated(target);
        }

        internal void OnObjectCreateAttach(int identity, int classId, int hash, uint id, FrameStream frame)
        {
            if (objectCreateAttach != null)
                objectCreateAttach(identity, classId, hash, id, frame);
        }

        internal void OnObjectCreateRequested(int identity, int classId, uint id, FrameStream frame, Action<NetworkObject> callback)
        {
            if (objectCreateRequested != null)
                objectCreateRequested(this, identity, classId, id, frame, callback);
        }

        internal void OnFactoryObjectCreated(NetworkObject obj)
        {
            if (factoryObjectCreated != null)
                factoryObjectCreated(obj);
        }

        /// <summary>
        /// 玩家可以调用的接受的事件调用的包装器
        /// A wrapper for the playerAccepted event call that chindren of this can call
        /// </summary>
        internal void OnPlayerAccepted(NetworkingPlayer player)
        {

            NetworkObject[] currentObjects;
            lock (NetworkObjectDict)
            {
                currentObjects = NetworkObjectDict.Values.ToArray();
            }
            // 服务器发送目前所有的网络对象给他
            NetworkObject.PlayerAccepted(player, currentObjects);
        }

        internal void DestoryPlayerNetworkObjects(NetworkingPlayer player)
        {

            List<NetworkObject> list = new List<NetworkObject>();
            lock (NetworkObjectDict)
            {
                NetworkObject[] currentObjects;
                currentObjects = NetworkObjectDict.Values.ToArray();

                for (int i = 0; i < currentObjects.Length; i++)
                {
                    if (currentObjects[i].Owner == player)
                        list.Add(currentObjects[i]);
                }
            }

            for(int i = 0; i < list.Count; i ++)
            {
                list[i].Destroy();
            }
            list.Clear();


        }


        /// <summary>
        ///在收到创建NetworkObject的消息时由NetWorker调用
        /// </ summary>
        /// <param name =“networker”>接收到消息以创建网络对象的网络接口</ param>
        /// <param name =“player”>请求创建该对象的玩家</ param>
        /// <param name =“frame”>要初始化对象的数据</ param>
        public void CreateNetworkObject( NetworkingPlayer player, Binary frame)
        {
            //获取身份，以便可以选择正确的类型/子类型
            // Get the identity so that the proper type / subtype can be selected
            int identity = frame.StreamData.GetBasicType<int>();
            int classId = frame.StreamData.GetBasicType<int>();

            if (IsServer)
            {
                //客户端正在请求创建一个新的联网对象
                // The client is requesting to create a new networked object
                if (Factory != null)
                {
                    Factory.NetworkCreateObject(this, identity, classId, 0, frame, (obj) =>
                    {
                        networkObjects.Add(obj);
                    });
                }
            }
            else if (IsClient)
            {
                int hash = frame.StreamData.GetBasicType<int>();

                //获取为该网络对象分配的服务器ID
                // Get the server assigned id for this network object
                uint id = frame.StreamData.GetBasicType<uint>();

                Loger.LogFormat("RoomScene Client Read hash={0}, id={1}", hash, id);

                if (hash != 0)
                {
                    //服务器正在响应创建请求
                    // The server is responding to the create request
                    OnObjectCreateAttach(identity, classId, hash, id, frame);
                    return;
                }

                OnObjectCreateRequested(identity, classId, id, frame, (obj) =>
                {
                    if (obj != null)
                        networkObjects.Add(obj);
                });

                //服务器要求创建一个新的联网对象
                // The server is dictating to create a new networked object
                if (Factory != null)
                {
                    Factory.NetworkCreateObject(this, identity, classId, id, frame, (obj) =>
                    {
                        networkObjects.Add(obj);
                        OnFactoryObjectCreated(obj);
                    });
                }
            }
        }

        public void CreateMultiNetworkObject(NetworkingPlayer player, Binary frame)
        {
            int index, count = frame.StreamData.GetBasicType<int>();
            int head = frame.StreamData.StartIndex();

            for (int i = 0; i < count; i++)
            {
                //返回头部，然后前进到下一个索引
                // Return to the head and then move forward to the next index
                frame.StreamData.MoveStartIndex(-frame.StreamData.StartIndex() + i * sizeof(int) + head);
                index = frame.StreamData.GetBasicType<int>(false);

                //移到主有效载荷开始的计数末尾
                // Move to the end of the count where the main payload starts
                frame.StreamData.MoveStartIndex((count - i) * sizeof(int));

                //移到有效载荷指定的索引
                // Move to the index specified by the payload
                frame.StreamData.MoveStartIndex(index);

                //为这个对象创建一个隔离的框架
                // Create an isolated frame for this object
                Binary subFrame = (Binary)frame.Clone();
                CreateNetworkObject( player, subFrame);
            }
        }


        public void ClearNetworkObjects(NetWorker networker)
        {
            NetworkObject[] targets = networkObjects.Where(n => n.Networker == this).ToArray();
            NetworkObject[] pendingTargets = pendingCreates.Where(n => n.Networker == this).ToArray();

            for (int i = 0; i < targets.Length; i++)
                networkObjects.Remove(targets[i]);

            for (int i = 0; i < pendingTargets.Length; i++)
                pendingCreates.Remove(pendingTargets[i]);
        }

        /// <summary>
        ///注册一个网络对象与这个网络, 如果forceId是0，会分配一个ID
        /// </ summary>
        /// <param name =“networkObject”>要使用此联网器注册的对象</ param>
        /// <returns> <c> true </ c>如果对象已经成功注册，否则<c> false </ c>已注册</ returns>
        public bool RegisterNetworkObject(NetworkObject networkObject, uint forceId = 0)
        {
            uint id = currentNetworkObjectId;

            lock (NetworkObjectDict)
            {
                // If we are forcing this object
                if (forceId != 0)
                {
                    if (NetworkObjectDict.ContainsKey(forceId))
                        return false;

                    id = forceId;

                    if (!networkObject.RegisterOnce(id))
                        throw new BaseNetworkException("The supplied network object has already been assigned to a networker and has an id");

                    //AddNetworkObject(forceId, networkObject);
                    //NetworkObjectList.Add(networkObject);
                }
                else
                {
                    do
                    {
                        if (NetworkObjectDict.ContainsKey(++currentNetworkObjectId))
                            continue;

                        if (!networkObject.RegisterOnce(currentNetworkObjectId))
                        {
                            //返回，因为下次调用这个方法会在检查之前增加
                            // Backtrack since the next call to this method will increment before checking
                            currentNetworkObjectId--;

                            throw new BaseNetworkException("The supplied network object has already been assigned to a networker and has an id");
                        }

                        //AddNetworkObject(currentNetworkObjectId, networkObject);
                        //NetworkObjectList.Add(networkObject);
                        break;
                    } while (IsBound);
                }
            }

            //将网络ID分配给网络对象
            // Assign the network id to the network object
            networkObject.RegisterOnce(id);

            // When this object is destroyed it needs to remove itself from the list
            networkObject.onDestroy += (RoomScene sender) =>
            {
                lock (NetworkObjectDict)
                {
                    NetworkObjectDict.Remove(networkObject.NetworkId);
                    NetworkObjectList.Remove(networkObject);
                }
            };

            return true;
        }


        /// <summary>
        /// 网络对象注册完成
        /// </summary>
        /// <param name="networkObject">网络对象</param>
        public void CompleteInitialization(NetworkObject networkObject)
        {
            lock (NetworkObjectDict)
            {
                if (NetworkObjectDict.ContainsKey(networkObject.NetworkId))
                    return;

                NetworkObjectDict.Add(networkObject.NetworkId, networkObject);
                NetworkObjectList.Add(networkObject);
            }
        }


        /// <summary>
        /// 网络对象注册完成时 调用
        /// missingObjectBuffer 保存的是该对象在还没被创建完成时，先接受到远程调用的方法 挂起。
        /// 等网络对象注册完成就调之前挂起时没处理的方法
        /// </summary>
        /// <param name="networkObject"></param>
        public void FlushCreateActions(NetworkObject networkObject)
        {
            List<Action<NetworkObject>> actions = null;
            lock (missingObjectBuffer)
            {
                missingObjectBuffer.TryGetValue(networkObject.NetworkId, out actions);
                missingObjectBuffer.Remove(networkObject.NetworkId);
            }

            if (actions == null)
                return;

            foreach (var action in actions)
                action(networkObject);
        }


        /// <summary>
        /// 一旦网络连接被绑定，就被调用
        /// 启动一个任务，刷新网络对象的属性
        /// Called once the network connection has been bound
        /// </summary>
        protected virtual void NetworkInitialize()
        {
            Task.Queue(() =>
            {
                while (IsBound)
                {
                    ulong step = Time.Timestep;
                    lock (NetworkObjectDict)
                    {
                        foreach (NetworkObject obj in NetworkObjectDict.Values)
                        {
                            // Only do the heartbeat (update) on network objects that
                            // are owned by the current networker
                            if ((obj.IsOwner && obj.UpdateInterval > 0) || (IsServer && obj.AuthorityUpdateMode))
                                obj.HeartBeat(step);
                        }
                    }

                    Thread.Sleep(10);
                }
            });
        }


        /// <summary>
        /// 这个可以调用的messageReceived事件调用的包装器
        /// A wrapper for the messageReceived event call that chindren of this can call
        /// </summary>
        public void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame)
        {
            byte routerId = frame.RouterId;
            if (routerId == RouterIds.RPC_ROUTER_ID || routerId == RouterIds.BINARY_DATA_ROUTER_ID || routerId == RouterIds.CREATED_OBJECT_ROUTER_ID)
            {
                uint id = frame.StreamData.GetBasicType<uint>();
                NetworkObject targetObject = null;

                lock (NetworkObjectDict)
                {
                    NetworkObjectDict.TryGetValue(id, out targetObject);
                }

                if (targetObject == null)
                {
                    // 收到该网络对象的消息包
                    // 但是该玩家机器上还没有创建网络对象
                    // 就将该消息缓存器来，等待网络对象创建完再掉用
                    lock (missingObjectBuffer)
                    {
                        if (!missingObjectBuffer.ContainsKey(id))
                            missingObjectBuffer.Add(id, new List<Action<NetworkObject>>());

                        missingObjectBuffer[id].Add((networkObject) =>
                        {
                            ExecuteRouterAction(routerId, networkObject, (Binary)frame, player);
                        });
                    }

                    // TODO:  If the server is missing an object, it should have a timed buffer
                    // that way useless messages are not setting around in memory
                    // TODO：如果服务器缺少一个对象，它应该有一个定时缓冲区
                    //这种无用的消息不会在内存中设置

                    return;
                }

                ExecuteRouterAction(routerId, targetObject, (Binary)frame, player);
            }
            // 创建网络对象
            else if (routerId == RouterIds.NETWORK_OBJECT_ROUTER_ID)
            {
                CreateNetworkObject(player, (Binary)frame);
            }
            // 在服务器接受客户端时，将服务器现有的所有网络对象 发给该玩家，让他创建
            else if (routerId == RouterIds.ACCEPT_MULTI_ROUTER_ID)
                CreateMultiNetworkObject(player, (Binary)frame);
        }

        private void ExecuteRouterAction(byte routerId, NetworkObject networkObject, Binary frame, NetworkingPlayer player)
        {
            // 执行RPC函数
            if (routerId == RouterIds.RPC_ROUTER_ID)
                networkObject.InvokeRpc(player, frame.TimeStep, frame.StreamData, frame.Receivers);
            else if (routerId == RouterIds.BINARY_DATA_ROUTER_ID)
                networkObject.ReadBinaryData(frame);
            else if (routerId == RouterIds.CREATED_OBJECT_ROUTER_ID)
                networkObject.CreateConfirmed(player);
        }


        public void FixedUpdate()
        {
            for (int i = 0; i < NetworkObjectList.Count; i++)
                NetworkObjectList[i].InterpolateUpdate();
        }

    }
}
