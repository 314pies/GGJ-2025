/* 
 * Headless Builder
 * (c) Salty Devs, 2022
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */


#if (HEADLESS && HEADLESS_STRIPLOGGING)
#undef ALLOW_LOGGING_INTERNAL
#define BLOCK_LOGGING_INTERNAL
#else
#define ALLOW_LOGGING_INTERNAL
#undef BLOCK_LOGGING_INTERNAL
#endif

using UnityEngine;
using System;


public static class Debug
{
    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    public static bool developerConsoleVisible
    {
        get { return UnityEngine.Debug.developerConsoleVisible; }
        set { UnityEngine.Debug.developerConsoleVisible = value; }
    }

#if UNITY_2017_1_OR_NEWER
    public static ILogger unityLogger
    {
        get { return UnityEngine.Debug.unityLogger; }
    }
#endif


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition)
    {
        UnityEngine.Debug.Assert(condition);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, UnityEngine.Object context)
    {
        UnityEngine.Debug.Assert(condition, context);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, object message)
    {
        UnityEngine.Debug.Assert(condition, message);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Assert(condition, message, context);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, format, args);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, context, format, args);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Break()
    {
        UnityEngine.Debug.Break();
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void ClearDeveloperConsole()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogAssertion(object message)
    {
        UnityEngine.Debug.LogAssertion(message);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogAssertion(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogAssertion(message, context);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, args);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

#if UNITY_2019_1_OR_NEWER
#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
    }
#endif


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }


#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

#if BLOCK_LOGGING_INTERNAL
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
#endif
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

}