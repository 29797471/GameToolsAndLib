using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevelopTool
{
    [Priority(10)]
    [Editor("proto编辑器", "/DevelopTool;component/Res/Images/Icons/google.ico")]
    public class ProtoSetting : Setting
    {

        [FolderPath("Proto文件目录", true), Priority(3),MinWidth(100)]
        public string ProtoFolder { get { return protoFolder; } set { protoFolder = value; Update("ProtoFolder"); } }
        public string protoFolder;


        [Priority(6)]
        [Button("生成文件"), Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;


        [Priority(7)]
        [Button("生成文件2"), Click]
        public MakeFile TemplateFile2
        {
            get { if (mTempleteFile2 == null) mTempleteFile2 = new MakeFile(); return mTempleteFile2; }
            set { mTempleteFile2 = value; Update("TemplateFile2"); }
        }
        public MakeFile mTempleteFile2;
        //[FolderPath("导出目录", true), Priority(4)]
        //public string MakePath { get { return makePath; } set { makePath = value; Update("MakePath"); } }
        //public string makePath;

        [Priority(5)]
        [CustomListText("基础类型"), MinWidth(100),MinHeight(50)]
        public ObservableCollection<string> BaseTypeList { get { if (baseTypeList == null) baseTypeList = new ObservableCollection<string>(); return baseTypeList; } set { baseTypeList = value; Update("BaseTypeList"); } }
        public ObservableCollection<string> baseTypeList;

        [Button,Click("Reflash"), Priority(4)]
        public string Btn { get { return "刷新目录,重新载入Proto文件"; } }
        public void Reflash()
        {
            ProtoModel.instance.InitProtoFiles();
        }

    }
}