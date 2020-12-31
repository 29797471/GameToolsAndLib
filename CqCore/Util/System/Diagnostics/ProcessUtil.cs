using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CqCore
{
    public static class ProcessUtil
    {
        /// <summary>
        ///  查询占用文件的进程
        /// </summary>
        public static Process GetProcessByLockFile(string fileName,string handlePath= @"C:\soft\Handle\handle64.exe")
        {
            //要检查被那个进程占用的文件
            Process tool = new Process();
            tool.StartInfo.FileName = handlePath;
            tool.StartInfo.Arguments = "\""+fileName+ "\"" + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();
            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                //Process.GetProcessById(int.Parse(match.Value)).Kill();
                var pro = Process.GetProcessById(int.Parse(match.Value));
                if(pro!=null)
                {
                    return pro;
                }
            }
            return null;

        }
        /// <summary>
        /// 获取当前进程的完整路径
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProcessFileName()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }


        /// <summary>
        /// 模糊查找进程
        /// </summary>
        public static Process FindProcess(Predicate<Process> match)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                if (match(p))
                {
                    return p;
                }
            }
            return null;
        }
        /// <summary>
        /// 模糊查找进程
        /// </summary>
        public static Process FindProcess(string str)
        {
            Process[] processes = Process.GetProcesses();
            
            foreach (Process p in processes)
            {
                try
                {
                    if (p.MainModule.FileName.Contains(str))
                    {
                        return p;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }
        /// <summary>
        /// 打开软件并执行命令
        /// </summary>
        public static Process Done(string fileName, string arguments = null)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (!arguments.IsNullOrEmpty())
            {
                proc.StandardInput.WriteLine(arguments);
                //proc.StandardInput.Write(arguments);
            }
            proc.Close();
            return proc;
        }

        /// <summary>
        /// 由系统根据相应的文件扩展名调用相应的执行程序打开文件.<para/>
        /// 如果是一个链接,会调用默认浏览器来打开链接
        /// </summary>
        public static void OpenExplorer(string url)
        {
            Process.Start("Explorer.exe", url);
        }

        /// <summary>
        /// 通过名称杀进程
        /// </summary>
        public static void KillProcess(string name)
        {
            var procs = Process.GetProcessesByName(name);

            foreach (var pro in procs)
            {
                pro.Kill();
            }
        }

        /// <summary>
        /// 通过名称杀进程
        /// </summary>
        public static void KillProcessByOccupyFile(string filePath)
        {
            Process tool = new Process();
            tool.StartInfo.FileName = "handle.exe";
            tool.StartInfo.Arguments = filePath + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardInput = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.StartInfo.CreateNoWindow = false;//是否显示窗体

            tool.Start();
            tool.WaitForExit();
            string outputTool = tool.StandardOutput.ReadToEnd();

            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                Process.GetProcessById(int.Parse(match.Value)).Kill();
            }
        }

        /// <summary>
        /// 系统调用文件或者文件夹的浏览窗口并选中路径
        /// </summary>
        public static void OpenFileOrFolderByExplorer(string path)
        {
            Process.Start("Explorer.exe", "/select," + path.ReplaceAll("/","\\"));
        }

        /// <summary>
        /// 执行程序,或者用系统的打开方式来打开文件
        /// </summary>
        public static Process Start(string fileName, string arguments=null,bool waitForExit=false)
        {
            var p=Process.Start(fileName, arguments);
            if(waitForExit)
            {
                p.WaitForExit();
            }
            return p;
        }
        /// <summary>
        /// SVN更新
        /// </summary>
        public static IEnumerator SVNUpdate(string folder)
        {
            var process = SVNCommand(string.Format("update /path:\"{0}\" ", folder));
            yield return GlobalCoroutine.ThreadPoolCall(() => process.WaitForExit());
        }

        /// <summary>
        /// SVN检出
        /// </summary>
        public static IEnumerator SVNCheckout(string url, string folder)
        {
            var process = SVNCommand(string.Format("checkout /url:\"{0}\" /path:\"{1}\"", url, folder));
            yield return GlobalCoroutine.ThreadPoolCall(() => process.WaitForExit());
        }
        /// <summary>
        /// SVN提交
        /// </summary>
        public static void SVNCommit(List<string> files)
        {
            int partLength = 200;
            if(files.Count> partLength)
            {
                SVNCommit(files.GetRange(0, partLength));
                SVNCommit(files.GetRange(partLength, files.Count- partLength));
                return;
            }
            SVNCommand(string.Format("commit /path:\"{0}\"", string.Join("*", files.ToArray())));
        }
        /// <summary>
        /// SVN命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="autoClose">如果没发生错误和冲突则自动关闭对话框</param>
        /// <returns></returns>
        static Process SVNCommand(string command,bool autoClose=true)
        {
            return Start("TortoiseProc.exe", string.Format("/command:{0} {1}",command,autoClose? "/closeonend:2":""));
        }
        /// <summary>
        /// 命令行
        /// </summary>
        public static Process DoneCmd(string command)
        {
            return Done("cmd.exe", command);
        }


        /// <summary>
        /// 杀excel进程
        /// </summary>
        public static void KillExcelProcess()
        {
            KillProcess("excel");
            KillProcess("et");
            KillProcess("JisuNumber");
            GC.Collect();
        }
    }
}
