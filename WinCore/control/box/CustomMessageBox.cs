using CqCore;
using System;
using System.Collections;
using System.Windows;

namespace WinCore
{
    public static class CustomMessageBox
    {
        static CustomMessageBox()
        {
            EventMgr.MsgBox.EventHandler += (obj, args) =>
            {
                CustomMessageBox.Show(args.content);
            };
        }
        [Editor("动画"), Width(400),  Height(250)]
        public class CustomMessageBoxData : NotifyObject
        {
            [TextBox("", 100), MinWidth(200)]
            public string Content { get { return mContent; } set { mContent = value; } }
            public string mContent;
        }
        /// <summary>
        /// 自定义通用提示框,支持粘贴复制
        /// </summary>
        public static void Show(string content, string title = "", Action action = null)
        {
            if (action != null)
            {
                var Target = new CustomMessageBoxData() { Content = content };
                var targetType = Target.GetType();
                var attr = AssemblyUtil.GetClassAttribute<EditorAttribute>(targetType);
                var data = WinUtil.OpenEditorWindow(Target);
                if (data != null)
                {
                    action?.Invoke();
                }
            }
            else
            {
                var dlg = new CustomMessageBoxWindow();
                dlg.DataContext = content;
                dlg.Title = title;
                dlg.Show();
            }
        }
        /// <summary>
        /// 系统通用提示框
        /// </summary>
        public static void ShowDialog(string content,string title="",Action<bool> action=null,bool modal = true)
        {
            if(action!=null)
            {
                var result=MessageBox.Show(content, title,
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question,
                     MessageBoxResult.No,
                       modal? MessageBoxOptions.DefaultDesktopOnly: MessageBoxOptions.None);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        action?.Invoke(true);
                        break;
                    case MessageBoxResult.No:
                        action?.Invoke(false);
                        break;
                }
            }
            else
            {
                MessageBox.Show(content, title);
            }
        }

        /// <summary>
        /// 系统通用提示框,用户操作后返回<para/>
        /// </summary>
        public static bool ShowDialogNormal(string content, string title = "")
        {
            var result= MessageBox.Show(content, title,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                     MessageBoxResult.No, MessageBoxOptions.None );
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// 通用提示框
        /// </summary>
        public static void ShowGeneralTips(string contnet, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    MessageBox.Show(contnet, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No, MessageBoxOptions.None);
                    break;
                case LogType.Assert:
                    MessageBox.Show(contnet, "断言", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.None);
                    break;
                case LogType.Warning:
                    MessageBox.Show(contnet, "警告", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.None);
                    break;
                case LogType.Log:
                    MessageBox.Show(contnet, "日志", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.None);
                    break;
                case LogType.Exception:
                    MessageBox.Show(contnet, "异常", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None);
                    break;
                default:
                    break;
            }
        }
    }
}
