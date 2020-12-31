using UnityCore;

namespace MVL
{
    /// <summary>
    /// 上级组件绑定的数据对象的改变会导致子组件重新绑定数据对象
    /// </summary>
    public class LinkObject : LinkParent
    {
        [TextBox("名称"), Visible("IsChild")]
        public string Name;

        public override string LocalPath
        {
            get
            {
                return "." + Name;
            }
        }
        protected override void UpdateProperty()
        {
            DataContent = AssemblyUtil.GetMemberValue(ParentNode.DataContent, Name);
        }
        protected override void OnPropertyChanged(string PropertyName)
        {
            if (PropertyName == Name)
            {
                UpdateProperty();
            }
        }
    }
}

