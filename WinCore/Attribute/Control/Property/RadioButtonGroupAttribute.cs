using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 单选组
/// </summary>
public class RadioButtonGroupAttribute : ControlPropertyAttribute
{
    public string selectIndexPath;
    public double deltaWidth;
    public RadioButtonGroupAttribute(string selectIndexPath,double deltaWidth=0f) :base()
    {
        this.selectIndexPath = selectIndexPath;
        this.deltaWidth = deltaWidth;
    }
    //待解决
    public override FrameworkElement CreateFrameworkElement()
    {
        var panel = new StackPanel() { Orientation= Orientation.Horizontal};
        SetPropertyChanged(panel, () =>
        {
            panel.Children.Clear();
            var names =(string[]) Target;
            var selectIndex = (int)AssemblyUtil.GetMemberValue(Parent, selectIndexPath);
            for (int i = 0; i < names.Length; i++)
            {
                var ctl = new RadioButton() { Content = names[i] };
                ctl.Margin = new Thickness(0, 0, deltaWidth, 0);
                ctl.IsChecked = (selectIndex == i);
                var temp = i;
                ctl.Checked += (sender, args) => AssemblyUtil.SetMemberValue(Parent, selectIndexPath, temp);
                panel.Children.Add(ctl);
            }
        });
        return panel;
    }
    
}


