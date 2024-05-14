using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PumpFrame
{
    /// <summary>
    /// 粒子管理系统，本系统管理游戏中所有的粒子系统，主要是指一次性播放的，非循环的粒子系统
    /// 循环的粒子系统，如子弹投射，场景火焰，直接编辑放置粒子系统预制体即可
    /// 提供一个接口：外界传入需求的粒子系统的key，和世界空间位置、旋转，即可在特定世界空间产生特定特定粒子系统并播放
    /// 播放完毕后被本类回收到对象池中
    /// </summary>
    public class ParticleMgr : Singleton<ParticleMgr>
    {
        private static Dictionary<string, ParticleUnitPool> _UnitPoolDict;

        public static void OnInit()
        {
            _UnitPoolDict = new Dictionary<string, ParticleUnitPool>();
        }

        //更新所有激活的unit的进度
        public static void OnUpdate(float deltaTime)
        {
            foreach (var listKv in _UnitPoolDict)
            {
                listKv.Value.OnUpdateUnit(deltaTime);
            }
        }

        #region Unit单位

        private ParticleUnitPool GetParticleUnitPool(string key)
        {
            ParticleUnitPool pool = null;
            if (!_UnitPoolDict.TryGetValue(key, out pool))
            {
                pool = new ParticleUnitPool(key);
                _UnitPoolDict.Add(key, pool);
            }
            return pool;
        }

        private void GetParticleUnit(string key, out ParticleUnit unit)
        {
            var unitPool = this.GetParticleUnitPool(key);
            unit = unitPool.UnitPool.Get();
        }

        public void PlayParticle(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            ParticleUnit unit;
            GetParticleUnit(key, out unit);
            unit.OnSetActive(unit.GetParticleDuration(), position, rotation, scale);
        }
        
        public void PlayParticle(string key, float duration, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            ParticleUnit unit;
            GetParticleUnit(key, out unit);
            unit.OnSetActive(duration, position, rotation, scale);
        }
        
        #endregion
    }
}