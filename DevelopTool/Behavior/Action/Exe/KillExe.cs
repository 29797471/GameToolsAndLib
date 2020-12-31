using CqCore;
using System.Diagnostics;
using System.Threading;

namespace CqBehavior.Task
{
    [Editor("关闭正在执行的程序")]
    [MenuItemPath("添加/行为节点/程序/关闭正在执行的程序")]
    public class KillExe : CqBehaviorNode
    {
        [MinWidth(350)]
        [FilePath("执行程序名",true)]
        [Priority(1)]
        public string ExeName { get { return mExeName; } set { mExeName = value; Update("ExeName"); } }
        public string mExeName;


        protected override void OnDone()
        {
            ThreadUtil.PoolCall(() =>
            {
                ProcessUtil.KillProcess(ExeName);
            });
        }
    }

}
