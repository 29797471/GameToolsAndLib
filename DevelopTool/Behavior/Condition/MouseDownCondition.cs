
using System;
using System.Collections;
using System.Windows.Forms;

namespace CqBehavior.Task
{
    /// <summary>
    /// 某键是否按下
    /// </summary>
    [MenuItemPath("添加/条件节点/鼠标键是否按下")]
    [Editor("鼠标键是否按下")]
    public class MouseDownCondition : CqBehaviorNode
    {
        [ComboBox("鼠标"), SelectedIndex("Mouse")]
        [Priority(1)]
        public string[] AA
        {
            get { return new string[] { "无", "左键", "右键", "中键" }; }
        }
        public int Mouse { get { return mMouse; } set { mMouse = value; Update("Mouse"); } }
        public int mMouse;

        protected override IEnumerator OnExecute()
        {
            yield return null;
            Result=(Control.MouseButtons == (Enum.GetValues(typeof(MouseButtons)) as MouseButtons[])[Mouse]);
        }
    }
}




