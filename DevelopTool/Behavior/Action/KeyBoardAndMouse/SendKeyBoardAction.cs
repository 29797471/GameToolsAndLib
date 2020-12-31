using CqCore;
using System.Collections;
using System.Windows.Input;

namespace CqBehavior.Task
{
    [Editor("发送键盘命令")]
    [MenuItemPath("添加/行为节点/外设/发送键盘命令")]
    public class SendKeyBoardAction : CqBehaviorNode
    {
        [ComboBox("按键方式"), SelectedIndex("KeyState")]
        [Priority(1)]
        public string[] AA
        {
            get { return new string[] { "点击", "按住", "抬起" }; }
        }
        public int KeyState { get { return mKeyState; } set { mKeyState = value; Update("KeyState"); } }
        public int mKeyState;

        [HotKey("快捷键")]
        [Priority(2)]
        public string Shortcut
        {
            get { if (mShortcutKey == null) mShortcutKey = ""; return mShortcutKey; }
            set { mShortcutKey = value; Update("Shortcut"); }
        }
        public string mShortcutKey;

        protected override IEnumerator OnExecute()
        {
            yield return null;
            switch (KeyState)
            {
                case 0:
                    {
                        //var x=User32.WindowFromPoint(2100, 300);
                        //var u=System.Diagnostics.ProcessUtil.FindProcess("Unity");
                        //User32.SetForegroundWindow(x);
                        var key = EnumUtil.ConvertStringToEnum<Key>(mShortcutKey);
                        int keyValue = KeyInterop.VirtualKeyFromKey(key);
                        SendKey.KeyClick((byte)keyValue);
                    }
                    break;
                case 1:
                    {
                        var Modifiers = EnumUtil.ConvertStringToEnum<ModifierKeys>(mShortcutKey);
                        SendKey.KeyDown((byte)Modifiers);
                    }
                    break;
                case 2:
                    {
                        var Modifiers = EnumUtil.ConvertStringToEnum<ModifierKeys>(mShortcutKey);
                        SendKey.KeyUp((byte)Modifiers);
                    }
                    break;
            }
        }
    }
}

