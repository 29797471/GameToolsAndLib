using System;
using System.Collections;

namespace CqCore
{
    public static class CqTweenCore
    {
        /// <summary>
        /// 基础缓动函数
        /// </summary>
        public static DelayHandle BaseTween<T>(Action<object> SetValue, T p0, T p1, float time,
            Func<float, float> Evaluate, Func<T, T, float, T> LerpUnclamped,  Action OnComplete = null, float startPercent = 0f)
        {
            return BaseFrame(t => SetValue(LerpUnclamped(p0, p1, Evaluate(t))), time, OnComplete, startPercent);
        }

        /// <summary>
        /// 基础缓动函数
        /// time 0~1变化的总时间
        /// </summary>
        public static DelayHandle BaseFrame(Action<float> OnFrame, float time,Action OnComplete = null, float startPercent = 0f)
        {
            var cancel = new CancelHandle();
            GlobalCoroutine.Start(CoreTween(OnFrame, time, startPercent), cancel, OnComplete);
            Action OnImmediately = () => { OnFrame(1); OnComplete?.Invoke(); };
            return new DelayHandle(cancel.CancelAll, OnImmediately);
        }

        /// <summary>
        /// 缓动核心函数 
        /// OnFrame 每帧回调 ,值从0~1
        /// GetDeltaTime 每帧的时间差
        /// </summary>
        static IEnumerator CoreTween(Action<float> OnFrame, float time,  float startPercent=0f)
        {
            for (float percent = startPercent; percent < 1; percent += GlobalCoroutine.deltaTime / time)
            {
                OnFrame(percent);
                yield return null;
            }
            OnFrame(1);
        }
    }
}
