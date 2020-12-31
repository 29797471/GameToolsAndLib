using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static partial class Vector3Util
    {
        /// <summary>
        /// 添加Vector3运算符扩展到通用表达式计算
        /// </summary>
        internal static void RegOperatorEx()
        {
            AssemblyUtil.ImportCustomOperator<Vector3,Vector3,float>(CalcOperator.Div, op_Division);
        }

        static List<float> temp_calc;

        /// <summary>
        /// 分量相除取平均值,多用于取插值系数
        /// </summary>
        public static float op_Division(this Vector3 aV, Vector3 bV)
        {
            if (temp_calc == null) temp_calc = new List<float>();
            if (bV.x != 0) temp_calc.Add(aV.x / bV.x);
            if (bV.y != 0) temp_calc.Add(aV.y / bV.y);
            if (bV.z != 0) temp_calc.Add(aV.z / bV.z);
            var result= ((temp_calc.Count == 0) ? float.NaN : temp_calc.Average());
            temp_calc.Clear();
            return result;
        }
        /// <summary>
        /// 分量相除
        /// </summary>
        public static Vector3 Division(this Vector3 t, Vector3 v)
        {
            return new Vector3(t.x / v.x, t.y / v.y, t.z / v.z);
        }

        /// <summary>
        /// a==b
        /// a,b在误差Epsilon范围内视为相等
        /// </summary>
        public static bool EqualsByEpsilon(this Vector3 a, Vector3 b, float Epsilon = 0.001f)
        {
            return a.x.EqualsByEpsilon(b.x, Epsilon) &&
                a.y.EqualsByEpsilon(b.y, Epsilon) &&
                a.z.EqualsByEpsilon(b.z, Epsilon);
        }

        /// <summary>
        /// 转y=0平面2d坐标
        /// </summary>
        public static Vector2 ToVector2(this Vector3 temp)
        {
            return new Vector2(temp.x,temp.z);
        }

        /// <summary>
        /// 栅格化
        /// </summary>
        public static Vector3 Rasterize(this Vector3 temp, float delta)
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
        /// 所有分量向下取整
        /// </summary>
        public static Vector3 Floor(this Vector3 temp)
        {
            return new Vector3(
                Mathf.Floor(temp.x),
                Mathf.Floor(temp.y),
                Mathf.Floor(temp.z)
                );
        }

        /// <summary>
        /// 所有分量4舍5入取整
        /// </summary>
        public static Vector3 Round(this Vector3 temp)
        {
            return new Vector3(
                Mathf.Round(temp.x),
                Mathf.Round(temp.y),
                Mathf.Round(temp.z)
                );
        }
        /// <summary>
        /// 多边形顶点顺序顺时针,还是逆时针
        /// </summary>
        public static bool IsPolyClockwise(Vector3[] vPts)
        {
            //沿着多边形的边求曲线积分,若积分为正,则是沿着边界曲线正方向(逆时针),反之为顺时针
            double d = 0;
            var nSize = vPts.Length;

            for (int i = 0, j = nSize - 1; i < nSize; j = i++)
            {
                d += -0.5 * (vPts[i].y + vPts[j].y) * (vPts[i].x - vPts[j].x);
            }
            //小于零为顺时针，大于零为逆时针
            return d < 0.0;
        }


    }
}
