using System;

namespace CqCore
{

    /// <summary>
    /// 线性插值缓动基类
    /// </summary>
    public abstract class CqTweenLerp: TweenHandleBase
    {
        public abstract object StartValue { set; get; }
        public abstract object EndValue { set; get; }
    }
}
