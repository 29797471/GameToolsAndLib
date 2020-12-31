using System;

namespace CqCore
{
    /// <summary>
    ///常用算法类
    /// </summary>
    public static partial class MathUtil
    {
        /// <summary>
        /// 求插值系数
        /// (b-a)/(c-a)
        /// </summary>
        public static float LerpT(float a, float b, float c)
        {
            return (b - a) / (c - a);
        }

        /// <summary>
        /// 求插值系数
        /// (b-a)/(c-a)
        /// </summary>
        public static float LerpT(object a,object b,object c)
        {
            if(_LerpT==null)
            {
                _LerpT = Arithmetic.ExpressionParser("(b-a)/(c-a)");
            }
            return (float)_LerpT(new object[] { a, b, c });
        }
        static Func<object[], object> _LerpT;

        /// <summary>
        /// 一次插值公式 a*(1-t)+b*t (一次贝塞尔)
        /// </summary>
        public static object LineLerpUnclamped(object a, object b, double t)
        {
            if(_LerpUnclamped==null)
            {
                _LerpUnclamped = Arithmetic.Parse_Fabt("a*(1-t)+b*t");
            }
            return _LerpUnclamped(a, b, t);
        }
        static Func<object,object,double,object> _LerpUnclamped;


        /// <summary>
        /// t小于0时为0 大于1时为1
        /// </summary>
        public static double LineLerp(double a, double b, double t)
        {
            if (t < 0) t = 0;
            else if (t > 1) t = 1;
            return a * (1 - t) + b * t;
        }

        /// <summary>
        /// t小于0时为0 大于1时为1
        /// </summary>
        public static object LineLerp(object a, object b, double t)
        {
            if (t < 0) t = 0;
            else if(t>1)t = 1;
            return LineLerpUnclamped(a, b, t);
        }

        /*
        /// <summary>
        /// 二次插值公式 a*(1-t)*(1-t)+b*2*t*(1-t)+c*t*t (二次贝塞尔)
        /// </summary>
        public static T Lerp2<T>(T a, T b, T c, float t) where T : IOper
        {
            return (T)a.Mul((1 - t) * (1 - t)).Add(b.Mul(2 * t * (1 - t))).Add(c.Mul(t * t));
        }

        /// <summary>
        /// 三次插值公式 a*(1-t)*(1-t)*(1-t)+b*3*(1-t)*t*t+c*3*(1-t)*t*t+d*t*t*t (三次贝塞尔)
        /// </summary>
        public static T Lerp3<T>(T a, T b, T c, T d, float t) where T : IOper
        {
            return (T)a.Mul((1 - t) * (1 - t) * (1 - t)).Add(b.Mul(3 * (1 - t) * (1 - t) * t)).Add(c.Mul((1 - t) * t * t)).Add(d.Mul(t * t * t));
        }
        */
    }
}


