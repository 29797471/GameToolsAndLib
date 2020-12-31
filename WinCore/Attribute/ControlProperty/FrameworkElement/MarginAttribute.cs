using System.Windows;

public class MarginAttribute : LinkControlMemberAttribute
{
    public MarginAttribute(double uniformLength, AttributeTarget at = AttributeTarget.Self)
        : base(new Thickness(uniformLength),at)
    {
    }
    public MarginAttribute(double left_right, double top_bottom, AttributeTarget at = AttributeTarget.Self) 
        : base(new Thickness(left_right, top_bottom, left_right, top_bottom),at)
    {
    }
    public MarginAttribute(double left, double top, double right, double bottom, AttributeTarget at = AttributeTarget.Self) 
        : base(new Thickness(left, top, right, bottom),at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.MarginProperty);
    }
}

