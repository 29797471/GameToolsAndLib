using System.Collections.ObjectModel;

namespace DevelopTool
{
    [Priority(6)]
    [Editor("配置表数据结构编辑器", "/DevelopTool;component/Res/Images/Icons/class.ico")]
    public class CustomStructSetting : Setting
    {
        //[ListText("变量分隔符"),MinWidth(100), MinHeight(50)]
        //public ObservableCollection<string> SeparatorList
        //{
        //    get { if (separatorList == null) separatorList = new ObservableCollection<string>(); return separatorList; }
        //    set { separatorList = value; Update("SeparatorList"); }
        //}
        //public ObservableCollection<string> separatorList;


        [Priority(3)]
        [Button("设置生成文件"),Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;        
        
        [Priority(4)]
        [CustomListText("基础类型"), MinWidth(100), MinHeight(50)]
        public ObservableCollection<string> BaseTypeList
        {
            get { if (baseTypeList == null) baseTypeList = new ObservableCollection<string>(); return baseTypeList; }
            set { baseTypeList = value; Update("BaseTypeList"); }
        }
        public ObservableCollection<string> baseTypeList;

        [Priority(5)]
        [TextBox("分级分割符"), MinWidth(100)]
        public string Separates
        {
            get { return mSeparates; }
            set { mSeparates = value; Update("Separates"); }
        }
        public string mSeparates;
    }
}