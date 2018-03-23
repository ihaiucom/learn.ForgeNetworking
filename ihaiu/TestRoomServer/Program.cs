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


            LobbyServer lobby = new LobbyServer(int.MaxValue);
            NetRoomInfo roomInfo = new NetRoomInfo();
            roomInfo.roomUid = 1;
            roomInfo.stageId = 1;
            lobby.CreateRoom(roomInfo);

            while (lobby.Socket.IsBound)
            {
                read = Console.ReadLine().ToLower();
                if (read == "s" || read == "stop")
                {
                    lock (lobby)
                    {
                        Console.WriteLine("Server stopped.");
                        lobby.Dispose();
                    }
                }
            }
        }
    }
}
