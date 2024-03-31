using UnityEngine;
using System;

/// <summary>
/// Update(float deltaTime) - 每一帧都会被调用,用于推进表现层
/// LogicTick() - 每段逻辑帧调用，固定deltaTime(由FrameMgr定制）
///
/// 流程解释：
/// 调用GameMgr接口从FrameHandler获得一个FrameUnit对象自行保存
/// 对FrameUnit进行注册回调，在必要时刻设置NoEnable
/// 
/// 使用实例：
///
/// 
/// </summary>

namespace PumpFrame
{
    public class FrameUnit
    {
        public bool IsValid { get; private set; }
        public bool IsActive { get; private set; }

        private Action<float> _updateAction;
        private Action _logicTickAction;

        internal void Clear()
        {
            IsValid = false;
            IsActive = false;
            _updateAction = null;
            _logicTickAction = null;
        }

        internal void Init()
        {
            IsValid = true;
            IsActive = true;
        }

        internal void Update(float deltaTime)
        {
            if (IsValid && IsActive)
            {
                _updateAction?.Invoke(deltaTime);
            }
        }

        internal void LogicTick()
        {
            if (IsValid && IsActive)
            {
                _logicTickAction?.Invoke();
            }
        }

        public void RegisterUpdateAction(Action<float> callback)
        {
            _updateAction = callback;
        }

        public void RegisterLogicTick(Action callback)
        {
            _logicTickAction = callback;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }
    }
}