using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PumpFrame
{
    /// <summary>
    /// 本SO文件在硬盘中仅存在一份，用于定义所有的特效
    /// todo:应该改为动态从硬盘资源包中加载prefab才对
    /// todo:这个迟早改掉修复 不应该这样子
    /// </summary>
    [CreateAssetMenu(menuName = "PumpFrame/Create ParticleConfig", fileName = "ParticleConfig")]
    public class ParticleConfig : SingletonScriptableObject<ParticleConfig>
    {
        public static Dictionary<string, GameObject> ParticleDict =>
            Instance.particleDict;
        
        public Dictionary<string, GameObject> particleDict;
        
        public static string[] GetKeyList()
        {
            return ParticleDict.Keys.ToArray();
        }

        public static GameObject GetParticlePrefab(string key)
        {
            GameObject go;
            if (!ParticleConfig.ParticleDict.TryGetValue(key, out go))
            {
                throw new Exception($"错误的特效key：{key}");
            }
            return go;
        }
    }
}