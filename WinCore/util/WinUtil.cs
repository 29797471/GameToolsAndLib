using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

public static partial class WinUtil
{
    /// <summary>
    /// 选择路径保存文件对话框,参数返回文件路径
    /// </summary>
    /// <param name="path">选择的文件路径</param>
    /// <param name="filter">形如.txt</param>
    /// <param name="Title"></param>
    /// <returns></returns>
    public static bool SaveFileDialog(ref string path,string filter= "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*", string Title="")
    {
        var dialog = new System.Windows.Forms.SaveFileDialog();
        dialog.FileName = path;
        dialog.Title = Title;
        dialog.Filter = filter;
        
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
        {
            return false;
        }
        path = dialog.FileName;
        return true;
    }
    public static string GetFileByOpenDialog( string path, string filter, string Title="")
    {
        //Microsoft.Win32.OpenFileDialog dialog =
        var dialog = new System.Windows.Forms.OpenFileDialog();
        dialog.Title = Title;
        dialog.Filter = filter;
        if (FileOpr.IsFilePath(path))
        {
            dialog.InitialDirectory = FileOpr.ToAbsolutePath(FileOpr.GetParentFolder(path));
            dialog.FileName = FileOpr.GetFileName(path);
        }

        switch (dialog.ShowDialog())
        {
            case System.Windows.Forms.DialogResult.Cancel:
                return null;
            default:
                return dialog.FileName;
        }
    }
    public static string[] GetFilesByOpenDialog(string path, string filter, string Title = "")
    {
        //Microsoft.Win32.OpenFileDialog dialog =
        var dialog = new System.Windows.Forms.OpenFileDialog();
        dialog.Title = Title;
        dialog.Filter = filter;
        dialog.Multiselect = true;
        if (FileOpr.IsFilePath(path))
        {
            dialog.InitialDirectory = FileOpr.ToAbsolutePath(FileOpr.GetParentFolder(path));
            dialog.FileName = FileOpr.GetFileName(path);
        }

        switch (dialog.ShowDialog())
        {
            case System.Windows.Forms.DialogResult.Cancel:
                return null;
            default:
                return dialog.FileNames;
        }
    }

    public static bool GetFolderByOpenDialog(ref string path,string description,bool isRelativePath=false)
    {
        System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
        // 设置根在桌面
        m_Dialog.RootFolder =  System.Environment.SpecialFolder.Desktop;
        m_Dialog.Description = description;

        if (FileOpr.IsFolderPath(path))
        {
            // 设置当前选择的路径
            m_Dialog.SelectedPath = FileOpr.ToAbsolutePath(path);
        }

        // 允许在对话框中包括一个新建目录的按钮
        m_Dialog.ShowNewFolderButton = true;

        switch (m_Dialog.ShowDialog())
        {
            case System.Windows.Forms.DialogResult.Cancel:
                return false;
            default:
                if(isRelativePath)
                {
                    path=FileOpr.ToRelativePath(path);
                }
                else
                {
                    path = m_Dialog.SelectedPath;
                }
                return true;
        }
    }
    /// <summary>
    /// 通过嵌入路径,获取嵌入资源
    /// </summary>
    public static Stream GetResource(string resName, UriKind kind = UriKind.Relative)
    {
        Uri uri = new Uri( resName, kind);
        return System.Windows.Application.GetResourceStream(uri).Stream;
    }

    /// <summary>
    /// 由嵌入资源路径 镜像生成文件
    /// </summary>
    public static void CreateFileFromRes(string resName,string filePath)
    {
        Stream s = GetResource(resName);
        byte[] buffer = new byte[s.Length];
        int dd = s.Read(buffer, 0, buffer.Length);
        //MessageBox.Show( StringUtil.GB2312.GetString(buffer) );
        FileStream ss = File.Create(filePath);
        ss.Write(buffer, 0, buffer.Length);
        ss.Close();
    }

    public static Stream GetAssemblyRes(string path)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        return executingAssembly.GetManifestResourceStream(path);
    }

    public static void PrintPanel(Grid panel )
    {
        //PrintDocumentImageableArea area = null;
        //XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(ref area);
        //if (xpsdw != null)
        //{
        //    xpsdw.Write(panel);
        //}

        PrintDialog dlg = new System.Windows.Controls.PrintDialog();
        if (dlg.ShowDialog() == true)
        {
            // Get selected printer capabilities.
            PrintCapabilities capabilities = dlg.PrintQueue.GetPrintCapabilities(dlg.PrintTicket);

            // Get scale of the print wrt to screen of Chart.
            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / panel.ActualWidth
                , capabilities.PageImageableArea.ExtentHeight / panel.ActualHeight);

            // Save old transform.
            System.Windows.Media.Transform savedTransform = panel.LayoutTransform;
            // Scale the Chart.
            panel.LayoutTransform = new System.Windows.Media.ScaleTransform(scale, scale);

            // Get the size of the printer page.
            Size sz = new Size(capabilities.PageImageableArea.ExtentWidth
                , capabilities.PageImageableArea.ExtentHeight);

            // Update the layout of the Chart to the printer page size.
            panel.Measure(sz);
            panel.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth
                , capabilities.PageImageableArea.OriginHeight), sz));

            // Print the Chart to printer to fit on the one page.
            dlg.PrintVisual(panel, "Print the Chart");

            // Restore old transform.
            panel.LayoutTransform = savedTransform;
        }
    }

    /// <summary>
    /// 检查.NET Framework版本
    /// </summary>
    public static string[] GetVersionFromRegistry()
    {
        List<string> list = new List<string>();
        //检查.NET Framework版本 1 - 4
        // Opens the registry key for the .NET Framework entry.
        using (RegistryKey ndpKey =
            RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
            OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
        {
            // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
            // or later, you can use:
            // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
            // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            foreach (string versionKeyName in ndpKey.GetSubKeyNames())
            {
                if (versionKeyName.StartsWith("v"))
                {

                    RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                    string name = (string)versionKey.GetValue("Version", "");
                    string sp = versionKey.GetValue("SP", "").ToString();
                    string install = versionKey.GetValue("Install", "").ToString();
                    if (install == "") //no install info, must be later.
                        list.Add(versionKeyName + "  " + name);
                    else
                    {
                        if (sp != "" && install == "1")
                        {
                            list.Add(versionKeyName + "  " + name + "  SP" + sp);
                        }

                    }
                    if (name != "")
                    {
                        continue;
                    }
                    foreach (string subKeyName in versionKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                        name = (string)subKey.GetValue("Version", "");
                        if (name != "")
                            sp = subKey.GetValue("SP", "").ToString();
                        install = subKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            list.Add(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                list.Add("  " + subKeyName + "  " + name + "  SP" + sp);
                            }
                            else if (install == "1")
                            {
                                list.Add("  " + subKeyName + "  " + name);
                            }
                        }
                    }
                }
            }
        }
#if false   /// 检查.NET Framework版本 4.5~

        Func<int, string> CheckFor45DotVersion = (releaseKey) =>
           {
               if (releaseKey >= 393295)
               {
                   return "4.6 or later";
               }
               if ((releaseKey >= 379893))
               {
                   return "4.5.2 or later";
               }
               if ((releaseKey >= 378675))
               {
                   return "4.5.1 or later";
               }
               if ((releaseKey >= 378389))
               {
                   return "4.5 or later";
               }
               // This line should never execute. A non-null release key should mean
               // that 4.5 or later is installed.
               return "No 4.5 or later version detected";
           };

        
        using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
        {
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
            {
                list.Add("Version: " + CheckFor45DotVersion((int)ndpKey.GetValue("Release")));
            }
            else
            {
                list.Add("Version 4.5 or later is not detected.");
            }
        }
#endif
        return list.ToArray();
    }
    /// <summary>
    /// 获得当前.Net版本
    /// </summary>
    /// <returns></returns>
    public static string GetVersionFromEnvironment()
    {
        return "Version: " + Environment.Version.ToString();
    }
}
