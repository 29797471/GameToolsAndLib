using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

public class TextBoxAttribute : ControlPropertyAttribute
{
    public TextBoxAttribute(string name=null,float width=0f) :base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new TextBox();
        
        //下划线
        //ctl.BorderBrush = Brushes.Black;
        //ctl.BorderThickness = new Thickness(0,0,0,2);
        
        var binding = new Binding(Info.Name);
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        if(Info.CanRead && !Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWay;
        }
        else if (!Info.CanRead && Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWayToSource;
        }
        ctl.SetBinding(TextBox.TextProperty, binding);

        //一个tab字符占用多少空格字符,默认是7~8个,而代码一般在4个
        var TabSize = 4;
        ctl.PreviewKeyDown += (sender, e) =>
        {
            if (e.Key == Key.Tab)
            {
                String tab = new String(' ', TabSize);
                int caretPosition = ctl.CaretIndex;
                ctl.Text = ctl.Text.Insert(caretPosition, tab);
                ctl.CaretIndex = caretPosition + TabSize;
                e.Handled = true;
            }
        };
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(TextBox));
        var binding = new Binding(Info.Name);
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        if (Info.CanRead && !Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWay;
        }
        else if (!Info.CanRead && Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWayToSource;
        }
        fef.SetBinding(TextBox.TextProperty, binding);
        
        return fef;
    }
}

