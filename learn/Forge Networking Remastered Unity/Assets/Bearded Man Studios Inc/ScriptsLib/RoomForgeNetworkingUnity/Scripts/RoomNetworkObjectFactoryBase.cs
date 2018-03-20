using UnityEngine;
using System.Collections;
using System;
using Rooms.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;

namespace Rooms.Forge.Networking
{
	public class NetworkObjectFactoryBase : INetworkObjectFactory
    {
		public virtual void NetworkCreateObject(RoomScene networker, int identity, uint id, FrameStream frame, System.Action<NetworkObject> callback)
		{
            // 这是完全失败之前的最终创建检查
            //This is the final creation check before failing completely
            NetworkObject obj = null;

			//switch (identity)
   //         {
   //             case Lobby.LobbyService.LobbyServiceNetworkObject.IDENTITY:
   //                 obj = new Lobby.LobbyService.LobbyServiceNetworkObject(networker, id, frame);
   //                 break;
   //         }

			if (callback != null)
				callback(obj);
		}
	}
}
