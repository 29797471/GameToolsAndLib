namespace UnityEngine
{
    public static partial class BezierUtil
    {
        /// <summary>
        /// 贝塞尔曲线升阶
        /// </summary>
        public static Vector3[] DegreeElevation(params Vector3[] vectors)
        {
            var n = vectors.Length;
            var ary = new Vector3[n+1];
            ary[0] = vectors[0];
            ary[n] = vectors[n - 1];
            for(int i=1;i<n;i++)
            {
                var ratio=((float)i) / n;
                ary[i] = ratio * vectors[i - 1] + (1 - ratio) * vectors[i];
            }
            return ary;
        }

        /// <summary>
        /// 三次贝塞尔切线方向
        /// </summary>
        public static Vector3 GetTangent(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var _t = 1 - t;
            //return a * 3 * _t* _t * (-1) +3* b * (_t*_t + 2 * _t *(-1)*t ) + 3* c * (_t*2 * t+(-1)*t*t)+ 3*d*t*t ;
            return 3 * (a * -1 * _t * _t + b * _t * (1 - 3 * t) + c * (2 - 3 * t) * t + d * t * t);

        }

        /// <summary>
        /// 二次贝塞尔切线方向
        /// </summary>
        public static Vector3 GetTangent(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return a * 2 * (1 - t) * (-1) + b * 2 * ((1 - t) + (-1) * t) + c * 2 * t;
        }
        /// <summary>
        /// 二次贝塞尔切线方向
        /// </summary>
        public static Vector2 GetTangent(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            return p0 * 2 * (1 - t) * (-1) + p1 * 2 * ((1 - t) + (-1) * t) + p2 * 2 * t;
        }

        /// <summary>
        /// 二次贝塞尔插值
        /// </summary>
        public static Vector2 LerpUnclamped(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            var x = 1 - t;
            return x * x * p0 + 2 * x * t * p1 + t * t * p2;
        }
        /// <summary>
        /// 二次贝塞尔插值
        /// </summary>
        public static Vector3 LerpUnclamped(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var x = 1 - t;
            return  x * x * p0 + 2 *  x * t * p1 +  t * t * p2;
        }
        /// <summary>
        /// 三次贝塞尔插值
        /// </summary>
        public static Vector2 LerpUnclamped(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var x = 1 - t;
            var xx = x * x;
            var tt = t * t;
            return p0 * xx * x + p1 * 3 *  xx * t + p2 * 3 *  x * tt + p3 * tt * t;
        }

        /// <summary>
        /// 三次贝塞尔插值
        /// </summary>
        public static Vector3 LerpUnclamped(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var x = 1 - t;
            var xx = x * x;
            var tt = t * t;
            return p0 * xx * x + p1 * 3 * xx * t + p2 * 3 * x * tt + p3 * tt * t;
        }

        /// <summary>
        /// 三次贝塞尔插值
        /// </summary>
        public static double LerpUnclamped(double p0, double p1, double p2, double p3, double t)
        {
            var x = 1 - t;
            var xx = x * x;
            var tt = t * t;
            return p0 * xx * x + p1 * 3 * xx * t + p2 * 3 * x * tt + p3 * tt * t;
        }

        /// <summary>
        /// 贝塞尔曲线长度<para/>
        /// partCount分隔段数
        /// </summary>
        public static float Length(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int partCount = 100)
        {
            if (partCount < 1) partCount = 1;
            //取点 默认 30个 
            float length = 0.0f;
            Vector3 lastPoint = p0;
            for (int i = 0; i < partCount; i++)
            {
                Vector3 nextPoint = LerpUnclamped(p0, p1, p2, p3, ((float)(i+1)) / partCount);
                length += Vector3.Distance(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
            return length;
        }
        /// <summary>
        /// 贝塞尔曲线长度
        /// 插入pointCount个点,分成pointCount+1段
        /// </summary>
        public static float Length(Vector3 p0, Vector3 p1, Vector3 p2,  int pointCount = 100)
        {
            if (pointCount < 2) { return 0; }
            //取点 默认 30个 
            float length = 0.0f;
            Vector3 lastPoint = p0;
            for (int i = 1; i <= pointCount; i++)
            {
                Vector3 point = LerpUnclamped(p0, p1, p2, (float)i / (float)pointCount);
                length += Vector3.Distance(point, lastPoint);
                lastPoint = point;
            }
            return length;
        }

        /// <summary>
        /// 二次贝塞尔切线斜率
        /// </summary>
        public static float GetTangentAngle(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            var dr = GetTangent(a, b, c, t);
            return Mathf.Atan2(dr.y, dr.x);
        }

        /// <summary>
        /// 过3点求二次贝塞尔中间控制点
        /// https://xuhehuan.com/2608.html
        /// </summary>
        public static Vector2 GetControlPos(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            var p01mod = (p0 - p1).magnitude;
            var p21mod = (p2 - p1).magnitude;
            return p1 - Mathf.Sqrt(p01mod * p21mod) * ((p0 - p1) / p01mod + (p2 - p1) / p21mod) / 2;
        }

        /// <summary>
        /// 过3点求二次贝塞尔中间控制点
        /// https://xuhehuan.com/2608.html
        /// </summary>
        public static Vector3 GetControlPos(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var p01mod = (p0 - p1).magnitude;
            var p21mod = (p2 - p1).magnitude;
            return p1 - Mathf.Sqrt(p01mod * p21mod) * ((p0 - p1) / p01mod + (p2 - p1) / p21mod) / 2;
        }
    }
}
