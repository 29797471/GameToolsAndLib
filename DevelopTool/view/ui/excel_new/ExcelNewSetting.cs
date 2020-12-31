namespace DevelopTool
{
    [Priority(6)]
    [Editor("Excel新版导出配置", "/DevelopTool;component/Res/Images/Icons/excel_new.ico")]
    public class ExcelNewSetting : Setting
    {
        [Priority(3)]
        [FolderPath("Excel目录", true)]
        public string ExcelPath { get { return excelFolderPath; } set { excelFolderPath = value; Update("ExcelPath"); } }
        public string excelFolderPath;

        [Priority(4)]
        [FolderPath("Excel配置目录", true)]
        public string ExcelDatPath
        {
            get { return mExcelDatPath; } 
            set { mExcelDatPath = value; Update("ExcelDatPath"); }
        }
        public string mExcelDatPath;

        /// <summary>
        /// 有效数据起始行
        /// Excel第1行为1
        /// 默认第2行开始数据
        /// </summary>
        [Priority(6)]
        [TextBox("有效数据起始行"),MinWidth(50)]
        public int StartDataRow
        {
            get { return mStartDataRow; }
            set { mStartDataRow = value; Update("StartDataRow"); }
        }
        public int mStartDataRow=2;


        [Priority(8)]
        [ListBox("生成文件列表"), Width(400), Height(200),DoubleSelectedValue(DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }
        public CustomList<MakeFile> mTemplateFileList;

        [Priority(10)]
        [Button("生成总文件"), Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;

        [Priority(11)]
        [Button("生成数据导出标记文件"), Click]
        public MakeFile MarkFile
        {
            get { if (mMarkFile == null) mMarkFile = new MakeFile(); return mMarkFile; }
            set { mMarkFile = value; Update("MarkFile"); }
        }
        public MakeFile mMarkFile;
    }
}