using CqCore;

namespace CqBehavior.Task
{
    [Editor("文件夹拷贝")]
    [MenuItemPath("添加/行为节点/文件操作/文件夹拷贝")]
    public class CopyFolder : CqBehaviorNode
    {
        [Priority(1)]
        [FolderPath("源文件夹",true), MinWidth(350)]
        public string SrcPath { get { return mSrcPath; } set { mSrcPath = value; Update("SrcPath"); } }
        public string mSrcPath;

        [Priority(2)]
        [MinWidth(350)]
        [FolderPath("目标文件夹内", true)]
        public string DstPath { get { return mDstPath; } set { mDstPath = value; Update("DstPath"); } }
        public string mDstPath;

        [Priority(3)]
        [CheckBox("是否覆盖")]
        public bool OverWrite { get { return mOverWrite; } set { mOverWrite = value; Update("OverWrite"); } }
        public bool mOverWrite;
        protected override void OnDone()
        {
            DirOpr.Copy(FileOpr.ToAbsolutePath(SrcPath), FileOpr.ToAbsolutePath(DstPath),mOverWrite);
        }
    }
}
