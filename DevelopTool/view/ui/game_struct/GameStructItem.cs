using DevelopTool;
using System.Collections.Generic;

/// <summary>
/// 数据结构成员
/// </summary>
[Editor("数据结构成员")]
public class GameStructItem : NotifyObject
{
    [Export("%Name%")]
    /// <summary>
    /// 表数据项头
    /// </summary>
    public string Name { get { return name; } set { name = value; Update("Name"); } }

    public string name;
    /// <summary>
    /// 变量名
    /// </summary>
    [TextBox("变量"), Width(100), Priority(1)]
    [Export("%Variable%")]
    [GridViewColumn("变量")]
    public string Variable { get { return variable; } set { variable = value; Update("Variable"); } }

    public string variable;

    /// <summary>
    /// 默认值
    /// </summary>
    [TextBox("默认值"), Width(80)]
    [Export("%DefaultValue%")]
    [GridViewColumn("默认值")]
    [Priority(5)]
    public string DefaultValue { get { return mDefaultValue; } set { mDefaultValue = value; Update("DefaultValue"); } }

    public string mDefaultValue;

    /// <summary>
    /// 备注
    /// </summary>
    [TextBox("备注"), Width(250), Priority(4)]
    [Export("%Desc%")]
    [GridViewColumn("备注")]
    public string Desc { get { return mDesc; } set { mDesc = value; Update("Desc"); } }

    public string mDesc;

    [Priority(2)]
    [ComboBox("类型"), SelectedValue("Type"), Width(100)]
    [GridViewColumn("类型")]
    public List<string> TypeList
    {
        get
        {
            return GameStructModel.instance.DefineTypeList;
        }
    }
    /// <summary>
    /// 变量类型
    /// </summary>

    [Export("%Type%")]
    public string Type { get { if (type == null) type = TypeList[0]; return type; } set { type = value; Update("Type"); } }

    public string type;
    /// <summary>
    /// 该项是否为数组
    /// </summary>
    [Priority(3)]
    [CheckBox("数组"), Width(50)]
    [Export("%ListStart%", "%|%", "%ListEnd%")]
    [GridViewColumn("数组")]
    public bool IsList { get { return isList; } set { isList = value; Update("IsList"); } }

    public bool isList;


    /// <summary>
    /// 该数据项是否完全有效,(判断变量名是否定义,用于导表是作为导出的标准)
    /// </summary>
    public bool IsValidData()
    {
        return Variable != "" && Variable!=null;
    }

    public override string ToString()
    {
        return Variable+","+Type+","+IsList;
    }
}