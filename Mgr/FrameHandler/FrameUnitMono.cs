using System;
using UnityEngine;

namespace PumpFrame
{
    public class FrameUnitMono : MonoBehaviour
    {
        private FrameUnit _frameUnit;
        protected FrameUnit FrameUnit
        {
            get
            {
                if (_frameUnit == null)
                {
                    _frameUnit = GameMgr.FrameHandler.GetFrameUnit(FrameUnitQueue);
                }
                return _frameUnit;
            }
        }

        protected virtual FrameUnitQueue FrameUnitQueue
        {
            get
            {
                return FrameUnitQueue.Normal;
            }
        }

        protected virtual void Awake()
        {
        }
        protected virtual void OnEnable()
        {
            FrameUnit?.SetActive(true);
        }
        protected virtual void OnDisable()
        {
            FrameUnit?.SetActive(false);
        }
        protected virtual void OnDestroy()
        {
            GameMgr.FrameHandler.RecycleFrameUnit(FrameUnit);
            _frameUnit = null;
        }
    }
}