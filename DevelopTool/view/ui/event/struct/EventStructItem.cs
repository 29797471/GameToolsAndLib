using DevelopTool;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 编辑数据结构成员的数据模型
/// </summary>
[Editor("数据结构成员")]
public class EventStructItem : NotifyObject
{
    public override string ToString()
    {
        return Name;
    }
    /// <summary>
    /// 表数据项
    /// </summary>
    [Priority(4)]
    [TextBox("名称"), Width(150)]
    [GridViewColumn("名称")]
    [Export("%ItemName%")]
    public string Name { get { return name; } set { name = value; Update("Name"); } }

    public string name;
    /// <summary>
    /// 变量名
    /// </summary>
    [Priority(1)]
    [TextBox("变量"), Width(100)]
    [GridViewColumn("变量")]
    [Export("%Variable%")]
    public string Variable { get { return variable; } set { variable = value; Update("Variable"); } }

    public string variable;
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
    /// 该项是否为数组
    /// </summary>
    [Priority(3)]
    [CheckBox("数组"), Width(50)]
    [GridViewColumn("数组")]
    [Export("%ListStart%", "%|%", "%ListEnd%")]
    public bool IsList { get { return isList; } set { isList = value; Update("IsList"); } }

    public bool isList;

    [GridViewColumn("说明")]
    [TextBox("说明"), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), Width(300)]
    [Priority(5)]
    public string Comment { get { return comment; } set { comment = value; Update("Comment"); } }

    public string comment;

    [Export("%Start_Comment%", "%End_Comment%")]
    public List<CommentPart> CommentMultiline
    {
        get
        {
            return StringUtil.SplitLine(Comment).ToList().ConvertAll(x=>new CommentPart() { CommentPartName=x });
        }
    }
    

    /// <summary>
    /// 该数据结构数据是否完全有效,(判断变量名是否定义,用于导表是作为导出的标准)
    /// </summary>
    public bool IsValidData()
    {
        return Variable != "" && Variable!=null;
    }

    public string ExecContent
    {
        get
        {
            return EventModel.instance.setting.ItemExecContent.Replace("%Variable%", Variable);
        }
    }
}