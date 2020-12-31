using System;
using System.ComponentModel;

namespace CqCore
{
    /// <summary>
    /// 实现这个接口的类,可以派列表改变通知
    /// </summary>
    public interface INotifyListChanged
    {
        //event ListChangedEventHandler ListChanged;
        void ListChanged_CallBack(Action<ListChangedType , int , int > OnListChangedCallBack, ICancelHandle handle);
    }
}
