using UnityEngine;
using System.Collections;
using Games;
using System;
using System.IO;

namespace com.ihaiu
{
    public class ConfigSetting 
    {


		public static void Load(string filename, Action<string, string> callback)
        {
#if UNITY_EDITOR
            if (Game.asset == null)
            {

                if (callback != null)
                {
                    filename = AssetManagerSetting.EditorGetConfigPath(filename);
                    callback(filename, File.ReadAllText(filename));
                }
                return;
            }
#elif UNITY
            Game.asset.LoadConfig(filename, callback);
#else

            string path = GetConfigPath(filename);
            if (File.Exists(path))
            {
                callback(filename, File.ReadAllText(path));
            }
            else
            {
                Loger.LogErrorFormat("没有找到配置文件 {0}", path);
            }
#endif
        }


        /** 获取Config文件路径 */
        public static string GetConfigPath(string path)
        {
            path = Game.setting.configPath + path;
            string result = path + ".csv";
            if (File.Exists(result))
                return result;


            result = path + ".json";
            if (File.Exists(result))
                return result;

            result = path + ".txt";
            if (File.Exists(result))
                return result;

            return path;
        }

    }
}
