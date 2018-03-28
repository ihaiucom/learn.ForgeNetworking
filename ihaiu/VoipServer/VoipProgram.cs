using Rooms.Forge.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ihaiu.VoipServer
{
    public class VoipProgram
    {
        static void Main(string[] args)
        {
            ProgramSetting setting = ProgramSetting.Load();
            if (!ProgramSetting.IsExists())
            {
                setting.Generate();
            }

            string read = string.Empty;

            LobbyServer lobby = new LobbyServer(int.MaxValue);
            lobby.Connect(setting.voipLobbyServerIp, setting.voipLobbyServerPort);


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
