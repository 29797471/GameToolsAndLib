using CqCore;
using System.Collections;
using System.Diagnostics;

namespace CqBehavior.Task
{
    [Editor("SVN操作")]
    [MenuItemPath("添加/行为节点/文件操作/SVN更新或者检出")]
    public class SVNUpdate : CqBehaviorNode
    {

        [Priority(1)]
        [FolderPath("本地目录", true), MinWidth(350)]
        public string FolderPath
        {
            get { return mFolderPath; }
            set { mFolderPath = value; Update("FolderPath"); }
        }
        public string mFolderPath;

        [Priority(2)]
        [TextBox("svn地址"), MinWidth(350)]
        public string SVNPath
        {
            get { return mSVNPath; }
            set { mSVNPath = value; Update("SVNPath"); }
        }
        public string mSVNPath;

        protected override IEnumerator OnExecute()
        {
            if(FileOpr.IsFolderPath(FolderPath))
            {
                yield return ProcessUtil.SVNUpdate(FolderPath);
            }
            else
            {
                yield return ProcessUtil.SVNCheckout(SVNPath, FolderPath);
            }
        }

    }
}

