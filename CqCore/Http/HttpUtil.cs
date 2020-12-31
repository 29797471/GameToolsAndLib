using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CqCore
{
    public static partial class HttpUtil
    {
        /// <summary>
        /// 检查网站是否可以访问
        /// </summary>
        public static bool CheckUrlVisit(string url)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 3000;
                req.ReadWriteTimeout = 3000;
                var resp = (HttpWebResponse)req.GetResponse();
                
                if(resp.StatusCode==HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (WebException)
            {
            }
            return false;
        }

        
        public enum BrowserStyle
        {
            Default,
            IE,
            Firefox,
            Qihoo,
        }
        /// <summary>
        /// 调用系统默认的浏览器打开链接
        /// </summary>
        public static void OpenUrl(string url, BrowserStyle bs = BrowserStyle.Default)
        {
            switch (bs)
            {
                case BrowserStyle.Default:
                    ProcessUtil.Start(url);
                    break;
                case BrowserStyle.IE:
                    ProcessUtil.OpenExplorer(url);
                    break;
                case BrowserStyle.Qihoo:
                    ProcessUtil.OpenExplorer(url);
                    break;
            }
        }

        public static void ToFirefox(this HttpWebRequest httpRequest)
        {
            httpRequest.Method = "GET";
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            httpRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            httpRequest.Headers.Add("Cookie", "B=2jmtkqldjk8m8&b=3&s=km; GUC=AQEBAQFcieVda0Ii1QTc&s=AQAAAMydREKT&g=XIif4A; PRF=t%3DBABA; GUCS=AStnxmZl");
            //httpRequest.Connection = "keep-alive";
            //httpRequest.Host = "g.csdnimg.cn";
            //httpRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0";
        }
        


        /// <summary>
        /// 下载文件到本地
        /// </summary>
        public static bool DownloadFile(string netUrl, string localPath)
        {
            var request = new CqRequest(netUrl);
            var bytes = request.Send<byte[]>();
            if (bytes == null) return false;
            //CqDebug.Log("下载"+bytes.Length);
            var localFolderPath = Path.GetDirectoryName(localPath);
            if (!localFolderPath.IsNullOrEmpty() && !Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }
            File.WriteAllBytes(localPath, bytes);
            CqDebug.LogInCoroutine(string.Format("下载完成:{0}\n到本地:{1}", netUrl, localPath));
            //CqDebug.Log("生成:" +localPath);
            return true;
        }
        
        /// <summary>
        /// 再另一线程中下载完成后在主线程中返回
        /// </summary>
        public static IEnumerator DownloadFile(string netUrl, string localPath, AsyncReturn<bool> cqReturn)
        {
            yield return GlobalCoroutine.ThreadPoolCall(() =>
            {
                cqReturn.data = DownloadFile(netUrl, localPath);
            });
        }
    }
}
