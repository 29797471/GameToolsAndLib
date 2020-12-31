using CqCore;

namespace System.Threading
{

    /// <summary>
    /// 切换线程调用的API
    /// </summary>
    public static class ThreadUtil
    {
        /// <summary>
        /// 开启线程异步延迟执行方法
        /// </summary>
        public static void DelayCall(float second, Action action, ICancelHandle handle = null)
        {
            DelayCall((int)(second * 1000), action, handle);
        }

        /// <summary>
        /// 开启线程异步延迟执行方法
        /// </summary>
        public static void DelayCall(int millisecond, Action action, ICancelHandle handle =null)
        {
            if (action == null) return;
            PoolCall(() =>
            {
                Thread.Sleep(millisecond);
                action();
            }, handle);
            //利用System.Threading.Timer实现延迟调用
            /*
            var t = new System.Threading.Timer((o) =>
            {
                action();
            });
            t.Change(millisecond, Timeout.Infinite);
            return new DelayHandle(t.Dispose, action);
            */
            //利用System.Timers.Timer实现延迟调用
            //System.Timers.Timer使用System.Threading.Timer。区别在于System.Timers.Timer可以用于可视化的插入窗体中.
            /*
            Timer tt = new Timer(millisecond);
            tt.Elapsed += (x, y) =>
            {
                tt.Dispose();
                action.Invoke();
            };
            tt.Start();
            return new DelayHandle(tt.Dispose, action );
             */
        }

        /// <summary>
        /// 开启线程异步执行方法
        /// </summary>
        public static void ThreadCall(Action action, ICancelHandle cancelHandle = null)
        {
            if (action == null) return;
            Thread thread;
            thread = new Thread(() =>
            {
                action();
                thread = null;
            });
            thread.Start();
            if (cancelHandle != null)
            {
                cancelHandle.CancelAct += () => { if (thread != null) thread.Abort(); };
            }
        }

        /// <summary>
        /// 由线程池分配线程异步执行一些短时间的,有可能是高频的任务
        /// </summary>
        public static void PoolCall(Action action, ICancelHandle handle =null)
        {
            if (action == null) return;
            if(handle!=null)
            {
                bool doAction = true;
                handle.CancelAct += () => doAction = false;
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    if(doAction)
                    {
                        action();
                    }
                });
            }
            else
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    action();
                });
            }
        }

        /// <summary>
        /// 开启线程异步调用,完成回调OnResult
        /// </summary>
        public static void PoolCall(Action fun, Action<bool> OnResult, ICancelHandle handle =null)
        {
            if (OnResult == null)
            {
                throw new Exception("回调函数不能为空");
            }
            PoolCall(() =>
            {
                try
                {
                    fun();
                    OnResult(true);
                }
                catch (Exception)
                {
                    OnResult(false);
                }
            }, handle);
        }
    }
}


