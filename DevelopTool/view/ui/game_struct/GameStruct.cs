using DevelopTool;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// 编辑数据结构的结构
/// </summary>
[Editor("数据结构")]
public class GameStruct : NotifyObject
{
    /// <summary>
    /// 可以隐式相互强转的数据结构类型
    /// </summary>
    [TextBox("隐式关联",100),MinWidth(100), Priority(1)]
    [Export("%ImplicitStruct%", 1)]
    public string ImplicitStruct { get { return mImplicitStruct; } set { mImplicitStruct = value; Update("ImplicitStruct"); } }
    public string mImplicitStruct;

    [Export("%Name%")]
    [TextBox("结构名称",100),MinWidth(100),Priority(0)]
    public string Name
    {
        get { if (name == null) name = "temp"; return name; }
        set { name = value; Update("Name");}
    }
    public string name;

    /// <summary>
    /// 类修饰符
    /// </summary>
    
    [Export("%ClassDefine%")]
    public string ClassDefine
    {
        get { if (mClassDefine == null) mClassDefine = ClassDefineList[0]; return mClassDefine; }
        set { mClassDefine = value; Update("ClassDefine"); }
    }

    public string mClassDefine;

    /// <summary>
    /// 类修饰符
    /// </summary>
    [ComboBox("类修饰符", 100), SelectedValue("ClassDefine"), Priority(2)]
    public ObservableCollection<string> ClassDefineList { get { return GameStructModel.instance.setting.ClassDefineList; } }

    [Export("%Start%", "%End%")]
    [ListView(),MinHeight(200), Priority(3)]
    public CustomList<GameStructItem> CustomerList
    {
        get {if(customerList==null) customerList = new CustomList<GameStructItem>(); return customerList; }
        set { customerList = value; Update("CustomerList"); } }

    public CustomList<GameStructItem> customerList;
}