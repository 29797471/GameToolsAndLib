using CqCore;
using System.Windows;
using System.Windows.Controls;

public class MinimumAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }

    public MinimumAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public MinimumAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Slider)SetBindingOrValue(fe, Slider.MinimumProperty);
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        fef.SetValue(Slider.MinimumProperty, Data);
    }
}