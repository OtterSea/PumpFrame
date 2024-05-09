using System;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace PumpFrame
{
    //按照顺序执行frameUnit
    public enum FrameUnitQueue
    {
        Normal = 0,     //玩家控制器，输入等处理
        Rigidbody,      //主要处理刚体位移（KCC）
        HitBox,         //主要处理打击盒，命中事件
        View,           //可视物动画更新，特效更新等
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
        private readonly List<List<FrameUnit>> _unitList;
        private float _remainTime;

        //比率
        public static readonly int LogicFrameRate = 30;
        public static readonly float LogicFrameDeltaTime = 0.033334f;

        public FrameHandler()
        {
            int defaultCapacity = 10;
            _remainTime = 0f;
            
            _unitList = new List<List<FrameUnit>>();
            string[] enumLength = System.Enum.GetNames(typeof(FrameUnitQueue));
            for (int i = 0; i < enumLength.Length; i++)
            {
                _unitList.Add(new List<FrameUnit>(defaultCapacity));
            }
        }

        internal FrameUnit GetFrameUnit(FrameUnitQueue queue = FrameUnitQueue.Normal)
        {
            List<FrameUnit> unitList = _unitList[(int)queue];
            
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

            foreach (var unitList in _unitList)
            {
                if (isTickLogic)
                {
                    foreach (FrameUnit unit in unitList)
                    {
                        unit.Update(deltaTime);
                        unit.LogicTick();
                    }
                }
                else
                {
                    foreach (FrameUnit unit in unitList)
                    {
                        unit.Update(deltaTime);
                    }
                }
            }
        }
    }
}