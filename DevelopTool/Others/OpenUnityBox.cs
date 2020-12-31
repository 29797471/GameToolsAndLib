using Business;
using CqCore;
using DevelopTool;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using WinCore;

[Editor("打开Unity"), Width(750), Height(300)]
public class OpenUnityBox : NotifyObject
{
    public string UnityFolder
    {
        get 
        {
            if(mUnityFolder.IsNullOrEmpty())
            {
                mUnityFolder = UnityExes[0];
            }
            return mUnityFolder;
        }
        set { mUnityFolder = value; Update("UnityFolder"); }
    }
    public string mUnityFolder;


    [Priority(1,2)]
    [Button, Click("UpdateFolder")]
    public string Btn2 { get { return "刷新安装目录"; } }
    public void UpdateFolder(object obj)
    {
        SettingModel.instance.setting.unityInstallFolders = null;
        Update("UnityExes");
    }

    [Priority(1)]
    [ComboBox("Unity安装目录",150), SelectedValue("UnityFolder")]
    public List<string> UnityExes
    {
        get
        {
            var list=SettingModel.instance.setting.unityInstallFolders;
            if(list==null)
            {
                var unityFolder = new List<string>();
                DirOpr.FindAllInDesk(ref unityFolder, null, folder => folder.Contains("Unity"), 2);
                foreach(var it in unityFolder)
                {
                    DirOpr.FindAll(ref list, it,null, folder => File.Exists(folder + @"\Editor\Unity.exe"));
                }
                SettingModel.instance.setting.unityInstallFolders = list;
                SettingModel.instance.Save();
            }
            return list;
        }
    }


    [Priority(2)]
    [FolderPath("Unity项目目录", false, 150), MinWidth(300)]
    public string ProjDir
    {
        get { return mProjDir; }
        set { mProjDir = value; Update("ProjDir"); }
    }
    public string mProjDir;

    public string TargetPlatform
    {
        set
        {
            mTargetPlatform = value;
            Update("TargetPlatform");
        }
        get
        {
            if(mTargetPlatform.IsNullOrEmpty())
            {
                mTargetPlatform = Platforms[0];
            }
            return mTargetPlatform;
        }
    }
    public string mTargetPlatform;


    [Priority(3)]
    [ComboBox("平台"), SelectedValue("TargetPlatform")]
    public string[] Platforms
    {
        get
        {
            return UnityPlatformDic.Values.ToArray();
        }
    }
    static Dictionary<string, string> mUnityPlatformDic;
    public static Dictionary<string,string> UnityPlatformDic
    {
        get
        {
            if(mUnityPlatformDic==null)
            {
                mUnityPlatformDic = new Dictionary<string, string>();
                mUnityPlatformDic["13"] = "Android";//Android
                mUnityPlatformDic["9"] = "iOS";//iOS
                mUnityPlatformDic["5"] = "StandaloneWindows";//StandaloneWindows
                mUnityPlatformDic["19"] = "StandaloneWindows64";//StandaloneWindows64
            }
            return mUnityPlatformDic;
        }
    }

    /// <summary>
    /// 1.关闭打开的unity<para/>
    /// 2.如果当前的库没有链接,读取平台,修改库名称为带平台后缀.<para/>
    /// 3.按打开的平台来将相应的平台库链接生成Libary.<para/>
    /// 4.调用unity命令传参数打开项目.
    /// </summary>
    public void Open()
    {
        if (mProjDir.IsNullOrEmpty()) return;
        if (mUnityFolder.IsNullOrEmpty()) return;

        UserSetting.Data.openUnityBox = this;
        UserSetting.Save();
        var unityExe = ProcessUtil.FindProcess(x => x.ProcessName == "Unity");
        if (unityExe != null)
        {
            bool result = false;
            CustomMessageBox.ShowDialog("关闭当前打开的unity", "提示", bl =>
            {
                result = bl;
            }, true);
            if (result) ProcessUtil.KillProcess("Unity");
        }
        {


            var unityExePath = UnityFolder + @"\Editor\Unity.exe";
            var Library = ProjDir + @"\Library";
            var Library_platform = string.Format("{0}_{1}", Library, mTargetPlatform);


            if (Directory.Exists(Library) ) 
            {

                //获取平台
                var x = FileOpr.ReadFile(Library+ @"\AssetImportState");

                var currentPlatform= UnityPlatformDic[x.Split(";")[0]];


                if( !Directory.Exists(Library + @"\_" + currentPlatform))
                //这不是一个链接文件夹
                {
                    Directory.Move(Library, Library + "_" + currentPlatform);
                    Directory.CreateDirectory(Library + "_" + currentPlatform + @"\_" + currentPlatform);
                }
                
            }
            if (!Directory.Exists(Library_platform + @"\_"+ mTargetPlatform))
            {
                Directory.CreateDirectory(Library_platform + @"\_" + mTargetPlatform);
            }
            if (!Directory.Exists(Library_platform))
            {
                Directory.CreateDirectory(Library_platform);
            }
            Command.instance.LinkFolder(Library_platform, Library);
            Thread.Sleep(500);
            ProcessUtil.Start(unityExePath, " -projectPath " + ProjDir + " -buildTarget " + mTargetPlatform);
        }
    }
}
