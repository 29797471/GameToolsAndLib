using CqCore;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 点,三角形,矩形,多边形,圆位置关系判定
    /// </summary>
    public static partial class Vector2Util
    {
        /// <summary>
        /// 两多边形有相交的面积(不只一条边或者点相交)
        /// </summary>
        public static float IntersectionArea(IList<Vector2> poly1, IList<Vector2> poly2, float Epsilon = 0.001f)
        {
            var x = PolygonIntersection(poly1, poly2, Epsilon);
            if (x == null) return 0f;
            return RealArea(x);
        }
        /// <summary>
        /// 两多边形有相交的面积(不只一条边或者点相交)
        /// </summary>
        public static bool HasIntersectionArea(IList<Vector2> poly1, IList<Vector2> poly2, float Epsilon = 0.001f)
        {
            var x = PolygonIntersection(poly1, poly2, Epsilon);
            if (x == null) return false;
            return x.Count >= 3;
        }

        /// <summary>
        /// 判定多边形和圆位置关系<para/>
        /// 0.包含<para/>
        /// 1.内切<para/>
        /// 2.相交<para/>
        /// 3.外切<para/>
        /// 4.相离
        /// </summary>
        public static PolyCircleRelations CalcPositionRelations(List<Vector2> poly1, Vector2 p, float r)
        {
            //先判定圆心和矩形的位置关系
            var s = p.InRangeX(poly1);
            switch (s)
            {
                case PolyPointRelations.Inside:
                    {
                        var state = PolyCircleRelations.Contains;
                        
                        int len = poly1.Count;
                        var segment = new Segment();
                        for (int i = 0, j = len - 1; i < len; j = i++)
                        {
                            segment.a = poly1[i];
                            segment.b = poly1[j];

                            float dis = segment.Distance(p, out Vector2 inse);
                            if (dis < r)
                            {
                                return PolyCircleRelations.Intersection;
                            }
                            else if (dis.EqualsByEpsilon(r))
                            {
                                state = PolyCircleRelations.Inscribe;
                            }
                        }
                        return state;
                    }
                case PolyPointRelations.Contain:
                    {
                        return PolyCircleRelations.Intersection;
                    }
                case PolyPointRelations.Outside:
                    {
                        var state = PolyCircleRelations.Separation;
                        
                        int len = poly1.Count;
                        var segment = new Segment();
                        for (int i = 0, j = len - 1; i < len; j = i++)
                        {
                            segment.a = poly1[i];
                            segment.b = poly1[j];

                            float dis = segment.Distance(p, out Vector2 inse);
                            if (dis < r)
                            {
                                return PolyCircleRelations.Intersection;
                            }
                            else if (dis.EqualsByEpsilon(r))
                            {
                                state = PolyCircleRelations.Circumscribe;
                            }
                        }
                        return state;
                    }
            }
            return PolyCircleRelations.Intersection;
        }

        /// <summary>
        /// 射线法
        /// 获取点是否在三角形内
        /// </summary>
        public static bool InRange(this Vector2 test, Vector2 v0, Vector2 v1, Vector2 v2)
        {
            bool x = false;

            if (((v0.y > test.y) != (v2.y > test.y)) &&
                    (test.x < (v2.x - v0.x) * (test.y - v0.y) / (v2.y - v0.y) + v0.x))
            {
                x = !x;
            }
            if (((v1.y > test.y) != (v0.y > test.y)) &&
                    (test.x < (v0.x - v1.x) * (test.y - v1.y) / (v0.y - v1.y) + v1.x))
            {
                x = !x;
            }
            if (((v2.y > test.y) != (v1.y > test.y)) &&
                    (test.x < (v1.x - v2.x) * (test.y - v2.y) / (v1.y - v2.y) + v2.x))
            {
                x = !x;
            }
            return x;
        }

        /// <summary>
        /// 射线法
        /// 判定一个点是否在不规则多边形内(凸凹多边形),在多边形外,边上或者顶点上返回false
        /// </summary>
        public static bool InRange(this Vector2 test, IList<Vector2> verts)
        {
            bool c = false;
            int len = verts.Count;
            for (int i = 0, j = len - 1; i < len; j = i++)
            {
                if (((verts[i].y > test.y) != (verts[j].y > test.y)) &&
                    (test.x < (verts[j].x - verts[i].x) * (test.y - verts[i].y) / (verts[j].y - verts[i].y) + verts[i].x))
                {
                    c = !c;
                }
            }
            return c;
        }
        /// <summary>
        /// 射线法<para/>
        /// 获取点与不规则多边形(可以凹凸)位置关系<para/>
        /// </summary>
        public static PolyPointRelations InRangeX(this Vector2 test, IList<Vector2> verts)
        {
            bool c = false;
            int len = verts.Count;
            var segment = new Segment(Vector2.zero, Vector2.zero);
            for (int i = 0, j = len - 1; i < len; j = i++)
            {
                segment.a = verts[i];
                segment.b = verts[j];

                if (segment.InSegment(test))
                {
                    return PolyPointRelations.Contain;
                }
                if (((verts[i].y > test.y) != (verts[j].y > test.y)) &&
                    (test.x < (verts[j].x - verts[i].x) * (test.y - verts[i].y) / (verts[j].y - verts[i].y) + verts[i].x))
                {
                    c = !c;
                }
            }
            return c ? PolyPointRelations.Inside : PolyPointRelations.Outside;
        }
    }
}
