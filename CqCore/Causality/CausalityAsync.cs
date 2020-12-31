using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 异步处理数据,缓存处理结果<para/>
    /// 保证同一内容只处理一次,不会重复处理<para/>
    /// 异步因果关系(同样的因得同样的果)<para/>
    /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理<para/>
    /// </summary>
    public class CausalityAsync<TKey, TValue>
    {
        Action<TKey, Action<TValue>> CalcValueAsync;
        Dictionary<TKey, TValue> cacheResult;//缓存结果
        Dictionary<TKey, Action<TValue>> cacheDoing;//缓存正在进行的计算

        /// <summary>
        /// 因果关系(同样的因得同样的果)
        /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理
        /// </summary>
        public CausalityAsync(Action<TKey, Action<TValue>> CalcValueAsync)
        {
            cacheResult = new Dictionary<TKey, TValue>();
            cacheDoing = new Dictionary<TKey, Action<TValue>>();
            this.CalcValueAsync = CalcValueAsync;
        }


        /// <summary>
        /// 异步获得结果<para/>
        /// 如果没有处理过,则开始处理,完成后回调<para/>
        /// 如果正在处理中,则等到完成后回调<para/>
        /// 如果已经完成处理过,则直接返回之前处理的结果
        /// </summary>
        public void CallAsync(TKey key, Action<TValue> OnComplete)
        {
            if (cacheResult.ContainsKey(key))//已经获得过结果
            {
                if(OnComplete!=null) OnComplete(cacheResult[key]);
            }
            else if (cacheDoing.ContainsKey(key))//正在获得过结果
            {
                cacheDoing[key] += value =>
                {
                    if (OnComplete != null) OnComplete(value);
                };
            }
            else//还未获取过
            {
                cacheDoing[key] = value =>
                {
                    if (OnComplete != null) OnComplete(value);
                };

                CalcValueAsync(key, value =>
                {
                    cacheResult[key] = value;
                    cacheDoing[key](value);
                    cacheDoing.Remove(key);
                });
            }
        }
        /// <summary>
        /// 对获取过的key同步获得结果
        /// </summary>
        public TValue Call(TKey key)
        {
            if (cacheResult.ContainsKey(key))
            {
                return cacheResult[key];
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            cacheResult.Clear();
            cacheDoing.Clear();
        }
    }
}
