using System.Collections;
using WinCore;

namespace CqBehavior.Task
{
    [Editor("打印")]
    [MenuItemPath("添加/行为节点/其它/打印")]
    public class PrintMessageBox : CqBehaviorNode
    {
        [MinWidth(100)]
        [TextBox("变量名")]
        [Priority(1)]
        public string Val1
        {
            get { if (mVal1 == null) mVal1 = ""; return mVal1; }
            set { mVal1 = value; Update("Val1"); }
        }
        public string mVal1;

        protected override IEnumerator OnExecute()
        {
            yield return null;
            //CustomMessageBox.Show(this[Val1].ToString());
        }
    }
}




