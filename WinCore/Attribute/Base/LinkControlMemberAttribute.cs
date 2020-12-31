
using CqCore;
using System;
using System.Windows;

/// <summary>
/// 修饰一个成员 作为一个控件的一个属性定义
/// </summary>
public class LinkControlMemberAttribute : LinkMemberAttribute
{
    public AttributeTarget at;

    /// <summary>
    /// 链接的另一个数据改变
    /// </summary>
    protected void SetLinkPropertyChanged(FrameworkElement fe,Action<object> fun)
    {
        CancelHandle cancel=null;
        fe.Loaded += (obj, e) =>
        {
            if (cancel != null)
            {
                return;
            }
            cancel = new CancelHandle();
            PropertyChanged_CallBack(fun, cancel);
            
        };
        fe.Unloaded += (obj1, e1) =>
        {
            if (cancel == null) return;
            cancel.CancelAll();
            cancel = null;
        };
        //if(Parent is INotifyPropertyChanged) fun?.Invoke(Data);


        //callback = AddListener_LinkPropertyChanged(fun);

    }

    public LinkControlMemberAttribute(object data=null, AttributeTarget at =  AttributeTarget.Self) :base(data)
    {
        this.at = at;
    }
    public LinkControlMemberAttribute(string path, AttributeTarget at = 0,string convertMethod=null):base(path,convertMethod)
    {
        this.at = at;
    }

    public void Init(FrameworkElement fe)
    {
        OnInitTargetControl(fe);
        var p = fe;
        switch (at)
        {
            case AttributeTarget.Parent:
                p = (FrameworkElement)p.Parent;
                break;
            case AttributeTarget.Grandparent:
                p = (FrameworkElement)p.Parent;
                p = (FrameworkElement)p.Parent;
                break;
            case AttributeTarget.Window:

                break;
        }
        OnInit(p);
        //LinkPropertyChanged?.Invoke(Data);
    }
    public void Init(FrameworkElementFactory fef)
    {
        switch (at)
        {
            case AttributeTarget.Parent:
                fef = (FrameworkElementFactory)fef.Parent;
                break;
            case AttributeTarget.Grandparent:
                fef = (FrameworkElementFactory)fef.Parent;
                fef = (FrameworkElementFactory)fef.Parent;
                break;
        }
        OnInit(fef);
    }
    protected virtual void OnInitTargetControl(FrameworkElement fe)
    {
    }
    protected virtual void OnInit(FrameworkElement fe)
    {
    }
    protected virtual void OnInit(FrameworkElementFactory fe)
    {
    }

    protected virtual void SetBindingOrValue(FrameworkElement fe, DependencyProperty dp)
    {
        switch (at)
        {
            case AttributeTarget.Parent:
                fe = (FrameworkElement)fe.Parent;
                break;
            case AttributeTarget.Grandparent:
                fe = (FrameworkElement)fe.Parent;
                fe = (FrameworkElement)fe.Parent;
                break;
        }
        SetLinkPropertyChanged(fe, o => fe.SetValue(dp, o));
    }

    protected virtual void SetBindingOrValue(FrameworkElementFactory fef, DependencyProperty dp)
    {

        if (path == null) fef.SetValue(dp, Data);
        else fef.SetBinding(dp, new System.Windows.Data.Binding(path));
    }
}