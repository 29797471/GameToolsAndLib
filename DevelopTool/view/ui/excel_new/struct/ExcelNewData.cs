using Aspose.Cells;
using CqCore;
using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinCore;

[Editor("表格")]
public class ExcelNewData : NotifyObject
{
    bool mHasOpen;
    /// <summary>
    /// 打开过的表格,才做保存和导出操作
    /// </summary>
    public bool HasOpen
    {
        get { return mHasOpen; }
        set { mHasOpen = value; }
    }
    ~ExcelNewData()
    {
        cancelHandle.CancelAll();
    }
    CancelHandle cancelHandle;
    public string Path
    {
        get { return path; }
        set
        {
            path = value; Update("Path");
            if(cancelHandle==null)
            {
                cancelHandle = new CancelHandle();
            }
            wf = new WatcherFile(path);
            wf.AddChanged(() => GlobalCoroutine.Call(ReLoad), cancelHandle);
        }
    }
    string path;
    WatcherFile wf;

    public void ReLoad()
    {
        mExcel = null;
        mSingleConfigList = null;
        Update("SingleConfigList");

        EventMgr.MsgPrint.Notify(string.Format("表格({0})在外部改变,重新载入!", ShortName), 5);
    }

    public string ConfigPath
    {
        get
        {
            return ExcelNewModel.instance.setting.ExcelDatPath + @"\" + ShortName + ".dat";
        }
    }

    Workbook Excel
    {
        get
        {
            if (mExcel == null)
            {
                mExcel = new Workbook(path);
#if DEBUG
                Console.WriteLine("读取表格:" + path);
#endif
            }
            return mExcel;
        }
    }
    Workbook mExcel;

    [TabControl()]
    [Export("%Start_Sheet%", "%End_Sheet%")]
    public ObservableCollection<SheetNewData> SingleConfigList
    {
        get
        {
            if (mSingleConfigList == null)
            {
                //合并配置中的分页数据到表格中的分页
                mSingleConfigList = new ObservableCollection<SheetNewData>();
                ObservableCollection<SheetNewData> temp;
                if (FileOpr.Exists(ConfigPath))
                {
#if DEBUG
                    Console.WriteLine("\n  读取配置:" + ConfigPath);
#endif
                    temp = Torsion.Deserialize<ObservableCollection<SheetNewData>>(FileOpr.ReadFile(ConfigPath));
                }
                else
                {
                    temp = new ObservableCollection<SheetNewData>();
                }
                if(Excel.FileFormat == FileFormatType.CSV)
                {
                    SheetNewData it;

                    if (temp.Count==0)
                    {
                        it = new SheetNewData();
                    }
                    else
                    {
                        it = temp[0];
                    }
                    it.Sheet = Excel.Worksheets[0];
                    it.Save = Save;
                    mSingleConfigList.Add(it);
                }
                else
                {
                    foreach (var sheet in Excel.Worksheets)
                    {
                        var it = temp.ToList().Find(x => x.SheetName == sheet.Name);

                        if (it == null)
                        {
                            it = new SheetNewData();
                        }
                        else
                        {
                            temp.Remove(it);
                        }
                        it.Sheet = sheet;
                        it.Save = Save;
                        mSingleConfigList.Add(it);
                    }
                    foreach (var it in temp)
                    {
                        CustomMessageBox.ShowDialog(
                            string.Format("配置({0})中的分页数据({1})在Excel中找不到,\n修改保存后原配置会被清除",
                            ShortName, it.SheetName));
                    }
                }
            }
            return mSingleConfigList;
        }
    }
    public ObservableCollection<SheetNewData> mSingleConfigList;

    public string ShortName
    {
        get
        {
            return FileOpr.GetNameByShort(path);
        }
    }

    public void Save()
    {
        if (HasOpen)
        {
            FileOpr.SaveFile(ExcelNewModel.instance.setting.ExcelDatPath + @"\" + ShortName + ".dat", Torsion.Serialize(mSingleConfigList));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="allNames"></param>
    /// <param name="forcedToExport">强制导出表格,不检查是否有编辑过</param>
    public void MakeFiles(List<string> allNames, bool forcedToExport = false)
    {
        var setting = ExcelNewModel.instance.setting;
#if !CMD
        if (HasOpen || forcedToExport)
#endif
        {
            foreach (var it in SingleConfigList)
            {
                if (it.CanExport)
                {
                    if (!allNames.Contains(it.OutFileName))
                    {
                        allNames.Add(it.OutFileName);
                    }
                    else
                    {
                        throw new Exception(string.Format("存在相同的导出名称:{0}", it.OutFileName));
                    }
                    for (int i = 0; i < setting.TemplateFileList.Count; i++)
                    {
                        if (i == 1 && !it.ExportCS) continue;
                        var makefile = setting.TemplateFileList[i];
                        makefile.Make(it);
                    }
                }
            }
        }
    }
}