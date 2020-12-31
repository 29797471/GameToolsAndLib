using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WinCore;


/// <summary>
/// 修饰一个属性,这个属性一般对应一个控件,
/// 如果有创建控件所必要的参数，在构造时传入
/// 这个类通常为控件的某个属性提供数据或者依赖变量
/// </summary>
public class ControlPropertyAttribute : LinkPropertyAttribute, IControlAttriubte
{
    public string name;
    public float width;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">显示名称</param>
    /// <param name="priority">在编辑面板中的显示优先级</param>
    public ControlPropertyAttribute(string name = null, float width = 0):base(null)
    {
        this.name = name;
        this.width = width;
    }

    /// <summary>
    /// 链接的另一个数据改变
    /// </summary>
    protected void SetPropertyChanged(FrameworkElement fe,Action fun)
    {
        CancelHandle handle = new CancelHandle();
        fe.Loaded += (obj, e) =>PropertyChanged_CallBack(fun,handle);
        fe.Unloaded += (obj, e) => handle.CancelAll();
    }

    protected void SetPropertyChanged(FrameworkElementFactory fef, Action fun)
    {
        CancelHandle handle = new CancelHandle();
        fef.AddHandler(FrameworkElement.LoadedEvent, (RoutedEventHandler)((obj, e) =>PropertyChanged_CallBack(fun, handle)));
        fef.AddHandler(FrameworkElement.UnloadedEvent, (RoutedEventHandler)((obj, e) => handle.CancelAll()));
    }

    protected Panel CreatePanel( float delta = 5)
    {
        var panel = new StackPanel();
        panel.Orientation = Orientation.Horizontal;
        panel.Margin = new Thickness(0, delta, 0, 0);

        //SetBindings(panel, attrs, AttributeTarget.Parent);
        return panel;
    }

    /// <summary>
    /// 根据一个类成员的多个特性属性创建控件
    /// </summary>
    public virtual void AddControl(Panel parent)
    {
        var panel = CreatePanel();
        
        parent.Children.Add(panel);

        if (!string.IsNullOrEmpty(name))
        {
            var label = new Label()
            {
                Content = name,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            if (width != 0) label.Width = width;
            panel.Children.Add(label);
        }

        var ctl=CreateFrameworkElement();
        //SetBindings(ctl, attrs);
        if(ctl!=null)panel.Children.Add(ctl);
        
        SetLinkControlProperty(ctl);

        //刷新绑定的对象
        //TargetChanged?.Invoke();
    }
    
    public virtual FrameworkElement CreateFrameworkElement()
    {
        return null;
    }

    void SetLinkControlProperty(FrameworkElement ctl)
    {
        if (ctl == null) return;
        var attrs = AssemblyUtil.GetMemberAttributes<LinkControlPropertyAttribute>(Info, false, Parent);
        foreach (var attr in attrs)
        {
            attr.Init(ctl, this);
        }
        var attrs2 = AssemblyUtil.GetMemberAttributes<LinkControlMemberAttribute>(Info, false, Parent);
        foreach (var attr in attrs2)
        {
            attr.Init(ctl);
        }
    }
    
    public virtual FrameworkElementFactory CreateFrameworkElementFactory()
    {
        return null;
    }
}

