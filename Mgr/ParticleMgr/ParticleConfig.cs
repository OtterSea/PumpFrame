using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace PumpFrame
{
    [CreateAssetMenu(menuName = "PumpFrame/Create ParticleConfig", fileName = "ParticleConfig")]
    public class ParticleConfig : SingletonScriptableObject<ParticleConfig>
    {
        public static Dictionary<string, GameObject> ParticleDict =>
            Instance.particleDict;
        
        [NonSerialized, OdinSerialize]
        public Dictionary<string, GameObject> particleDict;
    }
}