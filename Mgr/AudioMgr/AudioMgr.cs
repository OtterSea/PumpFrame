using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    public class AudioMgr : Singleton<ParticleMgr>
    {
        public static GameObject _gameMgr;
        
        public static void OnInit()
        {
            _gameMgr = GameMgr.Instance.gameObject;
            _gameMgr.AddComponent<AkInitializer>();
            //test
            LoadSoundBank("TestSoundBank");
        }

        /// <summary>
        /// 加载SoundBank
        /// </summary>
        /// <param name="bankName"></param>
        public static void LoadSoundBank(string bankName)
        {
            AkBankManager.LoadBank(bankName, false, false);
        }

        /// <summary>
        /// 卸载一个SoundBank
        /// </summary>
        /// <param name="bankName"></param>
        public static void UnLoadSoundBank(string bankName)
        {
            
        }
        
        /// <summary>
        /// 播放非3D特效的音频
        /// </summary>
        /// <param name="key"></param>
        public static void JustPlay2DAudio(string key)
        {
            // Debuger.Print($"AudioMgr-播放音频2D：{key}");
            // AkSoundEngine.PostEvent(key, _gameMgr);
        }

        /// <summary>
        /// 在特定GameObject上播放3D音频
        /// </summary>
        /// <param name="key"></param>
        /// <param name="go"></param>
        public static void JustPlay3DAudio(string key, GameObject posGo)
        {
            // Debuger.Print($"AudioMgr-播放音频3D：{key}");
            //从对象池拿一个播放音频的对象，依据posGo的位置进行摆放并播放音频
            AkSoundEngine.PostEvent(key, posGo);
        }
    }
}