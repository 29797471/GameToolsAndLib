using DevelopTool;
using System.Collections.ObjectModel;

[Editor("指引触发"), Width(550), Height(400)]
public class GuideTrigger : NotifyObject
{
    public override string ToString()
    {
        return Name;
    }

    [TextBox("名称:"), Priority(0)]
    public string Name
    {
        get { if (mName == null) mName = "指引触发"; return mName; }
        set { mName = value; Update("Name"); }
    }
    public string mName;

    public ObservableCollection<string> Chooses
    {
        get
        {
            return NewEvent.Chooses;
        }
        set
        {
            NewEvent.Chooses = value;
            Update("Chooses");
        }
    }

    [Priority(3)]
    [SelectTreeNode("事件"), SelectedValue("Chooses")]
    public TreeNode EventRoot
    {
        get
        {
            return EventModel.instance.Root;
        }
    }

    [Export("%Event%")]
    public EventExp NewEvent
    {
        get { if (mNewEvent == null) mNewEvent = new EventExp(); return mNewEvent; }
        set { mNewEvent = value; Update("NewEvent"); }
    }
    public EventExp mNewEvent;


    [Priority(5)]
    [Export("%Con%", 3)]
    [UnderLine("条件", 100), Width(350), ClickToEditCode("NewEvent")]
    public IExpression ConExec
    {
        get { if (mConExec == null) mConExec = CodeStyleNewModel.instance.DefaultValue("bool"); return mConExec; }
        set { mConExec = value; Update("ConExec"); }
    }
    public IExpression mConExec;
}

