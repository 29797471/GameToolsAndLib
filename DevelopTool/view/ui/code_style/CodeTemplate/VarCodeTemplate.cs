using CodeStyle;
using CqCore;
using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using WinCore;
/// <summary>
/// 编辑变量
/// </summary>
[Editor("代码-变量")]
public class VarCodeTemplate : NotifyObject, IExpression
{
    public int ItemsCount
    {
        get
        {
            return Items.Count;
        }
    }

    [Visibility("ItemsCount", AttributeTarget.Parent, "x=0")]
    [Label()]
    [Priority(1)]
    public string NoItem { get { return "没有该类型("+ StyleType + ")的变量"; } }

    [MenuItem("查看值对应的代码", "/WinCore;component/Res/find.ico")]
    public void ShowViewCode()
    {
        CustomMessageBox.Show(ExecContent);
    }
    public override string ToString()
    {
        return Content;
    }
   

    /// <summary>
    /// 编程语言
    /// </summary>
    public string Language
    {
        get { return mLanguage; }
        set { mLanguage = value; }
    }
    public string mLanguage;

    CodeStyleLanguage CodeLanguage
    {
        get
        {
            return CodeStyleNewModel.instance.GetNodeList().ToList().Find(x => x.Name == Language);
        }
    }

    /// <summary>
    /// 模版类型
    /// </summary>
    public string StyleType
    {
        get { return mStyleType; }
        set { mStyleType = value; Update("StyleType"); }
    }
    public string mStyleType;

    public string Content
    {
        get
        {
            return VarName;
        }
    }

    public string ExecContent
    {
        get
        {
            if (VarName == null) return null;
            return TriggerModel.instance.ToVarName(VarName);
        }
    }

    /// <summary>
    /// 变量名
    /// </summary>
    public string VarName
    {
        get { if (mVarName == null && Items.Count > 0) mVarName = Items[0]; return mVarName; }
        set { mVarName = value; Update("VarName"); }
    }
    public string mVarName;


    [Visibility("ItemsCount", AttributeTarget.Parent, "x>0")]
    [ComboBox(), SelectedValue("VarName"), VerticalAlignment(System.Windows.VerticalAlignment.Center)]
    [Priority(2, 1)]
    public List<string> Items
    {
        get
        {
            var types = TriggerModel.instance.setting.CustomerList.ToList().FindAll(x => x.VarType == StyleType).ConvertAll(x => x.Variable);
            return types;
        }
    }
}