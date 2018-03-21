﻿using Rooms.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDemoServer : MonoBehaviour {

    public LobbyServer server;

    void Start ()
    {
        server = new LobbyServer(int.MaxValue);
        NetRoomInfo roomInfo = new NetRoomInfo();
        roomInfo.roomUid = 1;
        roomInfo.stageId = 1;
        server.CreateRoom(roomInfo);
    }
	

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        server.Dispose();
        server = null;
    }
}
