using CqCore;
using System.Windows;
public class MinHeightAttribute : LinkControlMemberAttribute
{
    public MinHeightAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MinHeightAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.MinHeightProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(FrameworkElement.MinHeightProperty,Data);
    }
}