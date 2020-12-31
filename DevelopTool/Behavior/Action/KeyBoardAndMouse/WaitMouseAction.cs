using CqCore;
using System;
using System.Collections;
using System.Windows.Forms;
using WinCore;

namespace CqBehavior.Task
{
    /// <summary>
    /// 等待鼠标按下按键
    /// </summary>
    [MenuItemPath("添加/行为节点/外设/等待鼠标按下")]
    [Editor("等待鼠标按下")]
    public class WaitMouseAction : CqBehaviorNode
    {
        [ComboBox("鼠标"), SelectedIndex("Mouse")]
        [Priority(1)]
        public string[] AA
        {
            get { return new string[] { "左键", "右键", "中键" }; }
        }
        public int Mouse { get { return mMouse; } set { mMouse = value; Update("Mouse"); } }
        public int mMouse;

        [IsEnabled(false)]
        [MinWidth(300)]
        [TextBox("记录信息"), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true)]
        [Priority(2)]
        public string Print { get { return mPrint; } set { mPrint = value; Update("Print"); } }
        string mPrint;
        
        public MouseButton MouseValue
        {
            get
            {
                return (Enum.GetValues(typeof(MouseButton)) as MouseButton[])[Mouse];
            }
        }
        CancelHandle cancel;
        protected override IEnumerator OnExecute()
        {
            yield return null;

            cancel = new CancelHandle();
            HookManager.RunWin32GlobalMouseEvents();
            HookManager.MouseClickHandler += EventHandler_MouseClick;
            cancel.CancelAct += () => HookManager.MouseClickHandler -= EventHandler_MouseClick;

            while (cancel != null)
            {
                yield return null;
            }
        }

        private void EventHandler_MouseClick(object sender, MouseClickEventArgs e)
        {
            if (e.button == MouseValue)
            {
                Print = "位置:" + e.x + "," + e.y;
                cancel.CancelAll();
                cancel = null;
            }
        }

    }
}

