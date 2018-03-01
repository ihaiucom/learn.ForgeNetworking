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

using System.Collections.Generic;

namespace BeardedManStudios.Forge.Networking
{
	public static class MessageGroupIds
	{
		private static int groupId = START_OF_GENERIC_IDS;
		public static Dictionary<string, int> groupIdLookup = new Dictionary<string, int>();

		public static int GetId(string key)
		{
			if (!groupIdLookup.ContainsKey(key))
				groupIdLookup.Add(key, ++groupId);

			return groupIdLookup[key];
		}

		public const int TCP_FIND_GROUP_ID = 0;
        // 向服务器请求接受
		public const int NETWORK_ID_REQUEST = 1;
		public const int NETWORK_OBJECT_RPC = 2;
        // 创建网络对象请求
		public const int CREATE_NETWORK_OBJECT_REQUEST = 3;
		public const int NAT_SERVER_CONNECT = 4;
		public const int NAT_SERVER_REGISTER = 5;
		public const int NAT_ROUTE_REQUEST = 6;
        // 最大连接数, 服务不接受连接时 反馈给客户端的错误
		public const int MAX_CONNECTIONS = 7;
		public const int DISCONNECT = 8;
		public const int MASTER_SERVER_REGISTER = 9;
		public const int MASTER_SERVER_UPDATE = 10;
		public const int MASTER_SERVER_GET = 11;
        // 发起ping
		public const int PING = 12;
        // 反馈ping
		public const int PONG = 13;

        // 语言
		public const int VOIP = 14;
        /// 服务器数据缓存, 向服务请求服务器缓存的数据时用
		public const int CACHE = 15;
        /// 场景初始化
		public const int VIEW_INITIALIZE = 16;
        /// <summary>
        /// 加载完场景， 开始检测场景中的NetworkBehavior和NetworkObject，对他们初始化
        /// </summary>
		public const int VIEW_CHANGE = 17;

        // 启动通用的IDS
        public const int START_OF_GENERIC_IDS = 10000;
	}
}