using System;

namespace System
{
    public static partial class EaseFun
    {
        /// <summary>
        /// 直线
        /// </summary>
        public static double LinearEase(double x)
        {
            return x;
        }
        /// <summary>
        /// 二次
        /// </summary>
        public static double QuadraticEase(double x)
        {
            return x * x;
        }

        /// <summary>
        /// 三次
        /// </summary>
        public static double CubicEase(double x)
        {
            return x * x * x;
        }

        /// <summary>
        /// 四次
        /// </summary>
        public static double QuarticEase(double x)
        {
            var y = x * x;
            return y * y;
        }

        /// <summary>
        /// 五次
        /// </summary>
        public static double QuinticEase(double x)
        {
            return QuarticEase(x) * x;
        }

        /// <summary>
        /// 指数
        /// </summary>
        public static double ExpoEase(double x)
        {
            return Math.Pow(2, 10 * (x - 1));
        }

        /// <summary>
        /// 返回
        /// </summary>
        public static double BackEase(double x)
        {
            return Math.Pow(x, 3) - 0.3 * x * Math.Sin(x * Math.PI);
        }

        /// <summary>
        /// 正弦
        /// </summary>
        public static double SineEase(double x)
        {
            return Math.Sin(x * Math.PI / 2);
        }

        /// <summary>
        /// 圆弧
        /// </summary>
        public static double CircleEase(double x)
        {
            return 1-Math.Sqrt(1-x*x);
        }

        /// <summary>
        /// 弹性振荡
        /// </summary>
        public static double ElasticEase(double x)
        {
            return Math.Pow(100 ,x)* Math.Sin(8.5*Math.PI * x) / 100;
        }

        /// <summary>
        /// 反弹
        /// </summary>
        public static double BounceEase(double t)
        {
            if ((t /= 1) < (1 / 2.75))
            {
                return (7.5625 * t * t);
            }
            else if (t < (2 / 2.75))
            {
                return (7.5625 * (t -= (1.5 / 2.75)) * t + .75) ;
            }
            else if (t < (2.5 / 2.75))
            {
                return  (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) ;
            }
            else
            {
                return  (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) ;
            }
        }
        
    }
}
