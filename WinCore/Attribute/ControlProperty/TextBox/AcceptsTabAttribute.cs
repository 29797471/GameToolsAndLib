using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

public class AcceptsTabAttribute : LinkControlPropertyAttribute
{
    public AcceptsTabAttribute(bool value = true) : base(value)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, TextBox.AcceptsTabProperty);
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetValue(TextBox.AcceptsTabProperty, Data);
    }
}

