using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [Editor("文件拷贝")]
    [MenuItemPath("添加/行为节点/文件操作/文件拷贝")]
    public class CopyFile : CqBehaviorNode
    {
        [Priority(1)]
        [FilePath("源路径",true), MinWidth(350)]
        public string SrcPath { get { return mSrcPath; } set { mSrcPath = value; Update("SrcPath"); } }
        public string mSrcPath;

        [Priority(2)]
        [MinWidth(350)]
        [FilePath("目标路径", true)]
        public string DstPath { get { return mDstPath; } set { mDstPath = value; Update("DstPath"); } }
        public string mDstPath;

        [Priority(3)]
        [CheckBox("是否覆盖")]
        public bool OverWrite { get { return mOverWrite; } set { mOverWrite = value; Update("OverWrite"); } }
        public bool mOverWrite;
        protected override IEnumerator OnExecute()
        {
            if (!SrcPath.IsNullOrEmpty() && !DstPath.IsNullOrEmpty())
            {
                try
                {
                    FileOpr.Copy(SrcPath, DstPath, OverWrite);
                }
                catch (System.Exception e)
                {
                    Result = false;
                    EventMgr.MsgPrint.Notify(e.Message, 5);
                    yield break;
                }
                Result = true;
            }
            else Result = false;
        }
    }
}
