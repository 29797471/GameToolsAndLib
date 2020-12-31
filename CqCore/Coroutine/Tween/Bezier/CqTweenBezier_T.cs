using System;

namespace CqCore
{

    /// <summary>
    /// 贝塞尔插值缓动泛型类
    /// </summary>
    public abstract class CqTweenBezier<T>: CqTweenBezier
    {
        public abstract T BezierUnclamped(T a, T b,T c, float t);

        public T a;
        public T b;
        public T c;

        public T current
        {
            get
            {
                if (memberProxy == null)
                {
                    throw new Exception("代理成员未设置");
                }
                return (T)memberProxy.Value;
            }
        }

        public override object AValue
        {
            set
            {
                try
                {
                    a = (T)value;
                }
                catch (Exception)
                {
                }
            }
            get
            {
                return a;
            }
        }

        public override object BValue
        {
            set
            {
                try
                {
                    b = (T)value;
                }
                catch (Exception)
                {
                }
            }
            get
            {
                return b;
            }
        }

        public override object CValue
        {
            set
            {
                try
                {
                    c = (T)value;
                }
                catch (Exception)
                {
                }
            }
            get
            {
                return c;
            }
        }

        
        /// <summary>
        /// 在差值系数的改变中回调设置属性值.
        /// </summary>
        override protected void OnFrame(float t)
        {
            //CqDebug.Log("OnFrame:" + t);

            //此处T => object 经过装箱转化会产生gc
            memberProxy.Value = BezierUnclamped(a, b, c,Evaluate(t));
        }
    }
}
