using Aspose.Cells;
using DevelopTool;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

[Editor("")]
public class TranslatorNewModelData : NotifyObject
{

    [Image, Width(20)]
    public string FindIcon
    {
        get
        {
            return "/WinCore;component/Res/find.ico";
        }
    }

    [Priority(0, 1)]
    [TextBox(), ToolTip("检索过滤"), MinWidth(170)]
    public string Seach
    {
        get { return mSeach; }
        set { mSeach = value; Update("Seach"); Update("FilterItem"); }
    }
    string mSeach;

    public Predicate<object> FilterItem
    {
        get
        {
            if (string.IsNullOrEmpty(Seach)) return null;
            return o => o.ToString().ToLower().Contains(Seach.ToLower());
        }
    }

    //     [Priority(2)]
    //     [Button("导出Excel")]
    public void ExportExcel()
    {
        return;
        //var ce = new CustomExcel();
        //ce.Load(SettingModel.instance.setting.translatorNewSet.ExcelPath);
        //var cs = ce[0];
        //foreach (var it in NodeList)
        //{
        //    cs[it.ExcelRow, 0].Value= it.Id.ToString();
        //    cs[it.ExcelRow, 1].Value= it.StyleIndex.ToString();
        //    cs[it.ExcelRow, 2].Value= it.SysStr;
        //    cs[it.ExcelRow, 5].Value= it.TranFormat.ToString();
        //    //cs[it.ExcelRow, 3].Value= string.Join(",", it.childs.ConvertAll<string>(x => (x as ValueCodeTemplate).Data));
        //}
        //ce.Save();
    }
    [Export("%Language%")]
    public string Language
    {
        get
        {
            return mLanguage;
        }
    }
    string mLanguage;
    //[Priority(1)]
    //[Button("从Excel导入数据")]
    public void ImportExcelData(int language = 3)
    {
        if (!FileOpr.IsFilePath(TranslatorNewModel.instance.setting.ExcelPath)) return;
        NodeList.Clear();
        LoadLanguageExcel(TranslatorNewModel.instance.setting.ExcelPath, language);
        LoadLanguageExcel(TranslatorNewModel.instance.setting.ExcelPath2, language);

        mLanguage = TranslatorNewModel.instance.setting.LinkTypesList.ToList().Find(x =>
        {
            return x.Key == language.ToString();
        }).Value;
    }
    void LoadLanguageExcel(string path, int language)
    {
        if (path == null) return;
        var ce = new Workbook(path);
        var sheet = ce.Worksheets[0];
        for (int i = 0; i <sheet.Cells.MaxDataRow; i++)
        {
            var row = sheet.Cells.Rows[i];
            if (row==null || row[0]==null) continue;
            int Id;

            if (row[0].Type== CellValueType.IsNumeric)
            {
                Id = row[0].IntValue;
                var node = new TranslatorNewNode();
                node.Id = Id;
                node.ExcelRow = i;
                if (row[1] == null) node.mStyleIndex = 0;
                else node.mStyleIndex = row[1].IntValue;
                node.TranFormat = row[language].StringValue;
                NodeList.Add(node);
            }
        }
    }

    [Export("%Start_Translator%", "%End_Translator%")]
    //[ListViewHeaderClick("Id:Sort", "SysStr:Sort2", "StyleIndex:Sort3")]
    [ListView(),Filter("FilterItem"),Height(500),Width(1050),Margin(10,10),HorizontalAlignment( HorizontalAlignment.Left)]
    [Priority(1)]
    public ObservableCollection<TranslatorNewNode> NodeList
    {
        get { if (mNodeList == null) mNodeList = new ObservableCollection<TranslatorNewNode>(); return mNodeList; }
        set { mNodeList = value; Update("NodeList"); }
    }

    public ObservableCollection<TranslatorNewNode> mNodeList;
    
    //public void Sort()
    //{
    //    var q = from item in mNodeList orderby item.Id select item;
    //    NodeList = new ObservableCollection<TranslatorNewNode>(q);
    //}

    //public void Sort2()
    //{
    //    var q = from item in mNodeList orderby item.SysStr select item;
    //    NodeList = new ObservableCollection<TranslatorNewNode>(q);
    //}

    //public void Sort3()
    //{
    //    var q = from item in mNodeList orderby item.StyleIndex select item;
    //    NodeList = new ObservableCollection<TranslatorNewNode>(q);
    //}
}
