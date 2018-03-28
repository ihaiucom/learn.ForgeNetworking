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
namespace ihaiu.VoipServer
{
    public class ProgramSetting
    {
        public string   voipMasterServerIp    = "127.0.0.1";
        public ushort   voipMasterServerPort  = 15000;

        public string voipLobbyServerIp     = "127.0.0.1";
        public ushort voipLobbyServerPort   = 15101;

        public void Print()
        {
            Console.WriteLine("[VoipMaster]");
            Console.WriteLine("voipMasterServerIp=" + voipMasterServerIp);
            Console.WriteLine("voipMasterServerPort=" + voipMasterServerPort);
            Console.WriteLine("");

            Console.WriteLine("[VoipLobby]");
            Console.WriteLine("voipLobbyServerIp=" + voipLobbyServerIp);
            Console.WriteLine("voipLobbyServerPort=" + voipLobbyServerPort);
            Console.WriteLine("");
        }

        public static string path = "setting.txt";
        public static bool IsExists()
        {
            return File.Exists(path);
        }

        public void Generate()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("[VoipMaster]");
            sw.WriteLine("voipMasterServerIp=" + voipMasterServerIp);
            sw.WriteLine("voipMasterServerIp=" + voipMasterServerIp);
            sw.WriteLine("");

            sw.WriteLine("[VoipLobby]");
            sw.WriteLine("voipLobbyServerIp=" + voipLobbyServerIp);
            sw.WriteLine("voipLobbyServerPort=" + voipLobbyServerPort);
            sw.WriteLine("");

            File.WriteAllText(path, sw.ToString());
        }


        public static ProgramSetting Load()
        {
            return Load(path);
        }

        public static ProgramSetting Load(string path)
        {
            ProgramSetting o = new ProgramSetting();

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


            if (settingDict.ContainsKey("voipMasterServerIp"))
                o.voipMasterServerIp = settingDict["voipMasterServerIp"];

            if (settingDict.ContainsKey("voipMasterServerPort"))
                o.voipMasterServerPort = (ushort)Convert.ToUInt32(settingDict["voipMasterServerPort"]);



            if (settingDict.ContainsKey("voipLobbyServerIp"))
                o.voipLobbyServerIp = settingDict["voipLobbyServerIp"];

            if (settingDict.ContainsKey("voipLobbyServerPort"))
                o.voipLobbyServerPort = (ushort)Convert.ToUInt32(settingDict["voipLobbyServerPort"]);

            return o;
        }
    }
}
