using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinCore
{
    public static partial class HookManager
    {

        public static event EventHandler<MouseClickEventArgs> MouseClickHandler;
        public static event EventHandler<MouseMoveEventArgs> MouseMoveHandler;
        public static event EventHandler<MouseDoubleClickEventArgs> MouseDoubleClickHandler;
        public static event EventHandler<MouseDownEventArgs> MouseDownHandler;
        public static event EventHandler<MouseUpEventArgs> MouseUpHandler;

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="button">鼠标按键</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        internal static bool MouseClick(MouseButton button, int x, int y, object sender = null)
        {
            var eventData = new MouseClickEventArgs(button, x, y);
            MouseClickHandler?.Invoke(sender, eventData);
            return true;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="oldX">原始x</param>
        /// <param name="oldY">原始y</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        internal static bool MouseMove(int oldX, int oldY, int x, int y, object sender = null)
        {
            var eventData = new MouseMoveEventArgs(oldX, oldY, x, y);
            MouseMoveHandler?.Invoke(sender, eventData);
            return true;
        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="button">鼠标按键</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        internal static bool MouseDoubleClick(MouseButton button, int x, int y, object sender = null)
        {
            var eventData = new MouseDoubleClickEventArgs(button, x, y);
            MouseDoubleClickHandler?.Invoke(sender, eventData);
            return true;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="button">鼠标按键</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        internal static bool MouseDown(MouseButton button, int x, int y, object sender = null)
        {
            var eventData = new MouseDownEventArgs(button, x, y);
            MouseDownHandler?.Invoke(sender, eventData);
            return true;
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="button">鼠标按键</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        internal static bool MouseUp(MouseButton button, int x, int y, object sender = null)
        {
            var eventData = new MouseUpEventArgs(button, x, y);
            MouseUpHandler?.Invoke(sender, eventData);
            return true;
        }
    }
    /// <summary>鼠标点击</summary>
    [System.Serializable]
    public class MouseClickEventArgs : CustomEventArgs
    {

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public readonly MouseButton button;

        /// <summary>
        /// x
        /// </summary>
        public readonly int x;

        /// <summary>
        /// y
        /// </summary>
        public readonly int y;

        /// <summary>鼠标点击</summary>
        public MouseClickEventArgs(MouseButton button, int x, int y)
        {
            this.button = button;
            this.x = x;
            this.y = y;
        }
        public override bool Notify(object sender = null)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>鼠标移动</summary>
    [CqEvent("鼠标移动", false)]
    [System.Serializable]
    public class MouseMoveEventArgs : CustomEventArgs
    {

        /// <summary>
        /// 原始x
        /// </summary>
        public readonly int oldX;

        /// <summary>
        /// 原始y
        /// </summary>
        public readonly int oldY;

        /// <summary>
        /// x
        /// </summary>
        public readonly int x;

        /// <summary>
        /// y
        /// </summary>
        public readonly int y;

        /// <summary>鼠标移动</summary>
        public MouseMoveEventArgs(int oldX, int oldY, int x, int y)
        {
            this.oldX = oldX;
            this.oldY = oldY;
            this.x = x;
            this.y = y;
        }
        public override bool Notify(object sender = null)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>鼠标双击</summary>
    [CqEvent("鼠标双击", false)]
    [System.Serializable]
    public class MouseDoubleClickEventArgs : CustomEventArgs
    {

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public readonly MouseButton button;

        /// <summary>
        /// x
        /// </summary>
        public readonly int x;

        /// <summary>
        /// y
        /// </summary>
        public readonly int y;

        /// <summary>鼠标双击</summary>
        public MouseDoubleClickEventArgs(MouseButton button, int x, int y)
        {
            this.button = button;
            this.x = x;
            this.y = y;
        }
        public override bool Notify(object sender = null)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>鼠标按下</summary>
    [CqEvent("鼠标按下", false)]
    [System.Serializable]
    public class MouseDownEventArgs : CustomEventArgs
    {

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public readonly MouseButton button;

        /// <summary>
        /// x
        /// </summary>
        public readonly int x;

        /// <summary>
        /// y
        /// </summary>
        public readonly int y;

        /// <summary>鼠标按下</summary>
        public MouseDownEventArgs(MouseButton button, int x, int y)
        {
            this.button = button;
            this.x = x;
            this.y = y;
        }
        public override bool Notify(object sender = null)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>鼠标抬起</summary>
    [CqEvent("鼠标抬起", false)]
    [System.Serializable]
    public class MouseUpEventArgs : CustomEventArgs
    {

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public readonly MouseButton button;

        /// <summary>
        /// x
        /// </summary>
        public readonly int x;

        /// <summary>
        /// y
        /// </summary>
        public readonly int y;

        /// <summary>鼠标抬起</summary>
        public MouseUpEventArgs(MouseButton button, int x, int y)
        {
            this.button = button;
            this.x = x;
            this.y = y;
        }
        public override bool Notify(object sender = null)
        {
            throw new NotImplementedException();
        }
    }
}
