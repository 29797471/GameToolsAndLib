using CqCore;
using System.Windows;
public class MaxHeightAttribute : LinkControlMemberAttribute
{
    public MaxHeightAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MaxHeightAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.MaxHeightProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(FrameworkElement.MaxHeightProperty, Data);
    }
}