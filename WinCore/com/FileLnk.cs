//using System;
//using System.Runtime.InteropServices;
///// <summary>
///// 文件快捷方式
///// </summary>
//public class FileLnk
//{
//    IWshRuntimeLibrary.IWshShortcut shortcut;
//    /// <summary>
//    /// 快捷方式路径
//    /// </summary>
//    public FileLnk()
//    {
//    }
//    public static bool IsLnk(string lnkPath)
//    {
//        return FileOpr.GetNameByExtension(lnkPath) == ".lnk";
//    }
//    public static FileLnk GetLnk(string lnkPath)
//    {
//        var lnk = new FileLnk();
        
//        var shell = new IWshRuntimeLibrary.WshShell();
//        lnk.shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(lnkPath);
//        return lnk;
//    }

//    /// <summary>  
//    /// 获取快捷方式启动路径  
//    /// </summary>
//    public string TargetPath
//    {
//        get
//        {
//            return shortcut.TargetPath;
//        }
//    }

//    /// <summary>  
//    /// 获取快捷方式图标 
//    /// </summary>
//    public string IconLocation
//    {
//        get
//        {
//            return shortcut.IconLocation;
//        }
//    }
//    #region  API获取图标2  是我发现最简单 最好的方法
//    [DllImport("shell32.DLL", EntryPoint = "ExtractAssociatedIcon")]
//    private static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon); //声明函数
//    static System.IntPtr thisHandle;
//    public static System.Drawing.Icon SetIcon(string s)//S是要获取文件路径，返回ico格式文件
//    {
//        int RefInt = 0;
//        thisHandle = new IntPtr(ExtractAssociatedIconA(0, s, ref RefInt));
//        return System.Drawing.Icon.FromHandle(thisHandle);
//    }
//    #endregion
//}