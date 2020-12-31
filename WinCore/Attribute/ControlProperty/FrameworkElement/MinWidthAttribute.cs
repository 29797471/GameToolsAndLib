using CqCore;
using System.Windows;
public class MinWidthAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }

    public MinWidthAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MinWidthAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.MinWidthProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(FrameworkElement.MinWidthProperty, Data);
    }
}