using System.Collections.Generic;
using System.Linq;

namespace DevelopTool
{
    [Priority(3)]
    [Editor("代码转译器", "/DevelopTool;component/Res/Images/Icons/code_translation.ico")]
    public class CodeStyleNewSetting : Setting
    {
        [Priority(3)]
        [DropTarget, DragSource]
        [ListBox("语言列表"),Width(400), Height(200),DoubleSelectedValue(DoubleClickStyle.OpenEditorWindow)]
        public CustomList<CodeSetting> CodeSettingList
        {
            get { if (mCodeSettingList == null) mCodeSettingList = new CustomList<CodeSetting>(); return mCodeSettingList; }
            set { mCodeSettingList = value; Update("CodeSettingList"); }
        }
        public CustomList<CodeSetting> mCodeSettingList;

        [Priority(4)]
        [ComboBox("代码模版中编辑的语言"),SelectedIndex("SelectIndex")]
        public List<string> XX
        {
            get
            {
                return CodeSettingList.ToList().ConvertAll(x=>x.Name);
            }
        }

        public int mSelectIndex;
        public int SelectIndex
        {
            get { return mSelectIndex; }
            set { mSelectIndex = value;Update("SelectIndex"); }
        }
    }

    [Editor("编程语言设置"), Width(500),Height(700)]
    public class CodeSetting:NotifyObject
    {
        public override string ToString()
        {
            return Name;
        }

        [TextBox("编程语言:"),Priority(0)]
        public string Name
        {
            get { if (mName == null) mName = "temp"; return mName; }
            set { mName = value; Update("Name"); }
        }
        public string mName;
        
        [FilePath("表达式配置路径",true, "文件|*.dat") ,Priority(1)]
        public string ExpPath
        {
            get { if (mExpPath == null) mExpPath = "temp"; return mExpPath; }
            set { mExpPath = value; Update("ExpPath"); }
        }
        public string mExpPath;

        [FilePath("值配置路径", true, "文件|*.dat"), Priority(2)]
        public string ValuePath
        {
            get { if (mValuePath == null) mValuePath = "temp"; return mValuePath; }
            set { mValuePath = value; Update("ValuePath"); }
        }
        public string mValuePath;

        [Priority(3)]
        [ListView("类型-名称")]
        
        public CustomList<KeyValue> LinkTypeList
        {
            get
            {
                if (mLinkTypeList == null)
                {
                    mLinkTypeList = new CustomList<KeyValue>();
                }
                return mLinkTypeList;
            }
            set { mLinkTypeList = value; }
        }
        public CustomList<KeyValue> mLinkTypeList;

        /// <summary>
        /// 转译语言的类型列表
        /// </summary>
        public List<string> Types
        {
            get
            {
                return LinkTypeList.ToList().ConvertAll(x => x.Key);
            }
        }

        /// <summary>
        /// 生成文件列表
        /// </summary>
        [Priority(4)]
        [ListBox("生成文件列表"), Width(200), Height(200),DoubleSelectedValue( DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }
        public CustomList<MakeFile> mTemplateFileList;

    }
}
