
using CqCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;

public static partial class FileOpr
{

    #region 操作

    public static byte[] ReadAllBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// 执行程序
    /// </summary>
    public static void RunByRelativePath(string path, string args = null)
    {
        //估计360报毒是由于打开程序的操作引起的
        try
        {
            var url = FileOpr.ToAbsolutePath(path);
            ProcessUtil.Start(url, args);
            
        }
        catch (Exception )
        {
        }
    }


    /// <summary>
    /// 文件是否被占用
    /// </summary>
    public static bool IsOccupy(string mPath)
    {
        try
        {
            using (FileStream fs = new FileStream(mPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
            }
        }
        catch (System.Exception)
        {
            //文件被占用
            return true;
        }
        return false;
    }

    /// <summary>
    /// 确定指定的文件是否存在。
    /// </summary>
    public static bool Exists(string path)
    {
        return File.Exists(path);
    }
    /// <summary>
    /// 复制
    /// </summary>
    public static void Copy(string sourceFileName, string destFileName, bool overwrite=true)
    {
        var dstFolder = GetParentFolder(destFileName);
        if (!IsFolderPath(dstFolder))
        {
            Directory.CreateDirectory(dstFolder);
        }
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <summary>
    /// 复制文件夹到目标文件夹内
    /// </summary>
    [Obsolete("使用DirOpr代替")]
    public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite = true)
    {
        if (!IsFolderPath(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }

        var childDestFolder = destFolder + "\\" + Path.GetFileName(sourceFolder);
        foreach (string sub in Directory.GetDirectories(sourceFolder))
            CopyFolder(sub , childDestFolder);

        // 文件
        foreach (string file in Directory.GetFiles(sourceFolder))
            Copy(file, childDestFolder + "\\"+ Path.GetFileName(file), true);
    }

    /// <summary>
    /// 改文件名
    /// </summary>
    public static void ReName(string sourceFileName, string destFileName)
    {
        System.IO.File.Move(sourceFileName, destFileName);
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    public static bool Delete(string path)
    {
        if (File.Exists(@path))
        {
            //如果存在则删除
            File.Delete(@path);

            return true;
        }
        return false;
    }

    /// <summary>
    /// 遍历文件夹下所有文件(包含子文件夹下的文件)
    /// </summary>
    public static void PreorderTraversal(string folderPath,Action<string> OnFile,Action<string> OnFolder=null)
    {
        if (OnFile != null)
        {
            string[] allFiles = Directory.GetFiles(folderPath);
            foreach (var file in allFiles)
            {
                OnFile(file);
            }
        }
        string[] allFolders = Directory.GetDirectories(folderPath);

        foreach (var folder in allFolders)
        {
            PreorderTraversal(folder, OnFile, OnFolder);
            if (OnFolder != null) OnFolder(folder);
        }
    }
    /// <summary>
	/// 写文件, 当文件不存在时创建
	/// 文件存在时覆盖
	/// </summary>
	public static bool SaveFile(string path, byte[] content)
    {
        if (path.IsNullOrEmpty()) return false;
        var folder = Path.GetDirectoryName(path);
        if (folder.IsNullOrEmpty()) return false;
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        File.WriteAllBytes(path, content);
        return true;
    }
    /// <summary>
    /// 默认utf8无bom保存文件
    /// </summary>
    public static bool SaveFile(string path,string content,bool bom=false)
    {
        if (path.IsNullOrEmpty()) return false;
        var folder = Path.GetDirectoryName(path);
        if (!folder.IsNullOrEmpty() && !Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        if(bom)
        {
            File.WriteAllText(path, content, StringUtil.UTF8);
        }
        else
        {
            File.WriteAllText(path, content, StringUtil.UTF8NoBOM);
        }
        
        return true;
    }

    /// <summary>
    /// 写文件, 当文件不存在时创建
    /// over:true 文件存在时覆盖
    /// over:false (文件存在时不执行)
    /// </summary>
    [Obsolete]
    public static bool SaveFileX(string path, string content, bool over = true, FileType e = FileType.DEFAULT)
    {
        if (path == null || path=="") return false;
        var folder = Path.GetDirectoryName(path);


        if (File.Exists(path) && !over) return false;

        if (!Directory.Exists(folder) && folder != "")
        {
            Directory.CreateDirectory(folder);
        }

        if (e == FileType.DEFAULT)
        {
            File.WriteAllText(path, content);
        }
        else
        {
            Encoding ee = null;
            switch (e)
            {
                case FileType.Unicode:
                    ee = Encoding.Unicode;
                    break;
                case FileType.UTF8:
                    ee = Encoding.UTF8;
                    break;
                case FileType.ASCII:
                    ee = Encoding.ASCII;
                    break;
            }
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, ee))
                {
                    sw.Write(content);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 写文件, 当文件不存在时创建
    /// over:true 文件存在时覆盖
    /// over:false (文件存在时不执行)
    /// </summary>
    public static bool SaveFile(string path, byte[] content, bool over = true)
    {
        if (path == null || path == "") return false;
        var folder = Path.GetDirectoryName(path);


        if (File.Exists(path) && !over) return false;

        if (!Directory.Exists(folder) && folder != "")
        {
            Directory.CreateDirectory(folder);
        }
        var itt = File.Create(path);
        itt.Write(content, 0, content.Length);
        itt.Close();
        return true;
    }
    /// <summary>
    /// 读文件(默认UTF8)
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static string ReadFile(string path, Encoding encoding=null)
    {
        if (!Exists(path)) return null;
        if (encoding == null) encoding = StringUtil.UTF8;//encoding = Encoding.Default;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader sw = new StreamReader(fs, encoding))
            {
                return sw.ReadToEnd();
            }
        }
    }
    public static string ReadFile_UTF8(string path)
    {
        return ReadFile(path);
    }
    /// <summary>
    /// 无bom保存
    /// </summary>
    public static bool SaveFile_UTF8(string path,string content)
    {
        return SaveFile(path, content);
    }

    /// <summary>
    /// 将对象用Torsion序列化后,用Encoding.UTF8保存
    /// </summary>
    public static bool SaveObject(string path, object obj)
    {
        return SaveFile(path, Encoding.UTF8.GetBytes(Torsion.Serialize(obj)));
    }

    /// <summary>
    /// 用Encoding.UTF8格式反序列化文件后输出对象
    /// </summary>
    public static T ReadObject<T>(string path)
    {
        return Torsion.Deserialize<T>(Encoding.UTF8.GetString(File.ReadAllBytes(path)));
    }
    #endregion

    #region 获取

    /// <summary>  
    /// 获取文件的MD5码  
    /// </summary>  
    /// <param name="fileName">传入的文件名（含路径及后缀名）</param>  
    /// <returns></returns>  
    public static string GetMD5Hash(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    /// <summary>
    /// 获取文件夹下子文件
    /// </summary>
    public static string[] GetChildFiles(string folderPath, string searchPattern=null, SearchOption searchOption=SearchOption.AllDirectories)
    {
        return Directory.GetFiles(folderPath, searchPattern, searchOption);
    }


    /// <summary>
    /// 上一级目录
    /// </summary>
    public static string GetParentFolder(string path)
    {
        return Path.GetDirectoryName(ToAbsolutePath(path));
    }

    /// <summary>
    /// 上一级目录
    /// </summary>
    public static string GetParent(string folder)
    {
        return Directory.GetParent(folder).ToString();
    }

    /// <summary>
    /// 返回指定路径字符串的文件名和扩展名。
    /// </summary>
    public static string GetFileName(string file)
    {
        return Path.GetFileName(file);
    }

    /// <summary>
    /// 获取文件最后修改时间
    /// </summary>
    public static DateTime GetLastWriteTime(string file)
    {
        return new FileInfo(file).LastWriteTime;
    }

    /// <summary>
    /// 获取文件/文件夹大小
    /// </summary>
    public static long GetFileSize(string file)
    {
        if(File.Exists(file))
        {
            return new FileInfo(file).Length;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 返回指定的路径字符串的扩展名
    /// </summary>
    public static string GetNameByExtension(string file)
    {
        return Path.GetExtension(file);
    }

    /// <summary>
    /// 返回不具有扩展名的指定路径字符串的文件名。
    /// </summary>
    public static string GetNameByShort(string file)
    {
        return Path.GetFileNameWithoutExtension(file);
    }

    /// <summary>
    /// 通过相对路径获取绝对路径，路径以\分隔
    /// </summary>
    public static string ToAbsolutePath(string path)
    {
        return Path.GetFullPath(path);
    }


    /// <summary>
    /// 通过绝对路径获取相对路径(路径以分隔)
    /// </summary>
    public static string ToRelativePath(string path,string comparePath=null)
    {
        Uri uri = new Uri(Path.GetFullPath(path));
        Uri current = new Uri(comparePath==null?ProcessUtil.GetCurrentProcessFileName(): Path.GetFullPath(comparePath+"/"));
        var str = current.MakeRelativeUri(uri).ToString();
        str = Uri.UnescapeDataString(str);
        return str.Replace('/','\\');
    }
    #endregion


    #region 判断

    /// <summary>
    /// 是一个链接目录
    /// </summary>
    public static bool IsLinkPath(string path)
    {
        var att = File.GetAttributes(path);
        if (MathUtil.StateCheck(att, FileAttributes.ReparsePoint))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 是一个正确的文件路径
    /// </summary>
    public static bool IsFilePath(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// 是一个正确的文件夹路径
    /// </summary>
    public static bool IsFolderPath(string path)
    {
        return Directory.Exists(path);
    }
    #endregion

    #region 查询



    /// <summary>
    /// 查找文件
    /// </summary>
    public static string SearchFilePath(string file)
    {
        string path = file;
        if (File.Exists(path)) return path;
        path = DirOpr.GetDirectory() + @"\" + file;
        if (File.Exists(path)) return path;
        return file;
    }

    #endregion



   

    
    //6获取启动了应用程序的可执行文件的路径。效果和2、5一样。只是5返回的字符串后面多了一个"\"而已
    //public static string GetStartupPath()
    //{
    //    return System.Windows.Forms.Application.StartupPath;
    //}

    

    public static string CallExeName()
    {
        return System.Reflection.Assembly.GetEntryAssembly().Location.Substring(System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1).Replace(".exe", "");
    }

    /// <summary>
    /// 获得当前应用软件的版本
    /// </summary>
    public static Version CurrentVersion()
    {
        //Content.Text = "程序集版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n";
        //Content.Text += "文件版本：" + Application.ProductVersion.ToString() + "\n";
        //Content.Text += "部署版本：" + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
        return new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion);
    }

    /// <summary>
    /// 获得当前应用程序的根目录
    /// </summary>
    public static string CurrentApplicationDirectory()
    {
        return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
    }

    #region 压缩
    /// <summary>
    /// 解压
    /// </summary>
    public static void UnZipFile(string zipFilePath, string targetDir)
    {
        (new ICCEmbedded.SharpZipLib.Zip.FastZip()).ExtractZip(zipFilePath, targetDir, "");
    }
    /// <summary>
    /// 压缩
    /// </summary>
    public static void CreateZipFile(string zipFilePath, string[] files)
    {
        using (ICCEmbedded.SharpZipLib.Zip.ZipFile zip = ICCEmbedded.SharpZipLib.Zip.ZipFile.Create(zipFilePath))
        {
            zip.BeginUpdate();
            foreach (var file in files)
            {
                zip.Add(file);
            }
            zip.CommitUpdate();
        }
    }
    #endregion
}
