using System;
using System.IO;
using System.Net;
using System.Text;

/// <summary>
/// 文件操作类
/// </summary>
public static partial class FileOpr
{
    /// <summary>
    /// 以 System.Byte 数组形式下载指定的资源。此方法不会阻止调用线程。
    /// </summary>
    public static void DownloadDataAsync(string url, Action<byte[]> OnLoaded)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((s) =>
        {
            var client = new WebClient();
            client.Proxy = null;
            client.DownloadDataCompleted += (obj, e) =>
            {
                if (e.Error == null) OnLoaded(e.Result);
            };
            client.DownloadDataAsync(new Uri(url));
        });
    }

    /// <summary>
    /// 将具有指定 URI 的资源下载到本地文件。此方法不会阻止调用线程。
    /// </summary>
    public static Action DownloadFileAsync(string url,Action OnCompleted =null, Action<long,long> OnChanged = null,string fileName = null)
    {
        var client1 = new WebClient();
        System.Threading.ThreadPool.QueueUserWorkItem((s) =>
        {
            client1.Proxy = null;
            if (OnCompleted != null)
            {
                client1.DownloadFileCompleted += (obj, e) =>
                {
                    if(e.Error==null)OnCompleted();
                };
            }
            if (OnChanged != null)
            {
                client1.DownloadProgressChanged += (obj, e) =>
                {
                    OnChanged(e.BytesReceived, e.TotalBytesToReceive);
                };
            }
            client1.DownloadFileAsync(new Uri(url), (fileName == null) ? Path.GetFileName(url) : fileName);
        });
        return ()=>{ client1.CancelAsync(); };
    }

    
    
}


