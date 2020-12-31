using DevelopTool;
using System;
using System.Windows.Media;
using WinCore;

[Editor("生成文件"), Width(550), Height(350)]
public class MakeFile : NotifyObject
{
    /// <summary>
    /// 文件名
    /// </summary>
    [Priority(2)]
    [TextBox("导出文件名",100), MinWidth(350)]
    public string FileName
    {
        get { if (mFileName == null) mFileName = "temp"; return mFileName; }
        set { mFileName = value; Update("FileName"); }
    }
    public string mFileName;

    /// <summary>
    /// 模板
    /// </summary>
    [Priority(3)]
    [FileEdit("模版编辑")]
    public string TemplateContent
    {
        get { if (mTemplateContent == null) mTemplateContent = ""; return mTemplateContent; }
        set { mTemplateContent = value; Update("TemplateContent"); }
    }
    public string mTemplateContent;

    /// <summary>
    /// 生成文件导出路径
    /// </summary>
    [Priority(1)]
    [FolderPath("导出目录", true,100),MinWidth(300)]
    public string FolderPath
    {
        get { if (mFolderPath == null) mFolderPath = "temp"; return mFolderPath; }
        set { mFolderPath = value; Update("FolderPath"); }
    }
    public string mFolderPath;

    public void Make(object it=null)
    {
        if (it == null) return;
        if (it is IExport && (it as IExport).CanExport == false) return;
        if (!mFileName.IsNullOrEmpty() && !mFolderPath.IsNullOrEmpty())
        {
            var outputFileName = mFileName;
            var outputFileContent = mTemplateContent;
            if (it!=null)
            {
                outputFileName = ExportAttribute.InjectData(outputFileName, it);
                outputFileContent = ExportAttribute.InjectData(outputFileContent, it);
            }
            var savePath = mFolderPath + @"\" + outputFileName;

            FileOpr.SaveFile(savePath, outputFileContent);
#if DEBUG
            Console.WriteLine("  生成文件:" + savePath);
#endif
        }
    }
    [Priority(0)]
    [TextBox("名称", 100),MinWidth(350)]
    public string Name
    {   
        get { if (mName == null) mName = "temp"; return mName; }
        set { mName = value; Update("Name"); }
    }
    public string mName;

    public override string ToString()
    {
        return Name;
    }
}