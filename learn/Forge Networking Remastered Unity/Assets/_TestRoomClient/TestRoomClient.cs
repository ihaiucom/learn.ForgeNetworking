using Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomClient : MonoBehaviour {

    public LobbyClient client;
	void Start ()
    {
        client = new LobbyClient();

    }
	
	void Update () {
		
	}


    private void OnApplicationQuit()
    {
        client.Dispose();
    }
}
