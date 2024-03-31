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


    }
}