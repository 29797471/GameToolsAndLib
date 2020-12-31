using System;

namespace CqCore
{

    /// <summary>
    /// 线性插值缓动泛型类
    /// </summary>
    public abstract class CqTweenLerp<T>: CqTweenLerp
    {
        public abstract T LerpUnclamped(T a, T b, float t);

        public T start;
        public T end;

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

        public override object StartValue
        {
            set
            {
                try
                {
                    start = (T)value;
                }
                catch (Exception)
                {
                }
            }
            get
            {
                return start;
            }
        }

        public override object EndValue
        {
            set
            {
                try
                {
                    end = (T)value;
                }
                catch (Exception)
                {
                }
            }
            get
            {
                return end;
            }
        }

        
        /// <summary>
        /// 在差值系数的改变中回调设置属性值.
        /// </summary>
        override protected void OnFrame(float t)
        {
            //CqDebug.Log("OnFrame:" + t);

            //此处T => object  如果T是 struct 会经过装箱转化,并产生gc
            memberProxy.Value = LerpUnclamped(start, end, Evaluate(t));
        }
    }
}
