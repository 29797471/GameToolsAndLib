using DevelopTool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DevelopTool
{
    [Priority(4)]
    [Editor("触发器", "/DevelopTool;component/Res/Images/Icons/trigger.ico")]
    public class TriggerSetting : Setting
    {
        [Priority(3)]
        [Button("生成文件"),Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;

        [Priority(4)]
        [ComboBox("语言"), SelectedValue("Language")]
        public List<string> LanguageList
        {
            get
            {
                return CodeStyleNewModel.instance.setting.CodeSettingList.ToList().ConvertAll(x => x.Name);
            }
        }

        
        public string Language
        {
            get { if (mLanguage == null && LanguageList.Count>0) mLanguage = LanguageList[0]; return mLanguage; }
            set { mLanguage = value; Update("Language"); CodeTypeList = null; }
        }
        public string mLanguage;

        [Priority(5)]
        [ComboBox("执行"), SelectedValue("Action")]
        public List<string> CodeTypeList
        {
            set
            {
                Condition = null;
                Action = null;
                NewEvent = null;
                Update("CodeTypeList");
            }
            get
            {
                if (CodeStyleNewModel.instance.setting.CodeSettingList.Count == 0) return null;
                return CodeStyleNewModel.instance.setting.CodeSettingList.ToList().Find(x => x.Name == Language).Types;
            }
        }

        
        public string Action
        {
            get { if (mAction == null && CodeTypeList!=null && CodeTypeList.Count>0) mAction = CodeTypeList[0]; return mAction; }
            set { mAction = value; Update("Action"); }
        }
        public string mAction;

        [Priority(6)]
        [ComboBox("条件"), SelectedValue("Condition")]
        public List<string> CodeTypeList1
        {
            set
            {
                Condition = null;
                Action = null;
                NewEvent = null;
                Update("CodeTypeList");
            }
            get
            {
                if (CodeStyleNewModel.instance.setting.CodeSettingList.Count == 0) return null;
                return CodeStyleNewModel.instance.setting.CodeSettingList.ToList().Find(x => x.Name == Language).Types;
            }
        }

        public string Condition
        {
            get { if (mCondition == null && CodeTypeList!=null && CodeTypeList.Count>0) mCondition = CodeTypeList[0]; return mCondition; }
            set { mCondition = value; Update("Condition"); }
        }
        public string mCondition;

        [Priority(7)]
        [ComboBox("事件"), SelectedValue("NewEvent")]
        public List<string> CodeTypeList2
        {
            set
            {
                Condition = null;
                Action = null;
                NewEvent = null;
                Update("CodeTypeList");
            }
            get
            {
                if (CodeStyleNewModel.instance.setting.CodeSettingList.Count == 0) return null;
                return CodeStyleNewModel.instance.setting.CodeSettingList.ToList().Find(x => x.Name == Language).Types;
            }
        }

        public string NewEvent
        {
            get { if (mNewEvent == null && CodeTypeList != null && CodeTypeList.Count > 0) mNewEvent = CodeTypeList[0]; return mNewEvent; }
            set { mNewEvent = value; Update("NewEvent"); }
        }
        public string mNewEvent;


        [ListView("编辑触发器变量"), MinHeight(200), Priority(8)]
        public CustomList<TriggerVar> CustomerList
        {
            get { if (customerList == null) customerList = new CustomList<TriggerVar>(); return customerList; }
            set { customerList = value; Update("CustomerList"); }
        }

        public CustomList<TriggerVar> customerList;

        [Priority(9)]
        [TextBox("导出变量名前缀"),MinWidth(150)]
        public string PrefixVar
        {
            get { return mPrefixVar; }
            set { mPrefixVar = value; Update("PrefixVar"); }
        }
        public string mPrefixVar;


    }
}
/// <summary>
/// 触发器变量
/// </summary>
[Editor]
public class TriggerVar:NotifyObject
{
    /// <summary>
    /// 生成变量名
    /// </summary>
    [Export("%VarName%")]
    public string VarName
    {
        get
        {
            return TriggerModel.instance.ToVarName(Variable);
        }
    }

    /// <summary>
    /// 变量名
    /// </summary>
    [Priority(1)]
    [TextBox, Width(100)]
    [GridViewColumn("变量名称")]
    [Export("%Variable%")]
    public string Variable { get { return variable; } set { variable = value; Update("Variable"); } }

    public string variable;

    /// <summary>
    /// 默认值
    /// </summary>
    [TextBox, Width(80)]
    [GridViewColumn("默认值")]
    [Priority(3)]
    [Export("%DefaultValue%")]
    public string DefaultValue { get { return mDefaultValue; } set { mDefaultValue = value; Update("DefaultValue"); } }

    public string mDefaultValue;

    [Priority(2)]
    [ComboBox, SelectedValue("VarType"), Width(120)]
    [GridViewColumn("类型")]
    public List<string> TypeList
    {
        get
        {
            return CodeStyleNewModel.instance.GetCodeTypeList(TriggerModel.instance.setting.Language);
        }
    }

    public string VarType
    {
        get { if (mVarType == null) mVarType = TypeList[0]; return mVarType; }
        set { mVarType = value; Update("VarType"); }
    }

    public string mVarType;
}