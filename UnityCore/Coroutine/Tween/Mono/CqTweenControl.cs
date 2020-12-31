using System;
using UnityCore;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓动控制外部调用接口基类
/// </summary>
[ExecuteInEditMode]
public class CqTweenControl : CqTweenInternal,ICqTweenGroupItem
{
    public void TestPlay()
    {
        if(testPlaying)
        {
            var bl = PlayAndDo(() =>
            {
                testPlaying = false;
            });
            if (!bl) testPlaying = false;
        }
        else
        {
            InternalCancel();
        }
    }
    public void AddToGroup()
    {
        if(group!=null)group.Add(this);
    }
    [Header("完成后回调")]
    public UnityEvent OnComplete;

    [Header("完成一次后回调")]
    public UnityEvent OnOnceComplete;
    //void OnEnable()
    //{
    //    var bl = PlayAndDo(() =>
    //    {
    //        enabled = false;
    //    });
    //    if (!bl) enabled = false;
    //}
    //void OnDisable()
    //{
    //    InternalCancel();
    //}
    protected float startPercent;
    public void PlayWidthPercent(float startPercent)
    {
        this.startPercent = startPercent;
        Play();
        //this.startPercent = 0f;
    }
    public void Play()
    {
        InternalPlay(
            () =>
            {
                if (OnComplete != null) OnComplete.Invoke();
            },
            () =>
            {
                if (OnOnceComplete != null) OnOnceComplete.Invoke();
            });
    }
    public bool PlayAndDo(Action _OnComplete = null)
    {
        return InternalPlay(
            () =>
            {
                if (OnComplete != null) OnComplete.Invoke();
                if (_OnComplete != null) _OnComplete();
            },
            () =>
            {
                if (OnOnceComplete != null) OnOnceComplete.Invoke();
            });
    }
    public void Stop()
    {
        InternalCancel();
    }

    /// <summary>
    /// 立即完成
    /// </summary>
    [Button("立即完成"), Click("Immediately")]
    public string __Immediately;

    /// <summary>
    /// 立即完成
    /// </summary>
    public void Immediately()
    {
        InternalImmediately();
        if (OnComplete != null) OnComplete.Invoke();
    }
    
    void OnDestroy()
    {
        InternalCancel();
    }
    void OnApplicationQuit()
    {
        InternalCancel();
    }

    public virtual void SetCurrentByStart()
    {
        throw new NotImplementedException();
    }

    public virtual void SetCurrentByEnd()
    {
        throw new NotImplementedException();
    }

    public virtual void SetStart()
    {
        throw new NotImplementedException();
    }

    public virtual void SetEnd()
    {
        throw new NotImplementedException();
    }

    public virtual void SetCurrentStartOrEnd(bool isStart)
    {
        throw new NotImplementedException();
    }
}

