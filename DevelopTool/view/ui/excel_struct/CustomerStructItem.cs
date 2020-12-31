using DevelopTool;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// 数据结构成员
/// </summary>
[Editor("数据成员")]
public class CustomerStructItem : NotifyObject, IExport
{
    /// <summary>
    /// 变量名
    /// </summary>
    [Export("%Variable%",2)]
    [Priority(2)]
    [TextBox("变量名"), Width(150)]
    [GridViewColumn("变量名")]
    public string Variable { get { if (variable == null) variable = ""; return variable; } set { variable = value; Update("Variable"); } }

    public string variable;

    /// <summary>
    /// 变量类型
    /// </summary>
    [Export("%Type%",0)]
    public string Type
    {
        get { if (type == null) type = TypeList[0]; return type; }
        set { type = value; Update("Type"); Update("AllowIndex"); }
    }

    public string type;
    /// <summary>
    /// 该项是否为数组
    /// </summary>
    [Export("%ListStart%", "%|%", "%ListEnd%")]
    [Priority(4)]
    [CheckBox("数组"), Width(50)]
    [GridViewColumn("数组")]
    public bool IsList { get { return isList; } set { isList = value; Update("IsList"); Update("AllowIndex"); } }

    public bool isList;

    ///// <summary>
    ///// 数组分隔符
    ///// </summary>
    //[Export("%ListSeparator%",5),IsEnabled("IsList")]
    //[Priority(5)]
    //[TextBox("符号"), Width(50), ToolTip("数组分隔符")]
    //[GridViewColumn("符号")]
    //public string ListSeparator
    //{
    //    get { return mListSeparator; }
    //    set { mListSeparator = value; Update("ListSeparator"); }
    //}

    //public string mListSeparator;


    [Export("%Comment%",0)]
    public string Comment { get { return comment; } set { comment = value; Update("Comment"); } }

    public string comment;

    
    public bool CanExport => !IsNullOrEmpty;

    /// <summary>
    /// 该数据项是否完全有效,(判断变量名是否定义,用于导表是作为导出的标准)
    /// </summary>
    public bool IsNullOrEmpty
    {
        get => string.IsNullOrEmpty(Variable);
    }

    /// <summary>
    /// 是一个基础类型
    /// </summary>
    public bool IsBaseType()
    {
        return ExcelStructModel.instance.setting.baseTypeList.Contains(Type);
    }
    public override string ToString()
    {
        return Variable+","+Type+","+IsList;
    }
    //public static implicit operator string(CustomerStructItem customer)
    //{
    //    return customer.ToString();
    //}

    [ComboBox("类型"), SelectedValue("Type"), MinWidth(100), Priority(3)]
    [GridViewColumn("类型")]
    public List<string> TypeList
    {
        get
        {
            ///合并自定义类型和默认类型
            var t = ExcelStructModel.instance.setting.baseTypeList;
            var k = ExcelStructModel.instance.CustomStructList.ToList().ConvertAll(x => x.Name);
            return t.Concat<string>(k).ToList();
        }
    }

    
}