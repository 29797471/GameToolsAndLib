using CqCore;
using System.ComponentModel;
using UnityCore;
using UnityEngine;

namespace MVL
{
    /// <summary>
    /// 关联列表组件<para/>
    /// 上级组件绑定的数据对象的改变会导致子组件重新绑定数据对象
    /// </summary>
    [RequireComponent(typeof(IListMono))]
    public class LinkList : LinkParent
    {
        IListMono mListMono;
        IListMono ListMono
        {
            get
            {
                if (mListMono == null) mListMono = GetComponent<IListMono>();
                return mListMono;
            }
        }

        [TextBox("名称"), Visible("IsChild")]
        public string Name;

        public override string LocalPath
        {
            get
            {
                return "." + Name;
            }
        }

        protected override void OnPropertyChanged(string PropertyName)
        {
            if (PropertyName == Name)
            {
                UpdateProperty();
            }
        }
        protected override void UpdateProperty()
        {
            var data = AssemblyUtil.GetMemberValue(ParentNode.DataContent, Name);
            if (data is INotifyListChanged)
            {
                var listData = (INotifyListChanged)data;
                listData.ListChanged_CallBack(ListData_ListChanged, ClearBinding);
            }
            DataContent = data;

            if (data is ISetGetEnumerable)
            {
                var listData = (ISetGetEnumerable)data;
                ListMono.UpdateData = UpdateData;
                ListMono.DataCount = listData.Count;
            }
        }
        void UpdateData(GameObject obj, int index)
        {
            var child = obj.GetComponent<LinkItem>();
            child.index = index;
            child.LinkParent();
        }
        private void ListData_ListChanged(ListChangedType ListChangedType, int NewIndex,int OldIndex)
        {
            switch (ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        ListMono.Insert(NewIndex);
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    {
                        ListMono.RemoveAt(NewIndex);
                    }
                    break;
                case ListChangedType.Reset:
                    {
                        ListMono.DataCount = 0;
                    }
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                case ListChangedType.PropertyDescriptorChanged:
                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.ItemMoved:
                case ListChangedType.ItemChanged:
                    {
                        Debug.LogError(ListChangedType);
                    }
                    break;
            }
        }
    }
}