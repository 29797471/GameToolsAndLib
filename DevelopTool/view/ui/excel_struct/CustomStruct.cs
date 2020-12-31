using DevelopTool;
using System.Collections.ObjectModel;
/// <summary>
/// 编辑数据结构的结构
/// </summary>
[Editor("Excel数据结构")]
public class CustomStruct : NotifyObject, IExport
{
    [Export("%Name%",2)]
    [TextBox("名称", 100),Priority(2),MinWidth(100)]
    public string Name
    {
        get { if (name == null) name = "temp"; return name; }
        set { name = value; Update("Name");}
    }
    public string name;

    public bool CanExport => Make;

    [Export("%Make~","~Make%")]
    [CheckBox("生成"), MinWidth(100), Priority(3)]
    public bool Make
    {
        get { return mMake; }
        set { mMake = value; Update("Make"); }
    }
    public bool mMake;

    [Export("%Item~", "~Item%",5)]
    [ListView(), MinHeight(200)]
    [Priority(5)]
    public CustomList<CustomerStructItem> CustomerList
    {
        get {if(customerList==null) customerList = new CustomList<CustomerStructItem>(); return customerList; }
        set { customerList = value; Update("CustomerList"); } }

    

    public CustomList<CustomerStructItem> customerList;

}