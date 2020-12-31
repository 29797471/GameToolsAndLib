namespace DevelopTool
{
    [Priority(0)]
    [Editor("事件编辑器", "/DevelopTool;component/Res/Images/Icons/event.ico")]
    public class EventSetting : Setting
    {
        [Priority(3)]
        [TextBox("事件参数模版代码"),MinWidth(350)]
        public string ItemExecContent
        {
            get {if(itemExecContent==null) itemExecContent = ""; return itemExecContent; }
            set { itemExecContent = value; Update("ItemExecContent"); }
        }
        public string itemExecContent;

        [Priority(5)]
        [ListBox("生成文件列表"), Width(400),Height(200),DoubleSelectedValue( DoubleClickStyle.OpenEditorWindow)]
        public CustomList<MakeFile> TemplateFileList
        {
            get { if (mTemplateFileList == null) mTemplateFileList = new CustomList<MakeFile>(); return mTemplateFileList; }
            set { mTemplateFileList = value; Update("TemplateFileList"); }
        }
        public CustomList<MakeFile> mTemplateFileList;
    }
}