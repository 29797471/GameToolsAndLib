using System;
using System.Collections;

namespace CqCore
{
    public abstract class TweenHandleBase
    {
        public MemberProxy memberProxy;

        public Func<float, float> Evaluate;

        /// <summary>
        /// 缓动完成后回调
        /// </summary>
        public Action OnComplete;


        float endPercent=1f;

        float lastPercent;

        /// <summary>
        /// 缓动时回调0~1
        /// </summary>
        abstract protected void OnFrame(float t);

        void ToFrame(float t)
        {
            lastPercent = t;
            OnFrame(t);
        }

        /// <summary>
        /// 终止缓动
        /// </summary>
        public void Cancel()
        {
            if (cc != null)
            {
                cc.Stop();
                cc = null;
            }
            mIsTweening = false;
        }
        bool mIsTweening;

        /// <summary>
        /// 是否正在缓动
        /// </summary>
        public bool IsTweening
        {
            get
            {
                return mIsTweening;
            }
        }
        CqCoroutine cc;

        /// <summary>
        /// 播放缓动<para/>
        /// 由播放者来管理终止操作
        /// </summary>
        public void Play(float time,ICancelHandle cancelHandle=null,float startPercent = 0f, float endPercent = 1f)
        {
            if(memberProxy==null)
            {
                throw new Exception("代理成员未设置");
            }
            
            Cancel();
            this.endPercent = endPercent;
            mIsTweening = true;
            cc=GlobalCoroutine.Start(CoreTween(ToFrame, time, startPercent, endPercent), cancelHandle, _Complete);
        }

        /// <summary>
        /// 接入外部协程,完成一系列缓动
        /// </summary>
        public IEnumerator Play_IT(float time, float startPercent = 0f, float endPercent = 1f)
        {
            if (memberProxy == null)
            {
                throw new Exception("代理成员未设置");
            }
            yield return CoreTween(ToFrame, time, startPercent, endPercent);
        }

        /// <summary>
        /// 接入外部协程,完成一系列缓动
        /// </summary>
        public IEnumerator PlayTo_IT(float time, float endPercent = 1f)
        {
            if (memberProxy == null)
            {
                throw new Exception("代理成员未设置");
            }
            yield return CoreTween(ToFrame, time, lastPercent, endPercent);
        }

        void _Complete()
        {
            mIsTweening = false;
            OnComplete?.Invoke();
        }
        /// <summary>
        /// 立即完成
        /// </summary>
        public void Immediately()
        {
            Cancel();
            ToFrame(endPercent);
            _Complete();
        }

        /// <summary>
        /// 缓动核心函数 
        /// OnFrame 每帧回调 ,值从startPercent~endPercent
        /// </summary>
        static IEnumerator CoreTween(Action<float> _ToFrame, float time, float startPercent = 0f, float endPercent = 1f)
        {
            if (startPercent < endPercent)
            {
                for (float percent = startPercent; percent < endPercent; percent += GlobalCoroutine.deltaTime / time)
                {
                    _ToFrame(percent);
                    yield return null;
                }
                _ToFrame(endPercent);
            }
            else
            {
                for (float percent = startPercent; percent > endPercent; percent -= GlobalCoroutine.deltaTime / time)
                {
                    _ToFrame(percent);
                    yield return null;
                }
                _ToFrame(endPercent);
            }
        }
    }
}
