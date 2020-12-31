using System;
using System.Collections;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 支持嵌套的协程
    /// </summary>
    public class CqCoroutine : IEnumerator
    {
        private Stack<IEnumerator> executionStack;
        
        Action OnComplete;

        /// <summary>
        /// 是否在线程中循环
        /// </summary>
        public bool Updating { get; private set; }

        public CqCoroutine(IEnumerator iterator)
        {
            executionStack = new Stack<IEnumerator>();
            executionStack.Push(iterator);
        }

        /// <summary>
        /// 添加到主线程中循环执行<para/>
        /// 已经在执行返回false
        /// </summary>
        public bool Start(ICancelHandle handle,Action OnComplete = null)
        {
            if (Updating) return false;
            if (GlobalCoroutine.recordStacktrace)
            {
                stacktrace = "\n协程启动时的堆栈:\n" + new System.Diagnostics.StackTrace().ToString();
            }
            if (handle != null)
            {
                handle.CancelAct += ()=>Stop();
            }
            this.OnComplete = OnComplete;

            Updating = true;

            GlobalCoroutine._AddCqCoroutine(this, null);
            return true;
        }
        /// <summary>
        /// 终止这个协程
        /// </summary>
        public bool Stop()
        {
            if (!Updating) return false;
            Updating = false;
            return true;
        }

        /// <summary>
        /// 执行协程时的堆栈
        /// </summary>
        string stacktrace;

        internal void Update()
        {
            if (!Updating) return;
            try
            {
                if (MoveNext()==false)
                //if (!CheckUpdate(deltaTime))
                {
                    Stop();
                    OnComplete?.Invoke();
                    OnComplete = null;
                }
            }
            catch (Exception e)
            {
                CqDebug.Log(e + stacktrace, LogType.Exception);
            }
        }


        bool hasNoticeReturnValueType;
        public bool MoveNext()
        {
            IEnumerator i = executionStack.Peek();

            if (i.MoveNext())
            {
                object result = i.Current;
                if (result != null && !(result is IEnumerator))
                {
                    if (result.GetType().IsValueType && !hasNoticeReturnValueType)
                    {
                        hasNoticeReturnValueType = true;
                        CqDebug.Log(string.Format("协程的返回值是一个值类型({0})会产生装箱,建议使用 GlobalCoroutine.Sleep {1}", result, stacktrace), LogType.Warning);
                    }
                    
                    
                    result = GlobalCoroutine.GetCurrentToWaitFor(result);
                }
                if (result != null && result is IEnumerator)
                {
                    executionStack.Push((IEnumerator)result);
                    return MoveNext();
                }

                return true;
            }
            else
            {
                if (this.executionStack.Count > 1)
                {
                    this.executionStack.Pop();
                    return MoveNext();
                }
            }

            return false;
        }
        /*
        /// <summary>
        /// 防止在movenext中又嵌套执行本身
        /// </summary>
        bool isDoing = false;
        
        /// <summary>
        /// 每帧执行,返回false表示执行结束
        /// </summary>
        internal bool CheckUpdate(float deltaTime)
        {
            if (isDoing) return true;
            leftTime = Math.Max(leftTime-deltaTime, 0);
            leftFrames = Math.Max(leftFrames - 1, 0);

            if (leftTime == 0 && leftFrames == 0 )
            {
                //条件不成立时接着挂起
                if(CheckConditionCallBack != null && CheckConditionCallBack()==false)
                {
                    return true;
                }
                CheckConditionCallBack = null;
                isDoing = true;
                if (MoveNext() == false)
                {
                    return false;//执行完毕返回false
                }
                else
                {
                    isDoing = false;
                    var waitData=GlobalCoroutine.GetCurrentToWaitFor(Current);
                    
                    if(waitData==null)//等一帧
                    {

                    }
                    else if (waitData is float)//等X秒
                    {
                        leftTime = (float)waitData;
                    }
                    else if (waitData is int)//等X帧
                    {
                        leftFrames = (int)waitData;
                        if (leftFrames == 0)
                        {
                            return CheckUpdate(0f);
                        }
                    }
                    else if (waitData is Func<bool>)//等到条件成立
                    {
                        CheckConditionCallBack = (Func<bool>)waitData;
                    }
                }
            }
            return true;//接着挂起
        }
        */
        public void Reset()
        {
            throw new System.NotSupportedException("This Operation Is Not Supported.");
        }

        public object Current
        {
            get { return this.executionStack.Peek().Current; }
        }

        public bool Find(IEnumerator iterator)
        {
            return this.executionStack.Contains(iterator);
        }
    }
}
