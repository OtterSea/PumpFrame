using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PumpFrame
{ 
    public class MathHelper
    {
        private const double DOUBLE_DELTA = 1E-06;

        //两个float\double值是否相等
        public static bool IsFloatEqual(float value1, float value2)
        {
            return (value1 == value2) || Math.Abs(value1 - value2) < DOUBLE_DELTA;
        }
        public static bool IsDoubleEqual(double value1, double value2)
        {
            return (value1 == value2) || Math.Abs(value1 - value2) < DOUBLE_DELTA;
        }

        //限制angle的值在360度内
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}