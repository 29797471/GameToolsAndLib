using CqCore;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 利用协程实现延迟调用(UnityMainTheard)
/// </summary>
[Obsolete("Use GlobalCoroutine instead")]
public static class UnityDelay
{
    /// <summary>
    /// 延迟second秒后,执行action
    /// </summary>
    [Obsolete("Use GlobalCoroutine.DelayCall instead")]
    public static void Call(float second, Action action, ICancelHandle handle =null)
    {
        GlobalCoroutine.DelayCall(second, action, handle);
    }
    /// <summary>
    /// 延迟second秒后,执行action
    /// </summary>
    [Obsolete("Use GlobalCoroutine.DelayCall instead")]
    public static DelayHandle Call(float second, Action action)
    {
        var cancel = new CancelHandle();
        var handle = new DelayHandle(cancel.CancelAll,action);
        GlobalCoroutine.DelayCall(second, action, cancel);
        return handle;
    }

    /// <summary>
    /// 延迟frames帧后,执行action
    /// </summary>
    [Obsolete("Use GlobalCoroutine.DelayCall instead")]
    public static DelayHandle Call(int frames, Action action)
    {
        var cancel = new CancelHandle();
        var handle = new DelayHandle(cancel.CancelAll, action);
        GlobalCoroutine.DelayCall(frames, action, cancel);
        return handle;
    }

}