using CqCore;
using System;
using System.Collections;
using WinCore;

namespace DevelopTool
{
    public abstract class SingleModel<T> : SingleNotifyObject<T>, IModel
    {
        /// <summary>
        /// 为了处理当容器控件和管理器绑定时会读Children属性,添加这个属性,保证不产生异常.
        /// </summary>
        public object Children
        {
            get
            {
                return null;
            }
        }
        WatcherFile wf;
        public SingleModel()
        {
            if (FileOpr.IsFilePath(Setting.SetPath))
            {
                wf = new WatcherFile(Setting.SetPath);
                wf.AddChanged(()=>GlobalCoroutine.Call(OnFileChanged));
            }
            ReLoad();
        }
        
        public abstract Setting Setting  { get;  }

        public void MakeFileCommand()
        {
            Setting setting = AssemblyUtil.GetMemberValue(this, "setting") as Setting;

            if (setting!=null && setting.AllowCommand)
            {
                var attr=AssemblyUtil.GetClassAttribute<EditorAttribute>(this);
                Console.WriteLine("生成:" + attr.name);
                var t = CqDebug.ExecFun(() =>GlobalCoroutine.BlockingCall(MakeFiles()));
                Console.WriteLine(string.Format("   执行时间：{0}秒", t.ToString("n5")));
            }
        }

        public virtual IEnumerator MakeFiles()
        {
            yield return null;
        }

        public bool Save()
        {
            if(wf!=null)wf.EnableChanged = false;
            var bl= OnSave();
            if (wf != null) wf.EnableChanged = true;
            return bl;
        }
        public virtual bool OnSave()
        {
            return false;
        }
        void OnFileChanged()
        {
            CustomMessageBox.ShowDialog("配置文件" + Setting.SetPath + "在外部改变,\n重新载入?", "提示", (result) =>
            {
                if (result) ReLoad();
            });
        }
        protected virtual void ReLoad()
        {
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnHide()
        {
        }

    }
}
