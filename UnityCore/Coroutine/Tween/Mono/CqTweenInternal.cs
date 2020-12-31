using CqCore;
using System;
using UnityEngine;

/// <summary>
/// 循环缓动控制内部实现基类
/// </summary>
[ExecuteInEditMode]
public class CqTweenInternal : CqTweenData
{
    int realLoopTimes;

    public bool isPlaying { get { return handle != null; } }

    DelayHandle handle;

    Action _OnComplete;
    Action _OnOnceComplete;
    protected bool InternalPlay(Action _OnComplete,Action _OnOnceComplete)
    {
        if (comp == null || comp.com == null)
        {
            Debug.LogError(transform.PathInHierarchy() + "-未设置缓动属性");
            return false;
        }
        InternalCancel();
        realLoopTimes = loopTimes;
        this._OnComplete = _OnComplete;
        this._OnOnceComplete = _OnOnceComplete;

        var cancel = new CancelHandle();
        handle = new DelayHandle(cancel.CancelAll, LoopPlay);
        GlobalCoroutine.DelayCall(startDelay, LoopPlay, cancel);
        return true;
    }
    protected void InternalCancel()
    {
        //Debug.Log("InternalCancel");
        if (handle != null)
        {
            //Debug.Log("Cancel");
            handle.Cancel();
            handle = null;
        }
    }
    protected void InternalImmediately()
    {
        if (handle != null)
        {
            handle.Immediately();
            handle = null;
            if (_OnComplete != null) _OnComplete();
        }
    }
    void OncePlayEnd()
    {
        //if (pingpong)
        //{
        //    reverse = !reverse;
        //    switch (mode)
        //    {
        //        case TweenMode.StartToEnd:
        //            Mode = TweenMode.EndToStart;
        //            break;
        //        case TweenMode.EndToStart:
        //            Mode = TweenMode.StartToEnd;
        //            break;
        //        case TweenMode.ToStart:
        //            Mode = TweenMode.ToEnd;
        //            break;
        //        case TweenMode.ToEnd:
        //            Mode = TweenMode.ToStart;
        //            break;
        //    }
        //}
        var cancel = new CancelHandle();
        handle = new DelayHandle(cancel.CancelAll, LoopPlay);
        GlobalCoroutine.DelayCall(loopDelay, LoopPlay, cancel);
    }
    void LoopPlay()
    {
        if (realLoopTimes != 0)
        {
            realLoopTimes--;
            if (_OnOnceComplete != null) _OnOnceComplete();
            handle = TweenOnce(OncePlayEnd);
        }
        else
        {
            handle = null;
            if (_OnOnceComplete != null) _OnOnceComplete();
            if (_OnComplete != null) _OnComplete();
        }
    }
    protected virtual DelayHandle TweenOnce(Action _OnComplete)
    {
        return null;
    }
}
