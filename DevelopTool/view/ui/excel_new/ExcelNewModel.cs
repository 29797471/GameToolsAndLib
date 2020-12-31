using CqCore;
using System;
using System.Collections.Generic;
using WinCore;

namespace DevelopTool
{
    [Priority(6)]
    [Editor("Excel新版导出配置", "/DevelopTool;component/Res/Images/Icons/excel_new.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class ExcelNewModel : SingleModel<ExcelNewModel>
    {
        public override Setting Setting { get { return setting; } }
        public ExcelNewSetting setting { get { return SettingModel.instance.GetSetting<ExcelNewSetting>(); } }
        
        [Image, Width(20)]
        public string FindIcon
        {
            get
            {
                return "/WinCore;component/Res/find.ico";
            }
        }

        [Priority(0, 0, 1)]
        [TextBox(), ToolTip("检索过滤"), MinWidth(170)]
        public string Search
        {
            get { return mSearch; }
            set { mSearch = value; Update("Search"); Update("FilterItem"); }
        }
        string mSearch;

        public Predicate<object> FilterItem
        {
            get
            {
                if (string.IsNullOrEmpty(Search)) return null;
                return o => (o as ExcelNewData).ShortName.ToLower().Contains(Search.ToLower());
            }
        }

        [Export("%Start_StructList%", "%End_StructList%")]
        [Priority(0, 1)]
        [ListBox(), Width(300), Height(520), Filter("FilterItem"), SelectedValue("SelectObj"), DoubleSelectedValue("OnDoubleClick") ,DisplayMember("ShortName")]
        public ExcelNewList<ExcelNewData> NodeList
        {
            get { return mCustomStructList; }
            set { mCustomStructList = value; Update("CustomStructList"); }
        }

        ExcelNewList<ExcelNewData> mCustomStructList;
        
        public void OnDoubleClick(ExcelNewData obj)
        {
            NodeList.OnOpenExcel(obj);
        }
        public override void OnHide()
        {
            SelectObj = null;
        }

        [Priority(1)]
        [GroupBox(), Width(700),Height(570) ,Margin(30, 0)]
        public ExcelNewData SelectObj
        {
            get { return mSelectObj; }
            set
            {
                mSelectObj = value;
                Update("SelectObj");
                if (mSelectObj!=null) mSelectObj.HasOpen = true;
            }
        }
        ExcelNewData mSelectObj;

        public ExcelNewModel()
        {
            if (setting.excelFolderPath!=null && FileOpr.IsFolderPath(setting.excelFolderPath))
            {
                NodeList = new ExcelNewList<ExcelNewData>();
                FileOpr.PreorderTraversal(setting.excelFolderPath, file =>
                {
                    if (file.Contains("~$")) return;
                    string extentsion = FileOpr.GetNameByExtension(file);
                    var it = new ExcelNewData();
                    it.Path = file;
                    NodeList.Add(it);
                }, null);
            }
        }
        public override bool OnSave()
        {
            //FileOpr.DeleteFolder(setting.SetPath);
            foreach (var it in NodeList)
            {
                it.Save();
            }
            return true;
            //FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(NodeList));
        }
        /// <summary>
        /// 只生成打开过的配置表格,命令行生成时还是全部生成
        /// </summary>
        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
#if !CMD
            if (SelectObj==null)
            {
                var result= CustomMessageBox.ShowDialogNormal("是否导出全部?", "提示");
                ClearFolderAndMakeAll(result);
            }
            else
            {
                ClearFolderAndMakeAll();
            }
#else
            ClearFolderAndMakeAll(true);
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forcedToExport">导出全部</param>
        void ClearFolderAndMakeAll(bool forcedToExport = false)
        {
            //if(forcedToExport)
            //{
            //    for (int i = 0; i < setting.TemplateFileList.Count; i++)
            //    {
            //        var makefile = setting.TemplateFileList[i];
            //        FileOpr.DeleteFolder(makefile.FolderPath);
            //    }
            //}
            List<string> allNames = new List<string>();
            foreach (var itt in NodeList)
            {
                itt.MakeFiles(allNames, forcedToExport);
            }
            if (forcedToExport)setting.TemplateFile.Make(this);
            setting.MarkFile.Make(this);
        }
    }

    
    
}
