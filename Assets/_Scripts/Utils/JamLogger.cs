using System.Runtime.CompilerServices;
using UnityEngine;

namespace _Scripts.Utils
{
    public static class JamLogger
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private static readonly bool IsDevelopment = true;
#else
        private static readonly bool IsDevelopment = false;
#endif

        public static void LogInfo(string message, [CallerMemberName] string callerName = "")
        {
            if (IsDevelopment)
            {
                Debug.Log(FormatLogMessage("INFO", "green", message, callerName));
            }
        }

        public static void LogWarning(string message, [CallerMemberName] string callerName = "")
        {
            if (IsDevelopment)
            {
                Debug.LogWarning(FormatLogMessage("WARNING", "yellow", message, callerName));
            }
        }

        public static void LogError(string message, [CallerMemberName] string callerName = "")
        {
            if (IsDevelopment)
            {
                Debug.LogError(FormatLogMessage("ERROR", "red", message, callerName));
            }
        }

        private static string FormatLogMessage(string logType, string color, string message, string callerName)
        {
            return $"<color={color}>{logType}</color> | {callerName} | {message}";
        }
    }
}