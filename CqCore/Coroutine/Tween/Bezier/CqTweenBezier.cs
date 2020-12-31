using System;

namespace CqCore
{

    /// <summary>
    /// 贝塞尔插值缓动基类
    /// </summary>
    public abstract class CqTweenBezier: TweenHandleBase
    {
        public abstract object AValue { set; get; }
        public abstract object BValue { set; get; }
        public abstract object CValue { set; get; }
    }
}
