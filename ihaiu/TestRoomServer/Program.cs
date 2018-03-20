using Rooms.Forge.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestRoomServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string read = string.Empty;

            LobbyServer server = new LobbyServer(100);

            while(server.Socket.IsBound)
            {
                read = Console.ReadLine().ToLower();
                if (read == "s" || read == "stop")
                {
                    lock (server)
                    {
                        Console.WriteLine("Server stopped.");
                        server.Dispose();
                    }
                }
            }
        }
    }
}
