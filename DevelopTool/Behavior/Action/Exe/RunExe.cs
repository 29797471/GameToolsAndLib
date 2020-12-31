using System.Collections;
using System.Threading;

namespace CqBehavior.Task
{
    [Editor("执行程序")]
    [MenuItemPath("添加/行为节点/程序/执行程序")]
    public class RunExe : CqBehaviorNode
    {
        [MinWidth(350)]
        [FilePath("执行文件名",true)]
        [Priority(1)]
        public string ExeFilePath { get { return mExeFilePath; } set { mExeFilePath = value; Update("ExeFilePath"); } }
        public string mExeFilePath;

        [MinWidth(350)]
        [TextBox("传入参数"),TextWrapping(System.Windows.TextWrapping.Wrap),AcceptsReturn(true)]
        [Priority(2)]
        public string Args { get { if (mArgs == null) mArgs = ""; return mArgs; } set { mArgs = value; Update("Args"); } }
        public string mArgs;

        protected override void OnDone()
        {
            ThreadUtil.PoolCall(() =>
            {
                FileOpr.RunByRelativePath(ExeFilePath, Args);
            });
        }
    }

}
