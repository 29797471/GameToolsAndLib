using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 树形数据结构数据类,基类
    /// </summary>
    public abstract class ITreeDataNode<T> where T : ITreeDataNode<T>
    {
        public abstract string Name { get; set; }

        /// <summary>
        /// 对应的树节点
        /// </summary>
        public TreeNode<T> Node { get; internal set; }
    }

    /// <summary>
    /// 通用树形数据结构节点(参考Transform实现)
    /// </summary>
    public class TreeNode<T>: ICqSerialize where T : ITreeDataNode<T>
    {
        public string Name
        {
            get
            {
                if (Data == null) return null;
                return Data.Name;
            }
            set
            {
                if(Data!=null)
                {
                    Data.Name = value;
                }
            }
        }
        /// <summary>
        /// 挂在这个节点上的数据
        /// </summary>
        public T mData;
        /// <summary>
        /// 挂在这个节点上的数据
        /// </summary>
        public T Data
        {
            get
            {
                if (mData == null) return null;
                if (mData.Node == null) mData.Node = this;
                return mData;
            }
            set
            {
                mData = value;
                mData.Node = this;
            }
        }
        public List<TreeNode<T>> mChildren;
        TreeNode<T> mParent;
        public TreeNode<T> Parent { get { return mParent; } }
        public void OnDeserialize()
        {
            if (mChildren != null)
            {
                foreach (TreeNode<T> it in mChildren)
                {
                    it.mParent = this;
                }
            }
        }

        public void OnSerialize()
        {
        }
        /// <summary>
        /// 添加一个叶子节点
        /// </summary>
        public bool AddChildren(TreeNode<T> child) 
        {
            if (mChildren == null) mChildren = new List<TreeNode<T>>();
            if (!mChildren.Contains(child))
            {
                mChildren.Add(child);
                child.mParent = this;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除这个节点
        /// </summary>
        public bool Delete()
        {
            if (mParent.mChildren.Contains(this))
            {
                mParent.mChildren.Remove(this);
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
        public bool DeleteToParent()
        {
            var bl = Delete();
            if (bl && Parent.IsLeaf())
            {
                return Parent.DeleteToParent();
            }
            return bl;
        }
        #region 树的基本信息
        /// <summary>
        /// 返回树的叶子个数
        /// </summary>
        public int LeafCount()
        {
            int count = 0;
            if (mChildren == null) return 0;
            foreach (var child in mChildren)
            {
                if (child.IsLeaf())
                {
                    count++;
                }
                else
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
            return mParent == null;
        }

        /// <summary>
        /// 该节点的度,或者说子节点个数
        /// </summary>
        public int Degree()
        {
            if (mChildren == null) return 0;
            return mChildren.Count;
        }

        /// <summary>
        /// 叶子节点和分支节点的总数
        /// </summary>
        public int Size()
        {
            if (IsLeaf()) return 0;
            int count = 0;
            foreach (var child in mChildren)
            {
                count += child.Size() + 1;
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
            foreach (var child in mChildren)
            {
                height = Math.Max(height, child.Height() + 1);
            }
            return height;
        }
        #endregion
        #region 树的遍历和查找
        /// <summary>
        /// 先序遍历(深度优先遍历Depth-first Traversal的一种)
        /// </summary>
        public void PreorderTraversal(Action<TreeNode<T>> OnTraversal)
        {
            OnTraversal(this);
            if(mChildren!=null)
            {
                foreach (var child in mChildren)
                {
                    child.PreorderTraversal(OnTraversal);
                }
            }
        }

        /// <summary>
        /// 先序遍历查找
        /// 返回true时找到目标
        /// 终止遍历
        /// </summary>
        public TreeNode<T> FindByPreorder(Predicate<TreeNode<T>> match)
        {
            if (match(this)) return this;
            if(mChildren!=null)
            {
                foreach (var child in mChildren)
                {
                    var result = child.FindByPreorder(match);
                    if (result != null) return result;
                }
            }
            
            return null;
        }
        #endregion
    }
}
