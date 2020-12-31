using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Editor("代码模板")]
public class CodeStyleLanguage : NotifyObject
{
    public string Name
    {
        get { if (mName == null) mName = "temp"; return mName; }
        set { mName = value; Update("Name"); }
    }
    public string mName;

    public TreeNode ExpRoot { get; set; }
    public TreeNode ValueRoot { get; set; }

    //[Header("表达式"),Width(100)]
    [Editor("表达式",null, System.Windows.Controls.Orientation.Horizontal)]
    public class A : ATemp
    {
    }

    [Editor("值",null, System.Windows.Controls.Orientation.Horizontal)]
    public class B: ATemp
    {
    }

    [TabControl]
    public ObservableCollection<object> PosList
    {
        get
        {
            if (mPosList == null)
            {
                mPosList = new ObservableCollection<object>();
                mPosList.Add(new A() { Root = ExpRoot });
                mPosList.Add(new B() { Root = ValueRoot });
            }
            return mPosList;
        }
    }
    ObservableCollection<object> mPosList;
    

    public void Init(CodeSetting it)
    {
        Name = it.Name;
        string str = FileOpr.ReadFile(it.ExpPath);
        if (str != "")
        {
            ExpRoot = Torsion.Deserialize<TreeNode>(str);
        }
        else
        {
            ExpRoot = new TreeNode();
        }

        string str2 = FileOpr.ReadFile(it.ValuePath);
        if (str2 != "")
        {
            ValueRoot = Torsion.Deserialize<TreeNode>(str2);
        }
        else
        {
            ValueRoot = new TreeNode();
        }
    }
    public void Save(CodeSetting it)
    {
        FileOpr.SaveFile(it.ExpPath, Torsion.Serialize(ExpRoot));
        FileOpr.SaveFile(it.ValuePath, Torsion.Serialize(ValueRoot));
    }
}
public class ATemp : NotifyObject
{

    [Image, Width(20)]
    public string FindIcon
    {
        get
        {
            return "/WinCore;component/Res/find.ico";
        }
    }

    [TextBox(), ToolTip("检索过滤"), MinWidth(170)]
    [Priority(0, 0, 1)]
    public string Seach
    {
        get { return mSeach; }
        set { mSeach = value; Update("Seach"); Update("FilterItem"); }
    }
    string mSeach;

    public Predicate<object> FilterItem
    {
        get
        {
            if (string.IsNullOrEmpty(Seach)) return null;
            return o => o.ToString().ToLower().Contains(Seach.ToLower());
        }
    }

    [TreeView("CodeStyle"), Priority(0, 1), Width(300), Height(410), Filter("FilterItem"), SelectedValue("SelectObj")]
    public TreeNode Root
    {
        get { return mRoot; }
        set { mRoot = value; Update("Root"); }
    }
    TreeNode mRoot;

    [Priority(1)]
    [GroupBox(), Width(750), Height(430), Margin(30, 30)]
    public BaseTreeNotifyObject SelectObj
    {
        get { return mSelectObj; }
        set { mSelectObj = value; Update("SelectObj"); }
    }
    BaseTreeNotifyObject mSelectObj;
}