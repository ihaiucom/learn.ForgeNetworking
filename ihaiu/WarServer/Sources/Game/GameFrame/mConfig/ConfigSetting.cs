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

#endif
            Game.asset.LoadConfig(filename, callback);
        }
    }
}
