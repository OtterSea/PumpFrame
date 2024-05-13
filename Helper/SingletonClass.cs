using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace PumpFrame
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
    
    public abstract class SingletonMono<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    // [CreateAssetMenu(menuName = "PumpFrame/Create GameSetting", fileName = "PumpGameSetting")]
    public abstract class SingletonScriptableObject<T> : SerializedScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        protected static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                    {
                        throw new System.Exception("没有在Resources文件夹中找到对应的SO文件");
                    }
                    else if (assets.Length > 1)
                    {
                        throw new System.Exception("单例SO文件存在多个");
                    }
                    _instance = assets[0];
                }
                return _instance;
            }
        }
    }
}
