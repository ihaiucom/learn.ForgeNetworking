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

        public NetRoomClient(LobbyClient lobby, IRoomInfo roomInfo)
        {
            this.clientLobby = lobby;

            Initialize(lobby, roomInfo);
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
                    OnPlayerListEvent(playerList);
                }
                else
                {

                    switch (routerId)
                    {
                        case RouterIds.ROOM_JOIN_ROOM:
                            IRoleInfo roleInfo = NetRoleInfo.Read(frame.StreamData);
                            OnPlayerJoinRoom(roleInfo, player);
                            break;

                        case RouterIds.ROOM_LEFT_ROOM:
                            ulong roleUid = frame.StreamData.GetBasicType<ulong>();
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
