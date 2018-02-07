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
        // 路由ID -- 创建网络对象(完成)
		public const int CREATED_OBJECT_ROUTER_ID = 4;
        // 接受多路由器ID
        public const int ACCEPT_MULTI_ROUTER_ID = 5;
	}
}