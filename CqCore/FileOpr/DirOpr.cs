using System.Collections.Generic;
using System.IO;

namespace CqCore
{
    /// <summary>
    /// 文件夹操作类
    /// </summary>
    public static class DirOpr
    {
        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static bool Delete(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                CqDebug.Log("递归删除文件夹:" + path);
                return true;
            }
            return false;
        }
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// 清空文件夹
        /// </summary>
        public static void ClearOrCreate(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                CqDebug.Log("递归删除文件夹:" + path);
            }
            Directory.CreateDirectory(path);
            CqDebug.Log("创建文件夹:" + path);
        }

        /// <summary>
        /// 复制文件夹到目标文件夹内
        /// </summary>
        /// <param name="srcDir">要复制的文件夹</param>
        /// <param name="dstDir">复制到的目标文件夹内</param>
        /// <param name="overwrite">发现文件相同是否覆盖</param>
        /// <param name="excludeExtensions">排除后缀名,形如:.meta|.bundle</param>
        public static void Copy(string srcDir, string dstDir, bool overwrite = true, string excludeExtensions = null)
        {
            System.Predicate<string> predicate = null;
            if(!excludeExtensions.IsNullOrEmpty())
            {
                var ary = excludeExtensions.Split('|');
                predicate = file =>
                {
                    for (int i = 0; i < ary.Length; i++)
                    {
                        var extension = ary[i];
                        if (file.EndsWith(extension)) return true;
                    }
                    return false;
                };
            }
            Copy(srcDir, dstDir, overwrite, predicate);
        }

        /// <summary>
        /// 复制文件夹到目标文件夹内
        /// </summary>
        /// <param name="srcDir">要复制的文件夹</param>
        /// <param name="dstDir">复制到的目标文件夹内</param>
        /// <param name="overwrite">发现文件相同是否覆盖</param>
        /// <param name="excludeExtensions">排除后缀名,形如:.meta|.bundle</param>
        public static void Copy(string srcDir, string dstDir, bool overwrite, System.Predicate<string> excludeExtensions)
        {
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            var childDestFolder = dstDir + "\\" + Path.GetFileName(srcDir);

            if (!Directory.Exists(childDestFolder))
            {
                Directory.CreateDirectory(childDestFolder);
            }
            foreach (string sub in Directory.GetDirectories(srcDir))
            {
                Copy(sub, childDestFolder, overwrite, excludeExtensions);
            }

            // 文件
            foreach (string file in Directory.GetFiles(srcDir))
            {
                if (excludeExtensions==null || !excludeExtensions(file))
                {
                    File.Copy(file, childDestFolder + "\\" + Path.GetFileName(file), overwrite);
                }
            }
        }

        /// <summary>
        /// 遍历文件夹下所有文件(包含子文件夹下的文件)
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="OnFile">回调子文件的相对路径</param>
        public static void PreorderTraversal(string dirPath, System.Action<string> OnFile)
        {
            var parentDir = dirPath;

            if (OnFile != null)
            {
                string[] allFiles = Directory.GetFiles(parentDir);
                foreach (var file in allFiles)
                {
                    var fileName = FileOpr.ToRelativePath(file, parentDir);

                    OnFile(fileName);
                }
            }
            string[] allFolders = Directory.GetDirectories(parentDir);

            foreach (var folder in allFolders)
            {
                var dirName = FileOpr.ToRelativePath(folder, parentDir);
                PreorderTraversal(dirPath, OnFile, dirName);
            }
        }
        static void PreorderTraversal(string dirPath, System.Action<string> OnFile, string relativeDir)
        {
            var parentDir = dirPath;
            parentDir += "/" + relativeDir;

            if (OnFile != null)
            {
                string[] allFiles = Directory.GetFiles(parentDir);
                foreach (var file in allFiles)
                {
                    var fileName = FileOpr.ToRelativePath(file, parentDir);

                    OnFile(relativeDir + "/" + fileName);
                }
            }
            string[] allFolders = Directory.GetDirectories(parentDir);

            foreach (var folder in allFolders)
            {
                var dirName = FileOpr.ToRelativePath(folder, parentDir);
                dirName = relativeDir + "/" + dirName;
                PreorderTraversal(dirPath, OnFile, dirName);
            }
        }

        /// <summary>
        /// 先序遍历查找文件
        /// </summary>
        public static string FindFile(string dirPath, System.Predicate<string> OnFile)
        {
            var parentDir = dirPath;

            if (OnFile != null)
            {
                string[] allFiles = Directory.GetFiles(parentDir);
                foreach (var file in allFiles)
                {
                    //var fileName = FileOpr.ToRelativePath(file, parentDir);

                    if(OnFile(file))
                    {
                        return file;
                    }
                }
            }
            string[] allFolders = Directory.GetDirectories(parentDir);

            foreach (var folder in allFolders)
            {
                var file = FindFile(folder, OnFile);
                if (!file.IsNullOrEmpty()) return file;
            }
            return null;
        }

        /// <summary>
        /// 先序遍历查找匹配文件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dirPath"></param>
        /// <param name="OnFile"></param>
        /// <param name="OnFolder"></param>
        /// <param name="depth">搜索深度(为0时只在当前目录搜索)</param>
        public static void FindAll(ref List<string> list,string dirPath,
            System.Predicate<string> OnFile, System.Predicate<string> OnFolder, int depth =int.MaxValue)
        {
            if (list == null) list = new List<string>();
            
            var parentDir = dirPath;
            
            {
                if (OnFile != null)
                {
                    try
                    {
                        string[] allFiles = Directory.GetFiles(parentDir);
                        foreach (var file in allFiles)
                        {
                            //var fileName = FileOpr.ToRelativePath(file, parentDir);

                            if (OnFile(file))
                            {
                                list.Add(file);
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        //排除没有权限访问的路径
                    }
                }
            }
            
            
            {
                try
                {
                    string[] allFolders = Directory.GetDirectories(parentDir);

                    foreach (var folder in allFolders)
                    {
                        if (OnFolder != null)
                        {
                            if (OnFolder(folder))
                            {
                                list.Add(folder);
                                continue;
                            }
                        }
                        if(depth>0)FindAll(ref list, folder, OnFile, OnFolder, depth-1);
                    }
                }
                catch (System.Exception)
                {
                    //排除没有权限访问的路径
                }
            }
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="allDesk"></param>
        /// <param name="OnFile"></param>
        /// <param name="OnFolder"></param>
        /// <param name="depth">盘符下目录深度(盘符下的目录视为1级目录)</param>
        public static void FindAllInDesk(ref List<string> list, System.Predicate<string> OnFile, 
            System.Predicate<string> OnFolder, int depth = int.MaxValue, string allDesk = "CDEF")
        {
            foreach(var desk in allDesk)
            {
                FindAll(ref list, desk+":\\", OnFile, OnFolder, depth);
            }
        }

        /// <summary>
        /// 返回指定目录中所有文件和子目录的名称。
        /// </summary>
        public static string[] GetFileSystemEntries(string folderPath, string searchPattern = null)
        {
            if (!Directory.Exists(folderPath)) return null;
            if (searchPattern == null) return Directory.GetFileSystemEntries(folderPath);
            return Directory.GetFileSystemEntries(folderPath, searchPattern);
        }

        #region 当前App所在路径
        //2获取和设置当前目录(该进程从中启动的目录)的完全限定目录。  
        public static string GetDirectory()
        {
            return System.Environment.CurrentDirectory;
        }

        //3获取应用程序的当前工作目录。这个不一定是程序从中启动的目录啊
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
        //4获取程序的基目录。  
        public static string GetBaseDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
        //5获取和设置包括该应用程序的目录的名称。  
        public static string GetApplicationBase()
        {
            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }
        #endregion
    }
}
