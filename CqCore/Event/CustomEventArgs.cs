using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 事件消息体抽象类
    /// </summary>
    public abstract class CustomEventArgs : EventArgs
    {
        /// <summary>
        /// 分发事件
        /// </summary>
        public abstract bool Notify(object sender = null);
    }
}
