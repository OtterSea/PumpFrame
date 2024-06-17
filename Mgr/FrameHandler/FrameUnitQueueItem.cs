using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    public class FrameUnitQueueItem
    {
        public List<FrameUnit> _unitList;

        public int freezeLogicCount;

        public FrameUnitQueueItem(int defaultCapacity)
        {
            _unitList = new List<FrameUnit>(defaultCapacity);
            freezeLogicCount = 0;
        }
        
        private FrameUnit CreateFrameUnit()
        {
            FrameUnit unit = new FrameUnit();
            unit.Clear();
            return unit;
        }
        
        internal FrameUnit GetFrameUnit()
        {
            //从List寻找是否有无效的FrameUnit
            foreach (FrameUnit unit in _unitList)
            {
                if (!unit.IsValid)
                {
                    unit.Init();
                    return unit;
                }
            }

            //从对象池获得FrameUnit
            FrameUnit unitNew = this.CreateFrameUnit();
            unitNew.Init();
            _unitList.Add(unitNew);
            return unitNew;
        }

        internal void OnQueueItemUpdate(float deltaTime, bool isTickLogic)
        {
            if (isTickLogic)
            {
                if (this.freezeLogicCount > 0)
                {
                    this.freezeLogicCount--;
                    return;
                }
                for (int i = 0; i < _unitList.Count; i++)
                {
                    var unit = _unitList[i];
                    unit.Update(deltaTime);
                    unit.LogicTick();
                }
            }
            else
            {
                if (this.freezeLogicCount > 0)
                {
                    return;
                }
                for (int i = 0; i < _unitList.Count; i++)
                {
                    var unit = _unitList[i];
                    unit.Update(deltaTime);
                }
            }
        }

        internal void FreezeFrameUnitQueue(int freezeLogicCount)
        {
            this.freezeLogicCount = freezeLogicCount;
        }
    }
}