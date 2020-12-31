using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace CqCore
{
    /// <summary>
    /// 网络数据请求,返回类型T<para/>
    /// 封装HttpWebRequest,WebRequest
    /// </summary>
    public class CqRequest
    {
        WebRequest webRequest;

        /// <summary>
        /// 网络数据请求,返回类型T<para/>
        /// 封装HttpWebRequest,WebRequest
        /// </summary>
        public CqRequest(string url, Dictionary<string, string> headers = null, string userAgent = null, int timeout = 5000)
        {
            var uri = new Uri(url);
            webRequest = WebRequest.Create(uri);
            webRequest.ContentType = "application/json;charset=utf-8";
            if (timeout != 0) webRequest.Timeout = timeout;
            if (headers != null)
            {
                foreach (var it in headers)
                {
                    webRequest.Headers.Add(it.Key, it.Value);
                }
            }
            if (userAgent != null && webRequest is HttpWebRequest)
            {
                ((HttpWebRequest)webRequest).UserAgent = userAgent;
            }
        }
        /// <summary>
        /// 网络数据请求,返回类型T<para/>
        /// 封装HttpWebRequest,WebRequest
        /// </summary>
        public CqRequest(WebRequest webRequest)
        {
            this.webRequest = webRequest;
        }

        WebResponse webResponse;

        /// <summary>
        /// 切换到非主线程中发起请求<para/>
        /// 收到数据后在主线程中返回<para/>
        /// T:string 返回文本<para/>
        /// T:byte[] 返回数据流<para/>
        /// 其它:将文本Torsion反序列化后输出
        /// </summary>
        public IEnumerator SendAsync<T>(AsyncReturn<T> asyncReturn) where T:class
        {
            asyncReturn.data = null;
            //CqDebug.Log("请求(" + webRequest.RequestUri.AbsoluteUri + ")");
            yield return GlobalCoroutine.ThreadPoolCall(() =>
            {
                asyncReturn.data = Send<T>();
                
            });

            if (asyncReturn.data == null)
            {
                CqDebug.Log("请求(" + webRequest.RequestUri.AbsoluteUri + ")失败!",LogType.Error);
                //CqDebug.Log(string.Format("SendAsync {0} Error!", webRequest.RequestUri.AbsoluteUri), LogType.Error);
            }
        }

        /// <summary>
        /// 同步发送请求,返回数据<para/>
        /// T:string 返回文本<para/>
        /// T:byte[] 返回数据流<para/>
        /// 其它:将文本Torsion反序列化后输出
        /// </summary>
        public T Send<T>() where T : class
        {
            webResponse = _Request();

            if (typeof(T) == typeof(string))
            {
                return GetText() as T;
            }
            else if (typeof(T) == typeof(byte[]))
            {
                return GetBytes() as T;
            }
            else
            {
                //当请求的内容是一个ftp文件,而这个文件正在被更改时,返回内容可能是不完整的
                try
                {
                    return Torsion.Deserialize<T>(GetText());
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        //public bool IsDone { get; private set; }
        byte[] GetBytes()
        {
            if (webResponse == null) return null;
            BinaryReader reader = new BinaryReader(webResponse.GetResponseStream());
            return reader.ReadToEnd();
        }
        string GetText()
        {
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), GetTextEncoder());
            return reader.ReadToEnd();
        }


        /// <summary>
        /// 获取返回内容的编码
        /// </summary>
        private Encoding GetTextEncoder()
        {
            string contentType = webResponse.ContentType;
            if (!string.IsNullOrEmpty(contentType))
            {
                int num = contentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
                if (num > -1)
                {
                    int num2 = contentType.IndexOf('=', num);
                    if (num2 > -1)
                    {
                        string text = contentType.Substring(num2 + 1).Trim().Trim('\'', '"')
                            .Trim();
                        int num3 = text.IndexOf(';');
                        if (num3 > -1)
                        {
                            text = text.Substring(0, num3);
                        }

                        try
                        {
                            return Encoding.GetEncoding(text);
                        }
                        catch (ArgumentException ex)
                        {
                            CqDebug.Log($"Unsupported encoding '{text}': {ex.Message}",LogType.Warning);
                        }
                    }
                }
            }

            return Encoding.UTF8;
        }

        WebResponse _Request()
        {
            //if (!CheckUrlVisit(url))
            //{
            //    CqDebug.Log(string.Format("地址无法访问:{0}", url), LogType.Error);
            //    return null;
            //}
            WebResponse webResponse = null;
            var uri = webRequest.RequestUri;
            switch (uri.Scheme)
            {
                case "https":
                    {
                        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                        webRequest.Method = "GET";
                    }
                    break;
                case "http":
                    {
                        webRequest.Method = "GET";
                        //((HttpWebRequest)webRequest).ServicePoint.Expect100Continue = false;
                    }
                    break;
                case "ftp":
                    {
                        webRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                        var ftpWebRequest = (FtpWebRequest)webRequest;
                        ftpWebRequest.UseBinary = true;   // 指定数据传输类型
                        ftpWebRequest.UsePassive = true;
                        ftpWebRequest.KeepAlive = true;

                        //webRequest.Credentials = new NetworkCredential("cq","123456");
                    }
                    break;
            }

            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                webResponse = ex.Response;
                if (webResponse == null)
                {
                    CqDebug.LogInCoroutine(uri.AbsoluteUri + "\n" + ex.ToString(), LogType.Log);
                    return null;
                }
            }
            if (webResponse == null)
            {
                CqDebug.LogInCoroutine(string.Format("地址:{0} \n webResponse==null", uri.AbsoluteUri), LogType.Log);
                return null;
            }
            //string outPut = httpResponse.StatusCode.ToString() + "/n";
            //outPut += httpResponse.Method;
            //outPut += httpResponse.Headers;
            return webResponse;
        }
    }
}
