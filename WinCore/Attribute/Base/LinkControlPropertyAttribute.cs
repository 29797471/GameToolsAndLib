using CqCore;
using System;
using System.ComponentModel;
using System.Windows;


/// <summary>
/// 修饰一个成员属性 作为一个控件的一个属性定义
/// </summary>
public class LinkControlPropertyAttribute : LinkPropertyAttribute
{
    protected ControlPropertyAttribute controlAttr;

    /// <summary>
    /// 它所修饰的数据改变
    /// </summary>
    protected void SetPropertyChanged(FrameworkElement fe, Action fun)
    {
        CancelHandle cancelHandle = new CancelHandle();
        fe.Loaded += (obj, e) => PropertyChanged_CallBack(fun, cancelHandle);
        fe.Unloaded += (obj, e) => cancelHandle.CancelAll();
    }

    /// <summary>
    /// 链接的另一个数据改变
    /// 当控件移除时移除链接
    /// </summary>
    protected void SetLinkPropertyChanged(FrameworkElement fe,Action<object> fun)
    {
        CancelHandle cancel=null;
        fe.Loaded += (obj, e) =>
        {
            if (cancel != null) return;
            cancel = new CancelHandle();
            LinkPropertyChanged_CallBack(fun, cancel);
        };
        //if(Parent is INotifyPropertyChanged)fun?.Invoke(Data);
        fe.Unloaded += (obj1, e1) =>
        {
            if (cancel == null) return;
            cancel.CancelAll();
            cancel = null;
        };
        
    }

    public LinkControlPropertyAttribute(object data=null):base(data)
    {
    }
    public LinkControlPropertyAttribute(string path,string convertMethod=null, object arg0=null):base(path, convertMethod,arg0)
    {
    }

    public void Init(FrameworkElement fe,ControlPropertyAttribute cpa)
    {
        controlAttr = cpa;
        OnInit(fe);
        //LinkPropertyChanged?.Invoke(Data);
    }
    public void Init(FrameworkElementFactory fef,ControlPropertyAttribute cpa)
    {
        controlAttr = cpa;
        OnInit(fef);
    }
    protected virtual void OnInit(FrameworkElement fe)
    {
    }
    protected virtual void OnInit(FrameworkElementFactory fe)
    {
    }

    protected virtual void SetBindingOrValue(FrameworkElement fe, DependencyProperty dp)
    {
        SetLinkPropertyChanged(fe, (o) => { fe.SetValue(dp, o); });
    }
}