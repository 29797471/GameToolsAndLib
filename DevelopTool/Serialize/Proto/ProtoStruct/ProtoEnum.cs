using System.Collections.ObjectModel;

namespace Proto
{

    [System.Serializable]
    [MenuItemPath("添加/枚举")]
    [Editor]
    public class ProtoEnum : BaseTreeNotifyObject, IProtoNode
    {
        [PriorityAttribute(0)]
        [TextBox("注释"),MinWidth(100)]
        public string Comment { get { return mComment; } set { mComment = value; Update("Comment"); } }
        string mComment;

        [PriorityAttribute(1)]
        [ListView()]
        public ObservableCollection<EnumChild> Childs
        {
            get {if(mChilds==null) mChilds = new ObservableCollection<EnumChild>(); return mChilds; }
            set { mChilds = value; Update("Childs"); }
        }
        public ObservableCollection<EnumChild> mChilds;
    }
}