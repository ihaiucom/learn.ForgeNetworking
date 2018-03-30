using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Debug
	{
		

		public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
		}

		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
		}

		public static void DrawLine(Vector3 start, Vector3 end, Color color)
		{
		}

		public static void DrawLine(Vector3 start, Vector3 end)
		{
		}

		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
		{
		}

		public static void DrawRay(Vector3 start, Vector3 dir, Color color)
		{
		}

		public static void DrawRay(Vector3 start, Vector3 dir)
		{
		}

		public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			Debug.DrawLine(start, start + dir, color, duration, depthTest);
		}

		public static  void Break()
        {

        }

		public static  void DebugBreak()
        {

        }

		public static void Log(object message)
		{
		}

		public static void Log(object message, Object context)
		{
		}

		public static void LogFormat(string format, params object[] args)
		{
		}

		public static void LogFormat(Object context, string format, params object[] args)
		{
		}

		public static void LogError(object message)
		{
		}

		public static void LogError(object message, Object context)
		{
		}

		public static void LogErrorFormat(string format, params object[] args)
		{
		}

		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
		}

		public static  void ClearDeveloperConsole()
        {

        }

		public static void LogException(Exception exception)
		{
		}

		public static void LogException(Exception exception, Object context)
		{
		}

		public static void LogWarning(object message)
		{
		}

		public static void LogWarning(object message, Object context)
		{
		}

		public static void LogWarningFormat(string format, params object[] args)
		{
		}

		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, Object context)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message, Object context)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message, Object context)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, Object context, string format, params object[] args)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message, Object context)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(string format, params object[] args)
		{
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(Object context, string format, params object[] args)
		{
		}


		[Conditional("UNITY_ASSERTIONS"), Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
		public static void Assert(bool condition, string format, params object[] args)
		{
		}
	}
}
