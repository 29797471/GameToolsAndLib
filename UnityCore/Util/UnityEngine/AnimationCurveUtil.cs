using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 扩展曲线
    /// </summary>
    public static class AnimationCurveUtil
    {
        /// <summary>
        /// 克隆
        /// </summary>
        public static AnimationCurve Clone(this AnimationCurve curve)
        {
            var temp = new AnimationCurve(curve.keys)
            {
                preWrapMode = curve.preWrapMode,
                postWrapMode = curve.postWrapMode
            };
            return temp;
        }
        /// <summary>
        /// 清除所有
        /// </summary>
        public static void Clear(this AnimationCurve curve)
        {
            while(curve.length>0)
            {
                curve.RemoveKey(0);
            }
        }
        /// <summary>
        /// 构造曲线
        /// </summary>
        public static void SetEvaluate(this AnimationCurve curve,System.Func<float,float> evaluate,int sampling)
        {
            curve.Clear();
            for(float i=0;i<sampling;i++)
            {
                var t=i / (sampling-1);
                curve.AddKey(t, evaluate(t));
            }
        }

        /// <summary>
        /// 反向曲线
        /// </summary>
        public static Func<float,float> ReEvaluate(this AnimationCurve curve)
        {
            return  (t) => 1 - curve.Evaluate(1 - t);
        }

        /// <summary>
        /// 关于y=x对称曲线(反函数)
        /// </summary>
        public static AnimationCurve InverseEvaluate(this AnimationCurve curve)
        {
            var inverse = new AnimationCurve();
            foreach(var it in curve.keys)
            {
                inverse.AddKey(it.value, it.time);
            }
            return inverse;
        }
        /// <summary>
        /// 曲线的key到下一个key之间是一条直线
        /// </summary>
        public static bool IsBeeline(this AnimationCurve curve, int keyIndex)
        {
            if (keyIndex + 1 < curve.length)
            {
                var current = curve.keys[keyIndex];
                var next = curve.keys[keyIndex + 1];
                var key = (next.value - current.value) / (next.time - current.time);
                if ((Mathf.Abs(current.outTangent - key) <= 0.000001 && Mathf.Abs(next.inTangent - key) <= 0.000001) ||
                    float.IsInfinity(current.outTangent) ||
                     float.IsInfinity(next.inTangent) ||
                        float.IsInfinity(current.inTangent))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 由点生成折线
        /// </summary>
        public static AnimationCurve GetLineCure(Vector2[] points)
        {
            var keys = new Keyframe[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var p = points[i];
                keys[i] = new Keyframe(p.x, p.y);
            }
            for (var i = 0; i < points.Length - 1; i++)
            {
                var delta = points[i + 1] - points[i];
                var k = delta.y / delta.x;
                keys[i].outTangent = k;
                keys[i + 1].inTangent = k;
            }
            return new AnimationCurve(keys);
        }
        /// <summary>
        /// 曲线采样,返回有宽度的点列表
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="sampling">对其中的曲线段采样点数量</param>
        /// <returns></returns>
        public static List<CurveLinePoint> Sampling(this AnimationCurve curve, int sampling)
        {
            var pos = new List<CurveLinePoint>();
            float lastV = 0f;
            int i = 0;
            Keyframe key;
            for (; i < curve.length - 1; i++)
            {
                key = curve.keys[i];
                if (curve.IsBeeline(i))
                {
                    if (float.IsInfinity(key.inTangent))
                    {
                        pos.Add(new CurveLinePoint(key.time, lastV));
                    }
                    else if (float.IsInfinity(key.outTangent))
                    {
                        curve.keys[i + 1].inTangent = float.PositiveInfinity;
                    }
                    pos.Add(new CurveLinePoint(key.time, key.value));

                }
                else
                {
                    var nextKey = curve.keys[i + 1];
                    var start = key.time;
                    var end = nextKey.time;
                    for (int t = 0; t < sampling; t++)
                    {
                        var time = Mathf.LerpUnclamped(start, end, t * 1f / sampling);
                        pos.Add(new CurveLinePoint(time, curve.Evaluate(time), true));
                    }
                }
                lastV = key.value;
            }
            key = curve.keys[i];
            if (float.IsInfinity(key.inTangent))
            {
                pos.Add(new CurveLinePoint(key.time, lastV));
            }
            pos.Add(new CurveLinePoint(key.time, key.value));
            return pos;
        }

        /// <summary>
        /// 由非匀速运动的轨迹,推导距离和时间的变化曲线,使运动匀速化.
        /// </summary>
        public static AnimationCurve GetLengthCurve(System.Func<float, Vector3> tPos, int keyCount)
        {
            var curve = new AnimationCurve();

            var len = 0f;
            var lenAry = new List<float>();

            Vector3 lastPos = tPos(0);
            for (int i = 0; i < keyCount; i++)
            {
                if (i == 0)
                {
                }
                else
                {
                    var curPos = tPos(i * 1f / (keyCount - 1));
                    len += Vector3.Distance(lastPos, curPos);
                    lastPos = curPos;
                }
                lenAry.Add(len);
            }

            for (int i = 0; i < keyCount; i++)
            {
                curve.AddKey(new Keyframe(i * 1f / (keyCount - 1), lenAry[i] / len));
            }
            return curve.InverseEvaluate();
        }

    }
}
