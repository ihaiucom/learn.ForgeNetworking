using BeardedManStudios.Forge.Networking;
using Rooms.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDemoServer : MonoBehaviour {

    public LobbyServer lobby;

    void Start ()
    {
        Rpc.MainThreadRunner = UnityMainThread.Instance;
        lobby = new LobbyServer(int.MaxValue);
        NetRoomInfo roomInfo = new NetRoomInfo();
        roomInfo.roomUid = 1;
        roomInfo.stageId = 1;
        lobby.CreateRoom(roomInfo);
    }
	

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        if(lobby != null)
            lobby.Dispose();
        lobby = null;
    }
}
