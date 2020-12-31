using CqCore;
using System.Windows;

public class HeightAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }
    public HeightAttribute(double value, AttributeTarget at = 0) : base(value,at)
    {
    }
    public HeightAttribute(string path, AttributeTarget at = 0) : base(path,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.HeightProperty);
    }
}