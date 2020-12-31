using System;
namespace CqCore
{
    /// <summary>
    /// 延迟句柄
    /// </summary>
    public class DelayHandle
    {
        Action CallbackCancel;
        Action CallbackImmediately;
        public DelayHandle(Action CallbackCancel, Action CallbackImmediately)
        {
            this.CallbackCancel = CallbackCancel;
            this.CallbackImmediately = CallbackImmediately;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        public void Immediately()
        {
            Cancel();
            CallbackImmediately?.Invoke();
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        public void Cancel()
        {
            CallbackCancel?.Invoke();
        }
    }
}