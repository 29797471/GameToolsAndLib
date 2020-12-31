using CqCore;
using System.Collections.Generic;

namespace UnityEngine
{
    public static partial class Vector2Util
    {
        /// <summary>
        /// 向量a逆时针旋转到b经过的夹角
        /// </summary>
        public static float GetRoundAngleX(Vector2 a, Vector2 b)
        {
            return Mathf.Atan2(Cross(a, b), Vector2.Dot(a, b));
        }

        /// <summary>
        /// 向量a逆时针旋转到b经过的夹角(0~2*Pi)
        /// </summary>
        public static float GetRoundAngle(Vector2 a, Vector2 b)
        {
            return MathUtil.MoveToRange(Mathf.Atan2(Cross(a, b), Vector2.Dot(a, b)) , 0, 2*Mathf.PI);
        }
        /// <summary>
        /// 栅格化
        /// </summary>
        public static Vector2 Rasterize(this Vector2 temp,float delta)
        {
            if (delta > 0)
            {
                temp /= delta;
                temp = temp.Round();
                temp *= delta;
            }
            return temp;
        }
        /// <summary>
        /// 求插值系数
        /// </summary>
        public static float LerpT(Vector2 a, Vector2 b, Vector2 p,float Epsilon = 0.001f)
        {
            if (p.EqualsByEpsilon(a, Epsilon))
            {
                return 0;
            }
            if (p.EqualsByEpsilon(b, Epsilon))
            {
                return 1;
            }
            var n = (p - a).Division(b-a);
            if(!float.IsNaN(n.x) && !float.IsInfinity(n.x))
            {
                return n.x;
            }
            else
            {
                return n.y;
            }
        }
        /// <summary>
        /// 分量相除
        /// </summary>
        public static Vector2 Division(this Vector2 t, Vector2 v)
        {
            return new Vector2(t.x / v.x, t.y / v.y);
        }

        /// <summary>
		/// a==b
		/// a,b在误差Epsilon范围内视为相等
		/// </summary>
		public static bool EqualsByEpsilon(this Vector2 a, Vector2 b, float Epsilon = 0.001f)
        {
            return a.x.EqualsByEpsilon(b.x, Epsilon) && a.y.EqualsByEpsilon(b.y, Epsilon);
        }

        /// <summary>
		/// 所有分量4舍5入取整
		/// </summary>
		public static Vector2 Round(this Vector2 temp, int digits=0)
        {
            return new Vector2((float)System.Math.Round(temp.x, digits), (float)System.Math.Round(temp.y, digits));
        }

        /// <summary>
		/// y=0平面2d坐标转3d坐标
		/// </summary>
		public static Vector3 ToVector3(this Vector2 temp)
        {
            return new Vector3(temp.x, 0f, temp.y);
        }
        /// <summary>
        /// 带精度的等于零的判定
        /// </summary>
        public static bool EqualZero(this Vector2 temp, float limit = 0.001f)
        {
            return temp.x.EqualZero( limit) && temp.y.EqualZero(limit);
        }

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
		/// 二维向量叉乘x1y2-x2y1,
		/// 几何意义是两向量构成的平行四边形的面积,
		/// 也可以用来判定两个向量的夹角是顺时针还是逆时针
		/// </summary>
		/// <returns></returns>
		public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }
        /// <summary>
		/// 向量逆时针旋转deg角度
		/// </summary>
		/// <param name="v"></param>
		/// <param name="deg">角度</param>
		/// <returns></returns>
		public static Vector2 RotByDeg(this Vector2 v, float deg)
        {
            return v.Rot(deg * Mathf.Deg2Rad);
        }
        /// <summary>
        /// 向量逆时针旋转rad弧度
        /// </summary>
        /// <param name="v"></param>
        /// <param name="rad">弧度</param>
        /// <returns></returns>
        public static Vector2 Rot(this Vector2 v, float rad)
        {
            return new Vector2(v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad), v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad));
        }
        /// <summary>
        /// 向量逆时针旋转90度
        /// </summary>
        public static Vector2 Rot90(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }
        

        /// <summary>
        /// a+(b-a)*t=v 求t
        /// </summary>
        public static float InverseLerp(Vector2 a, Vector2 b, Vector2 v, float limit = 0.001f)
        {
            if(Mathf.Abs(b.x-a.x)> limit) return (v.x-a.x) / (b.x - a.x);
            else return (v.y - a.y) / (b.y - a.y);
        }
    }
}
