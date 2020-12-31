using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 修饰一个文件名,当文件改变时发出通知
/// </summary>
public class WatcherFileChangeAttribute : PropertyAttribute
{
    string changeFunName;
    /// <summary>
    /// 修饰一个文件名,当文件改变时发出通知
    /// </summary>
    /// <param name="changeFunName">文件改变时通知的外部函数名</param>
    public WatcherFileChangeAttribute(string changeFunName)
    {
        this.changeFunName = changeFunName;
    }
    protected override void OnSetTarget()
    {
        var cancel = new CancelHandle();
        PropertyChanged_CallBack(() =>
        {
            var path = (string)Target;
            if(FileOpr.IsFilePath(path))
            {
                var wf = new WatcherFile(path);
                wf.AddChanged(() =>
                {
                    AssemblyUtil.InvokeMethod(Parent, changeFunName);
                }, cancel);
            }
        }, cancel);
    }
}
