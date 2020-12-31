using System.Collections.ObjectModel;

namespace DevelopTool
{
    [Priority(20)]
    [Editor("通用数据结构", "/DevelopTool;component/Res/Images/Icons/class.ico")]
    public class CommonStructSetting : Setting
    {
        [Priority(3)]
        [CustomListText("基础类型"), MinHeight(200), MinWidth(100)]
        public ObservableCollection<string> BaseTypeList
        {
            get { if (baseTypeList == null) baseTypeList = new ObservableCollection<string>(); return baseTypeList; }
            set { baseTypeList = value; Update("BaseTypeList"); }
        }
        public ObservableCollection<string> baseTypeList;

        [Priority(5)]
        [ListBox("生成文件列表"), Width(400), Height(200), DoubleSelectedValue(DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }
        public CustomList<MakeFile> mTemplateFileList;
    }
}