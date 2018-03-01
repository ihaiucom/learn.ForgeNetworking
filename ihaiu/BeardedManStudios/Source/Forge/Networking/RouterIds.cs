namespace BeardedManStudios.Source.Forge.Networking
{
    /// <summary>
    /// 路由ID
    /// </summary>
	public class RouterIds
	{
        /// <summary>
        /// The router id that is used for when sending or getting binary messages
        /// 发送或获取二进制消息时使用的路由器ID (发起创建网络对象)
        /// </summary>
        public const int NETWORK_OBJECT_ROUTER_ID = 1;
        // 路由ID -- 调用RPC 方法
		public const int RPC_ROUTER_ID = 2;
        // 路由ID -- 二进制数据
		public const int BINARY_DATA_ROUTER_ID = 3;
        // 路由ID -- 创建网络对象(完成), 客户端向服务器发送确认创建网络对象完成
        public const int CREATED_OBJECT_ROUTER_ID = 4;
        // 路由ID -- 接受多路由器ID, 在服务器接受客户端时，将服务器现有的所有网络对象 发给该玩家，让他创建
        public const int ACCEPT_MULTI_ROUTER_ID = 5;

        // 大厅路由ID -- 创建房间
        public const int LOBBY_CREATE_ROOM = 6;
        // 大厅路由ID -- 加入房间
        public const int LOBBY_JOIN_ROOM = 7;
        // 大厅路由ID -- 离开房间
        public const int LOBBY_LEFT_ROOM = 8;
        // 大厅路由ID -- 加入观看房间
        public const int LOBBY_JOIN_WATCH_ROOM = 9;
        // 大厅路由ID -- 离开观看房间
        public const int LOBBY_LEFT_WATCH_ROOM = 10;
    }
}