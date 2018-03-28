using BeardedManStudios.Forge.MVCWebServer;
using BeardedManStudios.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppWeb
{
    NetWorker server;
    ForgeWebServer ws;
    public void Start()
    {

        server = new TCPServer(1000);
        ((TCPServer)server).Connect();


        server.playerTimeout += (player, sender) =>
        {
            Debug.Log("Player " + player.NetworkId + " timed out");
        };

        string pathToFiles = "fnwww/html";
        Dictionary<string, string> pages = new Dictionary<string, string>();
        TextAsset[] assets = Resources.LoadAll<TextAsset>(pathToFiles);
        foreach (TextAsset a in assets)
            pages.Add(a.name, a.text);

        ws = new ForgeWebServer(server, pages, ForgeWebServer.DEFAULT_PORT);
        ws.Start();
    }

    public void Stop()
    {
        if (ws == null)
            return;

        ws.Stop();
        server.Disconnect(true);
    }
}
