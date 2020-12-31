using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using WinCore;
using CqCore;

namespace DevelopTool
{
    [Priority(9)]
    [Editor("新版文本翻译配置","/DevelopTool;component/Res/Images/Icons/translator.ico")]
    public class TranslatorNewSetting : Setting
    {
        [Priority(3)]
        [FilePath("翻译Excel路径", true, "Excel文件|*.xls;*.xlsx"), MinWidth(100)]
        public string ExcelPath { get { return mExcelPath; } set { mExcelPath = value; Update("ExcelPath"); } }
        public string mExcelPath;

        [Priority(4)]
        [FilePath("翻译Excel路径2", true, "Excel文件|*.xls;*.xlsx"), MinWidth(100)]
        public string ExcelPath2 { get { return mExcelPath2; } set { mExcelPath2 = value; Update("ExcelPath2"); } }
        public string mExcelPath2;

        [Priority(6)]
        [ComboBox("语言"), MinWidth(100), SelectedValue("Language")]
        public List<string> LanguageList
        {
            get
            {
                return CodeStyleNewModel.instance.setting.CodeSettingList.ToList().ConvertAll(x => x.Name);
            }
        }

        
        public string Language
        {
            get
            {
                if (mLanguage == null)
                {
                    var lilst = LanguageList;
                    if (lilst.Count > 0) mLanguage = lilst[0];
                    else mLanguage = "";
                }
                return mLanguage;
            }
            set { mLanguage = value; Update("Language");  }
        }
        public string mLanguage;
        

        [Priority(10)]
        [TextBox("新版翻译描述"), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Desc
        {
            get { if (mDesc == null) mDesc = ""; return mDesc; }
            set { mDesc = value; Update("Desc"); }
        }
        public string mDesc;

        [Priority(5)]
        [ListBox("生成文件列表"), Width(400), Height(100),DoubleSelectedValue( DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }

        public CustomList<MakeFile> mTemplateFileList;


        [Priority(7)]
        [Button("生成编辑器配置"), Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;

        [Priority(9)]
        
        [ListView("表格索引->语言名称"),MinHeight(200)]
        public CustomList<KeyValue> LinkTypesList
        {
            get
            {
                if (mLinkTypesList == null)
                {
                    mLinkTypesList = new CustomList<KeyValue>();
                }
                return mLinkTypesList;
            }
            set { mLinkTypesList = value; }
        }

        public CustomList<KeyValue> mLinkTypesList;
    }
}