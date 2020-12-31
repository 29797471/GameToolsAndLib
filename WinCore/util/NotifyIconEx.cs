using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WinCore
{
    /// <summary>
    /// 右下任务栏相关提示通知
    /// </summary>
    public class NotifyIconEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="win"></param>
        /// <param name="icon">托盘图标</param>
        /// <param name="Text">最小化到托盘时，鼠标点击时显示的文本</param>
        public NotifyIconEx(Window win,Icon icon,string Text)
        {
            var notifyIcon = new NotifyIcon();
            notifyIcon.Text = Text;
            notifyIcon.Icon = icon; 
            notifyIcon.Visible = true;

            WindowState tempState = WindowState.Normal;
            notifyIcon.MouseClick += (obj, args) => 
            {
                if(args.Button== MouseButtons.Left)
                {
                    if (win.WindowState == WindowState.Minimized)
                    {
                        win.WindowState = tempState;
                        win.ShowInTaskbar = true;
                        win.Topmost = true;
                        win.Topmost = false;
                    }
                    else
                    {
                        tempState = win.WindowState;
                        win.WindowState = WindowState.Minimized;
                        win.ShowInTaskbar = false;
                    }
                }
            };
            
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon.ContextMenu.MenuItems.Add("打开游戏工具(&O)", (obj, args) => 
            {
                if (win.WindowState == WindowState.Minimized)
                {
                    win.WindowState = tempState;
                    win.ShowInTaskbar = true;
                    win.Topmost = true;
                    win.Topmost = false;
                }
            });
            notifyIcon.ContextMenu.MenuItems.Add("-");
            notifyIcon.ContextMenu.MenuItems.Add("退出(&E)", (obj, e) => 
            {
                win.Close();
            });
            win.Closed += (obj, args) =>
            {
                notifyIcon.Dispose();
            };
            //气泡通知
            EventMgr.MsgBalloon.EventHandler += (obj, args) =>
            {
                notifyIcon.ShowBalloonTip((int)args.duration, args.title, args.content, (ToolTipIcon)args.icon);
            };
        }
    }
}
