using CqCore;
using ICCEmbedded.SharpZipLib.Zip;
using System.Collections;

namespace CqBehavior.Task
{
    [Editor("压缩文件夹")]
    [MenuItemPath("添加/行为节点/文件操作/压缩文件夹")]
    public class ZipFolderCommand : CqBehaviorNode
    {
        [MinWidth(350)]
        [FolderPath("压缩目录", true)]
        [Priority(1)]
        public string FolderPath 
        {
            get { if (mFolderPath == null) mFolderPath = ""; return mFolderPath; } 
            set { mFolderPath = value; Update("FolderPath"); } 
        }
        public string mFolderPath;

        [MinWidth(350)]
        [FilePath("输出文件", true)]
        [Priority(2)]
        public string ZipFile { get { return mZipFile; } set { mZipFile = value; Update("ZipFile"); } }
        public string mZipFile;

        protected override IEnumerator OnExecute()
        {
            yield return GlobalCoroutine.ThreadPoolCall(() =>
            {
                var zip = new FastZip();
                zip.CreateZip(ZipFile, FolderPath, true, "\\.*$"/*"\\.(exe|dll|xml)$"*/);
            });
        }
    }
}

