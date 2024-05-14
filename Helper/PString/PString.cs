using System.Collections;
using System.Collections.Generic;
using Codice.Client.Commands;
using Sirenix.OdinInspector;
using UnityEngine;


namespace PumpFrame
{
    /// <summary>
    /// PString(PumpString) 可以当成一个String来使用的自定义类
    /// 此类用于子类继承并设定下拉框选项，以便可以在Inspector界面上选下拉框选内容
    /// 自测确实可以序列化
    /// *暂时无用*
    /// </summary>
    public class PString
    {
        public string str;
        
        //隐式转换
        public static implicit operator string(PString p)
        {
            return p.str;
        }
    }
}