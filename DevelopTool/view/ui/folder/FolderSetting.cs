using System.Collections.ObjectModel;

namespace DevelopTool
{
    [Priority(7)]
    [Editor("资源属性编辑器", "/DevelopTool;component/Res/Images/Icons/folder.ico")]
    public class FolderSetting : Setting
    {
        [Priority(3)]
        [FolderPath("资源目录", true)]
        public string FolderPath
        {
            get { return mFolderPath; }
            set { mFolderPath = value; Update("FolderPath"); }
        }
        public string mFolderPath;
    }
}

