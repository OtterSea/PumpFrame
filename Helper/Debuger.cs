using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using System.Reflection;
using Sirenix.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Edit -> ProjectSettings -> Player -> OtherSettings -> ScriptCompilation：
/// 添加程序关键字 PUMPBUG 才能看到 debug 输出
///
/// tips:
/// 主动抛出错误事件：
/// throw new EncoderFallbackException("这里写入自定义的报错文本");
/// </summary>

namespace PumpFrame
{
    public class Debuger
    {
        [Conditional("PUMPBUG")]
        public static void Print(object obj)
        {
            Debug.Log(obj);
        }

        [Conditional("PUMPBUG")]
        public static void Error(object obj)
        {
            Debug.LogError(obj);
        }

        [Conditional("PUMPBUG")]
        public static void Warning(object obj)
        {
            Debug.LogWarning(obj);
        }

        [Conditional("PUMPBUG")]
        public static void Assert(bool condition, string msg, Object context = null)
        {
            Debug.Assert(condition, msg, context);
        }

        [Conditional("PUMPBUG")]
        public static void DumpObject<T>(T obj) where T : new()
        {
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields) {
                Debuger.Print($"dump member-name: {field.Name} - {field.GetValue(obj)}");
            }
        }
        
        /// <summary>
        /// 暂停游戏
        /// </summary>
        public static void PausedGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
        }

        /// <summary>
        /// 恢复暂停游戏
        /// </summary>
        public static void UnPausedGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = false;
#endif
        }
    }
}