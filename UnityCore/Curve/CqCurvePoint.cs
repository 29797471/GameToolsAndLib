using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 曲线点
    /// </summary>
    [System.Serializable]
    public class CqCurvePoint
    {
        /// <summary>
        /// 端点
        /// </summary>
        [Vector("端点")]
        public Vector3 point;

        /// <summary>
        /// 进入的切线起始点
        /// </summary>
        
        public Vector3 inTangent
        {
            get
            {
                return inVec + point;
            }
            set
            {
                inVec = value - point;
            }
        }

        /// <summary>
        /// 离开的切线终止点
        /// </summary>
        public Vector3 outTangent
        {
            get
            {
                return outVec + point;
            }
            set
            {
                outVec = value - point;
            }
        }

        /// <summary>
        /// 进入的向量
        /// </summary>
        [HideInInspector]
        public Vector3 inVec;

        /// <summary>
        /// 离开的向量
        /// </summary>
        [HideInInspector]
        public Vector3 outVec;

        /// <summary>
        /// 奇点<para/>
        /// 进入的切线和离开的切线不对称
        /// </summary>
        [HideInInspector]
        public bool singular;


        /// <summary>
        /// 附加数据
        /// </summary>
        [HideInInspector]
        public int data;

        /// <summary>
        /// 贝塞尔曲线插值
        /// </summary>
        public Vector3 LerpUnclamped( CqCurvePoint p1,float t)
        {
            return BezierUtil.LerpUnclamped(point, outTangent, p1.inTangent, p1.point,t);
        }

        /// <summary>
        /// 贝塞尔切线插值
        /// </summary>
        public Vector3 GetTangent(CqCurvePoint p1, float t)
        {
            return BezierUtil.GetTangent(point, outTangent, p1.inTangent, p1.point, t);
        }

        /// <summary>
        /// 获得曲线上一点的差值系数基于距离的变化率
        /// </summary>
        public float GetDeltaT_Dis_K(CqCurvePoint b, float t, float deltaT = 0.00001f)
        {
            var pa = LerpUnclamped(b, t - deltaT);
            var pb = LerpUnclamped(b, t + deltaT);
            var deltaLenK = 1 / Vector3.Distance(pa / (deltaT * 2), pb / (deltaT * 2));

            return deltaLenK;
        }
        /// <summary>
        /// 贝塞尔曲线长度
        /// </summary>
        public float Length(CqCurvePoint p1, int partCount = 100)
        {
            return BezierUtil.Length(point, outTangent, p1.inTangent, p1.point);
        }

        /// <summary>
        /// 二分法求贝塞尔曲线和直线的交点返回t:0~1
        /// </summary>
        public float GetCrossoverPoint(CqCurvePoint p1, Ray ray, Matrix4x4 mat, float limit=0.01f)
        {
            System.Func<float, float> GetDis = t => mat.MultiplyVector(ray.GetVerticalVec(LerpUnclamped(p1, t))).magnitude;
            
            System.Func<float, float,float> OnCalc = null;
            int count = 10;
            float lastCalcT=float.NaN;
            OnCalc=(t0, t1) =>
            {
                var minDistance = float.MaxValue;
                var minT =0f;
                for (var i = 0; i < count+1; i++)
                {
                    var t = Mathf.LerpUnclamped(t0, t1, i * 1f / count);
                    var dst  = GetDis(t);
                    if (dst < minDistance)
                    {
                        minDistance = dst;
                        minT = t;
                    }
                    if (dst < limit)
                    {
                        return t;
                    }
                }
                if(float.IsNaN(lastCalcT))
                {
                    lastCalcT = minT;
                }
                else if(lastCalcT==minT)
                {
                    //两次计算没有变化,说明再算下去也没有必要
                    return -1;
                }
                else
                {
                    lastCalcT = minT;
                }
                var step = (t1 - t0) * 1f / count;
                return OnCalc(Mathf.Max(0,minT - step),Mathf.Min(1,minT + step));
            };
            return OnCalc(0, 1);
        }

        /// <summary>
        /// 判定是否是直线
        /// </summary>
        public bool IsLine(CqCurvePoint next)
        {
            return outVec == Vector3.zero && next.inVec == Vector3.zero;
        }
        /// <summary>
        /// 贝塞尔曲线一分为二算法
        /// http://www.360doc.com/content/16/0101/20/1489589_524673502.shtml
        /// </summary>
        public CqCurvePoint Split(CqCurvePoint next,float t)
        {
            var p = new CqCurvePoint();
            
            var A = point;
            var B = outTangent;
            var C = next.inTangent;
            var D = next.point;

            if (IsLine(next))
            {
                p.point = A * (1 - t) +D*t;
                return p;
            }
            var F = A * (1 - t) + B * t;
            var G = B * (1 - t) + C * t;
            var H = C * (1 - t) + D * t;

            var I = F * (1 - t) + G * t;
            var J = G * (1 - t) + H * t;

            var E = I * (1 - t) + J * t;

            p.inVec = I - E;
            p.point = E;
            p.outVec = J - E;

            outVec = F - A;
            next.inVec = H - D;

            singular = true;
            next.singular = true;
            p.singular = true;
            return p;
        }

        /// <summary>
        /// 平滑算法
        /// </summary>
        internal void Smooth(CqCurvePoint prev,CqCurvePoint next,float inTangentK)
        {
            outVec = (next.point - prev.point) * inTangentK;
            inVec = -outVec;
            singular = false;
        }
    }
}
