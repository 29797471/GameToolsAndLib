using System;
using UnityEngine;
using UnityCore;

public class CqTweenGroupItem : MonoBehaviour, ICqTweenGroupItem
{
    public CqTweenGroup group;
    void Awake()
    {
        if(group!=null)group.Add(this);
    }
    public virtual TweenMode Mode
    {
        set
        {
        }
    }

    TweenMode ICqTweenGroupItem.Mode { set => throw new NotImplementedException(); }

    public virtual bool isPlaying
    {
        get
        {
            return false;
        }
    }

    public virtual void Immediately()
    {
    }
    public virtual void Play()
    {
    }
    public virtual bool PlayAndDo(Action OnComplete = null)
    {
        return false;
    }

    public virtual void Stop()
    {
    }
    public virtual void SetCurrentByStart()
    {

    }
    public virtual void SetCurrentByEnd()
    {

    }
    public virtual void SetStart()
    {

    }
    public virtual void SetEnd()
    {

    }

    public virtual void SetCurrentStartOrEnd(bool isStart)
    {
    }

    bool ICqTweenGroupItem.PlayAndDo(Action OnComplete)
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.Play()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.Stop()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.Immediately()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.SetCurrentByStart()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.SetCurrentByEnd()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.SetStart()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.SetEnd()
    {
        throw new NotImplementedException();
    }

    void ICqTweenGroupItem.SetCurrentStartOrEnd(bool isStart)
    {
        throw new NotImplementedException();
    }
}