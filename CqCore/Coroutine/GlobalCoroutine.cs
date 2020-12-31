using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 全局协程操作类<para/>
    /// </summary>
    public static partial class GlobalCoroutine
    {
        /// <summary>
        /// 记录协程启动时的堆栈,便于定位代码
        /// </summary>
        public static bool recordStacktrace=false;

        /// <summary>
        /// 当前协程数量
        /// </summary>
        public static int CqCoroutineCount { get { return _list.Count; } }


        /// <summary>
        /// 每帧时差,用于驱动协程执行<para/>
        /// 对于Unity运行时这个值等于Time.deltaTime
        /// </summary>
        public static float deltaTime
        {
            get
            {
                return deltaTick / tick_seconds_scale;
            }
        }

        /// <summary>
        /// 1秒等于10^7tcik
        /// </summary>
        const float tick_seconds_scale = 10000000f;

        /// <summary>
        /// 1970-1-1至今的tick
        /// </summary>
        public static long Tick { get; private set; }

        /// <summary>
        /// 每帧的tick增量
        /// </summary>
        public static long deltaTick { get; private set; }

        /// <summary>
        /// 直接将对象的成员函数添加到委托会形成托管堆,带来比较高的gc.
        /// </summary>
        internal static Action _Update;

        static List<CqCoroutine> _list = new List<CqCoroutine>();

        /// <summary>
        /// 每帧调用一次,传入tick
        /// </summary>
        public static void Update(long tick)
        {
            deltaTick = tick - Tick;
            Tick = tick;
            
            _Update?.Invoke();
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                var it = _list[i];
                if (it.Updating) it.Update();
                else
                {
                    _list.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 停止当前所有协程
        /// </summary>
        public static void StopAllCoroutines()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].Stop();
            }
            _Update = null;
        }
        internal static void _AddCqCoroutine(CqCoroutine cc, ICancelHandle handle)
        {
            Call(() => _list.Add(cc), handle);
        }

        /// <summary>
        /// 自定义接协程返回对象作等待的处理            <para/>
        /// 1.返回int 等待x帧                        <para/>
        /// 2.返回float 等待x秒                      <para/>
        /// 3.返回null 等待1帧                       <para/>
        /// 4.返回WaitForFrames 等待x帧
        /// </summary>
        static internal Func<object, object> GetCurrentToWaitFor = current =>
        {
            if (current is int)
            {
                return Sleep((int)current);
            }
            else if (current is float)
            {
                return Sleep((float)current);
            }
            else
            {
                throw new Exception("未定义的协程返回类型" + current);
            }
        };

        /// <summary>
        /// 自定义接协程返回对象作等待的处理            <para/>
        /// 1.返回int 等待x帧                        <para/>
        /// 2.返回float 等待x秒                      <para/>
        /// 3.返回null 等待1帧                       <para/>
        /// 4.返回WaitForFrames 等待x帧
        /// </summary>
        public static void Define_GetCurrentToWaitFor(Func<object, object> _GetCurrentToWaitFor)
        {
            GetCurrentToWaitFor = _GetCurrentToWaitFor;
        }


        /// <summary>
        /// 获取经过seconds秒后的最新时刻tick
        /// </summary>
        public static long GetTickTime(float seconds)
        {
            return Tick + GetSpanTick(seconds);
        }
        /// <summary>
        /// 获取一段时间对应的tick长度
        /// </summary>
        public static long GetSpanTick(float realtimeSinceStartup)
        {
            return (long)(realtimeSinceStartup * tick_seconds_scale);
        }

        /// <summary>
        /// 获取时刻tick对应的剩余秒数
        /// </summary>
        public static float ToSeconds(long tick)
        {
            return (tick - Tick) / tick_seconds_scale;
        }

    }
}
