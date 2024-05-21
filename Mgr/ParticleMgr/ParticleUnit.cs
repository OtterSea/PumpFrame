using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    /// <summary>
    /// 粒子系统单位，管理粒子系统的播放位置、旋转、缩放
    /// </summary>
    public class ParticleUnit
    {
        //是否激活
        public bool isActive;
        
        //粒子系统物体
        public GameObject particleGo;
        
        //粒子系统组件
        public ParticleSystem particleSys;
        
        //存活时间
        public float remainTime;

        //创建时必须调用此接口
        public void OnCreate(GameObject prefab)
        {
            isActive = false;
            remainTime = 0f;
            particleGo = GameObject.Instantiate(prefab);
            particleGo.SetActive(false);
            particleSys = particleGo.GetComponent<ParticleSystem>();
            if (particleSys == null)
            {
                throw new Exception($"特效预制体没有ParticleSystem组件:{prefab.name}");
            }
            particleSys.Stop(true);
        }

        //销毁时必须调用此接口
        public void OnDestroy()
        {
            GameObject.Destroy(particleGo);
        }
        
        public bool OnUnitUpdate(float deltaTime)
        {
            remainTime -= deltaTime;
            // particleSys.Simulate(deltaTime);
            if (remainTime <= 0f)
            {
                OnSetUnActive();
                return false;
            }
            return true;
        }

        public void OnSetActive(float rt, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            isActive = true;
            remainTime = rt;
            particleGo.transform.position = position;
            particleGo.transform.rotation = rotation;
            particleGo.transform.localScale = scale;
            particleSys.Stop(true);
            particleGo.SetActive(true);
        }

        public void OnSetUnActive()
        {
            isActive = false;
            remainTime = 0f;
            particleGo.SetActive(false);
            // particleGo.transform.position = Vector3.zero;   //移动到相机之外此时的粒子系统应该是完成播放完毕
            particleSys.Stop(true);
        }

        /// <summary>
        /// 获得粒子特效的默认播放时长，简单使用duration
        /// </summary>
        /// <returns></returns>
        public float GetParticleDuration()
        {
            return particleSys.main.duration;
            // return 0f;
        }
    }
}