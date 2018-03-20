using System;
using System.Collections.Generic;
using System.IO;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/19/2018 4:03:16 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class MasterClientParameter
    {
        public int      masterServerId    = 3;
        public string   masterServerIp    = "172.16.52.101";
        public ushort   masterServerPort  = 23001;

        public string lobbyServerIp     = "127.0.0.1";
        public ushort lobbyServerPort   = 16000;

        public void Print()
        {

            Console.WriteLine("[Master]");
            Console.WriteLine("masterServerId=" + masterServerId);
            Console.WriteLine("masterServerIp=" + masterServerIp);
            Console.WriteLine("masterServerPort=" + masterServerPort);
            Console.WriteLine("");

            Console.WriteLine("[Lobby]");
            Console.WriteLine("lobbyServerIp=" + lobbyServerIp);
            Console.WriteLine("lobbyServerPort=" + lobbyServerPort);
            Console.WriteLine("");
        }


        public static MasterClientParameter Load()
        {
            string setting = "setting.txt";
            return Load(setting);
        }

        public static MasterClientParameter Load(string path)
        {
            MasterClientParameter o = new MasterClientParameter();

            Dictionary<string, string> settingDict = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] arr = line.Split('=');
                    if (arr.Length == 2)
                    {
                        string key = arr[0].Trim();
                        string val = arr[1].Trim();
                        settingDict[key] = val;
                    }
                }
            }

            if (settingDict.ContainsKey("masterServerId"))
                o.masterServerId = Convert.ToInt32(settingDict["masterServerId"]);

            if (settingDict.ContainsKey("masterServerIp"))
                o.masterServerIp = settingDict["masterServerIp"];

            if (settingDict.ContainsKey("masterServerPort"))
                o.masterServerPort = (ushort)Convert.ToUInt32(settingDict["masterServerPort"]);



            if (settingDict.ContainsKey("lobbyServerIp"))
                o.lobbyServerIp = settingDict["lobbyServerIp"];

            if (settingDict.ContainsKey("lobbyServerPort"))
                o.lobbyServerPort = (ushort)Convert.ToUInt32(settingDict["lobbyServerPort"]);

            return o;
        }
    }
}
