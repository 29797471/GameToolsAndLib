using System.Collections;

namespace CqBehavior.Task
{
    [Editor("发送鼠标命令")]
    [MenuItemPath("添加/行为节点/外设/发送鼠标命令")]
    public class SendMouseAction : CqBehaviorNode
    {
        [ComboBox("按键方式"), SelectedIndex("Mouse")]
        [Priority(1)]
        public string[] AA
        {
            get { return new string[] { "左键", "右键", "中键", "移动" }; }
        }
        public int Mouse { get { return mMouse; } set { mMouse = value; Update("Mouse"); } }
        public int mMouse;

        [MaxWidth(200)]
        [MinWidth(100)]
        [UnderLine("屏幕位置"), Click]
        [Priority(2)]
        public UVData ScreenPos
        {
            get { if (mScreenPos == null) mScreenPos = new UVData(); return mScreenPos; }
            set { mScreenPos = value; Update("ScreenPos"); }
        }
        public UVData mScreenPos;

        protected override IEnumerator OnExecute()
        {
            yield return null;
            switch (Mouse)
            {
                case 0:
                    SendKey.MouseClick(true, (int)ScreenPos.U, (int)ScreenPos.V);
                    break;
                case 1:
                    SendKey.MouseClick(false, (int)ScreenPos.U, (int)ScreenPos.V);
                    break;
                case 3:
                    SendKey.MouseMove( (int)ScreenPos.U, (int)ScreenPos.V);
                    break;
            }
            
        }
    }
}


