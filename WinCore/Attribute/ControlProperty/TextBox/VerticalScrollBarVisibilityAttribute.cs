using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

/// <summary>
/// 修饰TextBox当内容超出高度时显示垂直滚动条
/// </summary>
public class VerticalScrollBarVisibilityAttribute : LinkControlPropertyAttribute
{
    public VerticalScrollBarVisibilityAttribute(ScrollBarVisibility value= ScrollBarVisibility.Visible) : base(value)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, TextBoxBase.VerticalScrollBarVisibilityProperty);
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetValue(TextBoxBase.VerticalScrollBarVisibilityProperty, Data);
    }
}
