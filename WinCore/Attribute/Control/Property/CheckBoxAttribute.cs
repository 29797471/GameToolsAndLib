using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public class CheckBoxAttribute : ControlPropertyAttribute
{
    public CheckBoxAttribute(string name = null, float width = 0f) : base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new CheckBox();
        ctl.VerticalAlignment = VerticalAlignment.Center;
        //ctl.Content = name;
        ctl.SetBinding(CheckBox.IsCheckedProperty, Info.Name);

        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(CheckBox));
        var binding = new Binding(Info.Name);
        fef.SetBinding(CheckBox.IsCheckedProperty, binding);
        return fef;
    }
}