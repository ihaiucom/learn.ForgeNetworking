using System;
using System.Collections.Generic;
using System.Diagnostics;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 2:04:09 PM
*  @Description:    
* ==============================================================================
*/

namespace Games.SkillTrees
{

    public static class Debug
    {
        [Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void CheckEqual<T>(T l, T r)
        {
            if (!EqualityComparer<T>.Default.Equals(l, r))
            {
                Break("CheckEqualFailed");
            }
        }

        [Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void Check(bool b)
        {
            if (!b)
            {
                Break("CheckFailed");
            }
        }

        [Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void Check(bool b, string message)
        {
            if (!b)
            {
                Break(message);
            }
        }

        [Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void Check(bool b, string format, object arg0)
        {
            if (!b)
            {
                string message = string.Format(format, arg0);
                Break(message);
            }
        }

        [Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void Break(string msg)
        {
            LogError(msg);

    #if !BEHAVIAC_NOT_USE_UNITY
            UnityEngine.Debug.Break();
            //System.Diagnostics.Debug.Assert(false);
    #else
                //throw new Exception();
                System.Diagnostics.Debug.Assert(false);
    #endif
        }




        //[Conditional("BEHAVIAC_DEBUG")]
        [Conditional("UNITY_EDITOR")]
        public static void Log(string message)
        {
    #if !BEHAVIAC_NOT_USE_UNITY
            UnityEngine.Debug.Log(message);
    #else
                Console.WriteLine(message);
    #endif
        }

        //[Conditional("UNITY_EDITOR")]
        public static void LogWarning(string message)
        {
    #if !BEHAVIAC_NOT_USE_UNITY
            UnityEngine.Debug.LogWarning(message);
    #else
                Console.WriteLine(message);
    #endif
        }

        //[Conditional("UNITY_EDITOR")]
        public static void LogError(string message)
        {
            //LogManager.Instance.Flush(null);
    #if !BEHAVIAC_NOT_USE_UNITY
            UnityEngine.Debug.LogError(message);
    #else
                Console.WriteLine(message);
    #endif
        }

        //[Conditional("UNITY_EDITOR")]
        public static void LogError(Exception ex)
        {
            //LogManager.Instance.Flush(null);
    #if !BEHAVIAC_NOT_USE_UNITY
            UnityEngine.Debug.LogError(ex.Message);
    #else
                Console.WriteLine(ex.Message);
    #endif
        }
    }
}