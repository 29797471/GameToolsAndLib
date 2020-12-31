
using CqCore;
using System;
using System.IO;

/// <summary>
/// 文件监视类
/// FileSystemWatcher系统的文件改变有BUG,会由一次文件操作产生多个Changed事件
/// </summary>
public class WatcherFile
{
    public string path;
    FileSystemWatcher watcher;

    /// <summary>
    /// 可以收到文件变化的回调
    /// </summary>
    public bool EnableChanged
    {
        get
        {
            return watcher.EnableRaisingEvents;
        }
        set
        {
            watcher.EnableRaisingEvents = value;
        }
    }

    public WatcherFile(string path)
    {
        this.path = path;

        watcher = new FileSystemWatcher();
        watcher.Path = FileOpr.GetParentFolder(FileOpr.ToAbsolutePath(path));

        watcher.Filter = FileOpr.GetFileName(FileOpr.ToAbsolutePath(path));//*txt
        
        //这个就屏蔽了因为杀毒软件等各种外部因素导致Changed事件被多次触发。
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite |
           NotifyFilters.FileName | NotifyFilters.DirectoryName|NotifyFilters.Size|NotifyFilters.Security;

        watcher.EnableRaisingEvents = true;
    }

    public void AddChanged(Action OnChangedCallBack,ICancelHandle cancelHandle=null)
    {
        watcher.EnableRaisingEvents = true;
        FileSystemEventHandler h = (sender, args) =>
        {
            //触发改变处理完之后再开启
            watcher.EnableRaisingEvents = false;
            OnChangedCallBack();
            //由于本程序产生的文件改变不需要处理改变通知.因此转回主线程标记,这样OnChangedCallBack不会被多次触发.
            GlobalCoroutine.Call(() => watcher.EnableRaisingEvents = true);
        };
        watcher.Changed += h;
        if(cancelHandle!=null)
        {
            cancelHandle.CancelAct += () => watcher.Changed -= h;
        }
    }

}