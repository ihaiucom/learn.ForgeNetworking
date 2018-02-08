/*-----------------------------+-------------------------------\
|                                                              |
|                         !!!NOTICE!!!                         |
|                                                              |
|  These libraries are under heavy development so they are     |
|  subject to make many changes as development continues.      |
|  For this reason, the libraries may not be well commented.   |
|  THANK YOU for supporting forge with all your feedback       |
|  suggestions, bug reports and comments!                      |
|                                                              |
|                              - The Forge Team                |
|                                Bearded Man Studios, Inc.     |
|                                                              |
|  This source code, project files, and associated files are   |
|  copyrighted by Bearded Man Studios, Inc. (2012-2017) and    |
|  may not be redistributed without written permission.        |
|                                                              |
\------------------------------+------------------------------*/

using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;

namespace BeardedManStudios.Forge.Networking.DataStore
{
    /// <summary>
    /// The main class for managing and communicating data from the server cache
    /// </summary>
    /// <remarks>
    /// Cache is used to store any arbitrary data for your game, you can deposit supported data types to the server with a key, then access them again
    /// on any client by using the key to access the specific data stored. Cache.Set() and Cache.Request() being the two primary ways to use Cache.
    /// </remarks>
    /// <summary>
    ///用于管理和传递来自服务器缓存的数据的主要类
    /// </ summary>
    /// <注释>
    ///缓存用于存储游戏的任意数据，您可以使用密钥将支持的数据类型存入服务器，然后再次访问
    ///在任何客户端使用密钥访问存储的特定数据。 Cache.Set（）和Cache.Request（）是使用Cache的两个主要方法。
    /// </ remarks>
    public class Cache
	{
		public const int DEFAULT_CACHE_SERVER_PORT = 15942;

        //缓存对象的默认失效日期时间。
        // Default expiry datetime for a cached object.
        private readonly DateTime maxDateTime = DateTime.MaxValue;

        /// <summary>
        ///数据的内存缓存
        /// The memory cache for the data
        /// </summary>
        private Dictionary<string, CachedObject> memory = new Dictionary<string, CachedObject>();

		/// <summary>
		/// The main socket for communicating the cache back and forth
		/// </summary>
		public NetWorker Socket { get; private set; }

		// TODO:  Possibly make this global
		/// <summary>
		/// The set of types that are allowed and a byte mapping to them
		/// </summary>
		private static Dictionary<byte, Type> typeMap = new Dictionary<byte, Type>() {
			{ 0, typeof(byte) },
			{ 1, typeof(char) },
			{ 2, typeof(short) },
			{ 3, typeof(ushort) },
			{ 4, typeof(int) },
			{ 5, typeof(uint) },
			{ 6, typeof(long) },
			{ 7, typeof(ulong) },
			{ 8, typeof(bool) },
			{ 9, typeof(float) },
			{ 10, typeof(double) },
			{ 11, typeof(string) },
			{ 12, typeof(Vector) },/*
			{ 12, typeof(Vector2) },
			{ 13, typeof(Vector3) },
			{ 14, typeof(Vector4) },
			{ 15, typeof(Quaternion) },
			{ 16, typeof(Color) }*/
		};

        /// <summary>
        /// 回调堆栈所在的当前ID
        /// The current id that the callback stack is on
        /// </summary>
        private int responseHookIncrementer = 0;

        /// <summary>
        /// 请求数据时的主要回调堆栈
        /// The main callback stack for when requesting data
        /// </summary>
        private Dictionary<int, Action<object>> responseHooks = new Dictionary<int, Action<object>>();

		public Cache(NetWorker socket)
		{
			Socket = socket;
			Socket.binaryMessageReceived += BinaryMessageReceived;
		}

		private void RemoveExpiredObjects()
		{
			foreach (KeyValuePair<string, CachedObject> entry in memory)
				if (entry.Value.IsExpired())
					memory.Remove(entry.Key);
		}

        /// <summary>
        /// Called when the network as interpreted that a cache message has been sent from the server
        /// </summary>
        /// <param name="player">The server</param>
        /// <param name="frame">The data that was received</param>
        /// <summary>
        ///当网络被解释为缓存消息已经从服务器发送时调用
        /// </ summary>
        /// <param name =“player”>服务器</ param>
        /// <param name =“frame”>收到的数据</ param>
        private void BinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
		{
			if (frame.GroupId != MessageGroupIds.CACHE)
				return;

			if (sender is IServer)
			{
				byte type = ObjectMapper.Instance.Map<byte>(frame.StreamData);
				int responseHookId = ObjectMapper.Instance.Map<int>(frame.StreamData);
				string key = ObjectMapper.Instance.Map<string>(frame.StreamData);

				object obj = Get(key);

				// TODO:  Let the client know it is null
				if (obj == null)
					return;

				BMSByte data = ObjectMapper.BMSByte(type, responseHookId, obj);

				Binary sendFrame = new Binary(sender.Time.Timestep, sender is TCPClient, data, Receivers.Target, MessageGroupIds.CACHE, sender is BaseTCP);

				if (sender is BaseTCP)
					((TCPServer)sender).Send(player.TcpClientHandle, sendFrame);
				else
					((UDPServer)sender).Send(player, sendFrame, true);
			}
			else
			{
				byte type = ObjectMapper.Instance.Map<byte>(frame.StreamData);
				int responseHookId = ObjectMapper.Instance.Map<int>(frame.StreamData);

				object obj = null;

				if (typeMap[type] == typeof(string))
					obj = ObjectMapper.Instance.Map<string>(frame.StreamData);
				/*else if (typeMap[type] == typeof(Vector2))
					obj = ObjectMapper.Map<Vector2>(stream);
				else if (typeMap[type] == typeof(Vector3))
					obj = ObjectMapper.Map<Vector3>(stream);
				else if (typeMap[type] == typeof(Vector4))
					obj = ObjectMapper.Map<Vector4>(stream);
				else if (typeMap[type] == typeof(Color))
					obj = ObjectMapper.Map<Color>(stream);
				else if (typeMap[type] == typeof(Quaternion))
					obj = ObjectMapper.Map<Quaternion>(stream);*/
				else
					obj = ObjectMapper.Instance.Map(typeMap[type], frame.StreamData);

				if (responseHooks.ContainsKey(responseHookId))
				{
					responseHooks[responseHookId](obj);
					responseHooks.Remove(responseHookId);
				}
			}
		}

		/// <summary>
		/// Get an object from cache
		/// </summary>
		/// <typeparam name="T">The type of object to store</typeparam>
		/// <param name="key">The name variable used for storing the desired object</param>
		/// <returns>Return object from key otherwise return the default value of the type or null</returns>
		private T Get<T>(string key)
		{
			if (!Socket.IsServer)
				return default(T);

			if (!memory.ContainsKey(key))
				return default(T);

			if (memory[key] is T)
				return (T)memory[key].Value;

			return default(T);
		}

		/// <summary>
		/// Used on the server to get an object at a given key from cache
		/// </summary>
		/// <param name="key">The key to be used in the dictionary lookup</param>
		/// <returns>The object at the given key in cache otherwise null</returns>
		private object Get(string key)
		{
			if (!Socket.IsServer)
				return null;

			if (memory.ContainsKey(key))
				return memory[key].Value;

			return null;
		}

        /// <summary>
        /// Get an object from cache
        /// </summary>
        /// <param name="key">The name variable used for storing the desired object</param>
        /// <returns>The string data at the desired key or null</returns>
        /// <remarks>
        /// Allows a client (or the server) to get a value from the Cache, the value is read directly from the server.
        /// A callback must be specified, this is because the code has to be executed after a moment when the response from the server
        /// is received. Request can be done like this:
        /// <code>
        /// void getServerDescription(){
        /// Cache.Request<string>("server_description", delegate (object response){
        ///	 Debug.Log(((string) response));
        /// });
        /// }
        /// </code>
        /// The Cache only supports Forge's supported data Types, you can find a list of supported data Types in the NetSync documentation...
        /// </remarks>
        /// <summary>
        ///从缓存中获取对象
        /// </ summary>
        /// <param name =“key”>用于存储所需对象的名称变量</ param>
        /// <returns>所需键的字符串数据或null </ returns>
        /// <注释>
        ///允许客户端（或服务器）从缓存中获取值，该值直接从服务器读取。
        ///必须指定一个回调函数，这是因为代码必须在服务器响应之后执行
        ///收到。 请求可以这样做：
        /// <code>
        /// void getServerDescription（）{
        /// Cache.Request <string>（“server_description”，delegate（object response）{
        /// Debug.Log（（（string）response））;
        ///}）;
        ///}
        /// </ code>
        ///缓存仅支持Forge支持的数据类型，您可以在NetSync文档中找到支持的数据类型列表...
        /// </ remarks>
        public void Request<T>(string key, Action<object> callback)
		{
			if (callback == null)
				throw new Exception("A callback is needed when requesting data from the server");

			if (Socket.IsServer)
			{
				callback(Get<T>(key));
				return;
			}

			responseHooks.Add(responseHookIncrementer, callback);

			byte targetType = byte.MaxValue;

			foreach (KeyValuePair<byte, Type> kv in typeMap)
			{
				if (typeof(T) == kv.Value)
				{
					targetType = kv.Key;
					break;
				}
			}

			if (targetType == byte.MaxValue)
				throw new Exception("Invalid type specified");

			BMSByte data = ObjectMapper.BMSByte(targetType, responseHookIncrementer, key);

			Binary sendFrame = new Binary(Socket.Time.Timestep, Socket is TCPClient, data, Receivers.Server, MessageGroupIds.CACHE, Socket is BaseTCP);

			if (Socket is BaseTCP)
				((TCPClient)Socket).Send(sendFrame);
			else
				((UDPClient)Socket).Send(sendFrame, true);

			responseHookIncrementer++;
		}

        /// <summary>
        /// Inserts a NEW key/value into cache
        /// </summary>
        /// <typeparam name="T">The serializable type of object</typeparam>
        /// <param name="key">The name variable used for storing the specified object</param>
        /// <param name="value">The object that is to be stored into cache</param>
        /// <returns>True if successful insert or False if the key already exists</returns>
        /// <summary>
        ///将一个新的键/值插入到缓存中
        /// </ summary>
        /// <typeparam name =“T”>可序列化的对象类型</ typeparam>
        /// <param name =“key”>用于存储指定对象的名称变量</ param>
        /// <param name =“value”>要存储到缓存中的对象</ param>
        /// <returns>如果成功插入，则返回true;如果键已经存在，则返回False </ returns>
        public bool Insert<T>(string key, T value)
		{
			return Insert(key, value, maxDateTime);
		}

        /// <summary>
        /// Inserts a NEW key/value into cache
        /// </summary>
        /// <typeparam name="T">The serializable type of object</typeparam>
        /// <param name="key">The name variable used for storing the specified object</param>
        /// <param name="value">The object that is to be stored into cache</param>
        /// <param name="expireAt">The DateTime defining when the cached object should expire</param>
        /// <returns>True if successful insert or False if the key already exists</returns>
        /// <summary>
        ///将一个新的键/值插入到缓存中
        /// </ summary>
        /// <typeparam name =“T”>可序列化的对象类型</ typeparam>
        /// <param name =“key”>用于存储指定对象的名称变量</ param>
        /// <param name =“value”>要存储到缓存中的对象</ param>
        /// <param name =“expireAt”>定义缓存对象何时应该过期的DateTime </ param>
        /// <returns>如果成功插入，则返回true;如果键已经存在，则返回False </ returns>
        public bool Insert<T>(string key, T value, DateTime expireAt)
		{
			if (!(Socket is IServer))
				throw new Exception("Inserting cache values is not yet supported for clients!");

			if (!memory.ContainsKey(key))
				return false;

			memory.Add(key, new CachedObject(value, expireAt));

			return true;
		}

        /// <summary>
        /// Inserts a new key/value or updates a key's value in cache
        /// </summary>
        /// <typeparam name="T">The serializable type of object</typeparam>
        /// <param name="key">The name variable used for storing the specified object</param>
        /// <param name="value">The object that is to be stored into cache</param>
        /// <remarks>
        /// This inputs a value into the cache, this can only be called on the server, you can only input Types forge supports (See NetSync for supported Types).
        /// </remarks>
        /// <summary>
        ///插入新的键/值或更新缓存中的键值
        /// </ summary>
        /// <typeparam name =“T”>可序列化的对象类型</ typeparam>
        /// <param name =“key”>用于存储指定对象的名称变量</ param>
        /// <param name =“value”>要存储到缓存中的对象</ param>
        /// <注释>
        ///这将一个值输入到缓存中，这只能在服务器上调用，您只能输入Types forge支持（请参阅支持的NetSync类型）。
        /// </ remarks>
        public void Set<T>(string key, T value)
		{
			Set(key, value, maxDateTime);
		}

		/// <summary>
		/// Inserts a new key/value or updates a key's value in cache
		/// </summary>
		/// <typeparam name="T">The serializable type of object</typeparam>
		/// <param name="key">The name variable used for storing the specified object</param>
		/// <param name="value">The object that is to be stored into cache</param>
		/// <param name="expireAt">The DateTime defining when the cached object should expire</param>
		public void Set<T>(string key, T value, DateTime expireAt)
		{
			if (!(Socket is IServer))
				throw new Exception("Setting cache values is not yet supported for clients!");

			var cachedObject = new CachedObject(value, expireAt);

			if (!memory.ContainsKey(key))
				memory.Add(key, cachedObject);
			else
				memory[key] = cachedObject;
		}

		/// <summary>
		/// CachedObject class.
		/// </summary>
		public class CachedObject
		{
			public object Value { get; private set; }

			public DateTime ExpireAt { get; private set; }

			public CachedObject(object value, DateTime expireAt)
			{
				Value = value;
				ExpireAt = expireAt;
			}

			public bool IsExpired()
			{
				return DateTime.Now >= ExpireAt;
			}
		}
	}
}