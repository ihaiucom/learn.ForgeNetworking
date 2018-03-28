using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Source.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 7:25:35 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public class NetRoomClient : NetRoomBase
    {
        // 客户端Lobby
        internal LobbyClient clientLobby;

        public Dictionary<ulong, IRoleInfo> PlayerDict { get; protected set; }
        public List<IRoleInfo> PlayerList { get; protected set; }

        public NetRoomClient(LobbyClient lobby, IRoomInfo roomInfo)
        {
            this.clientLobby = lobby;
            this.PlayerList = new List<IRoleInfo>();
            this.PlayerDict = new Dictionary<ulong, IRoleInfo>();

            Initialize(lobby, roomInfo);
        }

        /// <summary>
        /// 获取玩家
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public IRoleInfo GetPlayer(ulong roleId)
        {
            if (PlayerDict.ContainsKey(roleId))
            {
                return PlayerDict[roleId];
            }
            return null;
        }

        /// <summary>
        /// 添加玩家
        /// </summary>
        protected void AddPlayer(IRoleInfo role)
        {
            if(PlayerDict.ContainsKey(role.uid))
            {
                return;
            }
            PlayerDict.Add(role.uid, role);
            PlayerList.Add(role);
        }

        /// <summary>
        /// 移除玩家
        /// </summary>
        protected void RemovePlayer(ulong roleId)
        {
            if (PlayerDict.ContainsKey(roleId))
            {
                PlayerList.Remove(PlayerDict[roleId]);
                PlayerDict.Remove(roleId);
            }
        }



        // 接收二进制数据
        public override void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            if (frame.GroupId == MessageGroupIds.ROOM)
            {
                byte routerId = frame.RouterId;
                if(routerId == RouterIds.ROOM_GET_PLAYERLIST)
                {
                    List<IRoleInfo> playerList = new List<IRoleInfo>();
                    int index, count = frame.StreamData.GetBasicType<int>();
                    int head = frame.StreamData.StartIndex();

                    for (int i = 0; i < count; i++)
                    {
                        //返回头部，然后前进到下一个索引
                        frame.StreamData.MoveStartIndex(-frame.StreamData.StartIndex() + i * sizeof(int) + head);
                        index = frame.StreamData.GetBasicType<int>(false);


                        //移到主有效载荷开始的计数末尾
                        frame.StreamData.MoveStartIndex((count - i) * sizeof(int));

                        //移到有效载荷指定的索引
                        frame.StreamData.MoveStartIndex(index);


                        //为这个对象创建一个隔离的框架
                        Binary subFrame = (Binary)frame.Clone();
                        IRoleInfo roleInfo = NetRoleInfo.Read(subFrame.StreamData);
                        AddPlayer(roleInfo);
                        playerList.Add(roleInfo);
                    }

                    OnPlayerListEvent(playerList);
                }
                else
                {

                    switch (routerId)
                    {
                        case RouterIds.ROOM_JOIN_ROOM:
                            IRoleInfo roleInfo = NetRoleInfo.Read(frame.StreamData);
                            AddPlayer(roleInfo);
                            OnPlayerJoinRoom(roleInfo, player);
                            break;

                        case RouterIds.ROOM_LEFT_ROOM:
                            ulong roleUid = frame.StreamData.GetBasicType<ulong>();
                            RemovePlayer(roleUid);
                            OnPlayerLeftRoom(roleUid, player);
                            break;
                    }
                }
            }
            else
            {
                base.OnBinaryMessageReceived(player, frame, sender);
            }
        }


        // 发送消息给服务器
        public void Send(FrameStream frame, bool reliable = false)
        {
            clientLobby.Send(frame, reliable);
        }


        // 自己主动 退出房间
        public void LeftRoom()
        {
            clientLobby.LeftRoom();
        }
        

        // 收到服务器离开房间消息
        public void OnLeftRoom()
        {

        }


    }
}
