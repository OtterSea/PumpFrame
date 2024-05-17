using System;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace PumpFrame
{
    //按照顺序执行frameUnit
    public enum FrameUnitQueue
    {
        Normal = 0,     //玩家控制器，输入，AI逻辑 PlayerController AIController等处理
        Rigidbody,      //主要处理刚体位移（KCC）
        HitBox,         //主要处理打击盒，命中事件 (为什么无人使用
        View,           //可视物动画更新，特效更新等
    }
    
    public class FrameHandler
    {
        //调用列表
        private readonly List<FrameUnitQueueItem> _queueItemList;
        private float _remainTime;

        //比率
        public static readonly int LogicFrameRate = 30;
        public static readonly float LogicFrameDeltaTime = 0.033334f;

        public FrameHandler()
        {
            int defaultCapacity = 10;
            _remainTime = 0f;
            
            _queueItemList = new List<FrameUnitQueueItem>();
            string[] enumLength = System.Enum.GetNames(typeof(FrameUnitQueue));
            for (int i = 0; i < enumLength.Length; i++)
            {
                _queueItemList.Add(new FrameUnitQueueItem(defaultCapacity));
            }
        }

        internal FrameUnit GetFrameUnit(FrameUnitQueue queue = FrameUnitQueue.Normal)
        {
            FrameUnitQueueItem queueItem = _queueItemList[(int)queue];
            return queueItem.GetFrameUnit();
        }

        internal void RecycleFrameUnit(FrameUnit unit)
        {
            unit.Clear();
        }

        /// <summary>
        /// GameMgr负责调用，更新所有Queue
        /// </summary>
        /// <param name="deltaTime"></param>
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

            foreach (var queueItem in _queueItemList)
            {
                queueItem.OnQueueItemUpdate(deltaTime, isTickLogic);
            }
        }

        /// <summary>
        /// 冻结某个Queue队列的执行，freezeLogicCount - 冻结逻辑帧时间
        /// </summary>
        /// <param name="freezeLogicCount"></param>
        public void FreezeFrameUnitQueue(FrameUnitQueue queue, int freezeLogicCount)
        {
            FrameUnitQueueItem queueItem = _queueItemList[(int)queue];
            queueItem.FreezeFrameUnitQueue(freezeLogicCount);
        }
    }
}