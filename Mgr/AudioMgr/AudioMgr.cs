using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    public class AudioMgr : Singleton<ParticleMgr>
    {


        public static void JustPlayAudio(string key)
        {
            Debuger.Print($"AudioMgr-播放音频：{key}");
        }
    }
}