using System;

namespace CqCore
{
    /// <summary>
    /// 继承于这个接口的类,可以派发成员变更事件<para/>
    /// 外部系统可以监听成员变更
    /// </summary>
    public interface INotifyMemberChanged
    {
        //event PropertyChangedEventHandler PropertyChanged;
        void MemberChanged_CallBack(Action<string> OnMemberChangedCallBack, ICancelHandle handle);
    }
}
