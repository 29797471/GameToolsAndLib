using CqCore;
using System.Windows;
using System.Windows.Controls;

public class OrientationAttribute : LinkControlMemberAttribute
{
    public OrientationAttribute(Orientation ori) : base(ori)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is StackPanel)
        {
            SetBindingOrValue(fe, StackPanel.OrientationProperty);
        }
    }
}

