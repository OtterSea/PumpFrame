using System;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace PumpFrame
{
    //按照顺序执行frameUnit
    public enum FrameUnitQueue
    {
        Normal,     //所有普通mono，最开始更新状态、动画机
        Rigidbody,  //主要处理刚体位移（KCC）
        HitBox,     //主要处理打击盒、命中事件
    }
    
    public class FrameHandler
    {
        private FrameUnit CreateFrameUnit()
        {
            FrameUnit unit = new FrameUnit();
            unit.Clear();
            return unit;
        }

        //调用列表
        private readonly Dictionary<FrameUnitQueue, List<FrameUnit>> _unitListDict;
        private float _remainTime;

        //比率
        public static readonly int LogicFrameRate = 30;
        public static readonly float LogicFrameDeltaTime = 1.0f / 30.0f;

        public FrameHandler()
        {
            int defaultCapacity = 10;
            _remainTime = 0f;

            _unitListDict = new Dictionary<FrameUnitQueue, List<FrameUnit>>()
            {
                {FrameUnitQueue.Normal, new List<FrameUnit>(defaultCapacity)},
                {FrameUnitQueue.Rigidbody, new List<FrameUnit>(defaultCapacity)},
                {FrameUnitQueue.HitBox, new List<FrameUnit>(defaultCapacity)},
            };
        }

        internal FrameUnit GetFrameUnit(FrameUnitQueue queue = FrameUnitQueue.Normal)
        {
            List<FrameUnit> unitList;
            if (!_unitListDict.TryGetValue(queue, out unitList))
                throw new Exception($"没有此队列的unitList：{queue}");
            
            //从List寻找是否有无效的FrameUnit
            foreach (FrameUnit unit in unitList)
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
            unitList.Add(unitNew);
            return unitNew;
        }

        internal void RecycleFrameUnit(FrameUnit unit)
        {
            unit.Clear();
        }

        internal void OnUpdate(float deltaTime)
        {
            //检查是否tick逻辑帧
            bool isTickLogic = false;
            _remainTime += deltaTime;
            if (_remainTime > LogicFrameDeltaTime)
            {
                _remainTime = 0f;
                isTickLogic = true;
            }

            foreach (var unitListKv in _unitListDict)
            {
                if (isTickLogic)
                {
                    foreach (FrameUnit unit in unitListKv.Value)
                    {
                        unit.Update(deltaTime);
                        unit.LogicTick();
                    }
                }
                else
                {
                    foreach (FrameUnit unit in unitListKv.Value)
                    {
                        unit.Update(deltaTime);
                    }
                }
            }
        }
    }
}