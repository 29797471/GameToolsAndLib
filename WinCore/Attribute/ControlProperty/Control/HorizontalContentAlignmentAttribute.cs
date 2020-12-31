using CqCore;
using System.Windows;
using System.Windows.Controls;

public class HorizontalContentAlignmentAttribute : LinkControlMemberAttribute
{
    public HorizontalAlignment value
    {
        get
        {
            return (HorizontalAlignment)Data;
        }
    }

    public HorizontalContentAlignmentAttribute(HorizontalAlignment value, AttributeTarget at = 0) : base(value,at)
    {
    }
    public HorizontalContentAlignmentAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, Control.HorizontalContentAlignmentProperty);
    }
}

