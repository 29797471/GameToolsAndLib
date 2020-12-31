using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public class DatePickerAttribute : ControlPropertyAttribute
{
    public DatePickerAttribute(string name = null, float width = 0f) : base(name, width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new DatePicker();
        //var binding = new Binding(Info.Name);
        //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //if (Info.CanRead && !Info.CanWrite)
        //{
        //    binding.Mode = BindingMode.OneWay;
        //}
        //else if (!Info.CanRead && Info.CanWrite)
        //{
        //    binding.Mode = BindingMode.OneWayToSource;
        //}
        //else
        //{
        //    binding.Mode = BindingMode.TwoWay;
        //}
        //ctl.SetBinding(DatePicker.TextProperty, binding);
        ctl.SetBinding(DatePicker.SelectedDateProperty, Info.Name);
        return ctl;
    }
}


