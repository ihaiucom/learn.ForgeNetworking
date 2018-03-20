using Rooms.Forge.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRoomClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string read = string.Empty;

            LobbyClient client = new LobbyClient();

            while (client.Socket.IsBound)
            {
                read = Console.ReadLine().ToLower();
                if (read == "s" || read == "stop")
                {
                    lock (client)
                    {
                        Console.WriteLine("Server stopped.");
                        client.Dispose();
                    }
                }
            }
        }
    }
}
