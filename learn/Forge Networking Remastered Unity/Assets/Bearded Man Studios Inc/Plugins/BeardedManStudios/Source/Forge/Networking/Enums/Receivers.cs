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

namespace BeardedManStudios.Forge.Networking
{
    /// <summary>
    /// 这通常与RPC或WriteCustom结合使用，以限制谁接到呼叫
    /// This is often used in conjunction with RPC or WriteCustom in order to limit who gets the call
    /// </summary>
    public enum Receivers
	{
        // 目标玩家
		Target = 0, // Used for direct messages to clients
        // 所有玩家
		All = 1,
        // 所有玩家并缓存
		AllBuffered = 2,
        // 其他玩家
		Others = 3,
        // 其他玩家并缓存
		OthersBuffered = 4,
        // 服务器
		Server = 5,
        // 周边所有玩家
		AllProximity = 6,
        // 周边其他玩家
		OthersProximity = 7,
        // 拥有者
		Owner = 8,
        // 消息组
		MessageGroup = 9,
        // 服务器和拥有者
        ServerAndOwner = 10
	}
}