using CqCore;
using System;
using System.Collections.Generic;

/// <summary>
/// 事件管理器<para/>
/// 主要处理那些可能会存在多个事件监听者的事件<para/>
/// 当是一对一的时候,或者 要处理返回值,没有必要使用事件管理器,参数传递委托就好<para/>
/// example: <para/>
///     EventMgr.ButtonClick.EventHandler +=OnButtonClick;<para/>
///     EventMgr.ButtonClick.Notify("btn1", "win2", this);
/// </summary>
public static class EventMgr
{
    public static void Dispose()
    {
        if (OnDispose != null)
        {
            OnDispose();
            //OnDispose = null;有可能会多次清除
        }
    }
    static event Action OnDispose;
    #region 研发工具事件 -> 弹框消息
    /// <summary>弹框消息</summary>
    public static class MsgBox
    {
        static MsgBox()
        {
            OnDispose += () => mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs : CustomEventArgs
        {

            /// <summary>
            /// 内容
            /// </summary>
            public /*readonly*/ string content;

            /// <summary>
            /// 标题
            /// </summary>
            public /*readonly*/ string title;

            /// <summary>
            /// 弹框样式
            /// </summary>
            public /*readonly*/ int style;

            /// <summary>弹框消息</summary>
            public _EventArgs() { }
            public override bool Notify(object sender = null)
            {
                return MsgBox.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        static _EventArgs eventData = new _EventArgs();

        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 弹框消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="style">弹框样式</param>
        public static void Notify(string content, string title, int style, object sender = null)
        {
            eventData.content = content;
            eventData.title = title;
            eventData.style = style;
            if (mEventHandler != null) mEventHandler(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                try
                {
                    mEventHandler(sender, eventData);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct += () => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct += () => mEventHandler -= temp;
        }
    }
    #endregion
    #region 研发工具事件 -> 气泡消息
    /// <summary>气泡消息</summary>
    public static class MsgBalloon
    {
        static MsgBalloon()
        {
            OnDispose += () => mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs : CustomEventArgs
        {

            /// <summary>
            /// 内容
            /// </summary>
            public /*readonly*/ string content;

            /// <summary>
            /// 标题
            /// </summary>
            public /*readonly*/ string title;

            /// <summary>
            /// 图标<para>0.无</para><para>1.信息</para><para>2.警告</para><para>3.错误</para>
            /// </summary>
            public /*readonly*/ int icon;

            /// <summary>
            /// 持续时间
            /// </summary>
            public /*readonly*/ float duration;

            /// <summary>气泡消息</summary>
            public _EventArgs() { }
            public override bool Notify(object sender = null)
            {
                return MsgBalloon.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        static _EventArgs eventData = new _EventArgs();

        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 气泡消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="icon">图标<para>0.无</para><para>1.信息</para><para>2.警告</para><para>3.错误</para></param>
        /// <param name="duration">持续时间</param>
        public static void Notify(string content, string title, int icon, float duration, object sender = null)
        {
            eventData.content = content;
            eventData.title = title;
            eventData.icon = icon;
            eventData.duration = duration;
            if (mEventHandler != null) mEventHandler(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                try
                {
                    mEventHandler(sender, eventData);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct += () => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct += () => mEventHandler -= temp;
        }
    }
    #endregion
    #region 研发工具事件 -> 打印消息
    /// <summary>打印消息</summary>
    public static class MsgPrint
    {
        static MsgPrint()
        {
            OnDispose += () => mEventHandler = null;
        }
        [System.Serializable]
        public class _EventArgs : CustomEventArgs
        {

            /// <summary>
            /// 内容
            /// </summary>
            public /*readonly*/ string content;

            /// <summary>
            /// 持续时间
            /// </summary>
            public /*readonly*/ float duration;

            /// <summary>打印消息</summary>
            public _EventArgs() { }
            public override bool Notify(object sender = null)
            {
                return MsgPrint.Notify(this, sender);
            }
        }
        static event EventHandler<_EventArgs> mEventHandler;
        static _EventArgs eventData = new _EventArgs();

        public static event EventHandler<_EventArgs> EventHandler { add { mEventHandler += value; } remove { mEventHandler -= value; } }
        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="duration">持续时间</param>
        public static void Notify(string content, float duration, object sender = null)
        {
            eventData.content = content;
            eventData.duration = duration;
            if (mEventHandler != null) mEventHandler(sender, eventData);
        }
        /// <summary>
        /// 分发事件通知
        /// </summary>
        static bool Notify(_EventArgs eventData, object sender)
        {

            if (mEventHandler != null)
            {
                try
                {
                    mEventHandler(sender, eventData);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public static void CallBack(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            mEventHandler += action;
            if (handle != null) handle.CancelAct += () => { mEventHandler -= action; };
        }
        public static void CallBackOnce(EventHandler<_EventArgs> action, ICancelHandle handle = null)
        {
            EventHandler<_EventArgs> temp = null;
            temp = (obj, e) =>
            {
                mEventHandler -= temp;
                if (action != null) action(obj, e);
            };
            mEventHandler += temp;
            if (handle != null) handle.CancelAct += () => mEventHandler -= temp;
        }
    }
    #endregion
}
