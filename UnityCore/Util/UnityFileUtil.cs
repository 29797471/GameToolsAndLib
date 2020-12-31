using CqCore;
using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityCore
{
    /// <summary>
    /// 加载本地文件
    /// *.png, *.assetbundle,*.dat,*.txt
    /// </summary>
    public static class UnityFileUtil
    {
        /// <summary>
        /// 从netFilePath下载到本地localFilePath,返回一个有返回值的迭代器
        /// </summary>
        public static IEnumerator DownloadFile(string netFilePath, string localFilePath, AsyncReturn<bool> cqReturn)
        {
            return DownloadMgr.instance.DownloadFile(netFilePath, ApplicationUtil.persistentDataPath + "/" + localFilePath, cqReturn);
        }

        /// <summary>
        /// 将对象用Torsion序列化后保存在本地persistentDataPath目录下
        /// </summary>
        public static bool SaveObject(string localPersistentDataPath, object obj)
        {
            return FileOpr.SaveObject(ApplicationUtil.persistentDataPath + "/" +localPersistentDataPath, obj);
        }

        /// <summary>
        /// 从本地persistentDataPath目录下读一个经过Torsion序列化生成的对象<para/>
        /// 当没有时从StreamAssets下读对象
        /// </summary>
        public static IEnumerator ReadObject<T>(string localPath, AsyncReturn<T> data)
        {
            var bytes = new AsyncReturn<byte[]>();
            yield return ReadLocalFile(localPath, bytes);
            if(bytes.data!=null) data.data=Torsion.Deserialize<T>(Encoding.UTF8.GetString(bytes.data));
            if (data.data == null) Debug.LogError(string.Format("ReadObject({0}) Error!", localPath));
        }

        /// <summary>
        /// 从本地persistentDataPath目录下读文件
        /// </summary>
        public static byte[] ReadPersistentDataFile(string localPersistentDataPath)
        {
            var file = ApplicationUtil.persistentDataPath + "/" + localPersistentDataPath;
            if (File.Exists(file))
            {
                return File.ReadAllBytes(file);
            }
            CqCore.CqDebug.Log("读取失败(" + localPersistentDataPath+")");
            return null;
        }

        /// <summary>
        /// 从本地persistentDataPath目录下读文件<para/>
        /// 当没有时从StreamAssets下读文件
        /// </summary>
        public static IEnumerator ReadLocalFile(string localPath, AsyncReturn<byte[]> cqReturn)
        {
            var file = ApplicationUtil.persistentDataPath + "/" + localPath;
            if (File.Exists(file))
            {
                yield return GlobalCoroutine.ThreadPoolCall(() =>
                {
                    cqReturn.data = File.ReadAllBytes(file);
                });
            }
            else
            {
                yield return ReadStreamAssetsFile(localPath, cqReturn);
            }
        }
        /// <summary>
        /// 从本地persistentDataPath目录下读文件,当没有时从StreamAssets下读文件
        /// 该函数应由UnityThread发起调用
        /// </summary>
        [Obsolete]
        public static void ReadFileAsync(string localPath, System.Action<byte[]> OnComplete, ICancelHandle handle =null)
        {
            var cqReturn = new AsyncReturn<byte[]>();
            GlobalCoroutine.Start(ReadLocalFile(localPath, cqReturn), handle,()=> 
            {
                OnComplete(cqReturn.data);
            });
        }

        /// <summary>
        /// 读取StreamAssets目录下资源文件
        /// </summary>
        public static IEnumerator ReadStreamAssetsFile(string path, AsyncReturn<byte[]> cqReturn)
        {
            var uwr = UnityWebRequest.Get(ApplicationUtil.streamingAssetsPath + "/"+path);
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (uwr.isNetworkError)
                {
                    Debug.LogError("isNetworkError url=" + uwr.url);
                }
                if (uwr.isHttpError)
                {
                    Debug.LogError("isHttpError url=" + uwr.url);
                }
                Debug.Log("url=" + uwr.url);
                Debug.Log(uwr.error);
                Debug.Log(uwr.responseCode);
            }
            else
            {
                cqReturn.data = uwr.downloadHandler.data;
            }
        }
    }
}
