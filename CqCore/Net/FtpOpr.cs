using CqCore;
using System;
using System.IO;
using System.Net;

/// <summary>
/// 通过ftp操作上传或者下载文件
/// </summary>
public class FtpOpr
{
    string ftpServerIP = string.Empty;
    NetworkCredential credentials;
    /// <summary>
    /// 通过ftp操作上传或者下载文件
    /// </summary>
    public FtpOpr(string FtpServerIP, string FtpUserID = null, string FtpPassword = null)
    {
        this.ftpServerIP = "ftp://" + FtpServerIP;
        credentials = new NetworkCredential(FtpUserID, FtpPassword);
    }
    const int bufferSize = 2048;
    static byte[] buffer = new byte[bufferSize];

    /// <summary>
    /// 异步下载文件
    /// </summary>
    public void DownloadFileAsync(string netFilePath, string saveLocalPath, Action<bool> OnLoad = null, Action<long, long> OnProgress = null)
    {
        netFilePath = StringUtil.UrlEncode(netFilePath);
        try
        {
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(ftpServerIP + "/" + netFilePath);
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.KeepAlive = false;
            reqFTP.UsePassive = false;
            reqFTP.Credentials = credentials;
            reqFTP.BeginGetResponse(asyncResult =>
            {
                try
                {
                    var dstFolder = FileOpr.GetParentFolder(saveLocalPath);
                    if (!FileOpr.IsFolderPath(dstFolder))
                    {
                        Directory.CreateDirectory(dstFolder);
                    }

                    var request = asyncResult.AsyncState as FtpWebRequest;
                    var response = request.EndGetResponse(asyncResult);
                    FileStream outputStream = new FileStream(saveLocalPath, FileMode.Create);
                    Stream ftpStream = response.GetResponseStream();
                    long totalBytes = GetFileSize(netFilePath);
                    int readCount;
                    byte[] tempbuffer = new byte[bufferSize];
                    readCount = ftpStream.Read(tempbuffer, 0, bufferSize);
                    long loadedBytes = 0;
                    while (readCount > 0)
                    {
                        loadedBytes = readCount + loadedBytes;
                        outputStream.Write(tempbuffer, 0, readCount);
                        //更新进度
                        if (OnProgress != null) OnProgress.Invoke(loadedBytes, totalBytes);//更新进度条    
                        readCount = ftpStream.Read(tempbuffer, 0, bufferSize);
                    }
                    ftpStream.Close();
                    outputStream.Close();
                    response.Close();
                    if (OnLoad != null) OnLoad(true);
                }
                catch (Exception e)
                {
                    CqDebug.Log(e.Message);
                    if (OnLoad != null) OnLoad(false);
                }
            }, reqFTP);
        }
        catch (Exception e)
        {
            CqDebug.Log(e.Message);
            if (OnLoad != null) OnLoad(false);
        }

    }
    /// <summary>
    /// 获取指定文件大小
    /// </summary>
    public long GetFileSize(string netFilePath)
    {
        FtpWebRequest reqFTP;
        long fileSize = 0;
        try
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerIP + "/" + netFilePath));
            reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = credentials;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            fileSize = response.ContentLength;
            ftpStream.Close();
            response.Close();
        }
        catch (Exception e)
        {
            CqDebug.Log(e.Message);
        }
        return fileSize;
    }

    /// <summary>
    /// 异步上传文件
    /// 不使用缓冲区，外部能同时进行多个异步调用
    /// </summary>
    public void UploadFileAsync(string localPath, string netPath)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((s) =>
        {
            FileInfo fileInf = new FileInfo(localPath);
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerIP + "/" + netPath));
            try
            {
                reqFTP.Credentials = credentials;
                reqFTP.Proxy = null;//不要代理
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int contentLen;
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream strm = reqFTP.GetRequestStream();
                byte[] tempbuffer = new byte[bufferSize];
                contentLen = fs.Read(tempbuffer, 0, bufferSize);
                while (contentLen != 0)
                {
                    strm.Write(tempbuffer, 0, contentLen);
                    contentLen = fs.Read(tempbuffer, 0, bufferSize);
                }

                // Close the file stream and the Request Stream 
                strm.Close();
                fs.Close();
            }

            catch (Exception ex)
            {
                CqDebug.Log(ex.Message);
                reqFTP.Abort();
            }
        });
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    public void MakeDir(string netFolder)
    {
        FtpWebRequest reqFTP;
        try
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerIP + "/" + netFolder));
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = credentials;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            ftpStream.Close();
            response.Close();
        }
        catch (Exception ex)
        {
            CqDebug.Log(ex.Message);
        }
    }

    /// <summary>
    /// 返回文件最后修改时间
    /// </summary>
    public long GetFileModify(string netFilePath)
    {
        FtpWebRequest reqFTP = null;
        try
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerIP + "/" + netFilePath));

            reqFTP.UseBinary = true;
            //reqFTP.UsePassive = false;
            reqFTP.Credentials = credentials;

            reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            long dt = response.LastModified.Ticks;

            response.Close();
            response = null;
            reqFTP = null;
            return dt;
        }
        catch (Exception)
        {
            return 0;
        }
    }
}
