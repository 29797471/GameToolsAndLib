using CqCore;
using System.Diagnostics;
using System.Linq;

namespace DevelopTool
{
    /// <summary>
    /// 文本翻译
    /// </summary>
    [Priority(9)]
    [Editor("新版文本翻译配置", "/DevelopTool;component/Res/Images/Icons/translator.ico")]
    public class TranslatorNewModel : SingleModel<TranslatorNewModel>
    {
        public override Setting Setting { get { return setting; } }
        public TranslatorNewSetting setting { get { return SettingModel.instance.GetSetting<TranslatorNewSetting>(); } }

        //[Priority(2)]
        //[Panel, MaxWidth(1500),  MaxHeight(700)]
        public TranslatorNewModelData Data
        {
            get { if (mData == null) mData = new TranslatorNewModelData(); return mData; }
            set { mData = value; Update("Data"); }
        }
        public TranslatorNewModelData mData;

        public override bool OnSave()
        {
            Data.ExportExcel();
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            ExportChinese(null);
        }

        [Priority(1)]
        [Button, Click("ExportAll"), Margin(0, 0, 0, 0)]
        public string Btn
        {
            get
            {
                return "导所有语言数据";
            }
        }

        [Priority(2)]
        [Button, Click("ExportChinese"), Margin(0, 0, 0, 0)]
        public string Btn1
        {
            get
            {
                return "导出中文数据";
            }
        }

        [Priority(3)]
        [Button, Click("OpenExcel"), Margin(0, 0, 0, 0)]
        public string Btn2
        {
            get
            {
                return "打开表格目录";
            }
        }

        public void OpenExcel(object obj)
        {
            ProcessUtil.OpenFileOrFolderByExplorer(setting.ExcelPath);
        }
        public void ExportAll(object obj)
        {
            //Data = new TranslatorNewModelData();
            //Data.ImportExcelData();
            foreach (var it in setting.LinkTypesList)
            {
                ExportLanguage(int.Parse(it.Key), it.Value);
            }
            EventMgr.MsgPrint.Notify("生成所有语言文件成功",5);
        }
        public void ExportChinese(object obj)
        {
            var it = setting.LinkTypesList.ToList()[0];
            ExportLanguage(int.Parse(it.Key), it.Value);

            EventMgr.MsgPrint.Notify("生成中文配置文件成功", 5);
        }
        void ExportLanguage(int index,string languageFileName)
        {
            var t = new TranslatorNewModelData();
            t.ImportExcelData(index);
            foreach (var makefile in setting.TemplateFileList)
            {
                //var temp = makefile.FileName;
                //makefile.FileName = languageFileName;
                makefile.Make(t);
                //makefile.FileName = temp;
            }
            setting.TemplateFile.Make(t);
        }
    }
}
