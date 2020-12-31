using CqCore;
using System.Windows;

public class VerticalAlignmentAttribute : LinkControlMemberAttribute
{
    public VerticalAlignmentAttribute(VerticalAlignment value, AttributeTarget at = 0) : base(value,at)
    {
    }
    public VerticalAlignmentAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.VerticalAlignmentProperty);
    }
}
