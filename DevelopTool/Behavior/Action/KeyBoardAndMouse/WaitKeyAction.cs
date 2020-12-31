using CqCore;
using System;
using System.Collections;
using System.Windows.Input;
using WinCore;

namespace CqBehavior.Task
{
    /// <summary>
    /// 等待按下按键
    /// </summary>
    [MenuItemPath("添加/行为节点/外设/等待键盘按下")]
    [Editor("等待键盘按下")]
    public class WaitKeyAction : CqBehaviorNode
    {
        [HotKey("快捷键")]
        [Priority(1)]
        public string Shortcut { get { return mShortcutKey; } set { mShortcutKey = value; Update("Shortcut"); } }
        public string mShortcutKey;


        [CheckBox("捕获时阻止其它程序触发")]
        [Priority(2)]
        public bool StopOthers { get { return mStopOthers; } set { mStopOthers = value; Update("StopOthers"); } }
        public bool mStopOthers;

        [IsEnabled(false)]
        [MinWidth(100)]
        [TextBox("打印"), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true)]
        [Priority(3)]
        public string Print { get { return mPrint; } set { mPrint = value; Update("Print"); } }
        string mPrint;

        CancelHandle cancel;
        protected override IEnumerator OnExecute()
        {
            yield return null;

            cancel = new CancelHandle();
            HookManager.KeyDown += HookManager_KeyDown;
            cancel.CancelAct+=()=> HookManager.KeyDown -= HookManager_KeyDown;

            while (cancel != null)
            {
                yield return null;
            }
        }
        private void HookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Print = "Key:" + Torsion.Serialize(e);
            try
            {
                var key = EnumUtil.ConvertStringToEnum<Key>(mShortcutKey);
                if (e.KeyCode.ToString() == mShortcutKey)
                {
                    cancel.CancelAll();
                    cancel = null;
                }
            }
            catch (Exception)
            {
                var a = (KeyGesture)new KeyGestureValueSerializer().ConvertFromString(mShortcutKey, null);
                if (e.KeyCode.ToString() != a.Key.ToString()) return;
                if (e.Control != (a.Modifiers == ModifierKeys.Control)) return;
                cancel.CancelAll();
                cancel = null;
            }
        }

        private void HookManager_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            var key = EnumUtil.ConvertStringToEnum<Key>("LeftCtrl");
            var b = KeyInterop.VirtualKeyFromKey(key);
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z') e.KeyChar = (char)('A' - 'a' + e.KeyChar);
            if (e.KeyChar == b)
            {
                cancel.CancelAll();
                cancel = null;
            }
        }
    }

}
