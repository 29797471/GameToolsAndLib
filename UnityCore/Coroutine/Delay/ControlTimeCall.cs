
using CqCore;
using System;
using System.Collections.Generic;

/// <summary>
/// 外部随机时间入队列
/// 当队列不为空时,间隔固定时间出队列
/// 可用于显示打字效果,显示公告,漂浮文字等
/// 使用方式
/// ctcGunKill = new ControlTimeCall&lt;object&gt;(Record_Time, OnGunKill);
/// 
/// private void OnTick(object it)
/// 
/// ctcGunKill.Push(111);
/// </summary>
public class ControlTimeCall<T>
{
    private float LoopDequeueTime;
    private Queue<T> queue;
    private Action<T> _callBack;
    private CqCoroutine cc;

    /// <summary>
    /// 外部随机时间入队列<para/>
    /// 当队列不为空时,间隔固定时间处理一个,出一个<para/>
    /// 使用方式<para/>
    /// ctcGunKill = new ControlTimeCall&lt;object&gt;(Record_Time, OnGunKill);<para/>
    /// 
    /// private void OnTick(object it)<para/>
    /// 
    /// ctcGunKill.Push(111);
    /// </summary>
    public ControlTimeCall(float minTime, Action<T> callBack)
    {
        LoopDequeueTime = minTime;
        _callBack = callBack;
        queue = new Queue<T>();
        cc = GlobalCoroutine.Start(LoopPlay());
    }
    
    public void Push(T it)
    {
        queue.Enqueue(it);
    }

    System.Collections.IEnumerator LoopPlay()
    {
        while(true)
        {
            if(queue.Count > 0)
            {
                var it = queue.Dequeue();
                _callBack(it);
                isPlay = true;
                yield return GlobalCoroutine.Sleep(LoopDequeueTime);
            }
            else
            {
                isPlay = false;
                yield return null;
            }
        }
    }
    public bool isPlay { get; private set; }
    public void Stop()
    {
        if (cc != null)
        {
            cc.Stop();
        }
        isPlay = false;
    }
    public void Destroy()
    {
        Stop();
    }
    ~ControlTimeCall()
    {
        Destroy();
    }

}