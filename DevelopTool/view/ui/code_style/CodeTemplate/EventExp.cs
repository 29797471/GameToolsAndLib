using CqEvent;
using DevelopTool;
using System.Collections.ObjectModel;

/// <summary>
/// 选中的事件
/// </summary>
[Editor("事件")]
public class EventExp:NotifyObject,IExportCode
{

    [SelectTreeNode("事件"),SelectedValue("Chooses")]
    public TreeNode Root
    {
        get
        {
            return EventModel.instance.Root;
        }
    }
    /// <summary>
    /// 事件分组列表
    /// </summary>
    public ObservableCollection<string> Chooses
    {
        get
        {
            //ChangeData();
            if (mChooses == null) mChooses = Root.FindByPreorder(x => x.IsLeaf()).GetPath();
            return mChooses;
        }
        set
        {
            mChooses = value;
            Update("Chooses");
        }
    }
    public ObservableCollection<string> mChooses;

    /// <summary>
    /// 事件
    /// </summary>
    public EventNode Struct
    {
        get
        {
            var n=Root[Chooses];
            if (n == null) throw new System.Exception("找不到事件:" + Torsion.Serialize(Chooses));
            return n.nodeObj as EventNode;
        }
    }

    /// <summary>
    /// 对应的代码
    /// </summary>
    public string ExecContent { get { return Struct.ExecContent; } }

    public string Content
    {
        get
        {
            return Struct.Name;
        }
    }
    public string eventName;
    //void ChangeData()
    //{
    //    if (!once)
    //    {
    //        once = true;
    //        if (eventName != null)
    //        {
    //            var node = Root.FindByPreorder(x => x.Name == eventName);
    //            Chooses = node.GetPath();

    //            eventName = null;
    //        }
    //    }
    //}
    //private bool once;

    /// <summary>
    /// 表达式或者值关联此事件
    /// </summary>
    public void LinkTo(IExpression it)
    {
        if (it is ExprCodeTemplate)
        {
            var k = it as ExprCodeTemplate;
            k.Ee = this;
        }
        else if (it is EventArgCodeTemplate)
        {
            var k = it as EventArgCodeTemplate;
            k.Ee = this;
        }
        else if (it is EventArgCodeTemplate)
        {
            var k = it as EventArgCodeTemplate;
            k.Ee = this;
        }
    }
}