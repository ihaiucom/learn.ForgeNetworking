using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
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
namespace Games
{
    public class NetRoomClient : NetRoomBase
    {
        public NetRoomClient(LobbyClient lobby, NetRoomInfo roomInfo)
        {
            this.lobby = lobby;
            this.uid = roomInfo.roomUid;
            this.stageId = roomInfo.stageId;
        }


        // 收到服务器离开房间消息
        public void OnLeftRoom()
        {

        }

        // 接收二进制数据
        public void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {

        }

    }
}
