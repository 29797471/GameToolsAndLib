using System;
using System.Linq;

public class BaseTreeNotifyObject : NotifyObject
{
    public override string ToString()
    {
        return Name;
    }

    [TextBox("名称", 100),MinWidth(100)]
    public string Name
    {
        get
        {
            if (mName == null)
            {
                var attr=AssemblyUtil.GetClassAttribute<EditorAttribute>(this);
                if (attr != null) mName = attr.name;
                else mName = GetType().Name;
            }
            return mName;
        }
        set { mName = value; Update("Name"); UpdateName(); }
    }
    public string mName;


    TreeNode mNode;
    public TreeNode Node
    {
        get
        {
            if (mNode == null) mNode = new TreeNode() { nodeObj = this };
            return mNode;
        }
        set
        {
            mNode = value;
        }
    }
    /// <summary>
    /// 上一级
    /// </summary>
    public BaseTreeNotifyObject Parent
    {
        get
        {
            return Node.Parent.nodeObj;
        }
    }

    /// <summary>
    /// 根
    /// </summary>
    public BaseTreeNotifyObject Root
    {
        get
        {
            return Node.Root.nodeObj;
        }
    }

    public void UpdateName()
    {
        Node.UpdateName();
    }

    /// <summary>
    /// 遍历该父节点下子节点
    /// </summary>
    public void ForEach(Action<BaseTreeNotifyObject> OnForEach)
    {
        OnForEach(this);
        Node.Children.ToList().ForEach(obj=>OnForEach(obj.nodeObj));
    }

    /// <summary>
    /// 先序遍历
    /// </summary>
    public void PreOrderTraversal(Action<BaseTreeNotifyObject> OnTraversal)
    {
        Node.PreorderTraversal(obj => OnTraversal(obj.nodeObj));
    }

    /// <summary>
    /// 先序遍历查找
    /// 返回true时找到目标
    /// 终止遍历
    /// </summary>
    public BaseTreeNotifyObject FindByPreorder(Predicate<BaseTreeNotifyObject> match)
    {
        return Node.FindByPreorder(x => match(x.nodeObj)).nodeObj;
    }

    /// <summary>
    /// 逐层遍历叶子节点
    /// </summary>
    public void LevelOrderTraversal(Action<BaseTreeNotifyObject> OnTraversal)
    {
        Node.LevelOrderTraversal(obj => OnTraversal(obj.nodeObj));
    }
    public void AddChildren(BaseTreeNotifyObject child)
    {
        Node.AddChildren(child.Node);
    }
    public void InsertNext(BaseTreeNotifyObject child)
    {
        Node.InsertNext(child.Node);
    }
    public bool Delete()
    {
        return Node.Delete(true);
    }

    public T Find<T>(Predicate<T> match)where T:BaseTreeNotifyObject
    {
        var result = Node.FindByPreorder((x)=> { return match(x.nodeObj as T); });
        if (result == null) return null;
        return result.nodeObj as T;
    }
}