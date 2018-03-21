using Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDemoServer : MonoBehaviour {

    LobbyServer server;

    void Start ()
    {
        server = new LobbyServer(int.MaxValue);
    }
	
	void Update ()
    {
		
	}

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        server.Dispose();
        server = null;
    }
}
