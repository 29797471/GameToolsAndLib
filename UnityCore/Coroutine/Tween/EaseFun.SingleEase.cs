using System;
using UnityEngine;

namespace UnityEngine
{
    public static partial class EaseFun
    {
        /// <summary>
        /// 直线
        /// </summary>
        public static float LinearEase(float x)
        {
            return x;
        }
        /// <summary>
        /// 二次
        /// </summary>
        public static float QuadraticEase(float x)
        {
            return x * x;
        }

        /// <summary>
        /// 三次
        /// </summary>
        public static float CubicEase(float x)
        {
            return x * x * x;
        }

        /// <summary>
        /// 四次
        /// </summary>
        public static float QuarticEase(float x)
        {
            var y = x * x;
            return y * y;
        }

        /// <summary>
        /// 五次
        /// </summary>
        public static float QuinticEase(float x)
        {
            return QuarticEase(x) * x;
        }

        /// <summary>
        /// 指数
        /// </summary>
        public static float ExpoEase(float x)
        {
            return Mathf.Pow(2, 10 * (x - 1));
        }

        /// <summary>
        /// 返回
        /// </summary>
        public static float BackEase(float x)
        {
            return Mathf.Pow(x, 3) - 0.3f * x * Mathf.Sin(x * Mathf.PI);
        }

        /// <summary>
        /// 正弦
        /// </summary>
        public static float SineEase(float x)
        {
            return Mathf.Sin(x * Mathf.PI / 2);
        }

        /// <summary>
        /// 圆弧
        /// </summary>
        public static float CircleEase(float x)
        {
            return 1 - Mathf.Sqrt(1 - x * x);
        }

        /// <summary>
        /// 弹性振荡
        /// </summary>
        public static float ElasticEase(float x)
        {
            return Mathf.Pow(100, x) * Mathf.Sin(8.5f * Mathf.PI * x) / 100;
        }

        /// <summary>
        /// 反弹
        /// </summary>
        public static float BounceEase(float t)
        {
            if ((t /= 1) < (1 / 2.75f))
            {
                return (7.5625f * t * t);
            }
            else if (t < (2 / 2.75f))
            {
                return (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f);
            }
            else if (t < (2.5f / 2.75f))
            {
                return (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f);
            }
            else
            {
                return (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f);
            }
        }

    }
}
