using System.Collections;
using System.Collections.Generic;

namespace CqBehavior.Task
{
    [Editor("文件夹资源清单生成")]
    [MenuItemPath("添加/行为节点/文件操作/资源文件夹md5清单生成")]
    public class FolderMd5List : CqBehaviorNode
    {
        [Priority(1)]
        [MinWidth(350)]
        [FolderPath("资源文件夹路径", true)]
        public string ResPath { get { return mResPath; } set { mResPath = value; Update("ResPath"); } }
        public string mResPath;

        protected override IEnumerator OnExecute()
        {
            if (FileOpr.IsFolderPath(ResPath))
            {
                var parentFolder = FileOpr.GetParentFolder(ResPath);
                var info = new ResInfo();

                string[] args = System.Environment.GetCommandLineArgs();
                if (args.Length > 2)//有app版本号
                {
                    info.appVersion = args[2];
                    info.resVersion = args[2];
                }
                else
                {
                    var lastV = Torsion.Deserialize<ResInfo>(FileOpr.ReadFile(parentFolder + "/info.dat"));
                    if (lastV != null)
                    {
                        info.appVersion = lastV.appVersion;
                    }
                    info.resVersion = TimeUtil.NowVersion;
                }
                var fileDic = new Dictionary<string, ResFileInfo>();

                FileOpr.PreorderTraversal(ResPath,
                    file =>
                    {
                        if (FileOpr.GetNameByExtension(file) == ".meta") return;
                        var fileInfo = new ResFileInfo();
                        var path = FileOpr.ToRelativePath(file, ResPath);
                        path = /*"Resources/"+*/path.Replace(@"\", "/");
                        fileInfo.md5 = FileOpr.GetMD5Hash(file);

                        fileInfo.size = FileOpr.GetFileSize(file);
                        fileInfo.lastWriteTime = FileOpr.GetLastWriteTime(file).Ticks;
                        fileDic[path] = fileInfo;
                        info.totalSize += fileInfo.size;
                    });

                FileOpr.SaveFile(parentFolder + "/md5.dat", Torsion.Serialize(fileDic));

                FileOpr.SaveFile(parentFolder + "/info.dat", Torsion.Serialize(info));

                Result = true;
            }
            else
            {
                Result = false;
            }

            yield break;
        }
    }
}

public class ResInfo
{
    public string resVersion;
    public string appVersion;
    public long totalSize;
}

public class ResFileInfo
{
    public string md5;
    public long size;
    public long lastWriteTime;
}

