using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public static class HandlesUtil
    {
        /// <summary>
        /// 绘制过三点的贝塞尔曲线
        /// </summary>
        public static void DrawBezier(Vector3 a,Vector3 b,Vector3 c,Color color,float lineWidth)
        {
            b = BezierUtil.GetControlPos(a, b, c);
            var startTangent = a / 3 + b * 2 / 3;
            var endTangent = b * 2 / 3 + c / 3;
            Handles.DrawBezier(a, c, startTangent, endTangent, color, null, lineWidth);
        }
    }
}
