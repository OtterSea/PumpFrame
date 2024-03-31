using System;
using UnityEngine;

namespace PumpFrame
{
    public class FrameUnitMono : MonoBehaviour
    {
        protected FrameUnit frameUnit;

        protected virtual FrameUnitQueue FrameUnitQueue
        {
            get
            {
                return FrameUnitQueue.Normal;
            }
        }

        protected virtual void Awake()
        {
            frameUnit = GameMgr.FrameHandler.GetFrameUnit(FrameUnitQueue);
        }
        protected virtual void OnEnable()
        {
            frameUnit?.SetActive(true);
        }
        protected virtual void OnDisable()
        {
            frameUnit?.SetActive(false);
        }
        protected virtual void OnDestroy()
        {
            GameMgr.FrameHandler.RecycleFrameUnit(frameUnit);
            frameUnit = null;
        }
    }
}