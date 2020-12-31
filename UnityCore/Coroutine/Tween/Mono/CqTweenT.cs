using CqCore;
using System;
using UnityCore;
using UnityEngine;

/// <summary>
/// 缓动泛型基类
/// </summary>
[ExecuteInEditMode]
public class CqTweenT<T> : CqTweenControl
{
    void Awake()
    {
        if (group != null) group.Add(this);
        if(testPlaying)
        {
            TestPlay();
        }
    }
    protected virtual Func<T, T, float, T> LerpUnclamped
    {
        get
        {
            return null;
        }
    }

    [ContextMenuItem("=当前值", "SetStart")]
    [ContextMenuItem("当前值=", "SetCurrentByStart")]
    [CqLabel("起始")]
    public T mStart;

    [ContextMenuItem("=当前值", "SetEnd")]
    [ContextMenuItem("当前值=", "SetCurrentByEnd")]
    [CqLabel("终止")]
    public T mEnd;

    protected T Start
    {
        get
        {
            return mStart;
        }
        set
        {
            mStart = value;
        }
    }
    protected T End
    {
        get
        {
            return mEnd;
        }
        set
        {
            mEnd = value;
        }
    }
    protected T Current
    {
        get
        {
            return (T)comp.Value;
        }
        set
        {
            if (comp != null )comp.Value= value;
        }
    }
    public override void SetStart()
    {
        if (comp != null && comp.com != null) Start = Current;
    }
    public override void SetEnd()
    {
        if (comp != null && comp.com != null) End = Current;
    }
    public override void SetCurrentByStart()
    {
        Current = Start;
    }
    public override void SetCurrentByEnd()
    {
        Current = End;
    }

    /// <summary>
    /// 将当前关联属性设置到起始值(true),或者终止值(false)
    /// </summary>
    public override void SetCurrentStartOrEnd(bool isStart)
    {
        if (isStart)
        {
            Current = Start;
        }
        else
        {
            Current = End;
        }
    }
    /// <summary>
    /// 将当前关联属性缓动到起始值(true),或者终止值(false)
    /// </summary>
    public bool TweenToEndOrStart
    {
        set
        {
            mode = value ? TweenMode.ToEnd: TweenMode.ToStart;
            Play();
        }
    }
    /// <summary>
    /// 将当前关联属性缓动到起始值(true),或者终止值(false)
    /// </summary>
    public bool TweenToStartOrEnd
    {
        set
        {
            mode = value ? TweenMode.ToStart : TweenMode.ToEnd;
            Play();
        }
    }

    /// <summary>
    /// 将当前关联属性设置到起始值(true),或者终止值(false)
    /// </summary>
    public bool ToStartOrEnd
    {
        set
        {
            if(value)
            {
                Current = Start;
            }
            else
            {
                Current = End;
            }
        }
    }



    protected override DelayHandle TweenOnce(Action mOnOnceComplete)
    {
        if (comp == null)
        {
            Debug.LogError(this + "中找不到comp");
        }
        
        T a = default(T);T b= default(T);
        switch (mode)
        {
            case TweenMode.StartToEnd:
                a = Start;
                b = End;
                break;
            case TweenMode.EndToStart:
                a = End;
                b = Start;
                break;
            case TweenMode.ToStart:
                a = Current;
                b = Start;
                break;
            case TweenMode.ToEnd:
                a = Current;
                b = End;
                break;
        }
        
        //缓动播放时如果往复运动选项开启,立即切换模式
        //这样当播放时可以中途终止并返回
        //常用于一些按钮点击的往复运动.
        if (pingpong)
        {
            switch (mode)
            {
                case TweenMode.StartToEnd:
                    Mode = TweenMode.EndToStart;
                    break;
                case TweenMode.EndToStart:
                    Mode = TweenMode.StartToEnd;
                    break;
                case TweenMode.ToStart:
                    Mode = TweenMode.ToEnd;
                    break;
                case TweenMode.ToEnd:
                    Mode = TweenMode.ToStart;
                    break;
            }
        }
        Func<float, float> Evaluate = curve.Evaluate;

        switch (mode)
        {
            case TweenMode.EndToStart:
            case TweenMode.ToStart:
                if(useBackCurve)
                {
                    Evaluate = backCurve.Evaluate;
                }
                else
                {
                    Evaluate = curve.ReEvaluate();
                }
                break;
        }
        var handle = new CqTweenLerp_object ()
        {
            memberProxy = comp.MemProxy,
            start = a,
            end = b,
            Evaluate = Evaluate,
        };
        handle.Play(duration, null ,startPercent);
        startPercent = 0f;
        handle.OnComplete = mOnOnceComplete;
        return new DelayHandle(handle.Cancel,handle.Immediately);
    }
}
