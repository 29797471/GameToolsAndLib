//using CqCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using UnityCore;
//using UnityEngine;

//namespace MVL
//{
//    /// <summary>
//    /// 上级组件绑定的数据对象的改变会导致子组件重新绑定数据对象
//    /// </summary>
//    public class LinkList : LinkParent
//    {
//        [TextBox("名称"), Visible("IsChild")]
//        public string Name;

//        public override string LocalPath
//        {
//            get
//            {
//                return "." + Name;
//            }
//        }

//        public LinkItem clone;

//        public List<LinkItem> CloneList { get; private set; }

//        protected override void OnPropertyChanged(string PropertyName)
//        {
//            if (PropertyName == Name)
//            {
//                UpdateProperty();
//            }
//        }
//        protected override void UpdateProperty()
//        {
//            clone.gameObject.SetActive(false);
//            if (CloneList != null)
//            {
//                Clear();
//                CloneList = null;
//            }
//            CloneList = new List<LinkItem>();
//            var data = AssemblyUtil.GetMemberValue(ParentNode.DataContent, Name);
//            if (data is INotifyListChanged)
//            {
//                var listData = (INotifyListChanged)data;
//                listData.AddListener_ListChanged(ListData_ListChanged, ClearBinding);
//            }
//            DataContent = data;

//            if (data is ISetGetEnumerable)
//            {
//                var listData = (ISetGetEnumerable)data;
//                for (int i = 0; i < listData.Count; i++)
//                {
//                    Insert(i);
//                }
//            }
//        }
//        void Clear()
//        {
//            foreach (var it in CloneList)
//            {
//                GameObject.Destroy(it.gameObject);
//            }
//            CloneList.Clear();
//        }
//        void Insert(int index)
//        {
//            var obj = clone.gameObject.Clone(string.Format("{0}_{1}", clone.name, index));
//            obj.SetActive(true);
//            obj.transform.SetSiblingIndex(index + 1);
//            var child = obj.GetComponent<LinkItem>();
//            CloneList.Insert(index, child);
//            child.LinkParent();
//        }
//        private void ListData_ListChanged(ListChangedType ListChangedType, int NewIndex, int OldIndex)
//        {
//            switch (ListChangedType)
//            {
//                case ListChangedType.ItemAdded:
//                    {
//                        Insert(NewIndex);
//                    }
//                    break;
//                case ListChangedType.ItemDeleted:
//                    {
//                        var item = CloneList[NewIndex];
//                        CloneList.RemoveAt(NewIndex);
//                        GameObject.Destroy(item.gameObject);
//                    }
//                    break;
//                case ListChangedType.Reset:
//                    {
//                        Clear();
//                    }
//                    break;
//                case ListChangedType.PropertyDescriptorDeleted:
//                case ListChangedType.PropertyDescriptorChanged:
//                case ListChangedType.PropertyDescriptorAdded:
//                case ListChangedType.ItemMoved:
//                case ListChangedType.ItemChanged:
//                    {
//                        Debug.LogError(ListChangedType);
//                    }
//                    break;
//            }
//        }
//    }
//}
    
