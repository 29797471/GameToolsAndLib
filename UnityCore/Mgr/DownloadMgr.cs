using CqCore;
using System;
using System.Collections;

namespace UnityCore
{
    /// <summary>
    /// 有策略的下载管理器
    /// 1.同时下载最大5个
    /// 2.下载数量计数
    /// </summary>
    public class DownloadMgr : Singleton<DownloadMgr>
    {
        /// <summary>
        /// 最大同时下载数量,通常ftp服务器有连接数量限制,所以要控制连接数
        /// </summary>
        public const int MaxDownloadingCount = 5;

        int downloadingCount;

        /// <summary>
        /// 正在下载的数量
        /// </summary>
        public int DownloadingCount
        {
            get
            {
                return downloadingCount;
            }
        }

        /// <summary>
        /// 当开始下载某文件时
        /// </summary>
        public Action<string> OnStartDownloadFile;

        /// <summary>
        /// 从netFilePath下载到本地localFilePath,返回一个有返回值的迭代器
        /// </summary>
        public IEnumerator DownloadFile(string netFilePath, string localFilePath, AsyncReturn<bool> cqReturn)
        {
            //当正在下载的数量达到5个时,等待下载完毕后再下载
            while (downloadingCount >= MaxDownloadingCount)
            {
                yield return null;
            }
            if (OnStartDownloadFile != null)
            {
                OnStartDownloadFile(netFilePath);
            }
            downloadingCount++;
            yield return HttpUtil.DownloadFile(netFilePath, localFilePath, cqReturn);
            downloadingCount--;
        }
    }
}

