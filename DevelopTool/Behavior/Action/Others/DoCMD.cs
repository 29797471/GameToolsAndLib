using Business;
using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [Editor("执行命令行")]
    [MenuItemPath("添加/其他/执行命令行")]
    public class DoCMD : CqBehaviorNode
    {
        [MinWidth(350),MinHeight(100)]
        [TextBox("命令"),AcceptsReturn(true)]
        [Priority(1)]
        public string Command { get { return mCommand; } set { mCommand = value; Update("Command"); } }
        public string mCommand;

        [Priority(3)]
        [FileEdit("在打开窗口中编辑")]
        public string DataX
        {
            get
            {
                return Command;
            }
            set
            {
                Command = value;
                Update("DataX");
            }
        }

        protected override void OnDone()
        {
            ProcessUtil.DoneCmd(Command);
        }
    }
}
