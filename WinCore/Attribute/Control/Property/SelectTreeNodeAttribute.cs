using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

[Editor()]
public class TreeComboBox : NotifyObject
{
    public TreeComboBox parent;
    public TreeComboBoxGroup group;
    [ComboBox(), Priority(1), DisplayMember("Name"), SelectedValue("Node")]
    public ObservableCollection<TreeNode> List
    {
        get
        {
            if(parent==null)
            {
                return group.Root.Children;
            }
            return parent.Node.Children;
        }
    }

    public TreeNode Node
    {
        get { return mNode; }
        set
        {
            if (mNode == value) return;
            mNode = value;
            Update("Node");
            group.UserChanging(this);
        }
    }
    TreeNode mNode;


    public void Select(string name=null)
    {
        foreach(var it in List)
        {
            if(it.Name==name)
            {
                Node = it;
                return;
            }
        }
        Node = List[0];
    }
}

[Editor()]
public class TreeComboBoxGroup : NotifyObject
{
    bool doing = false;
    public void UserChanging(TreeComboBox cb)
    {
        if (doing) return;
        doing = true;
        var i=SelectGroup.IndexOf(cb);
        while (SelectGroup.Last() != cb) SelectGroup.Remove(SelectGroup.Last());
        AutoMakeNext();
        Update("SelectGroup");
        mSelectValue.Clear();
        foreach(var  it in SelectGroup)
        {
            mSelectValue.Add(it.Node.Name);
        }
        OnSelectChanged?.Invoke(mSelectValue);
        doing = false;
    }
    public TreeNode Root
    {
        set
        {
            mRoot = value;
            Update("Root");

        }
        get { return mRoot; }
    }
    TreeNode mRoot;

    public void AddTreeComboBox(string name=null)
    {
        var item = new TreeComboBox() { group = this };
        
        if (SelectGroup.Count!=0)
        {
            item.parent = SelectGroup.Last();
        }
        item.Select(name);
        SelectGroup.Add(item);
    }
    [ItemsControl(null, Orientation.Horizontal)]
    public ObservableCollection<TreeComboBox> SelectGroup
    {
        get
        {
            if(mSelectGroup==null)
            {
                mSelectGroup = new ObservableCollection<TreeComboBox>();
            }
            return mSelectGroup;
        }
    }
    ObservableCollection<TreeComboBox> mSelectGroup;

    public Action<ObservableCollection<string>> OnSelectChanged;
    public ObservableCollection<string> SelectValue
    {
        set
        {
            if (mSelectValue == value) return;
            mSelectValue = value;
            doing = true;
            Update("SelectValue");
            SelectGroup.Clear();
            for (int i=0;i<mSelectValue.Count;i++)
            {
                AddTreeComboBox(mSelectValue[i]);
            }
            doing = false;
        }
        get
        {
            return mSelectValue;
        }
    }
    ObservableCollection<string> mSelectValue;


    public void AutoMakeNext()
    {
        while(!SelectGroup.Last().Node.IsLeaf())
        {
            AddTreeComboBox();
        }
    }
}

public class SelectTreeNodeAttribute : ControlPropertyAttribute
{
    public TreeComboBoxGroup ComboBoxGroup { get; private set; }

    public SelectTreeNodeAttribute(string name) : base(name)
    {
    }

    public override FrameworkElement CreateFrameworkElement()
    {
        ComboBoxGroup = new TreeComboBoxGroup();
        //由于wpf同一个集合数据的绑定,过滤会相互影响
        //所以克隆树来过滤操作
        ComboBoxGroup.Root = (TreeNode)Target;
        var panel=WinUtil.DrawPanel(ComboBoxGroup);
        return panel;
    }
}

