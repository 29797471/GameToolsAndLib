using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 因果关系(同样的因得同样的果)
    /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理
    /// </summary>
    public class Causality<T1,O>
    {
        Func<T1, O> CalcValue;
        Dictionary<T1, O> dic;

        /// <summary>
        /// 因果关系(同样的因得同样的果)
        /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理
        /// </summary>
        public Causality(Func<T1, O> CalcValue)
        {
            this.CalcValue = CalcValue;
            dic = new Dictionary<T1, O>();
        }

        /// <summary>
        /// 获得处理结果
        /// 有计算过:直接返回结果
        /// 没有计算过:计算结果,存储,并返回
        /// </summary>
        public O Call(T1 intput1)
        {
            if(!dic.ContainsKey(intput1))
            {
                dic[intput1] = CalcValue(intput1);
            }
            return dic[intput1];
        }
        
        public void Clear()
        {
            dic.Clear();
        }
        public static implicit operator Causality<T1, O>(Func<T1,O> fun)
        {
            return new Causality<T1, O>(fun);
        }
    }
    /// <summary>
    /// 因果关系(同样的因得同样的果)
    /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理
    /// </summary>
    public class Causality<T1,T2,O>
    {
        Func<T1, T2, O> fun;
        Causality2_1<T1,T2, O> dic;

        public Causality(Func<T1,T2, O> fun)
        {
            this.fun = fun;
            dic = new Causality2_1<T1,T2, O>();
        }
        public O Call(T1 intput1,T2 input2)
        {
            if (!dic.ContainsKey(intput1,input2))
            {
                dic[intput1,input2] = fun(intput1,input2);

            }
            return dic[intput1, input2];
        }
        public void Clear()
        {
            dic.Clear();
        }
        public static implicit operator Causality<T1, T2, O>(Func<T1, T2, O> fun)
        {
            return new Causality<T1, T2, O>(fun);
        }
    }
}
