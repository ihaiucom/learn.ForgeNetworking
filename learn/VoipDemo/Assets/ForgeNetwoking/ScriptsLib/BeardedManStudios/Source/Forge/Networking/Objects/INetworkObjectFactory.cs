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

namespace BeardedManStudios.Forge.Networking
{
	/// <summary>
	/// Every program that uses NetworkObject will need to have a factory
	/// to create objects with, this is the interface that it must implement
	/// </summary>
	public interface INetworkObjectFactory
	{
        /// <summary>
        /// There has been a request on the network to create a type of NetworkObject and this
        /// is the entry point for that message
        /// </summary>
        /// <param name="networker">The NetWorker that is the controller for the soon to be created NetworkObject</param>
        /// <param name="identity">The identity for the NetworkObject as an int so that this factory knows what kind of NetworkObject to create</param>
        /// <param name="id">The id that the server has assigned this new NetworkObject (if client)</param>
        /// <param name="frame">The data that was received on the network about this creation</param>
        /// <param name="callback">The callback with the network object that was successfully created</param>
        /// <returns></returns>
        /// <summary>
        ///在网络上有一个请求来创建一个NetworkObject类型
        ///是该消息的入口点
        /// </ summary>
        /// <param name =“networker”>即将创建NetworkObject的控制器的NetWorker </ param>
        /// <param name =“identity”> NetworkObject的标识为int，以便该工厂知道要创建哪种NetworkObject </ param>
        /// <param name =“id”>服务器分配了这个新的NetworkObject（如果客户端）的id </ param>
        /// <param name =“frame”>在网络上接收到的关于这个创建的数据</ param>
        /// <param name =“callback”>成功创建的网络对象的回调</ param>
        /// <returns> </ returns>
        void NetworkCreateObject(NetWorker networker, int identity, uint id, FrameStream frame, System.Action<NetworkObject> callback);
	}
}