/// <summary>
/// 数据结构成员
/// </summary>
[Editor("数据成员")]
public class ExcelNewStructItem : CustomerStructItem
{
    /// <summary>
    /// 表数据项头
    /// </summary>
    [Export("%Name%",0)]
    [Label("名字"), Width(130), Priority(1), ToolTip(0, "CellComment")]
    [GridViewColumn("名字")]
    public string Name { get { if (name == null) name = ""; return name; } set { name = value; Update("Name"); } }

    public string name;

    [Priority(6)]
    [Export("%IndexItemStart%", "%IndexItemEnd%",6)]
    [CheckBox("检索"), Width(50),IsEnabled("AllowIndex")]
    [GridViewColumn("检索")]
    public bool IsIndexItem
    {
        get { return mIsIndexItem; }
        set { mIsIndexItem = value; Update("IsIndexItem"); }
    }

    public bool mIsIndexItem;

    public bool AllowIndex
    {
        get
        {
            return IsBaseType() && IsList == false;
        }
    }
    
    public string CellComment { get; set; }
}