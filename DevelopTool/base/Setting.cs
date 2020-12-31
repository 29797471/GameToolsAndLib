using System;
using WinCore;

namespace DevelopTool
{
    public class Setting : NotifyObject, ICmdCommand
    {
        public string Name
        {
            get
            {
                return AssemblyUtil.GetClassAttribute<EditorAttribute>(this).name;
            }
        }
        [CheckBox("批处理生成"),Priority(1), MinWidth(100)]
        public bool AllowCommand
        {
            get { return mAllowCommand; }
            set { mAllowCommand = value; Update("AllowCommand"); }
        }
        public bool mAllowCommand;

        [WatcherFileChange("OnFileChanged")]
        [FilePath("配置路径", true, "文件|*.dat"), Priority(2), MinWidth(100)]
        public string SetPath { get { return setPath; } set { setPath = value; Update("SetPath"); } }
        public string setPath;

        

        
    }
}
