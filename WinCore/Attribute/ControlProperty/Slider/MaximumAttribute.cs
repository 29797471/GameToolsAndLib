using CqCore;
using System.Windows;
using System.Windows.Controls;

public class MaximumAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }

    public MaximumAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MaximumAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Slider)SetBindingOrValue(fe, Slider.MaximumProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(Slider.MaximumProperty, Data);
    }
}