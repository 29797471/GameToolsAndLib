using System.Collections.Generic;
using UnityEngine;

namespace CqCore
{
    /// <summary>
    /// 线段
    /// </summary>
    public struct Segment
    {
        public Vector2 a;
        public Vector2 b;

        /// <summary>
        /// 相关计算精度
        /// </summary>
        public static float Epsilon = 0.001f;

        /// <summary>
        /// 线段
        /// </summary>
        public Segment(Vector2 a,Vector2 b)
        {
            this.a = a;
            this.b = b;
        }
        /// <summary>
        /// 点在线段上(含两端点)
        /// </summary>
        public bool InSegment(Vector2 p, float Epsilon = 0.001f)
        {
            var v1 = a - p;
            var v2 = b - p;
            
            return Vector2Util.Cross(v1, v2).EqualZero(Epsilon) &&
                Mathf.Min(a.x, b.x).LessEqualsByEpsilon(p.x, Epsilon) &&
                Mathf.Max(a.x, b.x).GreaterEqualsByEpsilon( p.x, Epsilon) &&
                Mathf.Min(a.y, b.y).LessEqualsByEpsilon( p.y, Epsilon) &&
                Mathf.Max(a.y, b.y).GreaterEqualsByEpsilon( p.y, Epsilon);
        }

        /// <summary>
        /// 两条线段的位置关系可以分为三类：有重合部分、无重合部分但有交点、无交点
        /// </summary>
        public bool GetCrossPoint(Segment other, out Vector2? p)
        {
            //直线求交点
            p = Vector2Util.TryIntersect(a,b,other.a,other.b, 0);
            if (p == null)
            {
                return false;
            }
            else
            {
                var t = Vector2Util.LerpT(a, b, (Vector2)p);
                var tOther = Vector2Util.LerpT(other.a, other.b, (Vector2)p);
                return 0 <= t && t <= 1 && 0 <= tOther && tOther <= 1;
            }
        }

        /// <summary>
        /// 转射线
        /// </summary>
        public Ray2D ToRay2D()
        {
            return new Ray2D(a, b - a);
        }

        /// <summary>
		/// 获取点到线段的距离,参数返回最近点
		/// </summary>
		public float Distance(Vector2 p, out Vector2 intersection)
        {
            return p.DistanceBySegment(a, b, out intersection);
        }

        /// <summary>
        /// 两条线段求差集p-q<para/>
        /// 1.当两条线段没有交集(1.共线但是没有重叠的长度;2.不共线)时,返回null<para/>
        /// 2.当两条线段有交集时,返回差集,结果可能是一条或者两条线段<para/>
        /// 3.当两条线段完全相同时,返回差集的结果是长度为0的列表
        /// </summary>
        public static List<Segment> operator -(Segment p,Segment q)
        {
            return Vector2Util.LineSub(p.a,p.b,q.a,q.b, Epsilon);
        }

        /// <summary>
        ///两条线段并集<para/>
        /// 1.当两条线段不共线时不能合并
        /// 2.当两条线段外离时也不能合并
        /// </summary>
        public static Segment? operator +(Segment p, Segment q)
        {
            var seg = new Segment();
            var bl=Vector2Util.LineUnion(p.a, p.b, q.a, q.b,out seg.a,out seg.b, Epsilon);
            if (bl) { return seg; }
            return null;
        }
    }
}
