using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCore
{
    public static class DebugUtil
    {
        /// <summary>
		/// 采样圆周
		/// </summary>
		/// <param name="sampling">采样顶点数量(至少为2)</param>
		/// <param name="startRad">起始弧度</param>
		/// <param name="endRad">终止弧度</param>
		/// <returns></returns>
		public static List<Vector2> GetRing(int sampling = 21, float startRad = 0f, float endRad = 6.28318548f)
        {
            List<Vector2> list = new List<Vector2>();
            float num = startRad;
            float num2 = (endRad - startRad) / (float)(sampling - 1);
            for (int i = 0; i < sampling; i++)
            {
                list.Add(new Vector2(Mathf.Sin(num), Mathf.Cos(num)));
                num += num2;
            }
            return list;
        }

        /// <summary>
        /// 画虚线(2*x+pw*(2n+1))
        /// </summary>
        /// <param name="a">起点</param>
        /// <param name="b">终点</param>
        /// <param name="color">颜色</param>
        /// <param name="partWidth">单实线宽</param>
        /// <param name="startByDotted">两端是否以虚线起始</param>
        public static void DrawDottedLine(Vector2 a, Vector2 b, Color color,float partWidth=1f,bool startByDotted=false)
        {
            var dis = Vector2.Distance(a, b);
            var n = Mathf.FloorToInt((dis/ partWidth-1)/2);
            var x = (dis - partWidth * (2 * n + 1))/2;
            var dir = (b - a).normalized;

            Vector2 start = a + dir * x;
            Vector2 delta = dir * partWidth;
            if(startByDotted)
            {
                for (int i = 0; i < n + 1; i++)
                {
                    DrawLine(start,start+ delta,color);
                    start += delta * 2;
                }
            }
            else
            {
                DrawLine(a, start, color);
                DrawLine(b, b-dir*x, color);

                start += delta;
                for (int i = 0; i < n; i++)
                {
                    DrawLine(start, start + delta, color);
                    start += delta * 2;
                }
            }
        }

        /// <summary>
        /// 绘制由A指向B的线段并在中间有一个方向箭头
        /// </summary>
        public static void DrawArrowLine(Vector2 a, Vector2 b, Color color,  float arrowSize = 1f, float arrowPos = 0.618f)
        {
            DrawLine(a, b, color);
            var center = Vector2.LerpUnclamped(a, b, arrowPos);
            var vv = b - a;
            vv.Normalize();
            var n = center - vv * arrowSize * 2;
            var m = center + vv * arrowSize * 2;
            vv = new Vector2(vv.y, -vv.x);
            var up = n + vv * arrowSize;
            var down = n - vv * arrowSize;
            DrawLine(up, m, color);
            DrawLine(down, m, color);
            DrawLine(down, up, color);
        }
        /// <summary>
        /// 绘制在y=0平面上
        /// </summary>
        public static void DrawLine(Vector2 a, Vector2 b, Color color)
        {
            Debug.DrawLine(new Vector3(a.x, 0, a.y),
                    new Vector3(b.x, 0, b.y), color);
        }

        /// <summary>
        /// 绘制任意角度的矩形
        /// </summary>
        public static void DrawRect(Vector2 a, Vector2 b, float width, Color color)
        {
            var dir = (b - a).Rot90().normalized * width/2;
            DrawLine(a + dir, b + dir, color);
            DrawLine(a + dir, a - dir, color);
            DrawLine(a - dir, b - dir, color);
            DrawLine(b - dir, b + dir, color);
        }

        /// <summary>
        /// 以pos为圆心绘制多边形
        /// </summary>
        public static void DrawCirCle( float rad, Vector2 pos, Color color, int sampling=21)
        {
            var list = Vector2Util.GetRing( sampling);
            var delta = list[0] * rad;

            for (int i = 1; i < list.Count; i++)
            {
                var delta_next = list[i] * rad;
                DrawLine(delta + pos, delta_next + pos, color);
                delta = delta_next;
            }
        }


        /// <summary>
        /// 绘制胶囊
        /// </summary>
        public static void DrawCapsule(Vector2 a, Vector2 b, float rad, Color color)
        {
            var dir = (b - a).Rot90().normalized * rad;
            DrawLine(a + dir, b + dir, color);
            DrawLine(a - dir, b - dir, color);
            var aa = Vector2.Angle(dir, Vector2.up) * Mathf.Deg2Rad;
            if (Vector2Util.Cross(dir, Vector2.up) < 0) aa = -aa;

            var list = Vector2Util.GetRing(21, aa , aa + Mathf.PI);

            var delta = list[0] * rad;
            for (int i = 1; i < list.Count ; i++)
            {
                var delta_next = list[i] * rad;
                DrawLine(a - delta, a - delta_next, color);
                DrawLine(delta + b, delta_next + b, color);
                delta=delta_next;
            }
        }

        /// <summary>
        /// 绘制有旋转或者变换信息的包围盒
        /// </summary>
        public static void DrawBounds(Bounds bounds, Matrix4x4? worldMat, Color color)
        {
            bounds.ToWorldVertexs(worldMat, ref BoundsUtil.temp_vs);
            DebugDrawBox(BoundsUtil.temp_vs, color);
        }
        /// <summary>
        /// 利用Debug.DrawLine绘制这个包围盒
        /// </summary>
        public static void DrawBounds( Bounds bounds, Color color)
        {
            DrawBounds(bounds, null, color);
        }
        /// <summary>
        /// 绘制多边形
        /// </summary>
        public static void DrawPoly(IList<Vector3> polygon, Matrix4x4 worldMat, Color color)
        {
            DrawPoly(polygon.ToList().ConvertAll(x => worldMat.MultiplyPoint(x)), color);
        }
        /// <summary>
        /// 绘制多边形
        /// </summary>
        public static void DrawPoly(IList<Vector3> list,Color color)
        {
            var nSize = list.Count;
            for (int i = 0, j = nSize - 1; i < nSize; j = i++)
            {
                Debug.DrawLine(list[i], list[j], color);
            }
        }

        /// <summary>
        /// 绘制立方体
        /// </summary>
        static void DebugDrawBox(Vector3[] vs, Color color)
        {
            //下面
            Debug.DrawLine(vs[0], vs[1], color);
            Debug.DrawLine(vs[1], vs[2], color);
            Debug.DrawLine(vs[2], vs[3], color);
            Debug.DrawLine(vs[3], vs[0], color);
            //上面                     
            Debug.DrawLine(vs[4], vs[5], color);
            Debug.DrawLine(vs[5], vs[6], color);
            Debug.DrawLine(vs[6], vs[7], color);
            Debug.DrawLine(vs[7], vs[4], color);
            //中间                     
            Debug.DrawLine(vs[0], vs[4], color);
            Debug.DrawLine(vs[1], vs[5], color);
            Debug.DrawLine(vs[2], vs[6], color);
            Debug.DrawLine(vs[3], vs[7], color);
        }
        
    }
}
