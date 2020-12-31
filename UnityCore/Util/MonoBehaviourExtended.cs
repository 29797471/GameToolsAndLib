using CqCore;
using System;
using System.Collections;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 让添加时可以作删除的逻辑<para/>
    /// 让显示时可以做隐藏的逻辑<para/>
    /// 子类不应再重写OnDisable,OnDestroy<para/>
    /// 覆盖之类协程,关闭原本的协程接口,接入自定义协程处理
    /// </summary>
    public class MonoBehaviourExtended : MonoBehaviour
    {
        /// <summary>
        /// 脚本被删除时的委托回调
        /// </summary>
        protected Action OnDestroyCallBack;


        /// <summary>
        /// OnDisabled中执行的回调句柄<para/>
        /// 外部通常在OnEnable中执行一些需要异步函数,在函数中传入DisabledHandle,使得可以在OnDisabled中取消执行.<para/>
        /// 例:StartCoroutine_Global(XX_IEnumerator(),DisabledHandle)
        /// </summary>
        protected ICancelHandle DisabledHandle
        {
            get
            {
                if(mDisabledHandle==null)
                {
                    mDisabledHandle = new CancelHandle();
                }
                return mDisabledHandle;
            }
        }
        CancelHandle mDisabledHandle;

        /// <summary>
        /// OnDestroy中执行的回调句柄<para/>
        /// 外部通常在Awake或者Start中执行一些需要异步函数,在函数中传入DestroyHandle,使得可以在OnDestroy中取消执行.<para/>
        /// 例:StartCoroutine_Global(XX_IEnumerator(),DestroyHandle)
        /// </summary>
        protected ICancelHandle DestroyHandle
        {
            get
            {
                if (mDestroyHandle == null)
                {
                    mDestroyHandle = new CancelHandle();
                }
                return mDestroyHandle;
            }
        }
        CancelHandle mDestroyHandle;
        /// <summary>
        /// 该对象已被销毁
        /// </summary>
        protected bool IsDestroyed { get; private set; }
        void OnDestroy()
        {
            if (OnDestroyCallBack != null)
            {
                OnDestroyCallBack();
                OnDestroyCallBack = null;
            }
            if (mDestroyHandle != null) mDestroyHandle.CancelAll();
            IsDestroyed = true;
        }
        void OnDisable()
        {
            if (mDisabledHandle != null) mDisabledHandle.CancelAll();
        }

        /// <summary>
        /// 协程等待一段时间(frames帧)
        /// </summary>
        public IEnumerator Sleep(int frames = 1)
        {
            return GlobalCoroutine.Sleep(frames);
        }

        /// <summary>
        /// 协程等待一段时间(seconds秒)
        /// </summary>
        public IEnumerator Sleep(float seconds)
        {
            return GlobalCoroutine.Sleep(seconds);
        }

        #region 关闭原有协程接口,转接自定义协程管理
        /// <summary>
        /// 用GlobalCoroutine启动一个协程<para/>
        /// 当脚本被销毁时自动停止.<para/>
        /// 支持编辑器模式<para/>
        /// yield return null;表示等1帧<para/>
        /// 已覆盖原本的协程执行函数
        /// </summary>
        protected new CqCoroutine StartCoroutine(IEnumerator routine)
        {
            return GlobalCoroutine.Start(routine, DestroyHandle, null);
        }

        /// <summary>
        /// 用GlobalCoroutine启动一个协程<para/>
        /// 支持编辑器模式<para/>
        /// </summary>
        protected CqCoroutine StartCoroutine(IEnumerator routine, ICancelHandle cancelHandle, Action OnComplete = null)
        {
            return GlobalCoroutine.Start(routine, cancelHandle==null?DestroyHandle: cancelHandle, OnComplete);
        }

        /// <summary>
        /// 用GlobalCoroutine启动一个协程<para/>
        /// 当脚本被销毁时自动停止.<para/>
        /// 支持编辑器模式<para/>
        /// yield return null;表示等1帧<para/>
        /// 已覆盖原本的协程执行函数
        /// </summary>
        protected CqCoroutine StartCoroutine(IEnumerator routine, Action OnComplete = null)
        {
            return GlobalCoroutine.Start(routine, DestroyHandle, OnComplete);
        }

        [Obsolete("you not allow do call this method")]
        public new Coroutine StartCoroutine(string methodName)
        {
            throw new Exception("you not allow do call this method");
        }

        [Obsolete("you not allow do call this method")]
        public new Coroutine StartCoroutine(string methodName, object value)
        {
            throw new Exception("you not allow do call this method");
        }

        [Obsolete("you not allow do call this method")]
        public new void StopCoroutine(string methodName)
        {
            throw new Exception("you not allow do call this method");
        }

        [Obsolete("you not allow do call this method")]
        public new void StopCoroutine(IEnumerator routine)
        {
            throw new Exception("you not allow do call this method");
        }
        [Obsolete("you not allow do call this method")]
        public new void StopCoroutine(Coroutine routine)
        {
            throw new Exception("you not allow do call this method");
        }
        [Obsolete("you not allow do call this method")]
        public new void StopAllCoroutines()
        {
            throw new Exception("you not allow do call this method");
        }
        [Obsolete("you not allow do call this method")]
        public new Coroutine StartCoroutine_Auto(IEnumerator routine)
        {
            throw new Exception("you not allow do call this method");
        }
        #endregion

        //void OnGUI()
        //{
        //    if (!checkOnce)
        //    {
        //        gui = AssemblyUtil.GetClassAttribute<DrawGUIAttribute>(this);
        //        checkOnce = true;
        //    }
        //    if (gui != null) gui.Draw();
        //}
        [HideInInspector]
        [AllowMethodAttribute]
        public string __allowCallMethod;
    }
}
