using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 图形绘制算法
    /// </summary>
    public static class DrawCalc
    {
        /// <summary>
        /// 绘制包围盒
        /// </summary>
        public static void DrawBounds(Bounds bounds, Matrix4x4? worldMat, Action<Vector3, Vector3> DrawLine)
        {
            bounds.ToWorldVertexs(worldMat,ref BoundsUtil.temp_vs);
            var vs = BoundsUtil.temp_vs;
            DrawLine(vs[0], vs[1]);
            DrawLine(vs[1], vs[2]);
            DrawLine(vs[2], vs[3]);
            DrawLine(vs[3], vs[0]);
            //上面                
            DrawLine(vs[4], vs[5]);
            DrawLine(vs[5], vs[6]);
            DrawLine(vs[6], vs[7]);
            DrawLine(vs[7], vs[4]);
            //中间                
            DrawLine(vs[0], vs[4]);
            DrawLine(vs[1], vs[5]);
            DrawLine(vs[2], vs[6]);
            DrawLine(vs[3], vs[7]);
        }
        /// <summary>
        /// 绘制多边形
        /// </summary>
        public static void DrawPolygon(List<Vector2> poly, Action<Vector2, Vector2> DrawLine)
        {
            if (poly == null) return;
            var nSize = poly.Count;
            for (int i = 0, j = nSize - 1; i < nSize; j = i++)
            {
                DrawLine(poly[i], poly[j]);
            }
        }

        /// <summary>
        /// 绘制矩形
        /// 由a,b之间的有宽度的线段来定义
        /// </summary>
        public static void DrawRect(Vector2 a, Vector2 b, float width,
            Action<Vector2, Vector2> DrawLine)
        {
            if (a == b) return;
            var dir = (b - a).Rot90().normalized * width / 2;
            DrawLine(a + dir, b + dir);
            DrawLine(a + dir, a - dir);
            DrawLine(a - dir, b - dir);
            DrawLine(b - dir, b + dir);
        }
        /// <summary>
        /// 绘制矩形
        /// 由a,b之间的有宽度的线段来定义
        /// </summary>
        public static void DrawRect(Rect rect,
            Action<Vector2, Vector2> DrawLine)
        {
            //左下为原点的坐标系
            var leftBottom = rect.position;
            var leftTop = rect.position+Vector2.up*rect.height;
            var rightTop = rect.position+rect.size;
            var rightBottom = rect.position+ Vector2.right*rect.width;
            DrawLine(leftBottom, leftTop);
            DrawLine(rightTop, leftTop);
            DrawLine(rightTop, rightBottom);
            DrawLine(leftBottom, rightBottom);
        }

        /// <summary>
        /// 绘制圆
        /// </summary>
        public static void DrawCirCle(float rad, Vector2 pos, int sampling,
            Action<Vector2, Vector2> DrawLine)
        {
            var list = Vector2Util.GetRing(sampling);
            var delta = list[0] * rad;

            for (int i = 1; i < list.Count; i++)
            {
                var delta_next = list[i] * rad;
                DrawLine(delta + pos, delta_next + pos);
                delta = delta_next;
            }
        }

        /// <summary>
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public static void DrawBezier(Vector2 start,Vector2 control, Vector2 end, int sampling,
            Action<Vector2, Vector2> DrawLine)
        {
            Vector2 last = start;
            for (int i = 0; i < sampling+1; i++)
            {
                var next = BezierUtil.LerpUnclamped(start, control, end,(float)(i+1) / (sampling + 1));
                DrawLine(last, next);
                last = next;
            }
        }

        /// <summary>
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public static void DrawBezier(Vector3 start, Vector3 control, Vector3 end, int sampling,
            Action<Vector3, Vector3> DrawLine)
        {
            Vector3 last = start;
            for (int i = 0; i < sampling + 1; i++)
            {
                var next = BezierUtil.LerpUnclamped(start, control, end, (float)(i + 1) / (sampling + 1));
                DrawLine(last, next);
                last = next;
            }
        }

        /// <summary>
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public static void DrawBezier(Vector2 start, Vector2 controlStart, Vector2 controlEnd, Vector2 end, int sampling,
            Action<Vector2, Vector2> DrawLine)
        {
            Vector2 last = start;
            for (int i = 0; i < sampling + 1; i++)
            {
                var next = BezierUtil.LerpUnclamped(start, controlStart, controlEnd, end, (float)(i + 1) / (sampling + 1));
                DrawLine(last, next);
                last = next;
            }
        }
        /// <summary>
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public static void DrawBezier(Vector3 start, Vector3 controlStart, Vector3 controlEnd, Vector3 end, int sampling,
            Action<Vector3, Vector3> DrawLine)
        {
            Vector3 last = start;
            for (int i = 0; i < sampling + 1; i++)
            {
                var next = BezierUtil.LerpUnclamped(start, controlStart, controlEnd, end, (float)(i + 1) / (sampling + 1));
                DrawLine(last, next);
                last = next;
            }
        }

        /// <summary>
        /// 绘制胶囊
        /// </summary>
        public static void DrawCapsule(Vector2 a, Vector2 b, float rad,
            Action<Vector2, Vector2> DrawLine)
        {
            var dir = (b - a).Rot90().normalized * rad;
            DrawLine(a + dir, b + dir);
            DrawLine(a - dir, b - dir);
            var aa = Vector2.Angle(dir, Vector2.up) * Mathf.Deg2Rad;
            if (Vector2Util.Cross(dir, Vector2.up) < 0) aa = -aa;

            var list = Vector2Util.GetRing(21, aa, aa + Mathf.PI);

            var delta = list[0] * rad;
            for (int i = 1; i < list.Count; i++)
            {
                var delta_next = list[i] * rad;
                DrawLine(a - delta, a - delta_next);
                DrawLine(delta + b, delta_next + b);
                delta = delta_next;
            }
        }

        /// <summary>
        /// 绘制虚线(虚实相间,动态调整虚线宽度,两边半虚线宽(可拼接其它虚线))
        /// </summary>
        /// <param name="a">起点</param>
        /// <param name="b">终点</param>
        /// <param name="partWidth">每段实线宽</param>
        /// <param name="DrawLine"></param>
        public static void DrawDottedLine(Vector2 a, Vector2 b, float partWidth,Action<Vector2, Vector2> DrawLine)
        {
            if (a == b) return;
            float dis = Vector2.Distance(a, b);
            if (dis / partWidth > 10000)
            {
                throw new System.Exception("虚线段过多");
            }

            Vector2 normalized = (b - a).normalized;

            var realPartCount = Mathf.RoundToInt(dis / (partWidth * 2));

            var dottedWidth = dis / realPartCount - partWidth;
            Vector2 partWdithVec = normalized * partWidth;
            Vector2 dottedWdithVec = normalized * dottedWidth;

            Vector2 roundWdithVec = partWdithVec+ dottedWdithVec;

            Vector2 partStart = a + dottedWdithVec/2;
            Vector2 partEnd = partStart+ partWdithVec;
            
            for (int partIndex=0;partIndex<realPartCount;partIndex++)
            {
                DrawLine(partStart, partEnd);
                partStart += roundWdithVec;
                partEnd += roundWdithVec;
            }
        }
        
        /// <summary>
        /// 绘制一个箭头
        /// </summary>
        public static void DrawArrow(Vector2 center, Vector2 dir, float arrowSize,Action<Vector2, Vector2> DrawLine)
        {
            dir.Normalize();
            var n = center - dir * arrowSize * 2;
            var m = center + dir * arrowSize * 2;
            dir = new Vector2(dir.y, -dir.x);
            var up = n + dir * arrowSize;
            var down = n - dir * arrowSize;
            DrawLine(up, m);
            DrawLine(down, m);
            DrawLine(down, up);
        }
        public static void DrawLine(Vector2Int a,Vector2Int b, Action<Vector2Int> DrawPixel)
        {
            DrawLine(a.x, a.y, b.x, b.y, (x, y) =>
            {
                DrawPixel(new Vector2Int(x, y));
            });
        }

        public static void DrawPolygon(IList<Vector2> list, Action<Vector2,Vector2> DrawLine)
        {
            var nSize = list.Count;
            for (int i = 0, j = nSize - 1; i < nSize; j = i++)
            {
                DrawLine(list[i], list[j]);
            }
        }
        public static void DrawLine(int x0, int y0, int x1, int y1, Action<int,int> DrawPixel)
        {
            bool isSteep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (isSteep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);

            int error = deltax / 2;
            int ystep;
            int y = y0;

            if (y0 < y1)
                ystep = 1;
            else
                ystep = -1;

            for (int x = x0; x < x1; x++)
            {
                if (isSteep)
                    DrawPixel(y, x);
                else
                    DrawPixel(x, y);

                error = error - deltay;
                if (error < 0)
                {
                    y = y + ystep;
                    error = error + deltax;
                }
            }
        }


        /// <summary>
        /// Swap two ints by reference.
        /// </summary>
        private static void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
    }
}
