using CqCore;
using System.Windows;

public class WidthAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }
    
    public WidthAttribute(double value,AttributeTarget at=0) : base(value,at)
    {
    }
    public WidthAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.WidthProperty);
    }
}