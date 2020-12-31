using CqCore;
namespace MVL
{
    /// <summary>
    /// 上级组件绑定的数据对象的改变会导致子组件重新绑定数据对象
    /// </summary>
    public abstract class LinkChild : LinkBase
    {
        CancelHandle mClearBinding;
        protected CancelHandle ClearBinding
        {
            get
            {
                if(mClearBinding==null)
                {
                    mClearBinding = new CancelHandle();
                    DestroyHandle.CancelAct += ClearBinding.CancelAll;
                }
                return mClearBinding;
            }
        }

        /// <summary>
        /// 该组件重新从父对象监听它关注的属性数据对象
        /// </summary>
        public void LinkParent()
        {
            //Debug.Log(this.PathInHierarchy() + "::" + FullPath);
            ClearBinding.CancelAll();
            OnLinkParent();
            UpdateProperty();
            OnLink();
        }
        protected virtual void UpdateProperty()
        {
            
        }
        protected virtual void OnLink()
        {

        }
        protected virtual void OnLinkParent()
        {
            AddPropertyChanged();
        }
        protected void AddPropertyChanged()
        {
            if (ParentNode.DataContent is INotifyMemberChanged)
            {
                var parentData = (INotifyMemberChanged)ParentNode.DataContent;
                parentData.MemberChanged_CallBack(DataContent_PropertyChanged, ClearBinding);
            }
        }

        private void DataContent_PropertyChanged(string member)
        {
            OnPropertyChanged(member);
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {

        }
    }

}
