using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 4:08:16 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    /// <summary>
    /// The representation of an object on the network, this object can have
    /// properties that it serializes and RPC (remote procedure calls)
    /// 网络上对象的表示，这个对象可以有
    /// 序列化的属性和RPC（远程过程调用）
    /// </summary>
    public abstract class NetworkObject
    {
        // 子路由 -- 序列化字段
        protected const byte DIRTY_FIELD_SUB_ROUTER_ID = 1;
        // 子路由 -- 销毁
        protected const byte DESTROY_SUB_ROUTER_ID = 2;

        // 设置UpdateInterval的默认值， HeartBeat用于更新广播给其他玩家的Dirty值
        private const ulong DEFAULT_UPDATE_INTERVAL = 100;
        // RPC行为覆盖
        private const byte RPC_BEHAVIOR_OVERWRITE = 0x1;

        public const byte RPC_CLEAR_RPC_BUFFER = 0;
        public const byte RPC_REMOVE_RPC_FROM_BUFFER = 1;

        // 采纳分配所有权
        public const byte RPC_TAKE_OWNERSHIP = 2;
        // 分配所有权
        public const byte RPC_ASSIGN_OWNERSHIP = 3;

        /// <summary>
        /// A generic delegate for events to fire off while passing a NetworkObject source
        /// </summary>
        /// <param name="networkObject">The object source for this event</param>
        /// <summary>
        ///传递一个NetworkObject源的事件通用委托
        /// </ summary>
        /// <param name =“networkObject”>这个事件的对象来源</ param>
        public delegate void NetworkObjectEvent(NetworkObject networkObject);

        /// <summary>
        /// Used to create events that require BMSByte data
        /// </summary>
        /// <param name="data">The data that was read</param>
        /// <summary>
        ///用于创建需要BMSByte数据的事件
        /// </ summary>
        /// <param name =“data”>读取的数据</ param>
        public delegate void BinaryDataEvent(BMSByte data);

        /// <summary>
        /// Used for when an object is created on the network
        /// </summary>
        /// <param name="identity">The identity used to know what type of network object this is</param>
        /// <param name="hash">The hash id (if sent) to match up client created objects with the ids that the server will respond with asynchronously</param>
        /// <param name="id">The id for this network object</param>
        /// <param name="frame">The frame data for this object's creation (default values)</param>
        /// <summary>
        ///在网络上创建对象时使用
        /// </ summary>
        /// <param name =“identity”>标识用于知道这是什么类型的网络对象</ param>
        /// <param name =“hash”>哈希ID（如果发送）匹配客户端创建的对象与服务器将异步响应的ID </ param>
        /// <param name =“id”>这个网络对象的id </ param>
        /// <param name =“frame”>该对象创建的帧数据（默认值）</ param>
        public delegate void CreateEvent(int identity, int hash, uint id, FrameStream frame);

        /// <summary>
        /// Used for when any field event occurs, will pass the target field as a param
        /// </summary>
        /// <typeparam name="T">The acceptable network serializable data type</typeparam>
        /// <param name="field">The target field related to this event</param>
        /// <param name="timestep">The timestep for when this event happens</param>
        /// <summary>
        ///用于发生任何字段事件时，将作为参数传递目标字段
        /// </ summary>
        /// <typeparam name =“T”>可接受的网络可序列化数据类型</ typeparam>
        /// <param name =“field”>与此事件相关的目标字段</ param>
        /// <param name =“timestep”>发生此事件的时间步</ param>
        public delegate void FieldEvent<T>(T field, ulong timestep);

        /// <summary>
        /// Used for when any specific field change occurs, will pass the name of the field and the value
        /// You are encouraged to used this event for debugging only and use the explicit events
        /// during production
        /// </summary>
        /// <param name="fieldName">The name of the field that is being changed</param>
        /// <param name="value">The value of the changed filed</param>
        /// <param name="timestep">The timestep for when this event happens</param>
        /// <summary>
        ///用于发生任何特定字段更改时，将传递字段的名称和值
        ///鼓励您仅使用此事件进行调试，并使用显式事件
        ///在生产过程中
        /// </ summary>
        /// <param name =“fieldName”>正在更改的字段的名称</ param>
        /// <param name =“value”>已更改字段的值</ param>
        /// <param name =“timestep”>发生此事件的时间步</ param>
        public delegate void FieldChangedEvent(string fieldName, object value, ulong timestep);

        /// <summary>
        /// TODO: COMMENT THIS
        /// </summary>
        /// <param name="networker"></param>
        /// <param name="identity"></param>
        /// <param name="id"></param>
        /// <param name="frame"></param>
        /// <param name="callback"></param>
        public delegate void CreateRequestEvent(RoomScene networker, int identity, uint id, FrameStream frame, Action<NetworkObject> callback);

        /// <summary>
        /// 每当这个networkobject有其拥有的玩家改变了
        /// Called whenever this NetworkObject has its owning player changed
        /// </summary>
        public event RoomScene.BaseNetworkEvent ownershipChanged;

        /// <summary>
        /// 在网络上接收到此对象的二进制消息并需要读取时发生
        /// Occurs when a binary message was received on the network for this object and is needed to be read
        /// </summary>
        public event BinaryDataEvent readBinary;


        /// <summary>
        /// 在网络上设置并准备好此对象时发生
        /// Occurs when this object is setup and ready on the network
        /// </summary>
        public event RoomScene.BaseNetworkEvent onReady;
        public event RoomScene.BaseNetworkEvent onDestroy;

        /// <summary>
        /// 当前网络连接器上此对象的唯一标识
        /// The unique id for this object on the current networker
        /// </summary>
        public uint NetworkId { get; private set; }

        /// <summary>
        /// A refrerence to the networker that this network object is attached to
        /// 这个网络对象所连接到的网络的引用
        /// </summary>
        public RoomScene Networker { get; private set; }

        /// <summary>
        /// 获得当前玩家ID的助手（Networker.Me.NetworkId）
        /// A helper to get the current players's id (Networker.Me.NetworkId)
        /// </summary>
        public uint MyPlayerId { get { return Networker.Me.NetworkId; } }

        /// <summary>
        /// 如果当前NetWorker是此NetworkObject的所有者，则返回<c> true </ c>
        /// Returns <c>true</c> if the current NetWorker is the owner of this NetworkObject
        /// </summary>
        public bool IsOwner { get; private set; }

        /// <summary>
        /// If set to true on the server, the server can change the value of the properties
        /// of a network object. BEWARE, this can cause race conditions in data transfer, so
        /// only use as a last resort
        ///如果在服务器上设置为true，则服务器可以更改属性的值
        ///网络对象。 注意，这可能会导致数据传输中的竞争条件，所以
        ///只能作为最后的手段使用
        /// </summary>
        public bool AuthorityUpdateMode { get; set; }

        /// <summary>
        /// 如果这个设置为true，那么这个网络对象的字段只会被发送
        /// 通过邻近所有; 这个值可以在运行时改变
        /// If this is set to true then the fields for this network object will only be sent
        /// via proximity all; this value can be changed at runtime
        /// </summary>
        public bool ProximityBasedFields { get; set; }

        /// <summary>
        /// 这个网络对象可用的所有RPC的查找表
        /// A lookup table for all of the RPC's that are available to this network object
        /// </summary>
        public Dictionary<byte, Rpc> Rpcs { get; private set; }

        /// <summary>
        /// 这是从方法名称到它在Rpcs字典中的id的映射
        /// This is a mapping from the method name to the id that it is within the Rpcs dictionary
        /// </summary>
        protected Dictionary<string, byte> rpcLookup = new Dictionary<string, byte>();

        /// <summary>
        /// 用于将RPC ID转换为与其关联的字符串名称
        /// Used to convert from an RPC id to the string name associated with it
        /// </summary>
        protected Dictionary<byte, string> inverseRpcLookup = new Dictionary<byte, string>();

        /// <summary>
        /// 是<c> true </ c>，如果这个对象已经完全在网络上设置的话
        /// Is <c>true</c> if this object has been fully setup on the network
        /// </summary>
        public bool NetworkReady { get; private set; }

        // TODO：确保这些不会冲突
        // TODO:  Make sure that these do not collide
        /// <summary>
        /// The temporary hash that was sent from a client to this server, or the hash that a client is using
        /// to identify the attach method to this particular object when a server sends the id
        /// 从客户端发送到此服务器的临时哈希，或客户端正在使用的哈希
        /// 在服务器发送id时识别这个特定对象的附加方法
        /// </summary>
        private int hash = 0;

        public int CreateCode = 0;

        public int GlobalHash { get { return Networker.GlobalHash; } set { Networker.GlobalHash = value; } }


        /// <summary>
        /// 用于确定上次该对象已被更新的时间
        /// Used to determine the last time this object has been updated
        /// </summary>
        private ulong lastUpdateTimestep = 0;

        /// <summary>
        /// 用于标识这是什么类型（子类型）的对象
        /// Used to identify what type (subtype) of object this is
        /// </summary>
        public abstract int UniqueIdentity { get; }

        /// <summary>
        /// 创建这个对象的时间步
        /// The timestep that this object was created in
        /// </summary>
        protected ulong CreateTimestep { get; private set; }

        /// <summary>
        /// 此对象的每个更新之间的时间（以毫秒为单位）
        /// The time in milliseconds between each update for this object
        /// </summary>
        public ulong UpdateInterval { get; set; }
        protected bool hasDirtyFields = false;

        /// <summary>
        /// 创建这个网络对象的玩家的引用
        /// A reference to the player who created this network object
        /// </summary>
        public NetworkingPlayer Owner { get; private set; }

        public byte[] Metadata { get; private set; }

        /// <summary>
        /// 用于存储缓冲的rpc数据的结构将在接受的客户端上发送
        /// The structure to store the buffered rpc data in to be sent on accepted client
        /// </summary>
        protected struct BufferedRpc
        {
            public BMSByte data;
            public Receivers receivers;
            public byte methodId;
            public ulong timestep;
        }

        /// <summary>
        /// 存储此特定网络对象的缓冲RPC调用的列表
        /// Stores a list of buffered RPC calls for this particular Network Object
        /// </summary>
        protected List<BufferedRpc> rpcBuffer = new List<BufferedRpc>();

        /// <summary>
        /// 一旦客户端完成了注册过程，它将被设置为true
        ///，并准备开始接受已注册的RPC调用
        /// This is set to true once the client has completed it's registration process
        /// and is ready to start accepting registered RPC calls
        /// </summary>
        public bool ClientRegistered { get; private set; }

        /// <summary>
        /// 用来在网络上写脏字段而不用
        /// 每次创建一个新的byte[]
        /// Used to write dirty fields across the network without having
        /// to create a new byte[] every time
        /// </summary>
        protected BMSByte dirtyFieldsData = new BMSByte();

        /// <summary>
        /// Used to read the flags on the network, this is cached on creation
        /// so that it doesn't have to be created over and over
        /// 用于读取网络上的标志，这在创建时被缓存
        ///，这样就不必一遍又一遍地创建
        /// </summary>
        protected byte[] readDirtyFlags = null;

        /// <summary>
        /// ///这是SendBinaryData方法的缓存二进制数据byte[]
        /// This is the cached binary data byte[] for the SendBinaryData method
        /// </summary>
        private BMSByte sendBinaryData = new BMSByte();

        public bool IsServer { get { return Networker.IsServer; } }

        private Dictionary<NetworkingPlayer, int> currentRpcBufferCounts = new Dictionary<NetworkingPlayer, int>();

        /// <summary>
        /// 挂起的Rpc方法将被映射到的结构
        /// The struct that the pending Rpc methods will be mapped to
        /// </summary>
        private struct PendingRpc
        {
            public BMSByte data;
            public Receivers receivers;
            public NetworkingPlayer sender;
            public ulong timestep;
        }

        /// <summary>
        /// 这是准备就绪之前本地未决rpc的结构
        /// This is the struct for local pending rpc's before ready
        /// </summary>
        private struct PendingLocalRPC
        {
            public NetworkingPlayer TargetPlayer;
            public byte MethodId;
            public Receivers Receivers;
            public object[] Args;

            public override string ToString()
            {
                return string.Format("P [{0}], M [{1}], R [{2}], A [{3}]", TargetPlayer, MethodId, Receivers, Args.Length);
            }
        }


        /// <summary>
        /// 一旦客户端连接，需要执行的挂起Rpc列表
        /// The list of pending Rpc that will need to be executed once the client connects
        /// </summary>
        private List<PendingRpc> pendingClientRegisterRpc = new List<PendingRpc>();

        /// <summary>
        /// 这是客户端本地发送的待处理本地rpcs的列表
        /// This is a list of pending local rpcs to be send locally by the client
        /// </summary>
        private List<PendingLocalRPC> pendingLocalRpcs = new List<PendingLocalRPC>();

        /// <summary>
        /// TODO 基本没用到
        ///这个构造函数用来创建一个虚拟的网络对象
        ///在网络上不做任何事情，并且是暂时的
        ///直到实际的网络对象到达
        /// This constructor is used to create a dummy network object that
        /// doesn't do anything on the network and is useful to be temporary
        /// until the actual network object arrives
        /// </summary>
        public NetworkObject() { IsOwner = true; NetworkReady = false; }

        /// <summary>
        /// This creates an instance of this object and attaches it to the specified networker
        /// </summary>
        /// <param name="networker">The networker that this object is going to be attached to</param>
        /// <param name="forceId">If 0 then the first open id will be used from the networker</param>
        /// <summary>
        /// (主动创建, 先创建行为在创建对象) 这将创建此对象的一个实例并将其附加到指定的网络连接器
        /// </ summary>
        /// <param name =“networker”>这个对象将被附加到的网络工具</ param>
        /// <param name =“forceId”>如果是0，则第一个打开的id将会从网络中使用</ param>
        public NetworkObject(RoomScene networker, int createCode = 0, byte[] metadata = null)
        {
            UpdateInterval = DEFAULT_UPDATE_INTERVAL;
            CreateCode = createCode;

            Rpcs = new Dictionary<byte, Rpc>();
            Networker = networker;
            CreateNativeRpcs();

            // 调这个方法的就是拥有者
            // Whatever called this method is the owner
            Owner = networker.Me;
            IsOwner = true;
            Metadata = metadata;

            if (networker.IsServer)
                CreateObjectOnServer(null);
            else
            {
                //这是一个客户端，所以需要通过服务器请求创建
                // This is a client so it needs to request the creation by the server
                NetworkReady = false;

                //为这个对象创建一个散列，以便知道服务器的响应是
                //对于这个特定的创建，而不是另一个
                // Create a hash for this object so it knows that the response from the server is
                // for this particular create and not another one
                hash = GlobalHash == -1 ? (GlobalHash += 2) : ++GlobalHash;

                CreateCode = createCode;

                //告诉这个对象侦听来自服务器的创建网络对象消息
                // Tell this object to listen for the create network object message from the server
                Networker.objectCreateAttach += CreatedOnNetwork;
                //TODO: MOVED HERE (#1)

                BMSByte data = ObjectMapper.BMSByte(UniqueIdentity, hash, CreateCode);
                WritePayload(data);

                // Write if the object has metadata
                ObjectMapper.Instance.MapBytes(data, Metadata != null);
                if (Metadata != null)
                    ObjectMapper.Instance.MapBytes(data, Metadata);

                bool useMask = networker.IsTCPClient;
                Binary createRequest = new Binary(CreateTimestep, useMask, data, Receivers.Server, MessageGroupIds.CREATE_NETWORK_OBJECT_REQUEST, networker.IsTcp, RouterIds.NETWORK_OBJECT_ROUTER_ID, Networker.RoomUid);

                RoomScene.BaseNetworkEvent request = (RoomScene sender) =>
                {
                    sender.ClientSend(createRequest, true);
                };

                if (Networker.Me == null)
                    Networker.serverAccepted += request;
                else
                    request(networker);

                //TODO: FROM HERE (#1)
            }
        }

        /// <summary>
        /// Create an instance of a network object from the network
        /// </summary>
        /// <param name="networker">The NetWorker that is managing this network object</param>
        /// <param name="serverId">The id (if any) given to this object by the server</param>
        /// <param name="frame">The initialization data for this object that is assigned from the server</param>
        /// <summary>
        /// 从网络创建一个网络对象的实例
        /// （服务器， 接收到客户端要创建网络对象）
        /// </ summary>
        /// <param name =“networker”>管理此网络对象的NetWorker </ param>
        /// <param name =“serverId”>服务器给这个对象的id（如果有的话）</ param>
        /// <param name =“frame”>从服务器分配的这个对象的初始化数据</ param>
        public NetworkObject(RoomScene networker, uint serverId, FrameStream frame)
        {
            UpdateInterval = DEFAULT_UPDATE_INTERVAL;

            Rpcs = new Dictionary<byte, Rpc>();
            Networker = networker;

            if (Networker.IsServer)
                Owner = frame.Sender;
            else
                Owner = ((IClient)Networker.Networker).Server;

            CreateNativeRpcs();

            if (networker.IsServer)
            {
                //读取由客户端创建的哈希代码，以便可以中继查找
                // Read the hash code that was created by the client so that it can be relayed back for lookup
                hash = frame.StreamData.GetBasicType<int>();
                CreateCode = frame.StreamData.GetBasicType<int>();

                ReadPayload(frame.StreamData, frame.TimeStep);

                if (frame.StreamData.GetBasicType<bool>())
                    Metadata = ObjectMapper.Instance.Map<byte[]>(frame.StreamData);

                // 让所有客户知道正在创建一个新的对象, 跳过发起创建的客户端
                // Let all the clients know that a new object is being created
                CreateObjectOnServer(frame.Sender);

                // 通知发起的客户端
                Binary createObject = CreateObjectOnServer(frame.Sender, hash);

                //将消息发送回发送客户端，以便完成网络对象的设置
                // Send the message back to the sending client so that it can finish setting up the network object
                networker.ServerSend(frame, createObject, true);
            }
            else
            {
                CreateCode = frame.StreamData.GetBasicType<int>();

                Initialize(serverId, frame.TimeStep);
                ReadPayload(frame.StreamData, frame.TimeStep);

                if (frame.StreamData.GetBasicType<bool>())
                    Metadata = ObjectMapper.Instance.Map<byte[]>(frame.StreamData);

                BMSByte createdByteData = ObjectMapper.BMSByte(serverId);

                Binary createdFrame = new Binary(Networker.Time.Timestep, Networker.IsTCPClient, createdByteData, Receivers.Server, MessageGroupIds.GetId("NO_CREATED_" + NetworkId), Networker.IsTcp, RouterIds.CREATED_OBJECT_ROUTER_ID, Networker.RoomUid);

                networker.ClientSend(createdFrame, true);
            }
        }

        /// <summary>
        /// Go through and setup all of the RPCs that are a base part
        /// of all network objects
        /// 浏览并设置所有基本部分的RPC
        /// 所有网络对象
        /// </summary>
        private void CreateNativeRpcs()
        {
            RegisterRpc("ClearRpcBuffer", ClearRpcBuffer);
            RegisterRpc("RemoveRpcFromBuffer", RemoveRpcFromBuffer);
            RegisterRpc("TakeOwnership", TakeOwnership);
            RegisterRpc("AssignOwnership", AssignOwnership, typeof(bool));
        }

        /// <summary>
        /// 清除此网络对象的所有缓冲rpcs
        /// Clear all of the buffered rpcs for this network object
        /// </summary>
        public void ClearRpcBuffer()
        {
            SendRpc(RPC_CLEAR_RPC_BUFFER, Receivers.Server);
        }

        /// <summary>
        /// Allows you to remove all buffered rpcs with the given method name or
        /// just the first occurance (oldest one)
        /// </summary>
        /// <param name="methodName">The name to match and remove from the buffer</param>
        /// <param name="first">If <c>True</c> then only the first buffered rpc with the specified name will be removed</param>
        /// <summary>
        ///允许您使用给定的方法名称或删除所有缓冲的rpcs
        ///只是第一次出现（最老的一次）
        /// </ summary>
        /// <param name =“methodName”>匹配并从缓冲区中移除的名称</ param>
        /// <param name =“first”>如果<c> True </ c>，那么只有具有指定名称的第一个缓冲rpc将被删除</ param>
        private void RemoveRpcFromBuffer(string methodName, bool first = false)
        {
            SendRpc(RPC_REMOVE_RPC_FROM_BUFFER, Receivers.Server, methodName, first);
        }

        // 采纳分配所有权
        public void TakeOwnership()
        {
            SendRpc(RPC_TAKE_OWNERSHIP, Receivers.Server);
        }

        // 分配所有权
        public void AssignOwnership(NetworkingPlayer targetPlayer)
        {
            // Only the server is allowed to assign ownership
            if (!IsServer)
                return;

            if (Owner == targetPlayer)
                return;

            if (targetPlayer == Networker.Me)
                AssignOwnership(new RpcArgs { Args = new object[] { true } });
            else
                SendRpc(targetPlayer, RPC_ASSIGN_OWNERSHIP, true);

            if (Owner == Networker.Me)
                AssignOwnership(new RpcArgs { Args = new object[] { false } });
            else
                SendRpc(Owner, RPC_ASSIGN_OWNERSHIP, false);

            Owner = targetPlayer;
        }

        private void TakeOwnership(RpcArgs args)
        {
            if (!IsServer)
                return;

            if (!AllowOwnershipChange(args.Info.SendingPlayer))
                return;

            AssignOwnership(args.Info.SendingPlayer);
        }

        private void AssignOwnership(RpcArgs args)
        {
            //在旧所有者上调用所有权更改事件
            //call the ownership changed event on old owner

            if (!IsOwner)
                OwnershipChanged();

            IsOwner = args.GetNext<bool>();

            //呼叫新所有者的所有权事件
            //call ownership event on new owner
            if (!IsOwner)
                OwnershipChanged();
        }

        protected virtual void OwnershipChanged()
        {
            if (ownershipChanged != null)
                ownershipChanged(Networker);
        }

        /// <summary>
        /// 清除此网络对象的所有缓冲rpcs
        /// Clear all of the buffered rpcs for this network object
        /// </summary>
        private void ClearRpcBuffer(RpcArgs args)
        {
            //只允许对象的服务器或所有者清除缓冲区
            // Only allow the server or owner of the object to clear the buffer
            if (!IsServer && args.Info.SendingPlayer != Owner)
                return;

            lock (rpcBuffer)
            {
                rpcBuffer.Clear();
            }
        }

        /// <summary>
        /// Allows you to remove all buffered rpcs with the given method name or
        /// just the first occurance (oldest one)
        /// </summary>
        private void RemoveRpcFromBuffer(RpcArgs args)
        {
            // Only allow the server or owner of the object to remove from the buffer
            if (!IsServer && args.Info.SendingPlayer != Owner)
                return;

            string methodName = args.GetNext<string>();
            bool first = args.GetNext<bool>();

            // TODO:  If this is the server it should warn about invalid id
            byte rpcId;
            if (!rpcLookup.TryGetValue(methodName, out rpcId))
                return;

            lock (rpcBuffer)
            {
                for (int i = 0; i < rpcBuffer.Count; i++)
                {
                    if (rpcBuffer[i].methodId == rpcId)
                    {
                        rpcBuffer.RemoveAt(i--);

                        if (first)
                            break;
                        else
                            continue;
                    }
                }
            }
        }

        /// <summary>
        /// A method for creating the network object on the server only and skipping any particular player
        /// which is often the player that is requesting that this object is created
        /// </summary>
        /// <param name="skipPlayer">The player to be skipped</param>
        /// <returns>The Binary frame data with all of the initialization data</returns>
        /// <summary>
        ///仅在服务器上创建网络对象并跳过任何特定播放器的方法
        ///这通常是请求创建这个对象的玩家
        /// </ summary>
        /// <param name =“skipPlayer”>要跳过的玩家</ param>
        /// <returns>具有所有初始化数据的二进制帧数据</ returns>
        private Binary CreateObjectOnServer(NetworkingPlayer skipPlayer, int targetHash = 0)
        {
            UpdateInterval = DEFAULT_UPDATE_INTERVAL;

            // targetHash == 0 时， 服务器会生成NetworkId，并注册到网络对象列表， 而且还会通知所有客户端创建网络对象
            // targetHash !=0 时， 服务器只是生成信息，将NetworkId反馈给该发起的客户端

            //如果有一个目标散列，这个对象已经被初始化
            // If there is a target hash, this object has already been initialized
            if (targetHash == 0)
            {
                //注册网络对象
                // Register the network object
                Initialize();
            }

            //要发送给所有未请求创建该对象的客户端的数据
            // The data that is to be sent to all the clients who did not request this object to be created
            BMSByte data = ObjectMapper.BMSByte(UniqueIdentity, targetHash, NetworkId, CreateCode);

            //为此对象编写所有最新的数据
            // Write all of the most up to date data for this object
            WritePayload(data);

            //如果对象具有元数据，则写入
            // Write if the object has metadata
            ObjectMapper.Instance.MapBytes(data, Metadata != null);
            if (Metadata != null)
                ObjectMapper.Instance.MapBytes(data, Metadata);


            Binary createObject = new Binary(CreateTimestep, false, data, Receivers.All, MessageGroupIds.CREATE_NETWORK_OBJECT_REQUEST, Networker.IsTcp, RouterIds.NETWORK_OBJECT_ROUTER_ID, Networker.RoomUid);

            if (targetHash != 0)
                return createObject;

            // 通知其他客户端创建网络对象
            // (服务器主动创建的，skipPlayer = null )

            //如果有一个目标散列，我们只是生成创建对象框架
            // If there is a target hash, we are just generating the create object frame
            Networker.ServerSendAll(createObject, true, skipPlayer);

            return createObject;
        }

        /// <summary>
        /// 当该玩家被服务器接受时， 服务器发送目前所有的网络对象给他
        /// </summary>
        /// <param name="player"></param>
        /// <param name="networkObjects"></param>
		public static void PlayerAccepted(NetworkingPlayer player, NetworkObject[] networkObjects)
        {
            foreach (NetworkObject obj in networkObjects)
                obj.currentRpcBufferCounts.Add(player, obj.rpcBuffer.Count);

            Task.Queue(() =>
            {
                lock (player)
                {
                    BMSByte targetData = new BMSByte();
                    ulong timestep = 0;
                    RoomScene networker = null;
                    List<int> indexes = new List<int>();

                    foreach (NetworkObject obj in networkObjects)
                    {
                        if (obj.Owner == player)
                            continue;

                        indexes.Add(targetData.Size);

                        ObjectMapper.Instance.MapBytes(targetData, obj.UniqueIdentity, 0, obj.NetworkId, obj.CreateCode);

                        //为此对象编写所有最新的数据
                        // Write all of the most up to date data for this object
                        obj.WritePayload(targetData);

                        //如果对象具有元数据，则写入
                        // Write if the object has metadata
                        ObjectMapper.Instance.MapBytes(targetData, obj.Metadata != null);
                        if (obj.Metadata != null)
                            ObjectMapper.Instance.MapBytes(targetData, obj.Metadata);

                        timestep = obj.CreateTimestep;
                        networker = obj.Networker;
                    }

                    BMSByte indexBytes = ObjectMapper.BMSByte(indexes.Count);
                    for (int i = 0; i < indexes.Count; i++)
                        ObjectMapper.Instance.MapBytes(indexBytes, indexes[i]);

                    targetData.InsertRange(0, indexBytes);

                    if (targetData.Size > 0 && networker != null)
                    {
                        Binary targetCreateObject = new Binary(timestep, false, targetData, Receivers.Target, MessageGroupIds.CREATE_NETWORK_OBJECT_REQUEST, networker.IsTcp, RouterIds.ACCEPT_MULTI_ROUTER_ID, networker.RoomUid);

                        networker.ServerSend(player, targetCreateObject, true);
                    }
                }
            });
        }

        /// <summary>
        /// 服务器收到玩家 创建该网络对象完成
        /// </summary>
        /// <param name="player"></param>
		public void CreateConfirmed(NetworkingPlayer player)
        {
            SendBuffer(player);
        }

        /// <summary>
        /// Finish setting up this network object on the network and fire off any complete events
        /// </summary>
        /// <param name="id">The id that was assigned from the network (if client)</param>
        /// <param name="skipCreated">The objectCreated event will be fired if <c>true</c></param>
        /// <summary>
        ///完成在网络上设置这个网络对象，并发起任何完整的事件
        /// </ summary>
        /// <param name =“id”>从网络（如果客户端）分配的id </ param>
        /// <param name =“skipCreated”>如果objectCreated事件将被触发<c> true </ c> </ param>，
        private void Initialize(uint id = 0, ulong timestep = 0)
        {
            //这是一个服务器，所以它可以像平常一样创建对象
            // This is a server so it can create the object as normal
            NetworkReady = true;

            CreateTimestep = timestep == 0 ? Networker.Time.Timestep : timestep;

            //向网络注册该对象并获取它的唯一ID
            // Register this object with the networker and obtain it's unique id
            Networker.RegisterNetworkObject(this, id);

            // 当场景还在加载中，网络发来的网络对象 初始化就先将其冻结到pendingCreates列表里。等场景初始化完，就继续他们的处理
            if (Networker.PendCreates)
            {
                lock (Networker.pendingCreates)
                {
                    Networker.pendingCreates.Add(this);
                }

                return;
            }

            if (onReady != null)
                onReady(Networker);

            Networker.OnObjectCreated(this);
        }

        /// <summary>
        /// 如果NetWorker.objectCreated 没有被监听
        /// 就将这些等待初始化的网络对象初始化
        /// </summary>
        /// <param name="target">网络</param>
		public static void Flush(RoomScene target)
        {
            List<NetworkObject> pendingCreates = target.pendingCreates;
            lock (pendingCreates)
            {
                for (int i = 0; i < pendingCreates.Count; i++)
                {
                    if (!target.ObjectCreatedRegistered)
                        continue;

                    if (pendingCreates[i].onReady != null)
                        pendingCreates[i].onReady(target);

                    target.OnObjectCreated(pendingCreates[i]);
                    pendingCreates.RemoveAt(i--);
                }
            }

            target.PendCreates = false;
        }

        /// <summary>
        /// A method callback for the client to listen for when the object has been asynchronously created
        /// </summary>
        /// <param name="identity">The identity to describe what type / subtype of network object this is</param>
        /// <param name="hash">The hash id that was sent to match up with this hash id</param>
        /// <param name="id">The id that the server has given to this network object</param>
        /// <param name="frame">The initialization data for this network object</param>
        /// <summary>
        ///客户端侦听异步创建对象的方法回调
        ///发起创建的客户端，收到服务器反馈的网络ID
        /// </ summary>
        /// <param name =“identity”>用于描述网络对象的类型/子类型的标识</ param>
        /// <param name =“hash”>已经发送的哈希ID与这个哈希id </ param>匹配
        /// <param name =“id”>服务器给这个网络对象的id </ param>
        /// <param name =“frame”>这个网络对象的初始化数据</ param>
        private void CreatedOnNetwork(int identity, int hash, uint id, FrameStream frame)
        {
            //检查网络中的身份是否属于这种类型
            // Check to see if the identity from the network belongs to this type
            if (identity != UniqueIdentity)
                return;

            //如果哈希不属于这个对象，那么忽略它
            // If the hash does not belong to this object then ignore it
            if (hash != this.hash)
                return;

            Owner = Networker.Me;

            //找到了这个对象，把它从听到更多的创建消息中删除
            // This object has been found, remove it from listening to any more create messages
            Networker.objectCreateAttach -= CreatedOnNetwork;

            // Move the start index passed the identity bytes and the hash bytes
            frame.StreamData.MoveStartIndex(sizeof(int) * 2);

            Initialize(id);

            if (onReady != null)
                onReady(Networker);
        }

        /// <summary>
        /// This is called from the networker when the this object is created, it
        /// will contain the id for this object on the network
        /// </summary>
        /// <param name="id"></param>
        /// <returns><c>true</c> if the id has not already been assigned</returns>
        /// <summary>
        ///这是在创建这个对象时从网络中调用的
        ///将在网络上包含这个对象的id
        /// </ summary>
        /// <param name =“id”> </ param>
        /// <returns> <c> true </ c>如果id尚未被分配</ returns>
        public bool RegisterOnce(uint id)
        {
            //如果这个对象已经有一个id，忽略这个请求
            // If there already is an id for this object, ignore this request
            if (NetworkId != 0)
                return false;

            NetworkId = id;

            if (ClientRegistered)
                ClearClientPendingRPC();

            return true;
        }

        /// <summary>
        /// This will register a method to this network object as an Rpc
        /// </summary>
        /// <param name="methodName">The name of the method that is to be registered</param>
        /// <param name="callback">The callback to fire for this RPC when received a signal for it</param>
        /// <param name="argumentTypes">The types argument types for validation</param>
        /// <summary>
        ///这将注册一个方法到这个网络对象作为Rpc
        /// </ summary>
        /// <param name =“methodName”>要注册的方法的名称</ param>
        /// <param name =“callback”>当接收到一个信号时，为这个RPC启动的回调</ param>
        /// <param name =“argumentTypes”>用于验证的类型参数类型</ param>
        public void RegisterRpc(string methodName, Action<RpcArgs> callback, params Type[] argumentTypes)
        {
            //确保方法名称字符串是唯一的，尚未分配
            // Make sure that the method name string is unique and not already assigned
            if (rpcLookup.ContainsKey(methodName))
                throw new BaseNetworkException("The rpc " + methodName + " has already been registered");

            //每个网络对象只允许255个注册的RPC方法，因为id是一个字节
            // Each network object is only allowed 255 registered RPC methods as the id is a byte
            if (Rpcs.Count >= byte.MaxValue)
                throw new BaseNetworkException("You are only allowed to register " + byte.MaxValue + " Rpc methods per network object");


            // The id for this RPC is goign to be the next index in the dictionary
            byte id = (byte)Rpcs.Count;
            Rpcs.Add(id, new Rpc(callback, argumentTypes));
            rpcLookup.Add(methodName, id);
            inverseRpcLookup.Add(id, methodName);
        }

        /// <summary>
        /// 一旦所有的RPC方法都被注册，调用
        /// Called once all of the RPC methods have been registered
        /// </summary>
        public void RegistrationComplete()
        {
            ClientRegistered = true;

            if (NetworkId == 0)
                return;

            Networker.CompleteInitialization(this);
        }

        public void ReleaseCreateBuffer()
        {
            RegistrationComplete();

            lock (pendingClientRegisterRpc)
            {
                ClearClientPendingRPC();
            }
        }

        private void ClearClientPendingRPC()
        {
            foreach (PendingRpc rpc in pendingClientRegisterRpc)
                InvokeRpc(rpc.sender, rpc.timestep, rpc.data, rpc.receivers);

            foreach (PendingLocalRPC rpc in pendingLocalRpcs)
                SendRpc(rpc.TargetPlayer, rpc.MethodId, rpc.Args);

            pendingClientRegisterRpc.Clear();
            pendingLocalRpcs.Clear();
        }

        /// <summary>
        /// Used to call a RPC on the local process
        /// </summary>
        /// <param name="data">The data sent from the network to be mapped to the RPC input arguments</param>
        /// <param name="receivers">The receivers that were supplied on by the sender</param>
        /// <summary>
        ///用于调用本地进程的RPC
        /// </ summary>
        /// <param name =“data”>从网络发送的映射到RPC输入参数的数据</ param>
        /// <param name =“receivers”>发件人提供的接收者</ param>
        public void InvokeRpc(NetworkingPlayer sender, ulong timestep, BMSByte data, Receivers receivers)
        {
            // 如果对象还没注册完成，就挂起
            lock (pendingClientRegisterRpc)
            {
                if (!ClientRegistered)
                {
                    pendingClientRegisterRpc.Add(new PendingRpc()
                    {
                        data = data,
                        receivers = receivers,
                        sender = sender,
                        timestep = timestep
                    });

                    return;
                }
            }

            byte methodId = data.GetBasicType<byte>();

            if (!Rpcs.ContainsKey(methodId))
                throw new BaseNetworkException("The rpc " + methodId + " was not found on this network object");

            byte behaviorFlags = data.GetBasicType<byte>();

            bool overwriteExisting = (RPC_BEHAVIOR_OVERWRITE & behaviorFlags) != 0;

            object[] args = Rpcs[methodId].ReadArgs(data);

            RpcArgs rpcArgs = new RpcArgs(args, new RPCInfo { SendingPlayer = sender, TimeStep = timestep });

            //如果我们是服务器，我们需要确定这个RPC是否可以复制
            // If we are the server we need to determine if this RPC is okay to replicate
            if (Networker.IsServer && receivers != Receivers.Target)
            {
                string methodName = inverseRpcLookup[methodId];

                // Validate the RPC call using the method name and the supplied arguments from the client
                // then replicate to the correct receivers
                // Do not read or replicate if the server denies replication
                //使用方法名称和客户端提供的参数来验证RPC调用
                //然后复制到正确的接收器
                //如果服务器拒绝复制，则不要读取或复制
                if (ServerAllowRpc(methodId, receivers, rpcArgs))
                    SendRpc(null, methodId, overwriteExisting, receivers, sender, args);

                return;
            }

            //在未经验证的情况下调用客户端上的方法
            // Call the method on the client without validation
            Rpcs[methodId].Invoke(rpcArgs);
        }

        /// <summary>
        /// Called only on the server and will determine if an RPC call should be replicated
        /// </summary>
        /// <param name="methodId">The id of the RPC to be executed (this will match the generated constant)</param>
        /// <param name="receivers">The receivers that are being requested</param>
        /// <param name="args">The arguments that were supplied by the client when invoked</param>
        /// <returns>If <c>true</c> the RPC will be replicated to other clients</returns>
        protected virtual bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            return true;
        }

        /// <summary>
        /// Called only on the server and will determine if binary data should be replicated
        /// </summary>
        /// <param name="data">The data to be read and replicated</param>
        /// <param name="receivers">The receivers for the data to be replicated</param>
        /// <returns>If <c>true</c> the binary data will be replicated to other clients</returns>
        /// <summary>
        ///仅在服务器上调用，并确定是否应该复制二进制数据
        /// </ summary>
        /// <param name =“data”>要读取和复制的数据</ param>
        /// <param name =“receivers”>要复制的数据的接收者</ param>
        /// <returns>如果<c> true </ c>二进制数据将被复制到其他客户端</ returns>
        protected virtual bool ServerAllowBinaryData(BMSByte data, Receivers receivers)
        {
            return true;
        }

        /// <summary>
        /// Called only on the server and will determine if the ownership change request is allowed
        /// </summary>
        /// <param name="newOwner">The new player that is requesting ownership</param>
        /// <returns>If <c>true</c> then the ownership change will be allowed</returns>
        /// <summary>
        ///仅在服务器上调用，并确定是否允许所有权更改请求
        /// </ summary>
        /// <param name =“newOwner”>请求所有权的新玩家</ param>
        /// <returns>如果<c> true </ c>那么所有权更改将被允许</ returns>
        protected virtual bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            return true;
        }

        /// <summary>
        /// This will send the current buffer to the connecting player after they have created the object
        /// </summary>
        /// <param name="player">The player that will be receiving the RPC calls</param>
        /// <summary>
        ///这会在创建对象后将当前缓冲区发送给连接player
        /// </ summary>
        /// <param name =“player”>将接收RPC调用的player</ param>
        private void SendBuffer(NetworkingPlayer player)
        {
            int count;

            if (!currentRpcBufferCounts.TryGetValue(player, out count))
                return;

            currentRpcBufferCounts.Remove(player);

            lock (rpcBuffer)
            {
                for (int i = 0; i < count; i++)
                    FinalizeSendRpc(rpcBuffer[i].data, rpcBuffer[i].receivers, rpcBuffer[i].methodId, rpcBuffer[i].timestep, player);
            }
        }


        /// <summary>
        /// Build the network frame (message) data for this RPC call so that it is properly
        /// delegated on the network
        /// </summary>
        /// <param name="methodId">The id of the RPC to be called</param>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="args">The input arguments for the method call</param>
        public void SendRpc(byte methodId, Receivers receivers, params object[] args)
        {
            SendRpc(null, methodId, false, receivers, Networker.Me, args);
        }


        /// <summary>
        /// Build the network frame (message) data for this RPC call so that it is properly
        /// delegated on the network
        /// </summary>
        /// <param name="methodId">The id of the RPC to be called</param>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="replacePrevious">If <c>True</c> then the previous call to this method will be replaced with this one</param>
        /// <param name="args">The input arguments for the method call</param>
        public void SendRpc(byte methodId, bool replacePrevious, Receivers receivers, params object[] args)
        {
            SendRpc(null, methodId, replacePrevious, receivers, Networker.Me, args);
        }



        /// <summary>
        /// Build the network frame (message) data for this RPC call so that it is properly
        /// delegated on the network
        /// </summary>
        /// <param name="targetPlayer">The player that is being sent this RPC from the server</param>
        /// <param name="methodId">The id of the RPC to be called</param>
        /// <param name="args">The input arguments for the method call</param>
        /// <summary>
        ///为此RPC调用构建网络框架（消息）数据，以便它正确
        ///在网络上委托
        /// </ summary>
        /// <param name =“targetPlayer”>从服务器发送此RPC的Player</ param>
        /// <param name =“methodId”>要调用的RPC的id </ param>
        /// <param name =“args”>方法调用的输入参数</ param>
        public void SendRpc(NetworkingPlayer targetPlayer, byte methodId, params object[] args)
        {
            SendRpc(targetPlayer, methodId, false, Receivers.Target, Networker.Me, args);
        }


        /// <summary>
        /// Build the network frame (message) data for this RPC call so that it is properly
        /// delegated on the network
        /// </summary>
        /// <param name="targetPlayer">The player that is being sent this RPC from the server</param>
        /// <param name="methodId">The id of the RPC to be called</param>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="replacePrevious">If <c>True</c> then the previous call to this method will be replaced with this one</param>
        /// <param name="args">The input arguments for the method call</param>
        public void SendRpc(NetworkingPlayer targetPlayer, bool replacePrevious, byte methodId, params object[] args)
        {
            SendRpc(targetPlayer, methodId, replacePrevious, Receivers.Target, Networker.Me, args);
        }

        /// <summary>
        /// Build the network frame (message) data for this RPC call so that it is properly
        /// delegated on the network
        /// </summary>
        /// <param name="targetPlayer">The target player that should receive the RPC</param>
        /// <param name="methodId">The id of the RPC that is to be called</param>
        /// <param name="receivers">The clients / server to receive the message</param>
        /// <param name="args">The input arguments for the method call</param>
        /// <returns></returns>
        /// <summary>
        ///为此RPC调用构建网络框架（消息）数据，以便它正确
        ///在网络上委托
        /// </ summary>
        /// <param name =“targetPlayer”>应该接收RPC </ param>的目标Player
        /// <param name =“methodId”>要调用的RPC的id </ param>
        /// <param name =“receivers”>接收消息的客户机/服务器</ param>
        /// <param name =“args”>方法调用的输入参数</ param>
        /// <returns> </ returns>
        public void SendRpc(NetworkingPlayer targetPlayer, byte methodId, bool replacePrevious, Receivers receivers, NetworkingPlayer sender, object[] args)
        {
            // 如果接受这设置的是目标玩家， 并且当前对象不是服务器的
            if (receivers == Receivers.Target && !(Networker.IsServer))
                receivers = Receivers.Server;

            // 如果还没注册完成，就挂起
            if (!ClientRegistered)
            {
                pendingLocalRpcs.Add(new PendingLocalRPC()
                {
                    TargetPlayer = targetPlayer,
                    MethodId = methodId,
                    Receivers = receivers,
                    Args = args
                });

                return;
            }

            //确保传递的参数匹配所需的参数
            // Make sure that the parameters that were passed match the desired arguments
            Rpcs[methodId].ValidateParameters(args);

            ulong timestep = Networker.Time.Timestep;

            //服务器在发送给客户端之前应该执行RPC
            // The server should execute the RPC before it is sent out to the clients
            if (Networker.IsServer)
            {
                //如果我们只将消息发送给所有者，则需要指定该消息
                // If we are only sending the message to the owner, we need to specify that
                if (receivers == Receivers.Owner || receivers == Receivers.ServerAndOwner)
                    targetPlayer = Owner;

                //如果目标玩家是服务器，我们不需要做任何额外的工作
                // We don't need to do any extra work if the target player is the server
                if (targetPlayer == Networker.Me)
                {
                    InvokeRpcOnSelfServer(methodId, sender, timestep, args);
                    return;
                }
            }

            //将行为标志映射到rpc
            // Map the behavior flags to the rpc
            byte behaviorFlags = 0;
            behaviorFlags |= replacePrevious ? RPC_BEHAVIOR_OVERWRITE : (byte)0;

            // Map the id of the object into the data so that the program knows what fire from
            // Map the id of the Rpc as the second data into the byte array
            // Map all of the data to bytes
            BMSByte data = ObjectMapper.BMSByte(NetworkId, methodId, behaviorFlags);
            ObjectMapper.Instance.MapBytes(data, args);

            if (Networker.IsServer)
            {
                //缓存的RPC消息存储在NetworkObject级别上，而不是在NetWorker级别上
                // Buffered RPC messages are stored on the NetworkObject level and not on the NetWorker level
                if (receivers == Receivers.AllBuffered || receivers == Receivers.OthersBuffered)
                {
                    if (receivers == Receivers.AllBuffered)
                        receivers = Receivers.All;

                    if (receivers == Receivers.OthersBuffered)
                        receivers = Receivers.Others;

                    lock (rpcBuffer)
                    {
                        BufferedRpc rpc = new BufferedRpc()
                        {
                            data = new BMSByte().Clone(data),
                            receivers = receivers,
                            methodId = methodId,
                            timestep = timestep
                        };

                        bool replaced = false;
                        if (replacePrevious)
                        {
                            for (int i = 0; i < rpcBuffer.Count; i++)
                            {
                                if (rpcBuffer[i].methodId == methodId)
                                {
                                    rpcBuffer[i] = rpc;
                                    replaced = true;
                                    break;
                                }
                            }
                        }

                        if (!replaced)
                        {
                            //将RPC添加到接受发送的缓冲区
                            // Add the RPC to the buffer to be sent on accept
                            rpcBuffer.Add(rpc);
                        }
                    }
                }
            }

            if (!Networker.IsServer || receivers != Receivers.Server)
                FinalizeSendRpc(data, receivers, methodId, timestep, targetPlayer, sender);

            if (Networker.IsServer)
            {
                //调用目标player是服务器本身还是一个明确的接收者
                // Invoke if the the target player is the server itself or is an explicit receiver
                if (targetPlayer == Networker.Me || receivers == Receivers.Server || receivers == Receivers.ServerAndOwner)
                    InvokeRpcOnSelfServer(methodId, sender, timestep, args);

                //如果服务器发送给接收者，不要执行RPC
                //不包括它本身
                // Don't execute the RPC if the server is sending it to receivers
                // that don't include itself
                else if (receivers != Receivers.Owner && ((sender != Networker.Me && sender != null) ||
                    (receivers != Receivers.Others && receivers != Receivers.OthersBuffered &&
                    receivers != Receivers.OthersProximity && receivers != Receivers.Target)))
                {
                    InvokeRpcOnSelfServer(methodId, sender, timestep, args);
                }
            }
        }

        private void InvokeRpcOnSelfServer(byte methodId, NetworkingPlayer sender, ulong timestep, object[] args)
        {
            Rpcs[methodId].Invoke(new RpcArgs(args, new RPCInfo { SendingPlayer = sender, TimeStep = timestep }), sender == Networker.Me);
        }

        // 完成发送Rpc
        private void FinalizeSendRpc(BMSByte data, Receivers receivers, byte methodId, ulong timestep, NetworkingPlayer targetPlayer = null, NetworkingPlayer sender = null)
        {
            //用路由器生成一个二进制帧
            // Generate a binary frame with a router
            Binary rpcFrame = new Binary(timestep, Networker.IsTCPClient, data, receivers, MessageGroupIds.GetId("NO_RPC_" + NetworkId + "_" + methodId), Networker.IsTcp, RouterIds.RPC_ROUTER_ID, Networker.RoomUid);
            rpcFrame.SetSender(sender);

            if (targetPlayer != null && Networker.IsServer)
            {
                Networker.ServerSend(targetPlayer, rpcFrame, true);
            }
            else
            {
                Networker.Send(rpcFrame, true);
            }
        }

        /// <summary>
        /// Send raw binary data across the network to the associated NetworkObject
        /// </summary>
        /// <param name="data">The raw data to be sent</param>
        /// <param name="receivers">The receivers for this raw data</param>
        /// <param name="subRouter">Used to determine if this is a special type of binary data</param>
        /// <summary>
        ///通过网络发送原始的二进制数据到相关的NetworkObject
        /// </ summary>
        /// <param name =“data”>要发送的原始数据</ param>
        /// <param name =“receivers”>这个原始数据的接收者</ param>
        /// <param name =“subRouter”>用于确定这是否是特殊类型的二进制数据</ param>
        public void SendBinaryData(BMSByte data, Receivers receivers, byte subRouter = 0, bool reliable = true, bool skipOwner = false)
        {
            NetworkingPlayer skipPlayer = null;
            if (skipOwner && IsServer)
            {
                if (Owner == Networker.Me || !AuthorityUpdateMode)
                    skipPlayer = Owner;
            }


            lock (sendBinaryData)
            {
                sendBinaryData.Clear();

                // 将对象的id映射到数据中，以便程序知道发生了什么
                // Map the id of the object into the data so that the program knows what fire from
                ObjectMapper.Instance.MapBytes(sendBinaryData, NetworkId, subRouter);

                //将所有数据映射到字节
                // Map all of the data to bytes
                sendBinaryData.Append(data);

                // Generate a binary frame with a router
                Binary frame = new Binary(Networker.Time.Timestep, Networker.IsTCPClient, sendBinaryData, receivers, MessageGroupIds.GetId("NO_BIN_DATA_" + NetworkId), Networker.IsTcp, RouterIds.BINARY_DATA_ROUTER_ID, Networker.RoomUid);

                Networker.Send(frame, true, skipPlayer);
            }
        }

        /// <summary>
        /// There has been binary data sent across the network to this NetworkObject
        /// </summary>
        /// <param name="data">The data that has been sent</param>
        /// <param name="receivers">The receivers for this data</param>
        /// <summary>
        ///已经有通过网络发送到这个NetworkObject的二进制数据
        /// </ summary>
        /// <param name =“data”>已发送的数据</ param>
        /// <param name =“receivers”>这个数据的接收者</ param>
        public void ReadBinaryData(FrameStream frame)
        {
            //从二进制数据中获取子路由器
            // Get the subrouter from the binary data
            byte subRouter = frame.StreamData.GetBasicType<byte>();

            switch (subRouter)
            {
                //如果子路由器被设置为字段序列化，那么读取新的字段值
                // If the subRouter is set to be field serializations then read the new field values
                case DIRTY_FIELD_SUB_ROUTER_ID:
                    //如果发送者是所有者，则只能阅读
                    // Should only read if the sending player is the owner
                    if (Networker.IsClient || frame.Sender == Owner)
                    {
                        // TODO：允许服务器复制，如下面的其他示例
                        //将数据复制到其他客户端
                        // TODO:  Allow server to replicate as in the other example below
                        // Replicate the data to the other clients
                        if (Networker.IsServer)
                        {
                            BMSByte data = new BMSByte().Clone(frame.StreamData);

                            if (data != null)
                                SendBinaryData(data, Receivers.All, DIRTY_FIELD_SUB_ROUTER_ID, false, true);
                        }

                        ReadDirtyFields(frame.StreamData, frame.TimeStep);
                    }
                    return;
                case DESTROY_SUB_ROUTER_ID:
                    Destroy(true);
                    return;
            }

            if (Networker.IsServer)
            {
                //如果服务器拒绝复制，则不要读取或复制
                // Do not read or replicate if the server denies replication
                if (ServerAllowBinaryData(frame.StreamData, frame.Receivers))
                {
                    if (readBinary != null)
                        readBinary(frame.StreamData);

                    SendBinaryData(frame.StreamData, frame.Receivers);
                }

                return;
            }

            //在未经验证的情况下调用客户端上的事件
            // Call the event on the client without validation
            if (readBinary != null)
                readBinary(frame.StreamData);
        }

        /// <summary>
        /// Used to check if there are any updates to this object, it is called in 10ms intervals
        /// </summary>
        /// <param name="timeStep">The current timestep for the server</param>
        /// <summary>
        ///用来检查这个对象是否有任何更新，它以10ms的间隔被调用
        /// </ summary>
        /// <param name =“timeStep”>服务器的当前时间步长</ param>
        public void HeartBeat(ulong timeStep)
        {
            if (!hasDirtyFields)
                return;

            if (timeStep - lastUpdateTimestep > UpdateInterval)
            {
                BMSByte data = SerializeDirtyFields();

                if (data != null)
                    SendBinaryData(data, ProximityBasedFields ? Receivers.AllProximity : Receivers.All, DIRTY_FIELD_SUB_ROUTER_ID, false, true);

                hasDirtyFields = false;
                lastUpdateTimestep = timeStep;
            }
        }

        /// <summary>
        /// Called when data comes in for this network object that is needed to be read
        /// in order to update any values contained within it
        /// </summary>
        /// <param name="payload">The data from the network for this object</param>
        /// <param name="timestep">The timestep for this particular change</param>
        /// <summary>
        ///当数据进入需要读取的网络对象时调用
        ///以更新其中包含的任何值
        /// </ summary>
        /// <param name =“payload”>来自网络的这个对象的数据</ param>
        /// <param name =“timestep”>此特定更改的时间步骤</ param>
        protected abstract void ReadPayload(BMSByte payload, ulong timestep);

        /// <summary>
        /// Used to write any data on the network for this object to keep it up to date
        /// </summary>
        /// <param name="data">The data that is going to be sent across the network</param>
        /// <returns>The same input data for method chaining</returns>
        /// <summary>
        ///用于在网络上为该对象写入任何数据以使其保持最新状态
        /// </ summary>
        /// <param name =“data”>将通过网络发送的数据</ param>
        /// <returns>方法链接的相同输入数据</ returns>
        protected abstract BMSByte WritePayload(BMSByte data);

        /// <summary>
        /// 用于在网络上为在该对象上修改的各个字段写入任何数据
        /// Used to write any data on the network for the various fields that have been modified on this object
        /// </summary>
        protected abstract BMSByte SerializeDirtyFields();

        /// <summary>
        /// Used to read the data from the network for changes to this object
        /// </summary>
        /// <param name="data">The data that was received for this object</param>
        /// <param name="timestep">The timestep for this particular update</param>
        /// <summary>
        ///用于从网络读取数据以更改此对象
        /// </ summary>
        /// <param name =“data”>为此对象接收的数据</ param>
        /// <param name =“timestep”>这个特定更新的时间步</ param>
        protected abstract void ReadDirtyFields(BMSByte data, ulong timestep);


        /// <summary>
        /// This is used to destroy this object on the network
        /// </summary>
        public void Destroy(int timeInMilliseconds = 0)
        {
            if (timeInMilliseconds > 0)
            {
                Task.Queue(() =>
                {
                    Destroy(false);
                }, timeInMilliseconds);
            }
            else
                Destroy(false);
        }

        /// <summary>
        /// This is used to destroy this object on the network
        /// </summary>
        /// <param name="remoteCall">Used to know if this call was made over the network</param>
        /// <summary>
        ///这用于销毁网络上的这个对象
        /// </ summary>
        /// <param name =“remoteCall”>用于知道这个调用是否通过网络</ param>进行
        private void Destroy(bool remoteCall)
        {
            if ((IsOwner && !remoteCall) || Networker.IsServer)
                SendBinaryData(null, Receivers.Others, DESTROY_SUB_ROUTER_ID, skipOwner: Networker.IsServer && remoteCall);

            if (onDestroy != null)
                onDestroy(Networker);
        }

        public virtual void InterpolateUpdate() { }
    }
}
