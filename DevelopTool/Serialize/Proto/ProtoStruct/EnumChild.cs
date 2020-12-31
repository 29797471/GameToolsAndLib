
[System.Serializable]
[EditorAttribute]
public class EnumChild : NotifyObject
{
    [PriorityAttribute(0)]
    [TextBox("变量"), MinWidth(100)]
    [GridViewColumn("变量")]
    public string name { set { _name = value; Update("name"); } get { return _name; } }
    string _name;
    public string comment { get { return _comment; } set { _comment = value; Update("comment"); } }
    string _comment;

    [PriorityAttribute(1)]
    [TextBox("值"), MinWidth(100)]
    [GridViewColumn("值")]
    public int number { get { return _number; } set { _number = value; Update("number"); } }
    int _number;
}

