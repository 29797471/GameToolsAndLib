using CqCore;
using System.Collections;
using WinCore;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/行为节点/外设/键盘鼠标记录")]
    [Editor("键盘鼠标记录")]
    public class RecordInputAction : CqBehaviorNode
    {
        [IsEnabled(false)]
        [MinWidth(300)]
        [TextBox("记录信息"), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true)]
        [Priority(1)]
        public string Print { get { return mPrint; } set { mPrint = value; Update("Print"); } }
        string mPrint;

        CancelHandle cancel;
        protected override IEnumerator OnExecute()
        {
            yield return null;
            Print = "";
            HookManager.RunWin32GlobalMouseEvents();

            cancel = new CancelHandle();
            HookManager.MouseClickHandler += EventHandler_MouseClick;
            cancel.CancelAct +=()=> HookManager.MouseClickHandler -= EventHandler_MouseClick;
            HookManager.KeyDown += HookManager_KeyDown;
            cancel.CancelAct += () => HookManager.KeyDown -= HookManager_KeyDown;

            while (cancel != null)
            {
                yield return null;
            }

        }

        private void EventHandler_MouseClick(object sender, MouseClickEventArgs e)
        {
            Print += "\n";
            Print += "鼠标点击"+e.button+"位置:" + e.x + "," + e.y;
            cancel.CancelAll();
            cancel = null;
        }

        private void HookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Print += "\n";
            Print  += "Key:" + e.KeyCode.ToString();
            cancel.CancelAll();
            cancel = null;
        }

    }
}
