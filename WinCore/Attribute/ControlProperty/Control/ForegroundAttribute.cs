using CqCore;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 前景色
/// </summary>
public class ForegroundAttribute : LinkControlMemberAttribute
{
    public ForegroundAttribute(string name) : base(name)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Control)
        {
            SetBindingOrValue(fe, Control.ForegroundProperty);
        }
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        SetBindingOrValue(fef, Control.ForegroundProperty);
    }
}
