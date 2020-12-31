using CqCore;
using System.Collections.Generic;

namespace UnityEngine
{
    public static partial class Vector2Util
    {
        /// <summary>
        /// 两条线段求差集p-q<para/>
        /// 1.当两条线段没有交集(1.共线但是没有重叠的长度;2.不共线)时,返回null<para/>
        /// 2.当两条线段有交集时,返回差集,结果可能是一条或者两条线段<para/>
        /// 3.当两条线段完全相同时,返回差集的结果是长度为0的列表
        /// </summary>
        public static List<Segment> LineSub(Vector2 p0, Vector2 p1, Vector2 q0, Vector2 q1,float Epsilon = 0.001f)
        {
            float t0, t1;
            var r0 = q0.InSegment(p0, p1, out t0, Epsilon);
            var r1 = q1.InSegment(p0, p1, out t1, Epsilon);
            
            if (!r0 || !r1)//不共线时返回null
            {
                return null;
            }
            
            var c = new CqRange(0, 1) - new CqRange(t0, t1);

            if (c == null) return null;
            var list= new List<Segment>();
            System.Func<float, Vector2> GetPos = t =>
            {
                if (t == 0)
                {
                    return p0;
                }
                else if (t == 1)
                {
                    return p1;
                }
                else if (t == t0)
                {
                    return q0;
                }
                else if (t == t1)
                {
                    return q1;
                }
                throw new System.Exception("不可预期的系数" + t);
            };
            foreach (var it in c)
            {
                list.Add(new Segment(GetPos(it.min), GetPos(it.max)));
            }
            return list;
        }
        /// <summary>
        ///两条线段并集<para/>
        /// 1.当两条线段不共线时不能合并
        /// 2.当两条线段外离时也不能合并
        /// </summary>
        public static bool LineUnion(Vector2 p0, Vector2 p1, Vector2 q0,Vector2 q1,out Vector2 a,out Vector2 b, float Epsilon = 0.001f)
        {
            float t0,t1;
            var r0 = q0.InSegment(p0, p1, out t0, Epsilon);
            var r1 = q1.InSegment(p0, p1, out t1, Epsilon);
            a = Vector2.zero;
            b = Vector2.zero;

            if (!r0 || !r1)
            {
                return false;
            }
            var c = new CqRange(0, 1) + new CqRange(t0, t1);
            if (c == null) return false;
            var d = (CqRange)c;
            System.Func<float, Vector2> GetPos = t =>
            {
                if (t == 0)
                {
                    return p0;
                }
                else if (t == 1)
                {
                    return p1;
                }
                else if (t == t0)
                {
                    return q0;
                }
                else if (t == t1)
                {
                    return q1;
                }
                throw new System.Exception("不可预期的系数" + t);
            };
            a = GetPos(d.min);
            b = GetPos(d.max);
            return true;
        }

        /// <summary>
        /// 点在直线上的判定,在直线上时返回插值系数t
        /// </summary>
        public static bool InSegment(this Vector2 p, Vector2 a, Vector2 b,out float t, float Epsilon = 0.001f)
        {
            if (CalcArea(p,a,b).EqualZero(Epsilon))
            {
                t = LerpT(a, b, p,Epsilon);
                return true;
            }
            else
            {
                t = float.NaN;
                return false;
            }
        }
        /// <summary>
        /// 获取点到线段的距离,参数返回最近点
        /// </summary>
        public static float DistanceBySegment(this Vector2 p, Vector2 a, Vector2 b, out Vector2 intersection)
        {
            var dir = (b - a).normalized;

            //距离
            var dis = p.DistanceByLine(a, b);

            //交点
            var point = p + dir.Rot90() * dis; 

            float t = InverseLerp(a, b, point);

            if (t > 1)
            {
                intersection = b;
                return Vector2.Distance(p, b);
            }
            else if (t < 0)
            {
                intersection = a;
                return Vector2.Distance(p, a);
            }
            else
            {
                //交点在线段上
                intersection = point;
                return Mathf.Abs(dis);
            }
        }

        /// <summary>
		/// 获取点到直线的垂足
		/// </summary>
		public static Vector2 GetPedal(this Vector2 p, Vector2 a, Vector2 b)
        {
            var dis = p.DistanceByLine(a, b);
            var dir = (b - a).normalized;
            return p + dir.Rot90() * dis;
        }

        /// <summary>
        /// 获取点到直线的距离
        /// 为正时表示p,a,b按顺时针排列
        /// </summary>
        public static float DistanceByLine(this Vector2 p, Vector2 a, Vector2 b)
        {
            var dir = b - a;
            var A = dir.y;
            var B = -dir.x;
            var C = -a.x * dir.y + a.y * dir.x;
            return (A * p.x + B * p.y + C) / Mathf.Sqrt(A * A + B * B);
        }

        /// <summary>
        /// 获取两直线交点,当角度在10度以内视为平行,返回null
        /// </summary>
        public static Vector2? TryIntersect(Vector2 line1_a,Vector2 line1_b, Vector2 line2_a, Vector2 line2_b, float angleLimit = 10f)
        {
            Vector2 p1 = line1_a;
            Vector2 p2 = line1_b;
            Vector2 p3 = line2_a;
            Vector2 p4 = line2_b;

            float denominator = (p2.y - p1.y) * (p4.x - p3.x) - (p1.x - p2.x) * (p3.y - p4.y);
            // If denominator is 0, means parallel
            if (denominator == 0)
            {
                return null;
            }

            // Check angle between segments
            float angle = Vector2.Angle(line1_b- line1_a, line2_b- line2_a);
            // if the angle between two segments is too small, we treat them as parallel
            if (angle < angleLimit || (180f - angle) < angleLimit)
            {
                return null;
            }

            float x = ((p2.x - p1.x) * (p4.x - p3.x) * (p3.y - p1.y)
                    + (p2.y - p1.y) * (p4.x - p3.x) * p1.x
                    - (p4.y - p3.y) * (p2.x - p1.x) * p3.x) / denominator;
            float y = -((p2.y - p1.y) * (p4.y - p3.y) * (p3.x - p1.x)
                    + (p2.x - p1.x) * (p4.y - p3.y) * p1.y
                    - (p4.x - p3.x) * (p2.y - p1.y) * p3.y) / denominator;

            return new Vector2(x, y);
        }
    }
}
