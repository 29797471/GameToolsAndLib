using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    public class CqTweenLerp_object :CqTweenLerp<object>
    {
        Func<object, object, float, object> _LerpUnclamped;
        public override object LerpUnclamped(object a, object b, float t)
        {
            if (_LerpUnclamped == null) _LerpUnclamped = UnityUtil.GetLerpUnclamped(a.GetType());
            return _LerpUnclamped(a,b,t);
        }
    }
    public class CqTweenLerp_float : CqTweenLerp<float>
    {
        public override float LerpUnclamped(float a, float b, float t)
        {
            //return (1 - t) * a + t * b;
            return Mathf.LerpUnclamped(a, b, t);
        }
    }
    public class CqTweenLerp_Vector2 : CqTweenLerp<Vector2>
    {
        public override Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return Vector2.LerpUnclamped(a, b, t);
        }
    }
    public class CqTweenLerp_Vector3 : CqTweenLerp<Vector3>
    {
        public override Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return Vector3.LerpUnclamped(a, b, t);
        }
    }
    public class CqTweenLerp_Vector4 : CqTweenLerp<Vector4>
    {
        public override Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
        {
            return Vector4.LerpUnclamped(a, b, t);
        }
    }
    public class CqTweenLerp_Quaternion : CqTweenLerp<Quaternion>
    {
        public override Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            return Quaternion.LerpUnclamped(a, b, t);
        }
    }
    public class CqTweenLerp_Color : CqTweenLerp<Color>
    {
        public override Color LerpUnclamped(Color a, Color b, float t)
        {
            return Color.LerpUnclamped(a, b, t);
        }
    }

    /// <summary>
    /// 可定义运动轨迹的曲线运动
    /// </summary>
    public class CqTweenLerp_Vector2_XY : CqTweenLerp_Vector2
    {
        public Func<float, float> xk_ykEvaluate;
        protected override void OnFrame(float t)
        {
            var xk = Evaluate(t);
            var yk = xk_ykEvaluate(xk);
            
            memberProxy.Value = new Vector2(
                Mathf.LerpUnclamped(start.x, end.x, xk), 
                Mathf.LerpUnclamped(start.y, end.y, yk)
                );
        }
    }
}
