using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using WinCore;

/// <summary>
/// 界面表现相关的树节点
/// </summary>
//public class TreeNode: TreeNodeData
//{
    
//}
[Editor]
public class TreeNode : NotifyObject, ICqSerialize
{
    [MenuItem("添加", "/WinCore;component/Res/add.ico"), Priority(1)]
    public void AddItem(TreeNode SelectedNode/*, Type type*/)
    {
        //if (SelectedNode == null) SelectedNode = this;
        //var newNode = AssemblyUtil.CreateInstance(type) as BaseTreeNotifyObject;
        //SelectedNode.AddChildren(newNode.Node);
    }

    [MenuItem("删除(Del)", "/WinCore;component/Res/delete.ico", Key.Delete), Priority(2)]
    public void DelItem(TreeNode obj)
    {
        if (obj != null)
        {
            obj.Delete();
        }
    }

    [MenuItem("复制(Ctrl+C)", "/WinCore;component/Res/copy.ico", Key.C, ModifierKeys.Control), Priority(3)]
    public void CopyItem(object obj)
    {
        ClipboardUtil.CopyObject(obj);
    }

    [MenuItem("粘贴(Ctrl+V)", "/WinCore;component/Res/paste.ico", Key.V, ModifierKeys.Control), Priority(4)]
    public void PasteItem(TreeNode selectNode)
    {
        var clone = ClipboardUtil.PasteObject<TreeNode>();
        if (clone != null)
        {
            if (selectNode == null || selectNode.ToString() == "") Root.AddChildren(clone);
            else selectNode.InsertNext(clone);
        }
    }

    [MenuItem("信息(Ctrl+I)", "/WinCore;component/Res/info.ico", Key.I, ModifierKeys.Control), Priority(5)]
    public void button_info_Click(TreeNode SelectedNode)
    {
        if (SelectedNode == null) SelectedNode = this;
        var outStr = "高度:" + SelectedNode.Height() + "\n";
        outStr += "节点总数:" + SelectedNode.Size() + "\n";
        outStr += "叶子个数:" + SelectedNode.LeafCount() + "\n";
        outStr += "子节点数:" + SelectedNode.Children.Count+"\n";

        int c = 0;
        Root.PreorderTraversal(x => { if (x.Visibility == Visibility.Visible) c++; });
        outStr += "过滤后显示的节点数:" + (c-1);
        CustomMessageBox.Show(outStr);
    }


    #region 树显示相关的临时数据(折叠,显示)
    public Visibility IconVisibility
    {
        get { return mIconVisibility; }
        set { mIconVisibility = value; Update("IconVisibility"); }
    }
    Visibility mIconVisibility = Visibility.Collapsed;

    public string Icon { get { return mIcon; } set { mIcon = value; Update("Icon"); } }
    string mIcon = "/WinCore;component/Res/tree/play.ico";
    /// <summary>
    /// 是否展开(该界面数据不需要保存)
    /// </summary>
    public bool IsExpanded
    {
        get { return mIsExpanded; }
        set { mIsExpanded = value; Update("IsExpanded"); }
    }
    bool mIsExpanded;

    public Visibility Visibility
    {
        get { return mVisibility; }
        set { mVisibility = value; Update("Visibility"); }
    }
    Visibility mVisibility = Visibility.Visible;
    #endregion

    public TreeNode this[string childName]
    {
        get
        {
            return Children.ToList().Find(x => x.Name == childName);
        }
    }

    /// <summary>
    /// 通过树路径定位节点
    /// </summary>
    /// <param name="deepChildName"></param>
    /// <returns></returns>
    public TreeNode this[ObservableCollection<string> childPath]
    {
        get
        {
            TreeNode target = this;
            for(int i=0;i< childPath.Count;i++)
            {
                target= target[childPath[i]];
                if (target == null) return null;
            }
            return target;
        }
    }

    /// <summary>
    /// 获取该节点在树下的路径
    /// </summary>
    public ObservableCollection<string> GetPath()
    {
        if(this.IsRoot())
        {
            return new ObservableCollection<string>();
        }
        else
        {
            var list = Parent.GetPath();
            list.Add(Name);
            return list;
        }
    }


    

    public TreeNode Parent { get { return parent; } }
    TreeNode parent;

    public BaseTreeNotifyObject nodeObj;
    public ObservableCollection<TreeNode> Children
    {
        get { if (children == null) children = new ObservableCollection<TreeNode>(); return children; }
        set { children = value; Update("Children"); }
    }
    public ObservableCollection<TreeNode> children;
    public string Name
    {
        get
        {
            if (nodeObj == null) return "null";
            return nodeObj.Name;
        }
    }
    public override string ToString()
    {
        if (nodeObj == null) return "null";
        return nodeObj.ToString();
    }
    public string ToolTip
    {
        get
        {
            return null;
        }
    }
    public TreeNode Root
    {
        get
        {
            if (parent == null) return this;
            else return parent.Root;
        }
    }
    public void UpdateName()
    {
        Update("Name");
    }
    /// <summary>
    /// 建立父子关系
    /// </summary>
    public void InitRelation()
    {
        if (nodeObj != null) nodeObj.Node = this;
        if (children != null)
        {
            foreach (TreeNode it in children)
            {
                it.parent = this;
                it.InitRelation();
            }
        }
    }

    public bool AddChildren(TreeNode child)
    {
        if (!Children.Contains(child))
        {
            Children.Add(child);
            child.parent = this;
            IsExpanded = true;
            EventMgr.MsgPrint.Notify("添加:" + child.ToString(), 5);
            return true;
        }
        return false;
    }
    public int Index
    {
        get 
        {
            if (parent == null) return -1;
            return parent.Children.IndexOf(this); 
        }
    }
    public void InsertNext(TreeNode node)
    {
        parent.Children.Insert(Index+1, node);
        node.parent = parent;
    }
    public bool Delete(bool print=false)
    {
        if (Parent.Children.Contains(this))
        {
            Parent.Children.Remove(this);
            if(print)EventMgr.MsgPrint.Notify("删除:" + this.ToString(), 5);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 删除这个节点,并且删除后如果父节点没有孩子了,也要一并删除
    /// </summary>
    public bool DeleteToParent(bool print = false)
    {
        var bl = Delete(print);
        if(bl && Parent.IsLeaf())
        {
            return Parent.DeleteToParent(print);
        }
        return bl;
    }

    /// <summary>
    /// 先序遍历查找
    /// 返回true时找到目标
    /// 终止遍历
    /// </summary>
    public TreeNode FindByPreorder(Predicate<TreeNode> match) 
    {
        if (match(this)) return this;
        foreach (var child in Children)
        {
            var result=child.FindByPreorder(match);
            if (result!=null) return result;
        }
        return null;
    }

    /// <summary>
    /// 先序遍历(深度优先遍历Depth-first Traversal的一种)
    /// </summary>
    public void PreorderTraversal(Action<TreeNode> OnTraversal)
    {
        OnTraversal(this);
        foreach (var child in Children)
        {
            child.PreorderTraversal(OnTraversal);
        }
    }

    /// <summary>
    /// 先序遍历(深度优先遍历Depth-first Traversal的一种)
    /// </summary>
    public System.Collections.IEnumerator PreorderTraversalByCoroutine(Action<TreeNode> OnTraversal)
    {
        OnTraversal(this);
        foreach (var child in Children)
        {
            child.PreorderTraversal(OnTraversal);
            yield return null;
        }
    }

    /// <summary>
    /// 先序遍历叶子(深度优先遍历Depth-first Traversal的一种)
    /// </summary>
    public void PreorderTraversalLeaf(Action<TreeNode> OnTraversal)
    {
        if(IsLeaf())OnTraversal(this);
        else
        {
            foreach (var child in Children)
            {
                child.PreorderTraversalLeaf(OnTraversal);
            }
        }
        
    }

    /// <summary>
    /// 后序遍历(深度优先遍历Depth-first Traversal的一种)
    /// </summary>
    public void PostorderTraversal(Action<TreeNode> OnTraversal)
    {
        foreach (var child in Children)
        {
            child.PostorderTraversal(OnTraversal);
        }
        OnTraversal(this);
    }
    /// <summary>
    /// 逐层遍历(广度优先遍历Breadth-first traversal)
    /// </summary>
    public void LevelOrderTraversal(Action<TreeNode> OnTraversal)
    {
        List<TreeNode> traversalList = new List<TreeNode>() { this};
        
        while (true)
        {
            List<TreeNode> nextLayerList = new List<TreeNode>();
            
            foreach (var child in traversalList)
            {
                OnTraversal(child);
                if (!child.IsLeaf()) nextLayerList.AddRange(child.Children);
            }
            
            if (nextLayerList.Count == 0) break;
            traversalList = nextLayerList;
        }
    }
    
    /// <summary>
    /// 返回树的叶子个数
    /// </summary>
    public int LeafCount()
    {
        int count = 0;
        foreach (var child in Children)
        {
            if (child.IsLeaf())
            {
                count++;
            }else
            {
                count += child.LeafCount();
            }
        }
        return count;
    }
    /// <summary>
    /// 深度
    /// </summary>
    public int Layer
    {
        get
        {
            if (Parent == null) return 0;
            return Parent.Layer + 1;
        }
    }
    /// <summary>
    /// 该节点是否是叶子
    /// </summary>
    public bool IsLeaf()
    {
        return Degree() == 0;
    }

    /// <summary>
    /// 该节点是否是根节点
    /// </summary>
    public bool IsRoot()
    {
        return parent == null;
    }

    /// <summary>
    /// 该节点的度,或者说子节点个数
    /// </summary>
    public int Degree()
    {
        if (children == null) return 0;
        return children.Count;
    }

    /// <summary>
    /// 叶子节点和分支节点的总数
    /// </summary>
    public int Size()
    {
        if (IsLeaf()) return 0;
        int count = 0;
        foreach (var child in Children)
        {
            count += child.Size()+1;
        }
        return count;
    }
    /// <summary>
    /// 返回树/节点的高度(深度)
    /// </summary>
    public int Height()
    {
        if (IsLeaf()) return 0;
        int height = 0;
        foreach (var child in Children)
        {
            height =Math.Max(height, child.Height()+1);
        }
        return height;
    }

    public void OnDeserialize()
    {
        InitRelation();
    }

    public void OnSerialize()
    {
    }

    /// <summary>
    /// 过滤显示， 当叶子节点满足条件时，它和它的父节点都会被显示出来,并展开
    /// 当一个非叶子节点满足条件时,它和它的所有子节点都会被显示出来,并展开
    /// </summary>
    public Predicate<TreeNode> Filter
    {
        set
        {
            if(value==null)
            {
                PreorderTraversal(x =>
                {
                    x.IsExpanded = false;
                    x.Visibility = Visibility.Visible;
                });
            }
            else
            {
                PreorderTraversal(x =>
                {
                    x.IsExpanded = false;
                    x.Visibility = Visibility.Collapsed;
                });
                PreorderTraversal(x =>
                {
                    if (value(x))
                    {
                        x.Visibility = Visibility.Visible;
                        x.IsExpanded = true;
                        var parent = x.Parent;
                        while (parent != null)
                        {
                            parent.Visibility = Visibility.Visible;
                            parent.IsExpanded = true;
                            parent = parent.Parent;
                        }
                        x.PreorderTraversal(y =>
                        {
                            y.Visibility = Visibility.Visible;
                            y.IsExpanded = true;
                        });
                    }
                });
            }
        }
    }
}
