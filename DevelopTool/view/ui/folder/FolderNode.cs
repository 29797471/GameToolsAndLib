using DevelopTool;
using System.Collections.ObjectModel;

namespace ResModel
{
    [System.Serializable]
    [Editor("文件夹")]
    public class FolderNode : BaseTreeNotifyObject
    {
        [Priority(1)]
        [TextBox("分类"),MinWidth(100)]
        /// <summary>
        /// 变量类型索引
        /// </summary>
        public int TypeIndex { get { return mTypeIndex; } set { mTypeIndex = value; Update("TypeIndex"); } }
        public int mTypeIndex;

        public string Path
        {
            get
            {
                if (Node.Parent == null) return FolderModel.instance.setting.FolderPath;
                else return (Parent as FolderNode).Path +@"\"+ Name;
            }
        }
    }

}