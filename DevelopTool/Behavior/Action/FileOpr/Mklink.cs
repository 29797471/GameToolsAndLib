using Business;
using System.Collections;

namespace CqBehavior.Task
{
    [Editor("创建链接文件夹")]
    [MenuItemPath("添加/行为节点/文件操作/创建链接文件夹")]
    public class Mklink : CqBehaviorNode
    {
        [MinWidth(350)]
        [FolderPath("源目录",true)]
        [Priority(1)]
        public string FolderPath { get { return mFolderPath; } set { mFolderPath = value; Update("FolderPath"); } }
        public string mFolderPath;

        [MinWidth(350)]
        [FolderPath("联接到目录下", true)]
        [Priority(2)]
        public string DstFolderPath { get { return mDstFolderPath; } set { mDstFolderPath = value; Update("DstFolderPath"); } }
        public string mDstFolderPath;

        protected override IEnumerator OnExecute()
        {
            Result = Command.instance.LinkIntoFolder(FolderPath, DstFolderPath);
            yield break;
        }
    }
}
