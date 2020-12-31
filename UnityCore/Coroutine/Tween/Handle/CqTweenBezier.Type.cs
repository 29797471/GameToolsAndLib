using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    /*
    public class CqTweenBezier_object :CqTweenBezier<object>
    {
        Func<object, object,object, float, object> _BezierUnclamped;
        public override object BezierUnclamped(object a, object b,object c, float t)
        {
            if (_BezierUnclamped == null) _BezierUnclamped = UnityUtil.GetLerpUnclamped(a.GetType());
            return _BezierUnclamped(a,b,t);
        }
    }*/
    public class CqTweenBezier_Vector2 : CqTweenBezier<Vector2>
    {
        public override Vector2 BezierUnclamped(Vector2 a, Vector2 b, Vector2 c,float t)
        {
            //return (1 - t) * a + t * b;
            return BezierUtil.LerpUnclamped(a, b,c, t);
        }
    }
    public class CqTweenBezier_Vector3 : CqTweenBezier<Vector3>
    {
        public override Vector3 BezierUnclamped(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            //return (1 - t) * a + t * b;
            return BezierUtil.LerpUnclamped(a, b, c, t);
        }
    }
}
