using CqCore;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using WinCore;

namespace CqBehavior.Task
{
    [Editor("关闭占用文件的进程")]
    [MenuItemPath("添加/行为节点/程序/关闭占用文件的进程")]
    public class KillLockFile : CqBehaviorNode
    {
        [MinWidth(350)]
        [FilePath("被占用的文件或者文件夹",true)]
        [Priority(1)]
        public string ExeName { get { return mExeName; } set { mExeName = value; Update("ExeName"); } }
        public string mExeName;

        [MinWidth(350)]
        [FilePath("handle执行程序", true)]
        [Priority(2)]
        public string ExeName2 { get { return mExeName2; } set { mExeName2 = value; Update("ExeName2"); } }
        public string mExeName2;

        protected override IEnumerator OnExecute()
        {
            var pro=ProcessUtil.GetProcessByLockFile(ExeName,ExeName2);
            if(pro!=null)
            {
                CustomMessageBox.ShowDialog(string.Format("被进程{0}:{1}占用", pro.ProcessName, pro.Id));
            }
            yield break;
        }
    }

}
