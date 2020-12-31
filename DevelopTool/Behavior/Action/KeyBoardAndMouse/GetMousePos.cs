using System.Collections;

namespace CqBehavior.Task
{
    /// <summary>
    /// 等待按下按键
    /// </summary>
    [MenuItemPath("添加/行为节点/外设/获取当前鼠标位置")]
    [Editor("获取当前鼠标位置")]
    public class GetMousePos : CqBehaviorNode
    {
        [MaxWidth(200)]
        [MinWidth(100)]
        [UnderLine("鼠标位置"),Click]
        [Priority(1)]
        public ScreenPosVariable Pos
        {
            get { if (mPos == null)mPos = new ScreenPosVariable(); mPos.SetRoot(this);  return mPos; }
            set { mPos = value; Update("Pos"); }
        }
        public ScreenPosVariable mPos;

        protected override IEnumerator OnExecute()
        {
            Pos.SetValue(new UVData()
            {
                U = System.Windows.Forms.Cursor.Position.X,
                V = System.Windows.Forms.Cursor.Position.Y
            });
            Update("Pos");
            yield return null;
            Result = true;
        }
        
    }

}
