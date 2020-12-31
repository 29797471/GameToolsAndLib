using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

public static partial class WinUtil
{
    /// <summary>
    ///查找某种类型的子控件，并返回一个List集合
    /// </summary>
    public static T FindChild<T>(DependencyObject obj) where T : DependencyObject
    {
        return (T)FindChildByPreorder(obj, x => x is T);
    }
    /// <summary>
    /// 通过名称查找子控件，并返回一个List集合
    /// </summary>
    public static FrameworkElement FindChild(DependencyObject obj, string name)
    {
        return (FrameworkElement)FindChildByPreorder(obj, x => x is FrameworkElement && (x as FrameworkElement).Name == name);
    }

    /// <summary>
    /// 先序遍历查找子控件
    /// 找到一个满足条件的目标时终止遍历,返回这个结果
    /// </summary>
    public static DependencyObject FindChildByPreorder(DependencyObject obj,Predicate<DependencyObject> match)
    {
        if (match(obj)) return obj;
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj) ; i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            var result=FindChildByPreorder(child, match);
            if (result != null) return result;
        }
        return null;
    }
}
