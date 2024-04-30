using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpFrame
{
    public struct GizmosHelperCapsule
    {
        public Vector3 originPos;
        public Vector3 targetPos;
        public float radius;
        public Color color;
    }
    public class GizmosHelperMono : SingletonMono<GizmosHelperMono>
    {
        public List<GizmosHelperCapsule> capsuleList;

        public static void AddDrawCapsule(Vector3 originPos, Vector3 targetPos, float radius, Color color)
        {
            if (Instance.capsuleList == null)
            {
                Instance.capsuleList = new List<GizmosHelperCapsule>();
            }
            Instance.capsuleList.Add(new GizmosHelperCapsule()
            {
                originPos = originPos,
                targetPos = targetPos,
                radius = radius,
                color = color
            });
        }

        public static void ClearAllDrawCapsule()
        {
            Instance.capsuleList?.Clear();
        }
        
        public void OnDrawGizmos()
        {
            foreach (var ghc in capsuleList)
            {
                GizmosHelper.DrawCapsule(ghc.originPos, ghc.targetPos, ghc.radius, ghc.color);
            }
        }
    }
}