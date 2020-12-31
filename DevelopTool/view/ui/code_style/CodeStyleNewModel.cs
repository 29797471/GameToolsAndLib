using CodeStyle;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace DevelopTool
{
    /// <summary>
    /// 编辑数据结构面板关联的数据模块
    /// </summary>
    [Priority(3)]
    [Editor("代码转译器", "/DevelopTool;component/Res/Images/Icons/code_translation.ico")]
    public class CodeStyleNewModel : SingleModel<CodeStyleNewModel>
    {
        public override Setting Setting { get { return setting; } }
        
        public CodeStyleNewSetting setting { get { return SettingModel.instance.GetSetting<CodeStyleNewSetting>(); } }

        private ObservableCollection<CodeStyleLanguage> nodeList;

        public ObservableCollection<CodeStyleLanguage> GetNodeList()
        {
            return nodeList;
        }

        public void SetNodeList(ObservableCollection<CodeStyleLanguage> value)
        {
            nodeList = value;
        }

        [Priority(2)]
        [Panel]
        public CodeStyleLanguage SelectObj
        {
            get
            {
                if (mSelectObj == null || nodeList.IndexOf(mSelectObj) != setting.SelectIndex)
                {
                    if (nodeList!=null && nodeList.Count> setting.SelectIndex)
                    {
                        mSelectObj = nodeList[setting.SelectIndex];
                    }
                }
                return mSelectObj;
            }
            set { mSelectObj = value; Update("SelectObj"); }
        }
        CodeStyleLanguage mSelectObj;

        public CodeStyleNewModel()
        {
            SetNodeList(new ObservableCollection<CodeStyleLanguage>());
            foreach (var it in setting.CodeSettingList)
            {
                var cCodeStyleLanguage = new CodeStyleLanguage();
                cCodeStyleLanguage.Init(it);
                GetNodeList().Add(cCodeStyleLanguage);
            }
        }

        public override void OnHide()
        {
            if (SelectObj == null) return;
            ((CodeStyleLanguage.A)SelectObj.PosList[0]).SelectObj = null;
            ((CodeStyleLanguage.B)SelectObj.PosList[1]).SelectObj = null;
            SelectObj = null;
        }
        public override bool OnSave()
        {
            foreach (var it in setting.CodeSettingList)
            {
                var result= GetNodeList().ToList().Find(x => x.Name == it.Name);
                result.Save(it);
            }
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            foreach (var it in setting.CodeSettingList)
            {
                foreach (var makefile in it.TemplateFileList)
                {
                    makefile.Make(this);
                }
            }
        }
        /// <summary>
        /// 转译语言列表
        /// </summary>
        public List<string> LanguageList
        {
            get
            {
                return setting.CodeSettingList.ToList().ConvertAll(x => x.Name);
            }
        }

        /// <summary>
        /// 转译语言的类型列表
        /// </summary>
        public List<string> GetCodeTypeList(string language)
        {
            if (language == "") return null;
            return setting.CodeSettingList.ToList().Find(x => x.Name == language).Types;
        }
        /// <summary>
        /// 转译语言的数据结构
        /// </summary>
        public CodeStyleLanguage GetLanguageData(string language)
        {
            CodeStyleLanguage csl = null;
            if (language != null)
            {
                csl = GetNodeList().ToList().Find(x => x.Name == language);
            }
            if (csl == null)
            {
                csl = GetNodeList()[0];
            }
            return csl;
        }
        public ValueCodeTemplate DefaultValue(string type, string language = null)
        {
            if (GetNodeList().Count == 0) return null;
            var node = GetLanguageData(language).ValueRoot.FindByPreorder(x => x.IsLeaf() &&
              (x.nodeObj as CodeStyleNode).TypeName == type);
            if (node == null) return null;
            return (node.nodeObj as CodeStyleNode).NewValue;
        }
        /// <summary>
        /// 返回null时表示一条type的表达式都没定义
        /// </summary>
        public ExprCodeTemplate DefaultExpr(string type, string language = null)
        {
            if (GetNodeList().Count == 0) return null;
            var node = GetLanguageData(language).ExpRoot.FindByPreorder(x => x.IsLeaf() &&
            (x.nodeObj as CodeStyleNode).TypeName == type);
            if (node == null) return null;
            return (node.nodeObj as CodeStyleNode).NewExpr;
        }
    }
}

