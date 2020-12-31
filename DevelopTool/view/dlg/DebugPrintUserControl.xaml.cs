using CqCore;
using System;
using System.Windows.Controls;
using WinCore;

namespace DevelopTool
{
    /// <summary>
    /// DebugPrintUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DebugPrintUserControl : UserControl
    {
        CancelHandle handle;
        public DebugPrintUserControl()
        {
            InitializeComponent();
            EventMgr.MsgPrint.EventHandler += MsgPrint_EventHandler;
            Unloaded += (obj,e)=> EventMgr.MsgPrint.EventHandler -= MsgPrint_EventHandler;
            handle = new CancelHandle();
        }

        private void MsgPrint_EventHandler(object sender, EventMgr.MsgPrint._EventArgs e)
        {
            handle.CancelAll();

            print.Content = e.content + "\n" + DateTime.Now.ToString();

            if (e.duration > 0)
            {
                GlobalCoroutine.DelayCall(e.duration, ClearDebugContent, handle);
            }
        }



        void ClearDebugContent()
        {
            print.Content = "";
            handle.CancelAll();
        }
    }
}
