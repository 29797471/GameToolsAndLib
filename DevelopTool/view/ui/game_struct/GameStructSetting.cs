using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WinCore;

namespace DevelopTool
{
    [Priority(1)]
    [Editor("数据结构编辑器", "/DevelopTool;component/Res/Images/Icons/class.ico")]
    public class GameStructSetting : Setting
    {
        [Priority(3)]
        [CustomListText("基础类型"),MinHeight(50),MinWidth(100) ]
        public ObservableCollection<string> BaseTypeList
        {
            get { if (baseTypeList == null) baseTypeList = new ObservableCollection<string>(); return baseTypeList; }
            set { baseTypeList = value; Update("BaseTypeList"); }
        }
        public ObservableCollection<string> baseTypeList;

        [Priority(4)]
        [CustomListText("类型申明符"), MinHeight(50), MinWidth(100)]
        public ObservableCollection<string> ClassDefineList
        {
            get { if (mClassDefineList == null) mClassDefineList = new ObservableCollection<string>(); return mClassDefineList; }
            set { mClassDefineList = value; Update("ClassDefineList"); }
        }
        public ObservableCollection<string> mClassDefineList;

        [Priority(5)]
        [ListBox("生成文件列表"), Width(400), Height(200), DoubleSelectedValue( DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }
        public CustomList<MakeFile> mTemplateFileList;
        
    }
}