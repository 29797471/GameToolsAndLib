using CqCore;
using System;
using System.IO;

namespace Business
{
    /// <summary>
    /// Command 的摘要说明。
    /// </summary>
    public class Command:Singleton<Command>
    {
        /// <summary>
        /// 将 srcFolder 文件夹链接到 dstFolder 下
        /// </summary>
        public bool LinkIntoFolder(string srcFolder, string dstFolder)
        {
            return LinkFolder(srcFolder, dstFolder + @"\" + Path.GetFileNameWithoutExtension(srcFolder));
        }
        /// <summary>
        /// 如果dstFolder文件夹存在,会被删除.
        /// 将 srcFolder 文件夹链接到 dstFolder
        /// </summary>
        public bool LinkFolder(string srcFolder,string dstFolder)
        {
            srcFolder = FileOpr.ToAbsolutePath(srcFolder);
            dstFolder = FileOpr.ToAbsolutePath(dstFolder);

            Directory.CreateDirectory(FileOpr.GetParent(dstFolder));
            DirOpr.Delete(dstFolder);
            {
                
                var cmd = "mklink /J \"" + dstFolder + "\" \"" + srcFolder + "\"";
                RunCmd(cmd);
                return true;
            }
            //return false;
        }

        /// <summary>
        /// 文件夹拷贝
        /// </summary>
        [Obsolete("有BUG")]
        public void CopyFolder(string srcfolder, string dstFolder)
        {
            RunCmd(string.Format("rd /s /q  {0}", dstFolder));
            RunCmd(string.Format("md  {0}", dstFolder));
            RunCmd(string.Format("xcopy {0} {1} /d/e", srcfolder, dstFolder)); 
        }
        /// <summary>
        /// 执行CMD语句
        /// </summary>
        /// <param name="cmd">要执行的CMD命令</param>
        [Obsolete()]
        public void RunCmd(string cmd)
        {
            ProcessUtil.Done("cmd.exe", cmd);
        }
    }
}