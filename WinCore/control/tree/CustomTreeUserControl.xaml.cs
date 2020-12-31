using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinCore
{
    /// <summary>
    /// CustomTree.xaml 的交互逻辑
    /// </summary>
    public partial class CustomTreeUserControl : UserControl
    {
        public IEnumerable<Type> Types
        {
            set
            {
                mTypes = value;
                foreach (var type in mTypes)
                {
                    var attr = AssemblyUtil.GetMemberAttribute<MenuItemPathAttribute>(type);
                    if (attr != null)
                    {
                        var tooltip = AssemblyUtil.GetMemberAttribute<ToolTipAttribute>(type);
                        attr.AddMenuItem(this, () =>
                        {
                            var SelectedNode = (TreeNode)(treeView.SelectedItem == null ? Root : treeView.SelectedItem);
                            var newNode = AssemblyUtil.CreateInstance(type) as BaseTreeNotifyObject;
                            SelectedNode.AddChildren(newNode.Node);
                        }, tooltip!=null? tooltip.DataValue : null);
                    }
                }
            }
        }
        IEnumerable<Type> mTypes;

        public TreeNode Root
        {
            get { return DataContext as TreeNode; }
        }

        public TreeNode SelectedValue
        {
            get
            {
                return treeView.SelectedValue as TreeNode;
            }
        }


        /// <summary>
        /// 未实现
        /// </summary>
        public Predicate<TreeNode> Filter
        {
            set
            {
                if(Root!=null)Root.Filter = value;
                //tree.Filter = value;恶心的API
            }
        }

        /// <summary>
        /// 过滤显示，当子节点被显示时，父节点也会显示出来,并展开
        /// 目前只能过滤顶层,主要Items是作用在顶层目录树的,
        /// </summary>
        //public Predicate<TreeNode> Filter
        //{
        //    set
        //    {
        //        Root.PreorderTraversal(x => x.IsExpanded = true);
        //        if (value == null)
        //        {
        //            SetFilter(treeView, null);
        //        }
        //        else
        //        {
        //            SetFilter(treeView, (obj) =>
        //            (obj as TreeNode).FindByPreorder(x => value(x)) != null);
        //        }
        //    }
        //}

        /// <summary>
        /// 递归匹配过滤
        /// 在TreeView中遍历子节点的时候，未展开的节点通过ItemContainerGenerator.ContainerFromIndex是得不到的(并且貌似有时机问题,极其难用)
        /// </summary>
        public void SetFilter(ItemsControl ic, Predicate<object> MatchFilter)
        {
            ic.Items.Filter = MatchFilter;
            for (int i = 0; i < ic.Items.Count; i++)
            {
                var cc= ic.ItemContainerGenerator.ContainerFromIndex(i);
                ItemsControl d = (ItemsControl)ic.ItemContainerGenerator.ContainerFromIndex(i);
                if(d!=null) SetFilter(d,MatchFilter);
            }
        }
        public CustomTreeUserControl()
        {
            InitializeComponent();
        }

        
        public event RoutedPropertyChangedEventHandler<object> SelectedChanged
        {
            add
            {
                treeView.SelectedItemChanged += value;
            }
            remove
            {
                treeView.SelectedItemChanged -= value;
            }
        }
        

        /// <summary>
        /// 点击空白处,清除选中
        /// </summary>
        private void treeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid && treeView.SelectedItem != null)
            {
                var node = treeView.SelectedItem as TreeNode;
                var list = new Stack<int>();
                do
                {
                    list.Push(node.Index);
                    node = node.Parent;
                } while (node.Parent != null);
                ItemsControl b = treeView;
                while (list.Count > 0)
                {
                    b = b.ItemContainerGenerator.ContainerFromIndex(list.Pop()) as ItemsControl;
                    if (b == null) return;
                }
               (b as TreeViewItem).IsSelected = false;
            }
        }

    }
}
