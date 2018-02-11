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
    public interface IServer
    {
        /// 被禁玩家IP的列表
        List<string> BannedAddresses { get; set; }
        /// <summary>
        /// 踢掉玩家
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="forced">是否强制</param>
        void Disconnect(NetworkingPlayer player, bool forced);
        // 将玩家添加到黑名单
        void BanPlayer(ulong networkId, int minutes);
        // 提交断线的玩家
        void CommitDisconnects();
        /// 用于确定此服务器当前是否正在接受连接
        bool AcceptingConnections { get; }


        /// <summary>
        /// 设置服务器 不再接受玩家连接
        /// </summary>
        void StopAcceptingConnections();

        /// <summary>
        /// 设置服务器 开始接受玩家连接
        /// </summary>
        void StartAcceptingConnections();
    }
}