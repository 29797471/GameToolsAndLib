using CqCore;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 字体大小
/// </summary>
public class FontSizeAttribute : LinkControlMemberAttribute
{
    public FontSizeAttribute(string name) : base(name)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Control)
        {
            SetBindingOrValue(fe, Control.FontSizeProperty);
        }
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        SetBindingOrValue(fef, Control.FontSizeProperty);
    }
}
