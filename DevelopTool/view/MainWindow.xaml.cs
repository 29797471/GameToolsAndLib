using Business;
using CqCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinCore;

namespace DevelopTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly RoutedUICommand SaveCommand =
            new RoutedUICommand("SaveCommand", "SaveCommand", typeof(MainWindow));
        public static readonly RoutedUICommand MakeCommand =
            new RoutedUICommand("MakeCommand", "MakeCommand", typeof(MainWindow));
        
        public MainWindow()
        {
            InitializeComponent();

            NotifyIconEx notifyIcon=new NotifyIconEx(
                this, 
                DevelopTool.Properties.Resources.cq128X128,
                "游戏工具\n作者:CQ");

            object tempFocused = null;
            GotFocus += (sender, e) =>
            {
                tempFocused = e.Source;
            };
            foreach(IModel model in StartLogic.modelList )
            {
                var menuItem = new MenuItem();
                menuItem.Tag = model;
                var attr = AssemblyUtil.GetMemberAttribute<EditorAttribute>(model.GetType());

                menuItem.Header = attr.name;
                menuItem.Height = 30;
                menuItem.Icon = ShellIcon.DrawImage(attr.icon,25f);
                menuItem.Click += MenuItem_Click;
                tools.Items.Add(menuItem);
                var btn = new Button();
                btn.Width = 30;
                btn.Height = 30;
                btn.Margin = new Thickness(10, 0, 0, 0);
                btn.BorderBrush= new SolidColorBrush(Colors.Transparent);
                btn.BorderThickness = new Thickness( 0);

                btn.Background = new SolidColorBrush(Colors.Transparent);
                btn.ToolTip = attr.name;
                btn.Content = ShellIcon.DrawImage(attr.icon,25f);
                buttons.Children.Add(btn);
                btn.Click += (sender,args) => MenuItem_Click(menuItem, null);
            }
            StartLogic.OnSelectChange += OnSelectChange;
            MathUtil.BetweenRange(ref UserSetting.Data.mLastSelectItem, 0, tools.Items.Count-1);
            MenuItem_Click(tools.Items[UserSetting.Data.LastSelectItem], null);
            
        }

        private void OnHelpDlg(object sender, RoutedEventArgs e)
        {
            HelpNavigationWindow win = new HelpNavigationWindow();
            win.Source = new Uri("MenuPage.xaml", UriKind.Relative);
            win.Show();
        }
        private void OnAboutDlg(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow();
            win.ShowDialog();
        }
        private void OpenSettingWindow(object sender, RoutedEventArgs e)
        {
            var t = WinUtil.OpenEditorWindow(SettingModel.instance.setting);
            if(t!=null)
            {
                SettingModel.instance.setting = t;
                SettingModel.instance.Save();
            }
            //var win = new SettingWindow();
            //win.ShowDialog();
        }

        private void OnSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var model=StartLogic.Select;
            var modelName = AssemblyUtil.GetMemberAttribute<EditorAttribute>(model.GetType()).name;
            EventMgr.MsgPrint.Notify("保存配置中,稍后", 5);
            if(model.Save())
            {
                EventMgr.MsgPrint.Notify(string.Format("保存配置成功({0})", modelName), 5);
            }
            else
            {
                EventMgr.MsgPrint.Notify(string.Format("保存配置失败({0})", modelName), 5);
            }
            //Save?.Invoke();
        }

        private void OnMakeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalCoroutine.Start(MakeFiles());
        }
        System.Collections.IEnumerator MakeFiles()
        {
            var model = StartLogic.Select;
            var modelName = AssemblyUtil.GetMemberAttribute<EditorAttribute>(model.GetType()).name;
            EventMgr.MsgPrint.Notify("生成文件中,稍后", 0);
            yield return model.MakeFiles();
            EventMgr.MsgPrint.Notify(string.Format("生成成功({0})", modelName), 5);
        }
        private void OnCurveDlg(object sender, RoutedEventArgs e)
        {
            var win = new CurveWindow();
            win.DataContext = new MainViewModel();
            win.ShowDialog();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void OnSetJava(object sender, RoutedEventArgs e)
        {
            //if (checkBox.IsChecked == true) HookManager.MouseMove += HookManager_MouseMove;
            //else HookManager.MouseMove -= HookManager_MouseMove;
            IEnumerable<string> list = null;
            string[] java_paths =
                {
                @"C:\Program Files\Java\",
                @"D:\Program Files\Java\",
                @"C:\Program Files (x86)\Java\"
            };
            foreach (var java_path in java_paths)
            {
                string[] java_folders = DirOpr.GetFileSystemEntries(java_path);
                if (java_folders != null)
                {
                    if (list == null) list = java_folders;
                    else list = list.Concat(java_folders);
                }
            }

            var dlg = new ChooseWindow() { Title = "设置Java环境" };

            dlg.Edit(list);
            dlg.ShowDialog();
            if (dlg.SelectValue != null)
            {
                SysEnvironment.SetSysEnvironment("JAVA_HOME", @"C:\Program Files\Java\" + dlg.SelectValue);
                SysEnvironment.SetPathAfter(@"%JAVA_HOME%\bin");
                SysEnvironment.SetSysEnvironment("CLASSPATH", @".;%JAVA_HOME%\lib\tools.jar;%JAVA_HOME%\lib\dt.jar;");
            }
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartLogic.Select = (sender as MenuItem).Tag as IModel;
        }
        private void OnSelectChange()
        {
            var model = StartLogic.Select;
            groupBox.Content = WinUtil.DrawHCC<GroupBox>(model);
        }

        private void OpenHost(object sender, RoutedEventArgs e)
        {
            ProcessUtil.OpenFileOrFolderByExplorer(@"C:\Windows\System32\drivers\etc\hosts");
        }
        private void CheckDotNet(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show(string.Join("\r\n", WinUtil.GetVersionFromRegistry()));
        }
        private void CheckCurrentDotNet(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show( WinUtil.GetVersionFromEnvironment());
        }


        private void MSTSC(object sender, RoutedEventArgs e)
        {
            Command.instance.RunCmd(@"mstsc" );
        }

        private void CALC(object sender, RoutedEventArgs e)
        {
            Command.instance.RunCmd(@"calc");
        }

        private void KillExcelProcess(object sender, RoutedEventArgs e)
        {
            ProcessUtil.KillExcelProcess();
        }

        private void OnPlayCurveDlg(object sender, RoutedEventArgs e)
        {
            var win = new PlayBeziderWindow();
            win.ShowDialog();
        }

        private void ShowBar(object sender, RoutedEventArgs e)
        {
            viewBar.Visibility = viewBar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void CreateFolderLink(object sender, RoutedEventArgs e)
        {
            var a = new System.Drawing.Region();
            string srcFolder=null;
            string dstFolder = null;
            if (WinUtil.GetFolderByOpenDialog(ref srcFolder, "源目录"))
            {
                dstFolder = srcFolder;
                if (WinUtil.GetFolderByOpenDialog(ref dstFolder, "在此目录下生成同名链接目录"))
                {
                    Command.instance.LinkIntoFolder(srcFolder, dstFolder);
                }
            }
        }
        private void ShowLocalIP(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show("本地网卡IP列表:"+Torsion.Serialize(NetUtil.HostIPList));
        }
        /// <summary>
        /// 1.遍历盘符下2,3级目录查找包含Unity的文件夹并且子目录中有Unity.exe
        /// 2.
        /// </summary>
        private void OpenUnity(object sender, RoutedEventArgs e)
        {
            var openUnityBox = WinUtil.OpenEditorWindow(UserSetting.Data.openUnityBox);
            if(openUnityBox != null)
            {
                openUnityBox.Open();
            }
        }

        private void MenuItem_OpenAppFolder(object sender, RoutedEventArgs e)
        {
            var a = DirOpr.GetDirectory();
            var b = DirOpr.GetCurrentDirectory();
            var c = DirOpr.GetBaseDirectory();
            var d = DirOpr.GetApplicationBase();
            HttpUtil.OpenUrl(a);
        }
    }
}

