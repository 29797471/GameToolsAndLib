using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public class LabelAttribute : ControlPropertyAttribute
{
    public LabelAttribute(string name = null, float width = 0f) :base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new Label();
        ctl.SetBinding(Label.ContentProperty, Info.Name);
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Label));
        var binding = new Binding(Info.Name);
        fef.SetBinding(Label.ContentProperty, binding);

        return fef;
    }
}


