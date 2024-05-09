using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    public class CommonHelper
    {
        #region 向量计算

        //获得2d向量的角度 从平面俯视来看 角度是顺时针从y轴正方向旋转得到的大角度
        public static float GetAngle(Vector2 direction)
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360f - angle : angle;
        }

        #endregion

        #region 数学相关
        
        //C#的数字并没有bool含义
        public static int BoolToInteger(bool value)
        {
            if (value)
                return 1;
            return 0;
        }
        public static bool IntToBool(int value)
        {
            return value != 0;
        }
        
        #endregion
        
        #region C#类相关

        /// <summary>
        /// 深拷贝一个C#类对象，有空测一下是否能测Class类成员对象
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepCloneClass<T>(T obj) where T : class, new()
        {
            Type objType = obj.GetType();
            object cloneObj = Activator.CreateInstance(objType);
            foreach (var fieldInfo in objType.GetFields())
            {
                fieldInfo.SetValue(cloneObj, fieldInfo.GetValue(obj));
            }
            return cloneObj as T;
        }
        
        #endregion
        
        #region Unity相关
        
        public static Transform GetChildTransformByName(Transform parent, string childName)
        {
            Transform tran;
            for (int i = 0; i < parent.childCount; i++)
            {
                tran = parent.GetChild(i);
                if (tran.name.Equals(childName))
                {
                    return tran;
                }
            }
            return null;
        }

        //判断obj是否是指定LayerMask层的
        public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            int objLayerMask = 1 << obj.layer;
            return (layerMask.value & objLayerMask) > 0;
        }

        public static LayerMask GetTargetLayerMask(string targetLayer)
        {
            int layer = LayerMask.NameToLayer(targetLayer);
            if (layer < 0)
            {
                throw new Exception($"错误的layer名：{targetLayer}, 检查layer是否存在");
            }
            return new LayerMask(){ value = 1 << layer };
        }
        
        #endregion
    }
}