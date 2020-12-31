using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCore
{
    public class MemoryDataNode : ITreeDataNode<MemoryDataNode>
    {
        public long size;
        public int instanceID;
        public string mName;

        public bool markAdd;
        public bool markRemove;


        public override string Name
        {
            get => mName;
            set => mName = value;
        }

        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject MakeGameObject(int tabNum, int spaceNum,bool deltaFormat=false)
        {
            var obj = new GameObject();
            var layerName = Name;
            if (!Node.IsLeaf())
            {
                layerName=string.Format("{0}({1})", Name, Node.LeafCount());
            }
            if (markAdd)
            {
                layerName = "+++ " + layerName;
                deltaFormat = false;
            }
            else if (markRemove)
            {
                layerName = "--- " + layerName;
                deltaFormat = false;
            }
            obj.name = FontUtil.FillTbl(layerName, tabNum) + ' '.Repeat(spaceNum * 3) + StringUtil.FormatBytes(size, deltaFormat);
            if (Node.mChildren != null)
            {
                spaceNum--;
                Node.mChildren.Sort(x => -x.Data.size);
                foreach (var it in Node.mChildren)
                {
                    var itObj = it.Data.MakeGameObject(tabNum, spaceNum, deltaFormat);
                    itObj.transform.SetParent(obj.transform);
                }
                spaceNum++;
            }
            return obj;
        }

        /// <summary>
        /// 生成比较对象
        /// </summary>
        public static MemoryDataNode operator -(MemoryDataNode a,MemoryDataNode b)
        {
            if(a.instanceID!=b.instanceID)
            {
                return null;
            }
            var deltaSize = a.size - b.size;
            if (deltaSize == 0) return null;
            var node = new TreeNode<MemoryDataNode>();
            node.Data = new MemoryDataNode()
            {
                size = deltaSize,
                mName =  a.Name ,
            };
            var aChildren = a.Node.mChildren;
            if(aChildren==null)
            {
                aChildren = new List<TreeNode<MemoryDataNode>>();
            }
            
            var bChildren = b.Node.mChildren;
            if (bChildren == null) bChildren = new List<TreeNode<MemoryDataNode>>();
            else bChildren = bChildren.ToList();//浅表克隆(插入删除不改原表)

            for (int i=0;i< aChildren.Count;i++)
            {
                var itA = aChildren[i];
                var itB = bChildren.Find(x => x.Data.instanceID == itA.Data.instanceID);
                
                if(itB!=null)
                {
                    bChildren.Remove(itB);
                    var xx = itA.Data - itB.Data;
                    if (xx!=null)
                    {
                        node.AddChildren(xx.Node);
                    }
                }
                else
                {
                    var itAdd = Torsion.Clone(itA);
                    itAdd.PreorderTraversal(x => x.Data.markAdd = true);
                    node.AddChildren(itAdd);
                }
            }
            for(int i=0;i<bChildren.Count;i++)
            {
                var it = bChildren[i];
                var itRemove = Torsion.Clone(it);
                itRemove.PreorderTraversal(x => x.Data.markRemove = true);
                node.AddChildren(itRemove);
            }
            return node.Data;
        }
    }
}
