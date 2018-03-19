using BeardedManStudios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ihaiu
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "172.16.52.101";
            ushort port = 13002;
            string read = string.Empty;
            bool setInputArg = false;

            Dictionary<string, string> arguments = ArgumentParser.Parse(args);

            if (args.Length > 0)
            {
                if (arguments.ContainsKey("host"))
                    host = arguments["host"];

                if (arguments.ContainsKey("port"))
                    ushort.TryParse(arguments["port"], out port);

            }
            else if(setInputArg)
            {
                Console.WriteLine("不输入参数将会是默认的设置.");
                Console.WriteLine("Enter Host IP (Default: 172.16.52.101):");
                read = Console.ReadLine();
                if (string.IsNullOrEmpty(read))
                    host = "172.16.52.101";
                else
                    host = read;

                Console.WriteLine("Enter Port (Default: 13002):");
                read = Console.ReadLine();
                if (string.IsNullOrEmpty(read))
                    port = 13002;
                else
                {
                    ushort.TryParse(read, out port);
                }
            }

            Console.WriteLine(string.Format("Hosting ip [{0}] on port [{1}]", host, port));
            PrintHelp();


            // 测试客户登录
            MasterClient server = new MasterClient();
            server.ToggleLogging();
            server.Connect(host, 13002);

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
                else if (read == "r" || read == "restart")
                {
                    lock (server)
                    {
                        if (server.IsRunning)
                        {
                            Console.WriteLine("Server stopped.");
                            server.Dispose();
                        }
                    }

                    Console.WriteLine("Restarting...");
                    Console.WriteLine(string.Format("Hosting ip [{0}] on port [{1}]", host, port));
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
(r)estart - Restarts the hosting service even when stopped
(l)og - Toggles logging (starts off)
(q)uit - Quits the application
(h)elp - Get a full list of comands");
        }
    }
}
