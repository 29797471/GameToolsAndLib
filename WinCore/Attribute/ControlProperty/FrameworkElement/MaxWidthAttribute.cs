using CqCore;
using System.Windows;
public class MaxWidthAttribute : LinkControlMemberAttribute
{
    public MaxWidthAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MaxWidthAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.MaxWidthProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(FrameworkElement.MaxWidthProperty, Data);
    }
}