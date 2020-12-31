using CqCore;
using System.Windows;

public class ToolTipAttribute : LinkControlMemberAttribute
{
    public ToolTipAttribute(string value, AttributeTarget at = 0) : base((object)value, at)
    {
    }
    public ToolTipAttribute(AttributeTarget at ,string path) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.ToolTipProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        SetBindingOrValue(fef, FrameworkElement.ToolTipProperty);
    }
}