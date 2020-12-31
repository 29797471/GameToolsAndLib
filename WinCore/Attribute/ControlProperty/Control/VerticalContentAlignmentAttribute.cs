using CqCore;
using System.Windows;
using System.Windows.Controls;

public class VerticalContentAlignmentAttribute : LinkControlMemberAttribute
{
    public VerticalAlignment value
    {
        get
        {
            return (VerticalAlignment)Data;
        }
    }

    public VerticalContentAlignmentAttribute(VerticalAlignment value, AttributeTarget at = 0) : base(value,at)
    {
    }
    public VerticalContentAlignmentAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, Control.VerticalContentAlignmentProperty);
    }
}

