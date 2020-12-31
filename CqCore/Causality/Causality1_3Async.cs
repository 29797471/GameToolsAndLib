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
    public class Causality1_3Async<InKey, OutValue1,OutValue2, OutValue3>
    {
        Action<InKey, Action<OutValue1,OutValue2, OutValue3>> CalcValueAsync;
        Dictionary<InKey, Action> cacheResult;//缓存结果
        Dictionary<InKey, Action<OutValue1,OutValue2, OutValue3>> cacheDoing;//缓存正在进行的计算

        /// <summary>
        /// 因果关系(同样的因得同样的果)
        /// 通过一个外部定义的函数,来完成执行,记录执行结果,下次同样的参数传入不再处理
        /// </summary>
        public Causality1_3Async(Action<InKey, Action<OutValue1,OutValue2, OutValue3>> CalcValueAsync)
        {
            cacheResult = new Dictionary<InKey, Action>();
            cacheDoing = new Dictionary<InKey, Action<OutValue1,OutValue2, OutValue3>>();
            this.CalcValueAsync = CalcValueAsync;
        }

        Action<OutValue1, OutValue2, OutValue3> _OnComplete;
        /// <summary>
        /// 异步获得结果<para/>
        /// 如果没有处理过,则开始处理,完成后回调<para/>
        /// 如果正在处理中,则等到完成后回调<para/>
        /// 如果已经完成处理过,则直接返回之前处理的结果
        /// </summary>
        public void CallAsync(InKey key, Action<OutValue1,OutValue2, OutValue3> OnComplete)
        {
            if (cacheResult.ContainsKey(key))//已经获得过结果
            {
                if (OnComplete != null)
                {
                    _OnComplete = OnComplete;
                    cacheResult[key]();
                }
            }
            else if (cacheDoing.ContainsKey(key))//正在获得过结果
            {
                cacheDoing[key] += (v1,v2,v3) =>
                {
                    if (OnComplete != null) OnComplete(v1,v2,v3);
                };
            }
            else//还未获取过
            {
                cacheDoing[key] = (v1, v2, v3) =>
                {
                    if (OnComplete != null) OnComplete(v1, v2, v3);
                };

                CalcValueAsync(key, (v1, v2, v3) =>
                {
                    cacheResult[key] = ()=>
                    {
                        _OnComplete(v1, v2, v3);
                    };
                    cacheDoing[key](v1, v2, v3);
                    cacheDoing.Remove(key);
                });
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
