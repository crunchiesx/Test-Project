using UnityEngine;
using System.Diagnostics;

namespace Crunchies.Utility
{
    public static class Log
    {
        // --- STANDARD LOGS ---

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(object message, Object context = null)
        {
            UnityEngine.Debug.Log("<color=#32CD32>" + message + "</color>", context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Format(string format, Object context = null, LogType logType = LogType.Log, LogOption logOptions = LogOption.None, params object[] args)
        {
            UnityEngine.Debug.LogFormat(logType, logOptions, context, "<color=#32CD32>" + format + "</color>", args);
        }

        // --- WARNINGS ---

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Warning(object message, Object context = null)
        {
            UnityEngine.Debug.LogWarning("<color=#FFD700>" + message + "</color>", context);
        }

        // --- ERRORS ---

        // Errors usually stay in production builds so you can see why a player's game crashed
        public static void Error(object message, Object context = null)
        {
            UnityEngine.Debug.LogError("<color=#FF4500>" + message + "</color>", context);
        }

        // Logs an error when a required component is missing on a GameObject.
        public static void MissingComponent(string component, Object context = null)
        {
            Error($"No {component} found!", context);
        }

        // Generic version that takes the component type as a generic parameter. Usage: Log.MissingComponent<Transform>(this);
        public static void MissingComponent<T>(Object context = null) where T : Component
        {
            MissingComponent(typeof(T).Name, context);
        }

        // Logs an error when a required reference is not assigned in the inspector.
        public static void MissingReference(string reference, Object context = null)
        {
            Error($"Missing {reference} reference!", context);
        }

        // Generic version that takes the reference type as a generic parameter. Usage: Log.MissingReference<Transform>(this);
        public static void MissingReference<T>(Object context = null) where T : Object
        {
            MissingReference(typeof(T).Name, context);
        }
    }
}