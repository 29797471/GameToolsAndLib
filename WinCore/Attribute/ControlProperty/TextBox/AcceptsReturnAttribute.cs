using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

public class AcceptsReturnAttribute : LinkControlPropertyAttribute
{

    public AcceptsReturnAttribute(bool value=true) : base(value)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
     
   SetBindingOrValue(fe, TextBox.AcceptsReturnProperty);
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetValue(TextBox.AcceptsReturnProperty, Data);
    }
}
