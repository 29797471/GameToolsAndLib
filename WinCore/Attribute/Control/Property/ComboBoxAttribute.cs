using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

/// <summary>
/// 修饰一个下拉框的索引或者是值
/// </summary>
public class ComboBoxAttribute : ControlPropertyAttribute
{
    public ComboBoxAttribute(string name = null,  float width = 0f) : base(name, width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ComboBox();
        ctl.SetBinding(ItemsControl.ItemsSourceProperty, Info.Name);
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(ComboBox));
        fef.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(Info.Name));
        //if (isIndex)
        //{
        //    fef.SetBinding(ComboBox.SelectedIndexProperty, binding);
        //}
        //else
        //{
        //    fef.SetBinding(ComboBox.SelectedValueProperty, binding);
        //}

        return fef;
    }
}

