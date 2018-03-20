using BeardedManStudios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ihaiu
{
    class Program
    {
        static void Main(string[] args)
        {
            string read = string.Empty;

            MasterClientParameter parameter = MasterClientParameter.Load();
            parameter.Print();


            PrintHelp();


            // 测试客户登录
            MasterClient server = new MasterClient();
            server.Init(parameter);
            server.ToggleLogging();
            server.ConnectMaster();

            while (true)
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
                else if (read == "l" || read == "log")
                {
                    if (server.ToggleLogging())
                        Console.WriteLine("Logging has been enabled");
                    else
                        Console.WriteLine("Logging has been disabled");
                }
                else if (read == "q" || read == "quit")
                {
                    lock (server)
                    {
                        Console.WriteLine("Quitting...");
                        server.Dispose();
                    }
                    break;
                }
                else if (read == "h" || read == "help")
                    PrintHelp();
                else
                    Console.WriteLine("Command not recognized, please try again");
            }
        }


        private static void PrintHelp()
        {
            Console.WriteLine(@"Commands Available
(s)top - Stops hosting
(l)og - Toggles logging (starts off)
(q)uit - Quits the application
(h)elp - Get a full list of comands");
        }
    }
}
