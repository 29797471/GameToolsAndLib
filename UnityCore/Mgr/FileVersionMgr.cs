using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 本地文件更新的策略
/// 1.app启动时读取资源服info,本地info对比确定更新app还是资源
/// 2.更新资源时读取网络文件md5清单和本地文件md5清单 确定需要在进度中更新的资源
/// 3.完成后读取dy_md5,确定动态更新的资源的最新版本.在加载动态更新资源时先对比版本确定是否下载资源.
/// </summary>
public class FileVersionMgr : Singleton<FileVersionMgr>
{
    /// <summary>
    /// 资源服资源清单生成
    /// </summary>
    /// <param name="assetsPath">资源服Asset目录</param>
    /// <param name="appVersion">当前客户端版本(Application.version)</param>
    /// <param name="resVersion">当前资源版本</param>
    /// <param name="appFileName">app文件名</param>
    /// <param name="IsDynamicUpdate">是否是动态更新的资源(动态更新的资源,不在启动进度条中更新)</param>
    /// <returns></returns>
    public static bool MakeInfoAndMd5File(string assetsPath,string appVersion,string resVersion,string appFileName,Func<string,ResFileInfo,bool> IsDynamicUpdate=null)
    {
        if (FileOpr.IsFolderPath(assetsPath))
        {
            var parentFolder = FileOpr.GetParentFolder(assetsPath);
            var infoFileData = new ResVersionInfo();
            infoFileData.resVersion = resVersion;
            infoFileData.appVersion = appVersion;
            infoFileData.appFileName = appFileName;
            var md5FileData = new Dictionary<string, ResFileInfo>();
            var dy_md5FileData = new Dictionary<string, ResFileInfo>();
            FileOpr.PreorderTraversal(assetsPath,
                file =>
                {
                    if (FileOpr.GetNameByExtension(file) == ".meta") return;
                    var fileInfo = new ResFileInfo();
                    var path = FileOpr.ToRelativePath(file, assetsPath);
                    path = /*"Resources/"+*/path.Replace(@"\", "/");
                    fileInfo.md5 = FileOpr.GetMD5Hash(file);

                    fileInfo.size = FileOpr.GetFileSize(file);
                    fileInfo.lastWriteTime = FileOpr.GetLastWriteTime(file).Ticks;
                    if(IsDynamicUpdate!=null && IsDynamicUpdate(path,fileInfo))
                    {
                        dy_md5FileData[path] = fileInfo;
                        infoFileData.dynamicCheckSize+= fileInfo.size;
                    }
                    else
                    {
                        md5FileData[path] = fileInfo;
                        infoFileData.startCheckSize += fileInfo.size;
                    }
                });
            //对于本地资源清单生成IsDynamicUpdate=null,也不会生成dy_md5
            //对于资源服IsDynamicUpdate!=null
            if (IsDynamicUpdate!=null)
            {
                FileOpr.SaveFile(parentFolder + "/" + dy_checkFile, Torsion.Serialize(dy_md5FileData));
            }

            FileOpr.SaveFile(parentFolder + "/"+ start_checkFile , Torsion.Serialize(md5FileData));

            FileOpr.SaveFile(parentFolder + "/"+ infoFile, Torsion.Serialize(infoFileData)+
                "\n//启动更新资源总大小:"+StringUtil.FormatBytes(infoFileData.startCheckSize)+
                " 动态加载资源总大小:"+ StringUtil.FormatBytes(infoFileData.dynamicCheckSize));
            return true;
        }
        return false;
    }
    public class ResVersionInfo
    {
        public string resVersion;
        public string appVersion;
        public string appFileName;
        public long startCheckSize;
        public long dynamicCheckSize;
    }
    public class ResFileInfo
    {
        public string md5;
        public long size;
        public long lastWriteTime;
    }
    /// <summary>
    /// 启动时强制更新资源需要的版本资源比对文件
    /// </summary>
    public const string start_checkFile ="start_check.txt";

    /// <summary>
    /// 版本信息文件
    /// </summary>
    public const string infoFile = "info.txt";

    /// <summary>
    /// 资源加载时检查更新的版本资源对比文件
    /// </summary>
    public const string dy_checkFile = "dy_check.txt";

    /// <summary>
    /// 测试检查本地资源版本号
    /// </summary>
    public string CheckLocalVersion(string relativePath)
    {

        return localFileMD5[relativePath].md5;
    }
    /// <summary>
    /// 测试检查网络资源版本号
    /// </summary>
    public string CheckNetVersion(string relativePath)
    {
        return netAllFileMD5[relativePath].md5;
    }

    /// <summary>
    /// 相对路径对应的启动时更新的文件的Md5值
    /// </summary>
    Dictionary<string, ResFileInfo> netFileMD5;

    /// <summary>
    /// 相对路径对应的网络文件的Md5值
    /// </summary>
    Dictionary<string, ResFileInfo> netAllFileMD5;

    /// <summary>
    /// 相对路径对应的本地文件的Md5值
    /// </summary>
    Dictionary<string, ResFileInfo> localFileMD5;

    ResVersionInfo localResInfo;
    public ResVersionInfo LocalResInfo { get { return localResInfo; } }
    ResVersionInfo netResInfo;
    public ResVersionInfo NetResInfo { get { return netResInfo; } }


    /// <summary>
    /// 检查文件是否是最新
    /// </summary>
    bool IsFileNew(string relativePath)
    {
        /*
        if (!localFileMD5.ContainsKey(relativePath)) Debug.Log("本地没有这个文件:" + relativePath);
        else if(localFileMD5[relativePath].md5 != netAllFileMD5[relativePath].md5) Debug.Log("MD5不一致:" + relativePath);
        */
        return localFileMD5.ContainsKey(relativePath) && localFileMD5[relativePath].md5 == netAllFileMD5[relativePath].md5;
    }

    string netFolder;
    string cachFolder;
    string files;

    /// <summary>
    /// 初始化版本控制管理器
    /// </summary>
    /// <param name="netFolder">网络文件目录(存放Files,md5,info)</param>
    /// <param name="cachFolder">本地缓存文件相对目录(存放Assets,md5,info)</param>
    /// <param name="files">版本文件夹目录(存放AssetBundles,LuaScripts)</param>
    public void Init(string netFolder, string cachFolder,string files)
    {
        this.netFolder = netFolder;
        this.cachFolder = cachFolder;
        this.files = files;
    }


    /// <summary>
    /// 比对APP版本,资源版本,
    /// </summary>
    public void CompareInfo(Action<bool,ResVersionInfo, ResVersionInfo> OnComplete)
    {
        GlobalCoroutine.Start(CompareInfo_Coroutine(OnComplete));
    }

    IEnumerator CompareInfo_Coroutine(Action<bool, ResVersionInfo, ResVersionInfo> OnComplete)
    {
        var resFileInfo = new AsyncReturn<Dictionary<string, ResFileInfo>>();
        
        yield return UnityFileUtil.ReadObject(cachFolder + "/" + start_checkFile , resFileInfo);
        if (resFileInfo.data == null)
        {
            localFileMD5 = new Dictionary<string, ResFileInfo>();
        }
        else
        {
            localFileMD5 = resFileInfo.data;
        }

        {

            var request = new CqRequest(netFolder + "/" + dy_checkFile);
            yield return request.SendAsync(resFileInfo);
            if (resFileInfo.data == null)
            {
                
                netAllFileMD5 = new Dictionary<string, ResFileInfo>();
            }
            else
            {
                netAllFileMD5 = resFileInfo.data;
            }
        }
        

        var resInfo = new AsyncReturn<ResVersionInfo>();

        yield return UnityFileUtil.ReadObject(cachFolder + "/" + infoFile, resInfo);
        if(resInfo.data==null)
        {
            localResInfo = new ResVersionInfo();
        }
        else
        {
            localResInfo = resInfo.data;
        }
        localResInfo.appVersion = Application.version;

        {
            var request = new CqRequest(netFolder + "/" + infoFile);
            yield return request.SendAsync(resInfo);
            if (resInfo.data == null)
            {
                //Debug.LogError("网络info读取失败");
                OnComplete(false, localResInfo, netResInfo);
            }
            else
            {
                netResInfo = resInfo.data;
                OnComplete(true, localResInfo, netResInfo);
            }
        }
        
        
    }

    /// <summary>
    /// app运行中检测更新,一般发生在切换回前台时处理
    /// 由于资源版本只维护在启动时更新的资源列表,所以动态更新的资源不会触发这个更新弹窗,也不需要即时更新
    /// </summary>
    public void CheckUpdateOnce(Action<ResVersionInfo> UpdateAppCallBack, Action<ResVersionInfo> UpdateResCallBack)
    {
        var resInfo = new AsyncReturn<ResVersionInfo>();
        var request = new CqRequest(netFolder + "/" + infoFile);
        GlobalCoroutine.Start(request.SendAsync(resInfo),null,()=>
        {
            //如果app还没更新,这时候切换到后台再切换回来localResInfo=null
            if (resInfo.data != null && localResInfo!=null)
            {
                var _netResInfo = resInfo.data;
                if(localResInfo.appVersion.CompareTo(_netResInfo.appVersion)<0)
                {
                    UpdateAppCallBack(_netResInfo);
                }
                else if(localResInfo.resVersion.CompareTo(_netResInfo.resVersion)<0)
                {
                    UpdateResCallBack(_netResInfo);
                }
            }
        });
    }

    /// <summary>
    /// CompareRes
    /// </summary>
    public void CompareMd5(Action<bool> OnComplete)
    {
        GlobalCoroutine.Start(CompareMd5_Coroutine(OnComplete));
    }
    /// <summary>
    /// CompareRes
    /// </summary>
    IEnumerator CompareMd5_Coroutine(Action<bool> OnComplete)
    {
        var resFileInfo = new AsyncReturn<Dictionary<string, ResFileInfo>>();
        var request = new CqRequest(netFolder + "/" + start_checkFile);
        yield return request.SendAsync(resFileInfo);

        if (resFileInfo.data == null)
        {
            //Debug.LogError("网络md5读取失败");
            netFileMD5 = new Dictionary<string, ResFileInfo>();
            OnComplete(false);
        }
        else
        {
            netFileMD5 = resFileInfo.data;
            foreach(var it in netFileMD5)
            {
                netAllFileMD5[it.Key] = it.Value;
            }
            OnComplete(true);
        }
    }

    /// <summary>
    /// 提供给系统获取资源时调用,这些资源是在调用时检查更新,更新完成后回调
    /// </summary>
    /// <param name="relativePath">相对路径</param>
    /// <param name="checkUpdate">是否检查更新后再加载</param>
    /// <param name="cqReturn">返回加载的文件数据</param>
    public IEnumerator LoadFile(string relativePath, bool checkUpdate, AsyncReturn<byte[]> cqReturn)
    {
        //当初始化完毕 并且 需要获取最新的资源时 检查更新本地文件
        if(checkUpdate)
        {
            yield return CheckUpdateFile(relativePath);
        }
        yield return UnityFileUtil.ReadLocalFile(ToLocalPath(relativePath), cqReturn);
    }

    /// <summary>
    /// 通过文件相对路径获取本地沙盒或者用户目录下相对路径
    /// </summary>
    string ToLocalPath(string relativePath)
    {
        return cachFolder + "/"+ files +"/" + relativePath;
    }
    /// <summary>
    /// 通过文件相对路径获取网络文件路径
    /// </summary>
    string ToNetPath(string relativePath)
    {
        return netFolder + "/"+ files + "/" + relativePath;
    }
    /// <summary>
    ///检查更新本地文件到最新<para/>
    /// 如果在下载更新的过程中,则等着下载完成时回调<para/>
    /// 如果文件是最新的,则直接返回
    /// </summary>
    IEnumerator CheckUpdateFile(string relativePath,bool save=true)
    {
        //Debug.Log("同步" + relativePath);
        var localFilePath = ToLocalPath(relativePath);
        var netFilePath = ToNetPath(relativePath);
        if(netAllFileMD5==null || localFileMD5==null)//在尚未完成更新流程时直接返回,然后读取本地文件.
        {
            yield break;
        }
        if (!netAllFileMD5.ContainsKey(relativePath))
        {
            //如果游戏启动时资源版本是一致的,将不会下载资源服md5,所以在md5中的资源不一定会包含在netAllFileMD5中,但是本地一定是最新的.
            //Debug.LogError("在资源服清单中找不到\t" + relativePath);
            yield break;
        }

        //本地资源文件不存在，或者文件与资源服务器上的文件不一致时,下载后,(覆盖)到本地
        if (!IsFileNew(relativePath))
        {
            var cqReturn = new AsyncReturn<bool>();
            //Debug.Log("下载:" + netFilePath);
            
            yield return UnityFileUtil.DownloadFile(netFilePath, localFilePath, cqReturn);
            if (cqReturn.data)
            {
                localFileMD5[relativePath] = netAllFileMD5[relativePath];
                if(save)
                {
                    UnityFileUtil.SaveObject(cachFolder + "/" + start_checkFile , localFileMD5);
                }
            }
            else
            {
                Debug.LogError("下载失败:" + netFilePath);
            }
        }
        else
        {
            //Debug.Log("不需要更新:" + netFilePath);
        }
    }

    /// <summary>
    /// 比对资源服md5文件清单,得到需要更新的文件列表,和总大小
    /// </summary>
    public List<string> CheckUpdateFiles(out long totalUpdateSize)
    {
        totalUpdateSize = 0L;
        var updateFiles = new List<string>();

        foreach (var it in netFileMD5)
        {
            var itRelativePath = it.Key;
            if (!IsFileNew(itRelativePath))
            {
                updateFiles.Add(itRelativePath);
                totalUpdateSize += it.Value.size;
            }
        }
        if (updateFiles.Count == 0)//当只是版本不一致,更新文件都一样时,也需要同步版本
        {
            localResInfo = Torsion.Clone(netResInfo);
            UnityFileUtil.SaveObject(cachFolder + "/" + infoFile, localResInfo);
        }
        return updateFiles;
    }

    public void UpdateFiles(List<string> updateFiles, Action<int, string, long> OnProgress, Action OnComplete)
    {
        GlobalCoroutine.Start(_UpdateFiles_It(updateFiles, OnProgress),null, OnComplete);
    }

    /// <summary>
    /// 更新部分文件到最新<para/>
    /// </summary>
    IEnumerator _UpdateFiles_It(List<string> updateFiles, System.Action<int, string, long> OnProgress)
    {
        var loadedCount = 0;
        var downLoadSize = 0L;
        foreach (var relativePath in updateFiles)
        {
            var info = netFileMD5[relativePath];

            OnProgress(loadedCount, relativePath, downLoadSize);
            yield return CheckUpdateFile(relativePath,false);
            loadedCount++;
            downLoadSize += info.size;
        }
        OnProgress(loadedCount, "", downLoadSize);
        localResInfo =Torsion.Clone( netResInfo);
        UnityFileUtil.SaveObject(cachFolder + "/" + infoFile, localResInfo);
        UnityFileUtil.SaveObject(cachFolder + "/" + start_checkFile , localFileMD5);
        //CqCore.CqDebug.Log("保存" + cachMd5File);
    }
}
