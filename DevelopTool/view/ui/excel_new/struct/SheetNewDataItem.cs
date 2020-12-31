using System.Collections.Generic;

[Editor()]
public class SheetNewDataItem
{
    [Export("%VarValueData~%", "%~VarValueData%")]
    public List<VarValueData> KeyValues { get; set; }
}
public class VarValueData : NotifyObject
{
    [Export("%Var%")]
    public string Key { get { return mKey; } set { mKey = value; Update("Key"); } }
    public string mKey;

    [Export("%Value%")]
    public string Value { get { return mValue; } set { mValue = value; Update("Value"); } }
    public string mValue;
}