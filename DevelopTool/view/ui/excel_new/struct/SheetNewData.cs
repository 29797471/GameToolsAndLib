using Aspose.Cells;
using CqCore;
using DevelopTool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WinCore;

/// <summary>
/// 单数据表格数据和操作
/// </summary>
[Editor()]
public class SheetNewData : NotifyObject, IExport
{
    public System.Action Save;
    public override string ToString()
    {
        return Sheet_Name;
    }

    public string SheetName;

    /// <summary>
    /// 导出配置文件名
    /// </summary>
    [TextBox("导出配置名:"), MinWidth(150), Priority(4)]
    [Export("%NAME%")]
    public string OutFileName
    {
        get
        {
            return mOutFileName;
        }
        set
        {
            mOutFileName = value;
            Update("OutFileName");
        }
    }
    public string mOutFileName;

    /// <summary>
    /// 导出配置文件名
    /// </summary>
    [CheckBox("导出C#接口"), MinWidth(150), Priority(5)]
    public bool ExportCS
    {
        get
        {
            return mExportCS;
        }
        set
        {
            mExportCS = value;
            Update("ExportCS");
        }
    }
    public bool mExportCS;

    /// <summary>
    /// 导出配置文件名
    /// </summary>
    [Export("%ConstVal~%", "%~ConstVal%")]
    [CheckBox("常量表"), MinWidth(150), Priority(5,1)]
    public bool ConstVal
    {
        get
        {
            return mConstVal;
        }
        set
        {
            mConstVal = value;
            Update("ConstVal");
        }
    }
    public bool mConstVal;

    public Worksheet Sheet
    {
        get { return sheet; }
        set
        {
            sheet = value;

            if (IsCSV)
            {
                SheetName = "";
            }
            else
            {
                SheetName = sheet.Name;
            }
            //合并已保存的数据到表数据项中
            var temp = new ObservableCollection<ExcelNewStructItem>();

            for (int j = sheet.Cells.MinColumn, c = sheet.Cells.MaxColumn; j <= c; j++)
            {
                var cell = sheet.Cells[0, j];
                if (cell == null) continue;
                var name = cell.StringValue;
                if (!name.IsNullOrEmpty())
                {
                    var it = ItemList.ToList().Find(x => x.Name == name);
                    if (it == null)
                    {
                        it = new ExcelNewStructItem() { Name = name };
                    }
                    it.comment = null;
                    var comment = sheet.Comments[0, j];
                    if (comment != null)
                    {
                        it.CellComment = comment.Note.ToString();
                    }
                    temp.Add(it);
                }
            }

            //ItemList = temp;
            ItemList.Clear();
            foreach (var it in temp)
            {
                ItemList.Add(it);
            }
        }
    }
    Worksheet sheet;
    
    [Export("%DateItem~%","%~DataItem%")]
    public List<SheetNewDataItem> ExportData
    {
        get
        {
            var list = new List<SheetNewDataItem>();
            for (int i = ExcelNewModel.instance.setting.StartDataRow - 1, count = Sheet.Cells.MaxDataRow + 1; i < count; ++i)
            {
                if (Sheet.Cells[i,0].StringValue.IsNullOrEmpty()) continue;
                var item = new SheetNewDataItem();
                item.KeyValues = new List<VarValueData>();
                for (int j = 0; j < ItemList.Count; j++)
                {
                    var it = ItemList[j];
                    var cell = Sheet.Cells[i,j];
                    if (it.IsNullOrEmpty) continue;
                    var varValue = new VarValueData() 
                    { 
                        Key = it.Variable,
                        Value = TableDataFormat.instance.ToLua(cell.StringValue, it),
                    };
                    item.KeyValues.Add(varValue);
                }
                list.Add(item);
            }
            
            return list;
        }
    }

    [Export("%Key1%")]
    public string Key1
    {
        get
        {
            var keyList = KeyList;
            if (keyList.Count < 1) return null;
            return keyList[0].Variable;
        }
    }

    [Export("%Type1%")]
    public string Type1
    {
        get
        {
            var keyList = KeyList;
            if (keyList.Count < 1) return null;
            return keyList[0].Type;
        }
    }

    [Export("%Key2%")]
    public string Key2
    {
        get
        {
            var keyList = KeyList;
            if (keyList.Count < 2) return null;
            return keyList[1].Variable;
        }
    }
    [Export("%Type2%")]
    public string Type2
    {
        get
        {
            var keyList = KeyList;
            if (keyList.Count < 2) return null;
            return keyList[1].Type;
        }
    }

    /// <summary>
    /// 检索变量数量
    /// </summary>
    public int KeyNumber
    {
        get
        {
            return KeyList.Count;
        }
    }

    public List<ExcelNewStructItem> KeyList
    {
        get
        {
            return ItemList.ToList().FindAll(x => x.IsIndexItem);
        }
    }

    [Editor("修改表头"), Width(300), Height(200)]
    public class ModifyHeader: NotifyObject
    {
        public string mTitle;
        [TextBox("表头:",50),MinWidth(200),Priority(1)]
        public string Title
        {
            set
            {
                mTitle = value;
                Update("Title");
            }
            get
            {
                return mTitle;
            }
        }
    }
    public bool IsCSV
    {
        get
        {
            return Sheet.Workbook.FileFormat == FileFormatType.CSV;
        }
    }

    /// <summary>
    /// 双击修改表头
    /// </summary>
    /// <param name="obj"></param>
    public void OnDoubleClick(ExcelNewStructItem obj)
    {
        if (IsCSV) return;
        var data = new ModifyHeader() { Title = obj.Name };
        var editor = WinUtil.OpenEditorWindow(data);
        if (editor != null)
        {
            for (int i = sheet.Cells.MinColumn; i <= sheet.Cells.MaxColumn; i++)
            {
                var cell = sheet.Cells[0, i];
                if (cell.StringValue == obj.Name)
                {
                    cell.Value=editor.Title;

                    System.Action SaveModifile = () =>
                    {
                        //同时保存配置和表.
                        obj.Name = editor.Title;
                        Save();
                        sheet.Workbook.Save(sheet.Workbook.FileName);
                    };
                    if (FileOpr.IsOccupy(sheet.Workbook.FileName))
                    {
                        CustomMessageBox.ShowDialog(string.Format("文件({0}被占用),无法保存,是否关闭?", sheet.Workbook.FileName), "提示", (bl) =>
                        {
                            if (bl)
                            {
                                ProcessUtil.KillExcelProcess();
                                SaveModifile();
                            }
                        });
                    }
                    else
                    {
                        SaveModifile();
                    }
                    break;
                }
            }
            
        }
    }

    [Priority(6)]
    [ListView(),Height(370),MaxWidth(950), DoubleSelectedValue("OnDoubleClick")]
    [Export("%Item~%", "%~Item%")]
    public ObservableCollection<ExcelNewStructItem> ItemList
    {
        get
        {
            if (mItemList == null)
            {
                mItemList = new ObservableCollection<ExcelNewStructItem>();
            }
            return mItemList;
        }
        set
        {
            mItemList = value;
            Update("ItemList");
        }
    }

    [Export("%OneKey[%", "%]OneKey%")]
    public bool IsOneKey
    {
        get
        {
            return KeyNumber == 1;
        }
    }

    [Export("%DoubleKey[%", "%]DoubleKey%")]
    public bool IsDoubleKey
    {
        get
        {
            return KeyNumber == 2; 
        }
    }

    public ObservableCollection<ExcelNewStructItem> mItemList;
    
    

    [MenuItem("修改分页名称"), IsEnabled("IsCSV", "x=0")]
    public void MenuItemModifySheetName()
    {
        var data = new ModifySheetName() { Title = Sheet_Name };
        var editor = WinUtil.OpenEditorWindow(data);
        if (editor != null)
        {
            SheetName = editor.Title;
            System.Action SaveModifile = () =>
            {
                Sheet.Name = SheetName;
                Save();
                sheet.Workbook.Save(sheet.Workbook.FileName);
            };
            if (FileOpr.IsOccupy(sheet.Workbook.FileName))
            {
                CustomMessageBox.ShowDialog(string.Format("文件({0}被占用),无法保存,是否关闭?", sheet.Workbook.FileName), "提示", (bl) =>
                {
                    if (bl)
                    {
                        ProcessUtil.KillExcelProcess();
                        SaveModifile();
                    }
                });
            }
            else
            {
                SaveModifile();
            }

        }
    }

    [MenuItem("单分页导出")]
    public void Make()
    {
        foreach (var makefile in ExcelNewModel.instance.setting.TemplateFileList)
        {
            makefile.Make(this);
        }
    }
    [Editor("修改分页名称"), Width(300), Height(200)]
    public class ModifySheetName : NotifyObject
    {
        public string mTitle;
        [TextBox("分页名称:", 50), MinWidth(200), Priority(1)]
        public string Title
        {
            set
            {
                mTitle = value;
                Update("Title");
            }
            get
            {
                return mTitle;
            }
        }
    }
    [Export("%Sheet%")]
    public string Sheet_Name
    {
        get
        {
            if (Sheet.Workbook.FileFormat == FileFormatType.CSV) return "";
            return Sheet.Name;
        }
    }

    [Export("%Excel%")]
    public string ExcelName
    {
        get
        {
            return FileOpr.GetFileName(Sheet.Workbook.FileName);
        }
    }

    /// <summary>
    /// 是否需要导出
    /// </summary>
    public bool CanExport
    {
        get
        {
            return !OutFileName.IsNullOrEmpty();
        }
    }
}

