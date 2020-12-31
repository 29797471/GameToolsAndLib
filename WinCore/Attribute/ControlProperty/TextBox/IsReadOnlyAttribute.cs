using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

public class IsReadOnlyAttribute : LinkControlPropertyAttribute
{
    public IsReadOnlyAttribute(bool value = true) : base(value)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, TextBox.IsReadOnlyProperty);
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetValue(TextBox.IsReadOnlyProperty, Data);
    }
}


