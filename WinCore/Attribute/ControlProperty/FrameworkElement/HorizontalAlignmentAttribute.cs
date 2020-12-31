using CqCore;
using System.Windows;

public class HorizontalAlignmentAttribute : LinkControlMemberAttribute
{
    public HorizontalAlignmentAttribute(HorizontalAlignment value, AttributeTarget at = 0) : base(value,at)
    {
    }
    public HorizontalAlignmentAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.HorizontalAlignmentProperty);
    }
}
