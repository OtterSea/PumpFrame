using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PumpFrame.Editor
{
    /// <summary>
    /// 使用的时候记得套上UNITY_EDITOR
    /// 存粹辅助用绘制Gizmos，请在OnGizmos中使用
    /// </summary>
    public class GizmosHelper
    {
        public static void DrawCapsule(Vector3 originPos, Vector3 targetPos, float radius, Color color)
        {
            Vector3 direction = targetPos - originPos;
            Quaternion dirRotation = Quaternion.FromToRotation(Vector3.up, direction);
            Handles.color = color;
            
            //八个方向
            Vector3 pointLeft = dirRotation * Vector3.left;
            Vector3 pointRight = dirRotation * Vector3.right;
            Vector3 pointForward = dirRotation * Vector3.forward;
            Vector3 pointBack = dirRotation * Vector3.back;
            // Vector3 pointLeftForward = dirRotation * new Vector3(-1,0,-1).normalized;
            // Vector3 pointLeftBack = dirRotation * new Vector3(-1,0,1).normalized;
            // Vector3 pointRightForward = dirRotation * new Vector3(1,0,-1).normalized;
            // Vector3 pointRightBack = dirRotation * new Vector3(1,0,1).normalized;
            
            //圆柱体线
            Handles.DrawLine(originPos+pointLeft*radius, targetPos+pointLeft*radius);
            Handles.DrawLine(originPos+pointRight*radius, targetPos+pointRight*radius);
            Handles.DrawLine(originPos+pointForward*radius, targetPos+pointForward*radius);
            Handles.DrawLine(originPos+pointBack*radius, targetPos+pointBack*radius);
            // Handles.DrawLine(originPos+pointLeftForward*radius, targetPos+pointLeftForward*radius);
            // Handles.DrawLine(originPos+pointLeftBack*radius, targetPos+pointLeftBack*radius);
            // Handles.DrawLine(originPos+pointRightForward*radius, targetPos+pointRightForward*radius);
            // Handles.DrawLine(originPos+pointRightBack*radius, targetPos+pointRightBack*radius);
            
            //ori半圆
            Handles.DrawWireDisc(originPos, -direction, radius);
            Handles.DrawWireArc(originPos, pointLeft, pointBack, 180, radius);
            Handles.DrawWireArc(originPos, pointBack, pointRight, 180, radius);
            // Handles.DrawWireArc(originPos, pointLeftBack, pointLeftForward, 180, radius);
            // Handles.DrawWireArc(originPos, pointRightBack, pointLeftBack, 180, radius);
            
            //tar半圆
            Handles.DrawWireDisc(targetPos, direction, radius);
            Handles.DrawWireArc(targetPos, pointLeft, pointForward, 180, radius);
            Handles.DrawWireArc(targetPos, pointBack, pointLeft, 180, radius);
            // Handles.DrawWireArc(targetPos, pointLeftBack, pointLeftForward, -180, radius);
            // Handles.DrawWireArc(targetPos, pointRightBack, pointRightForward, 180, radius);
        }
    }
}