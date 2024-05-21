using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PumpFrame
{
    public class ParticleUnitPool
    {
        private string _particleKey;
        private GameObject _particlePrefab;

        private ObjectPool<ParticleUnit> _unitPool;
        public ObjectPool<ParticleUnit> UnitPool => _unitPool;

        private List<ParticleUnit> _activeList;
        public List<ParticleUnit> ActiveList => _activeList;

        public ParticleUnitPool(string key)
        {
            _particleKey = key;
            _particlePrefab = ParticleConfig.GetParticlePrefab(key);
            _activeList = new List<ParticleUnit>();
            
            _unitPool = new ObjectPool<ParticleUnit>(
                CreateUnit,
                null,
                null,
                OnDestroyUnit
            );
        }

        public void OnDestroy()
        {
            _particleKey = null;
            _particlePrefab = null;
            UnitPool.Clear();
            ActiveList.Clear();
        }

        private ParticleUnit CreateUnit()
        {
            ParticleUnit unit = new ParticleUnit();
            unit.OnCreate(_particlePrefab);
            return unit;
        }

        // 改为外界处理setActive
        // private void OnGetUnit(ParticleUnit unit)
        // {
        // }

        // 外界处理
        // private void OnReleaseUnit(ParticleUnit unit)
        // {
        // }

        private void OnDestroyUnit(ParticleUnit unit)
        {
            unit.OnDestroy();
        }

        public void OnUpdateUnit(float deltaTime)
        {
            for (int i = ActiveList.Count - 1; i >= 0; i--)
            {
                ParticleUnit unit = ActiveList[i];
                if (!unit.OnUnitUpdate(deltaTime))
                {
                    UnitPool.Release(unit);
                    ActiveList.RemoveAt(i);
                }
            }
        }
        
        public void SetOneUnitActive(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var unit = UnitPool.Get();
            unit.OnSetActive(unit.GetParticleDuration(), position, rotation, scale);
            ActiveList.Add(unit);
        }

        public void SetOneUnitActive(float duration, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var unit = UnitPool.Get();
            unit.OnSetActive(duration, position, rotation, scale);
            ActiveList.Add(unit);
        }
    }
}

