using System.Collections;

namespace CqBehavior.Task
{
    /// <summary>
    /// 等待按下按键
    /// </summary>
    [MenuItemPath("添加/行为节点/屏幕/设置屏幕位置")]
    [Editor("设置屏幕位置")]
    public class SetScreenPos : CqBehaviorNode
    {
        [MaxWidth(200)]
        [MinWidth(100)]
        [UnderLine("源位置"), Click]
        [Priority(1)]
        public ScreenPosVariable Pos1
        {
            get { if (mPos1 == null) mPos1 = new ScreenPosVariable(); mPos1.SetRoot(this); return mPos1; }
            set { mPos1 = value; Update("Pos1"); }
        }
        public ScreenPosVariable mPos1;

        [MaxWidth(200)]
        [MinWidth(100)]
        [Priority(2)]
        [UnderLine("目标位置"), Click]
        public ScreenPosVariable Pos2
        {
            get { if (mPos2 == null) mPos2 = new ScreenPosVariable(); mPos2.SetRoot(this); return mPos2; }
            set { mPos2 = value; Update("Pos2"); }
        }
        public ScreenPosVariable mPos2;

        protected override IEnumerator OnExecute()
        {
            yield return null;
            Pos2.SetValue(Pos1.GetValue());
        }
        
    }

}
