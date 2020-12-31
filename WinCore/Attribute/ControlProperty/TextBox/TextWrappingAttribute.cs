using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

public class TextWrappingAttribute : LinkControlPropertyAttribute
{

    public TextWrappingAttribute(System.Windows.TextWrapping value = TextWrapping.Wrap) : base(value)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, TextBox.TextWrappingProperty);
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetValue(TextBox.TextWrappingProperty, Data);
    }
}

