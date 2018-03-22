using BeardedManStudios.Forge.Networking;
using Rooms.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDemoClient : MonoBehaviour {

    public LobbyClient lobby;

    void Start ()
    {
        Rpc.MainThreadRunner = UnityMainThread.Instance;

        lobby = new LobbyClient();
        NetRoomInfo roomInfo = new NetRoomInfo();
        roomInfo.roomUid = 1;
        roomInfo.stageId = 1;
        lobby.roomInfo = roomInfo;

        NetRoleInfo roleInfo = new NetRoleInfo();
        roleInfo.uid = 1;
        roleInfo.name = "ZF";
        lobby.roleInfo = roleInfo;

        lobby.Socket.serverAccepted += OnServerAccepted;

        //lobby.TestCreateRoom(roomInfo);
    }

    // 服务器接收连接了
    private void OnServerAccepted(NetWorker sender)
    {

        Loger.LogFormat("LobbyClient OnServerAccepted {0}", sender);
        lobby.JoinRoom();
    }


    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        lobby.Dispose();
        lobby = null;
    }
}
