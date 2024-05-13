using UnityEngine.Events;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * 优化GameMgr
 */

namespace PumpFrame
{
    public class GameMgr : SingletonMono<GameMgr>
    {
        private FrameHandler _frameHandler;
        public static FrameHandler FrameHandler
        {
            get
            {
                if (Instance._frameHandler == null)
                {
                    Instance._frameHandler = new FrameHandler();
                }

                return Instance._frameHandler;
            }
        }

        private void Start()
        {
            ParticleMgr.OnInit();
        }

        private void Update()
        {
            FrameHandler.OnUpdate(Time.deltaTime);
            ParticleMgr.OnUpdate(Time.deltaTime);
        }

        #region GameMgr功能

        /// <summary>
        /// 关闭游戏
        /// </summary>
        public static void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}