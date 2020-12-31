using CqCore;
using DevelopTool;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 数据结构成员
/// </summary>
[Editor("数据结构成员")]
public class CommonStructItem : NotifyObject
{
    /// <summary>
    /// 变量名
    /// </summary>
    [Export("%Variable%")]
    [TextBox("变量"), MinWidth(150), Priority(1)]
    [GridViewColumn("变量")]
    public string Variable
    {
        get { return variable; }
        set { variable = value; Update("Variable"); }
    }

    public string variable;

    /// <summary>
    /// 默认值
    /// </summary>
    [TextBox("默认值"), Width(80)]
    [GridViewColumn("默认值")]
    [Priority(5)]
    [Export("%DefaultValue%")]
    public string DefaultValue
    {
        get { return mDefaultValue; }
        set { mDefaultValue = value; Update("DefaultValue"); }
    }

    public string mDefaultValue;

    [Export("%DefaultValueStart%", "%DefaultValueEnd%")]
    public bool HasDefaultValue
    {
        get
        {
            return !DefaultValue.IsNullOrEmpty();
        }
    }
    [Export("%NoDefaultValueStart%", "%NoDefaultValueEnd%")]
    public bool NoDefaultValue
    {
        get
        {
            return DefaultValue.IsNullOrEmpty();
        }
    }

    /// <summary>
    /// 备注
    /// </summary>
    [Priority(10)]
    [TextBox("备注"), MinWidth(350),AcceptsReturn,AcceptsTab]
    [Export("%Desc%")]
    [GridViewColumn("备注")]
    public string Desc
    {
        get { return mDesc; }
        set { mDesc = value; Update("Desc"); }
    }

    public string mDesc;

    [Export("%Start_Comment%", "%End_Comment%")]
    public List<CommentPart> CommentMultiline
    {
        get
        {
            return StringUtil.SplitLine(Desc).ToList().ConvertAll(x => new CommentPart() { CommentPartName = x });
        }
    }

    [Priority(2)]
    [ComboBox("类型"), SelectedValue("Type"), Width(100)]
    [GridViewColumn("类型")]
    public List<string> TypeList
    {
        get
        {
            return CommonStructModel.instance.DefineTypeList;
        }
    }
    /// <summary>
    /// 变量类型
    /// </summary>
    [Export("%Type%")]
    public string Type
    {
        get { if (type == null) type = TypeList[0]; return type; }
        set { type = value; Update("Type"); }
    }

    public string type;
    /// <summary>
    /// 该项是否为数组
    /// </summary>
    [Export("%ListStart%", "%|%", "%ListEnd%")]
    [Priority(3)]
    [CheckBox("数组"), Width(50)]
    [GridViewColumn("数组")]
    public bool IsList
    {
        get { return isList; }
        set { isList = value; Update("IsList"); }
    }

    public bool isList;


    /// <summary>
    /// 该项是否为数组
    /// </summary>
    [Export("%IsConst%", "%-IsConst%")]
    [Priority(9)]
    [CheckBox("常量"), Width(50)]
    [GridViewColumn("常量")]
    public bool IsConst
    {
        get { return mIsConst; }
        set { mIsConst = value; Update("IsConst"); }
    }
    public bool mIsConst;

}