using CqCore;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 由Unity主线程调用异步函数
    /// </summary>
    public static class UnityThread
    {
        static Thread mainThread;

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            //Debug.Log("UnityThread Init");
            mainThread = Thread.CurrentThread;
        }
        /// <summary>
        /// 该函数可以由非主线程发起调用,
        /// 转到Unity主线程中调用函数
        /// </summary>
        [Obsolete("Use GlobalCoroutine.Call instead")]
        public static void Call(Action action, ICancelHandle handle = null)
        {
            GlobalCoroutine.Call(action, handle);
        }
        
    }
}
