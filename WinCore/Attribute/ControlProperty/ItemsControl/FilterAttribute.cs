using System;
using System.Windows;
using System.Windows.Controls;
using WinCore;

/// <summary>
/// 链接一个过滤字符串,
/// 提供给集合容器控件作元素过滤
/// (坑:在UI上设置过滤其实是作用在集合数据上,当该集合数据绑定到其它UI时也会被过滤,所以对集合数据的过滤应该在适当的时候清除)
/// </summary>
public class FilterAttribute : LinkControlPropertyAttribute
{
    /// <summary>
    /// 将集合中的元素显示名称和依赖属性作比较,过滤不包含的元素
    /// path 关联一个过滤器属性,形如Predicate<object> Filter
    /// </summary>
    public FilterAttribute(string path) :base(path)
    {
    }
    
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is ItemsControl)
        {
            var ic = fe as ItemsControl;

            Action<object> fun = y =>
            {
                ic.Items.Filter = (Predicate<object>)y;
            };
            SetLinkPropertyChanged(fe,fun);
            fe.Unloaded += (obj, e) =>
            {
                //由于wpf同一个集合数据的绑定,过滤会相互影响
                ic.Items.Filter = null;//解决wpf的坑,集合的过滤会被保留
            };

        }
        else if (fe is CustomTreeUserControl)
        {
            var ic = fe as CustomTreeUserControl;
            SetLinkPropertyChanged(ic, y =>
            {
                ic.Filter = (Predicate<object>)y;
            });
        }
        /*
        else if(controlAttr is SelectTreeNodeAttribute)
        {
            var d = controlAttr as SelectTreeNodeAttribute;
            SetLinkPropertyChanged(fe, y =>
            {
                d.Filter = (Predicate<object>)y;
            });
        }
        */
        
    }
    
}
