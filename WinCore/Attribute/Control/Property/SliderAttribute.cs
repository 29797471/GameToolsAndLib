using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public class SliderAttribute : ControlPropertyAttribute
{
    public SliderAttribute(string name = null, float width = 0f) : base(name, width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new Slider();
        ctl.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.TopLeft;

        SetPropertyChanged(ctl, () => ctl.Value = Convert.ToDouble(Target));
        if (Target is int)
        {
            ctl.IsSnapToTickEnabled = true;
            ctl.ValueChanged += (obj, e) =>
            {
                Target = Convert.ToInt32(ctl.Value);
            };
        }
        else
        {
            ctl.ValueChanged += (obj, e) =>
            {
                Target = ctl.Value;
            };
        }
        
        
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Slider));
        return fef;
    }
}


