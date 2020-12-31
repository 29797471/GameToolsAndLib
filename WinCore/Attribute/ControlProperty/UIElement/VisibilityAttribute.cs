using System;
using System.Windows;


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class VisibilityAttribute : LinkControlMemberAttribute
{
    
    /// <summary>
    /// Target0.自己 1.父容器 
    /// </summary>
    public VisibilityAttribute(bool value, AttributeTarget at = 0) : base(value,at)
    {
        defaultConvert=o=> (bool)o ? Visibility.Visible: Visibility.Collapsed;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">源数据path</param>
    /// <param name="convert">类型转换</param>
    /// <param name="parameter">类型转换传入参数</param>
    /// <param name="toPanel">作用在panel上</param>
    public VisibilityAttribute(string path, AttributeTarget at = 0, string convertExpression = null) 
        : base(path, at, convertExpression)
    {
        defaultConvert = o => (bool)o ? Visibility.Visible : Visibility.Collapsed;
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.VisibilityProperty);
    }
}