using System.Collections;

namespace CqBehavior.Task
{
    /// <summary>
    /// 采样当前屏幕坐标的像素,传给变量
    /// </summary>
    [MenuItemPath("添加/行为节点/屏幕/采样屏幕坐标的像素")]
    [Editor("采样屏幕坐标的像素")]
    public class SamplingScreenPixel : CqBehaviorNode
    {
        [Priority(1)]
        [MaxWidth(200)]
        [MinWidth(100)]
        [UnderLine("屏幕位置"), Click]
        public ScreenPosVariable Pos
        {
            get { if (mPos == null) mPos = new ScreenPosVariable(); mPos.SetRoot(this); return mPos; }
            set { mPos = value;Update("Pos"); }
        }
        public ScreenPosVariable mPos;

        [Width(200)]
        [UnderLine("输出颜色"), Click]
        [Priority(2)]
        public ColorVariable ColorVal2
        {
            get { if (mColorVal2 == null) mColorVal2 = new ColorVariable(); /*mColorVal2.SetRoot(this); */return mColorVal2; }
            set { mColorVal2 = value; Update("ColorVal2"); }
        }
        public ColorVariable mColorVal2;

        protected override IEnumerator OnExecute()
        {
            var ScreenPos = Pos.GetValue();
            Update("Pos");
            ColorVal2.SetValue( WinUtil.GetScreenPixel((int)ScreenPos.U, (int)ScreenPos.V));
            Update("ColorVal2");
            yield return null;
        }
    }
}
