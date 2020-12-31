using Business;
using System.Collections;
using System.IO;

namespace CqBehavior.Task
{
    [Editor("文件模版生成")]
    [MenuItemPath("添加/行为节点/文件操作/文件模版生成")]
    public class TempMakeFile : CqBehaviorNode
    {
        [Priority(6)]
        [GroupBox()]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;
        protected override void OnDone()
        {
            TemplateFile.Make("");
        }
    }
}

