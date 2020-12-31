using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [Editor("修改文件内容")]
    [MenuItemPath("添加/行为节点/文件操作/修改文件内容")]
    public class ModifyFile : CqBehaviorNode
    {
        [Priority(1)]
        [FilePath("文件路径", true), MinWidth(350)]
        public string SrcPath { get { return mSrcPath; } set { mSrcPath = value; Update("SrcPath"); } }
        public string mSrcPath;

        [Priority(2)]
        [MinWidth(350)]
        [TextBox("查找起始")]
        public string StartStr
        {
            get { return mStartStr; } set { mStartStr = value; Update("StartStr"); }
        }
        public string mStartStr;

        [Priority(3)]
        [MinWidth(350)]
        [TextBox("查找结尾")]
        public string EndStr
        {
            get { return mEndStr; }
            set { mEndStr = value; Update("EndStr"); }
        }
        public string mEndStr;

        [Priority(4)]
        [MinWidth(350)]
        [TextBox("修改内容")]
        public string Content
        {
            get { return mContent; }
            set { mContent = value; Update("Content"); }
        }
        public string mContent;

        protected override IEnumerator OnExecute()
        {
            var fileContent = FileOpr.ReadFile(SrcPath);
            fileContent = StringUtil.ReplaceSubStr(fileContent, StartStr, EndStr, x => StartStr+Content+ EndStr);
            Result=FileOpr.SaveFile(SrcPath, fileContent);
            yield break;
        }
    }
}