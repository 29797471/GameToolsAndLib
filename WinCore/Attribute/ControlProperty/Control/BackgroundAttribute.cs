using CqCore;
using System.Windows;
using System.Windows.Controls;

public class BackgroundAttribute : LinkControlMemberAttribute
{
    public BackgroundAttribute(string name) : base(name)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Control)
        {
            SetBindingOrValue(fe, Control.BackgroundProperty);
        }
    }
}
