using Games.Wars;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrueSync;
using UnityEngine;

public class TSLoger
{
    public static string TSInfo()
    {
        return string.Format("Ticks={0}, Time={1},  time={2}, frameCount={3}    ", TrueSyncManager.Ticks, TrueSyncManager.Time, War.currentRoom.LTime.time, War.currentRoom.LTime.frameCount);
    }

    private static StringBuilder _sb;
    private static StringBuilder sb
    {
        get
        {
            if(_sb == null)
            {
                _sb = new StringBuilder();
            }
            return _sb;
        }
    }


    public static void Log(object message)
    {
        //string ts = TSInfo();
        //sb.AppendLine(ts + message);
        //Debug.Log(ts + message);
    }


    public static void LogFormat(string format, params object[] args)
    {
        //Log(string.Format(format, args));
    }

    public static void Reset()
    {
        sb.Remove(0, sb.Length);
        sb.Length = 0;
    }

    public static void Save()
    {
        //File.WriteAllText("../ts.log", sb.ToString());
    }
}
