using CqCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;

namespace CqBehavior.Task
{
    [Editor("删除文件夹")]
    [MenuItemPath("添加/行为节点/文件操作/删除文件夹")]
    public class DelFolder : CqBehaviorNode
    {
        [Priority(1)]
        [FolderPath("文件夹",true), MinWidth(350)]
        public string SrcPath
        {
            get 
            { 
                return mSrcPath; 
            }
            set 
            {
                mSrcPath = value; Update("SrcPath");
            }
        }
        public string mSrcPath;

        [Priority(2)]
        [ComboBox("样式"), SelectedIndex("SuitGroupIndex"), MinWidth(100),VerticalAlignment(System.Windows.VerticalAlignment.Center)]
        public ObservableCollection<string> SuitGroup
        {
            get
            {
                return new ObservableCollection<string>(new string[] {"删除","创建" });
            }
        }

        /// <summary>
        /// 选中
        /// </summary>
        public int SuitGroupIndex { get { return mSuitGroupIndex; } set { mSuitGroupIndex = value; Update("SuitGroupIndex"); } }
        public int mSuitGroupIndex;

        protected override IEnumerator OnExecute()
        {
            if (!SrcPath.IsNullOrEmpty())
            {
                if (mSuitGroupIndex == 0)
                {
                    var bl = DirOpr.Delete(SrcPath);
                    //                    if(bl)
                    //                    {
                    //                        CqDebug.Log("  删除文件夹:" + SrcPath);
                    //#if DEBUG
                    //                        System.Console.WriteLine("  删除文件夹:" + SrcPath);
                    //#endif
                    //                    }
                }
                else Directory.CreateDirectory(SrcPath);
                Result = true;
            }
            else Result = false;

            yield break;
        }
    }
}
