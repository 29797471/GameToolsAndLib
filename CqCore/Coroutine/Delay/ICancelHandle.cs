using System;

namespace CqCore
{
    /// <summary>
    ///  表示对象可以派发取消事件<para/>
    ///  部分逻辑可以监听取消事件阻止操作.
    /// </summary>
    public interface ICancelHandle
    {
        /// <summary>
        /// 通过+=注入取消时执行的回调<para/>
        /// 当操作实例执行CancelAll后,所有操作都会被清除<para/>
        /// 所以不需要定义-=
        /// </summary>
        event Action CancelAct;
    }
}
