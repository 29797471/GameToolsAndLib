using System;

namespace CqCore
{
    /// <summary>
    /// 终止操作的句柄<para/>
    /// 操作对象通过CancelAct+=添加撤销的操作<para/>
    /// 管理对象通过CancelAll执行所有撤销操作并清除<para/>
    /// 一般情况下不需要通过CancelAct-=取消撤销操作
    /// </summary>
    public class CancelHandle: ICancelHandle
    {

        public event Action CancelAct;

        /*
        public static implicit operator CancelHandle(Action action)
        {
            var handle = new CancelHandle();
            handle.CancelAct += action;
            return handle;
        }*/

        /// <summary>
        /// 执行通过CancelAct+=添加的所有操作<para/>
        /// 完成后清除所有委托
        /// </summary>
        public void CancelAll()
        {
            CancelAct?.Invoke();
            CancelAct = null;
        }
    }
}
