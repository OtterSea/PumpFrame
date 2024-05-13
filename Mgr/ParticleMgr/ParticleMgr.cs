using System;
using System.Collections;
using System.Collections.Generic;
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
        private static Dictionary<string, List<ParticleUnit>> _unitDict;

        public static void OnInit()
        {
            _unitDict = new Dictionary<string, List<ParticleUnit>>();
        }

        //更新所有激活的unit的进度
        public static void OnUpdate(float deltaTime)
        {
            foreach (var listKV in _unitDict)
            {
                foreach (var unit in listKV.Value)
                {
                    if (unit.isActive)
                    {
                        unit.OnUnitUpdate(deltaTime);
                    }
                }
            }
        }

        #region Unit单位

        private void GetParticleUnit(string key, out ParticleUnit unit)
        {
            GameObject prefab = null;
            if (!ParticleConfig.ParticleDict.TryGetValue(key, out prefab))
            {
                throw new Exception($"错误的特效key：{key}");
                return;
            }
        }

        public void PlayParticle(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            ParticleUnit unit;
            GetParticleUnit(key, out unit);
            //设置
            // unit.OnSetActive();
        }
        
        #endregion
    }
}