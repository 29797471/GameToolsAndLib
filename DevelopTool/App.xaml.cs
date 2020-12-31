using CqCore;
using System;
using System.Windows;
using System.Windows.Threading;
using WinCore;

namespace DevelopTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            //防止多开
            mutex = new System.Threading.Mutex(true, "OnlyRun_CRNS");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                MessageBox.Show("程序已经在运行！", "提示");
                Shutdown();
                //Environment.Exit(0);
            }

            StartLogic.InitModel();
            
            var tt = new System.Windows.Forms.Timer();
            tt.Interval = 20;
            var deltaTime = tt.Interval / 1000f;
            tt.Tick+= (x, y) =>
            {
                GlobalCoroutine.Update(DateTime.Now.Ticks);
            };
            tt.Start();

        }
        /// <summary>
        /// WPF .NET 4.0 OpenClipboard 失败 (异常来自 HRESULT:0x800401D0 (CLIPBRD_E_CANT_OPEN)) BUG解决
        /// </summary>
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as System.Runtime.InteropServices.COMException;
            if (comException != null && comException.ErrorCode == -2147221040)e.Handled = true;
        }
        protected override void OnExit(ExitEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
