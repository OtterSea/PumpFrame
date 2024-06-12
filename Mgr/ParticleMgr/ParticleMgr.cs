using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static Dictionary<string, ParticleUnitPool> _unitPoolDict;

        public static void OnInit()
        {
            _unitPoolDict = new Dictionary<string, ParticleUnitPool>();
        }

        //更新所有激活的unit的进度
        public static void OnUpdate(float deltaTime)
        {
            foreach (var listKv in _unitPoolDict)
            {
                listKv.Value.OnUpdateUnit(deltaTime);
            }
        }

        #region Unit单位

        private ParticleUnitPool GetParticleUnitPool(string key)
        {
            ParticleUnitPool pool = null;
            if (!_unitPoolDict.TryGetValue(key, out pool))
            {
                pool = new ParticleUnitPool(key);
                _unitPoolDict.Add(key, pool);
            }
            return pool;
        }

        /// <summary>
        /// 在一个特定的地方播放一个特效，持续时间由特效的duration决定
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void PlayParticle(string key, Vector3 position, Quaternion rotation)
        {
            var unitPool = Instance.GetParticleUnitPool(key);
            unitPool.SetOneUnitActive(position, rotation);
        }
        
        /// <summary>
        /// 在一个特定地方播放一个特效，持续时间自己输入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="duration"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void PlayParticle(string key, float duration, Vector3 position, Quaternion rotation)
        {
            var unitPool = Instance.GetParticleUnitPool(key);
            unitPool.SetOneUnitActive(duration, position, rotation);
        }

        /// <summary>
        /// 依据指定key从对象池中获得对应的粒子效果unit供给外界使用
        /// 由外界管理生命周期（用于持续一段时间的跟踪在某个GO上）
        /// 生命周期结束时，调用Release释放对应的unit
        /// </summary>
        /// <param name="key"></param>
        public static ParticleUnit GetParticleUnit(string key)
        {
            var unitPool = Instance.GetParticleUnitPool(key);
            var unit = unitPool.GetOneUnit();
            unit.OnSetActive();
            return unit;
        }

        /// <summary>
        /// 释放unit到对象池中，并设置其为无父对象，不可见
        /// </summary>
        /// <param name="unit"></param>
        public static void ReleaseParticleUnit(ParticleUnit unitGo)
        {
            unitGo.OnSetUnActive();
        }
        
        #endregion
    }
}