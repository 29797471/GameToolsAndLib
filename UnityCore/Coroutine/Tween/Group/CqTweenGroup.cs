using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓动组
/// </summary>
[ExecuteInEditMode]
public class CqTweenGroup : CqTweenGroupItem
{
    [TextBox("缓动描述", true), ToolTip("对该缓动组的解释"), Height(40)]
    public string desc;

    [ContextMenu("打印列表")]
    public void TestCount()
    {
        if(tweenList==null)
        {
            Debug.Log("列表为空");
        }
        else Debug.Log("列表长度为:"+tweenList.Count);
    }
    List<ICqTweenGroupItem> tweenList;
    public void Add(ICqTweenGroupItem item)
    {
        if (tweenList == null) tweenList = new List<ICqTweenGroupItem>();
        if (!tweenList.Contains(item)) tweenList.Add(item);
    }

    public override TweenMode Mode
    {
        set
        {
            if(tweenList!=null)
            {
                foreach (ICqTweenGroupItem it in tweenList)
                {
                    it.Mode = value;
                }
            }
        }
    }
    public UnityEvent OnComplete;

    [CheckBox("测试播放"), OnValueChanged("TestPlay")]
    public bool testPlaying;

    public void TestPlay()
    {
        if (testPlaying)
        {
            var bl = PlayAndDo(() =>
            {
                testPlaying = false;
            });
            if (!bl) testPlaying = false;
        }
        else
        {
            Stop();
        }
    }
    //void OnEnable()
    //{
    //    var bl= PlayAndDo(() =>
    //    {
    //        enabled = false;
    //    });
    //    if (!bl) enabled = false;
    //}
    //void OnDisable()
    //{
    //    Stop();
    //}
    void OnDestroy()
    {
        Stop();
    }

    public override bool isPlaying
    {
        get
        {
            if (tweenList != null && tweenList.Count > 0)
            {
                for (int i = 0; i < tweenList.Count; i++)
                {
                    if (tweenList[i].isPlaying) return true;
                }
            }
            return false;
        }
    }

    public override void Play()
    {
        PlayAndDo();
    }
    /// <summary>
    /// 当全部完成后回调完成
    /// </summary>
    public override bool PlayAndDo(Action _OnComplete = null)
    {
        void fun()
        {
            _OnComplete?.Invoke();

            if (OnComplete != null) OnComplete.Invoke();
        }
        if (tweenList != null && tweenList.Count > 0)
        {
            int leftCount = tweenList.Count;//剩余未完成数
            for (int i = 0; i < tweenList.Count; i++)
            {
                if (tweenList[i] is ICqTweenGroupItem tween)
                {
                    var bl = tween.PlayAndDo(() => { leftCount--; if (leftCount == 0) fun(); });
                    if (bl == false) return false;
                }
                else
                {
                    leftCount--;
                }

            }
            return true;
        }
        return false;
    }
    public override void Stop()
    {
        if (tweenList != null)
        {
            for (int i = 0; i < tweenList.Count; i++)
            {
                (tweenList[i] as ICqTweenGroupItem).Stop();
            }
        }
    }

    [Button("立即完成"),Click("Immediately")]
    public string __Immediately;

    public override void Immediately()
    {
        if (tweenList != null)
        {
            for (int i = 0; i < tweenList.Count; i++)
            {
                (tweenList[i] as ICqTweenGroupItem).Immediately();
            }
        }
    }

    [ContextMenu("当前值=起始值")]
    public  void SetCurrentByStart()
    {
        if(tweenList!=null)
        {
            foreach(ICqTweenGroupItem tween in tweenList)
            {
                tween.SetCurrentByStart();
            }
        }
    }

    [ContextMenu("当前值=终止值")]
    public  void SetCurrentByEnd()
    {
        if (tweenList != null)
        {
            foreach (ICqTweenGroupItem tween in tweenList)
            {
                tween.SetCurrentByEnd();
            }
        }
    }
    [ContextMenu("起始值=当前值")]
    public void SetStart()
    {
        if (tweenList != null)
        {
            foreach (ICqTweenGroupItem tween in tweenList)
            {
                tween.SetStart();
            }
        }
    }

    public void SetCurrentStartOrEnd(bool isStart)
    {
        if (tweenList != null)
        {
            foreach (ICqTweenGroupItem tween in tweenList)
            {
                tween.SetCurrentStartOrEnd(isStart);
            }
        }
    }

    [ContextMenu("终止值=当前值")]
    public void SetEnd()
    {
        if (tweenList != null)
        {
            foreach (ICqTweenGroupItem tween in tweenList)
            {
                tween.SetEnd();
            }
        }
    }
}
