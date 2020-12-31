
using Proto;
using System;
using System.Collections.Generic;
/// <summary>
/// 特性 optional,repeated,required
/// </summary>
public enum EDistinction
{
    optional,
    required,
    repeated,
}
[Editor]
[System.Serializable]
public class ProtoExpression : NotifyObject
{
    public ProtoMessage parent;

    [PriorityAttribute(0)]
    [ComboBox("特征"), SelectedValueAttribute("distinction"), Width(130)]
    [GridViewColumn("特征")]
    public object distinctions
    {
        get
        {
            return Enum.GetValues(typeof(EDistinction));
        }
    }
    [Export("%style%")]
    public string ExportStyle
    {
        get
        {
            return distinction == EDistinction.repeated ? "1" : "0";
        }
    }

    public EDistinction distinction
    {
        set
        {
            _distinction = value;
            Update("distinction");
        }
        get
        {
            return _distinction;
        }
    }

    EDistinction _distinction;

    [Export("%type%")]
    /// <summary>
    /// int32,int64,string...
    /// </summary>
    //[TextBox("类型"), Width(130)]
    //[GridViewColumn("类型")]
    //[PriorityAttribute(1)]
    public string FieldType
    {
        set
        {
            mFieldType = value;
            Update("FieldType");
        }
        get
        {
            return mFieldType;
        }
    }
    string mFieldType;

    
    [PriorityAttribute(1)]
    [GridViewColumn("类型"),MinWidth(50)]
    [ComboBox("类型1"),  SelectedValue("FieldType")]
    public List<string> types
    {
        get
        {
            return parent.EditTypes;
        }
    }

    [Export("%name%")]
    [TextBox("变量"), Width(200)]
    [GridViewColumn("变量")]
    [PriorityAttribute(2)]
    public string name { set { _name = value; Update("name"); } get { return _name; } }
    string _name;

    public object defaultValue
    {
        set
        {
            _defaultValue = value;
            Update("defaultValue");
        }
        get
        {
            return _defaultValue;
        }
    }
    object _defaultValue;

    public string comment { get { return _comment; } set { _comment = value; Update("comment"); } }
    string _comment;
}