using System;
using System.Collections;

namespace CqCore
{
    /// <summary>
    /// 全局协程操作类
    /// </summary>
    public static partial class GlobalCoroutine
    {

        /// <summary>
        /// 执行一个协同程序,支持嵌套<para/>
        /// yield return null;表示等1帧
        /// </summary>
        /// <param name="iterator">传入一个协程执行函数的调用</param>
        /// <param name="handle">终止这个协程执行的外部操作句柄</param>
        /// <param name="OnComplete">当协程执行完成后回调</param>
        public static CqCoroutine Start(IEnumerator iterator, ICancelHandle handle = null, Action OnComplete = null)
        {
            var coroutine = new CqCoroutine(iterator);

            coroutine.Start(handle, OnComplete);

            return coroutine;
        }

        /// <summary>
        /// 从协程中切换到线程池中的一个线程执行委托,并等待完成后返回
        /// </summary>
        public static IEnumerator ThreadPoolCall(Action action)
        {
            if (action == null) yield break;
            bool isDone = false;
            var bl = System.Threading.ThreadPool.QueueUserWorkItem((s) =>
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                }
                isDone = true;
            });
            while (!isDone) yield return null;
        }

        private static readonly int mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        /// <summary>
        /// 将委托添加主线程中,由update驱动执行<para/>
        /// 对于unity项目同时兼容编辑器环境和运行环境
        /// </summary>
        public static void Call(Action action, ICancelHandle handle = null)
        {
            if (System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId)
            {
                action.Invoke();
                return;
            }
            Action DoOnce = null;

            DoOnce = () =>
            {
                action.Invoke();
                _Update -= DoOnce;
            };
            _Update += DoOnce;
            if (handle != null)
            {
                handle.CancelAct += () => _Update -= DoOnce;
            }
        }
        /// <summary>
        /// 延迟second秒后,执行action
        /// </summary>
        public static CqCoroutine DelayCall(float second, Action action, ICancelHandle handle = null)
        {
            return Start(DelayCall(second, action), handle);
        }
        /// <summary>
        /// 延迟frames帧后,执行action
        /// </summary>
        public static CqCoroutine DelayCall(int frames, Action action, ICancelHandle handle = null)
        {
            return Start(DelayCall(frames, action), handle);
        }

        /// <summary>
        /// 每second秒后,执行action
        /// </summary>
        public static CqCoroutine LoopCall(float second, Action action, ICancelHandle handle = null)
        {
            return Start(LoopCall(second, action), handle);
        }
        /// <summary>
        /// 每frames帧后,执行action
        /// </summary>
        public static CqCoroutine LoopCall(int frames, Action action, ICancelHandle handle = null)
        {
            return Start(LoopCall(frames, action), handle);
        }

        /// <summary>
        /// 像普通函数一样执行协程,忽视所有协程内部的返回,直到函数退出。
        /// </summary>
        public static void BlockingCall(IEnumerator iterator)
        {
            var coroutine = new CqCoroutine(iterator);
            while (coroutine.MoveNext()) ;
        }

        static IEnumerator DelayCall(float seconds, Action action)
        {
            yield return Sleep(seconds);
            action();
        }

        static IEnumerator DelayCall(int frames, Action action)
        {
            yield return Sleep(frames);
            action();
        }

        static IEnumerator LoopCall(int frames, Action action)
        {
            while (true)
            {
                yield return Sleep(frames);
                action();
            }
        }

        static IEnumerator LoopCall(float second, Action action)
        {
            while (true)
            {
                yield return Sleep(second);
                action();
            }
        }
    }
}
