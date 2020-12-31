using CqCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 复选组
/// </summary>
public class CheckBoxGroupAttribute : ControlPropertyAttribute
{
    public string selectIndexPath;
    public double deltaWidth;
    public CheckBoxGroupAttribute(string selectIndexPath,double deltaWidth=0f) :base()
    {
        this.selectIndexPath = selectIndexPath;
        this.deltaWidth = deltaWidth;
    }
    
    public override FrameworkElement CreateFrameworkElement()
    {
        var panel = new StackPanel() { Orientation= Orientation.Horizontal};
        System.Action<int> funAdd = (state) =>
        {
            var t = AssemblyUtil.GetMemberValue(Parent, selectIndexPath);
            AssemblyUtil.SetMemberValue(Parent, selectIndexPath, MathUtil.StateAdd(t, state));
        };
        System.Action<int> funDel = (state) =>
        {
            var t = AssemblyUtil.GetMemberValue(Parent, selectIndexPath);
            AssemblyUtil.SetMemberValue(Parent, selectIndexPath, MathUtil.StateDel(t, state));
        };
        SetPropertyChanged(panel, () =>
        {
            panel.Children.Clear();
            if(Target is string[])
            {
                var names = Target as string[];
                var selectIndex = (int)AssemblyUtil.GetMemberValue(Parent, selectIndexPath);
                for (int i = 0; i < names.Length; i++)
                {
                    var ctl = new CheckBox() { Content = names[i] };

                    ctl.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    ctl.Margin = new Thickness(0, 0, deltaWidth, 0);
                    var flag = 1 << i;
                    ctl.IsChecked = MathUtil.StateCheck(selectIndex, flag);

                    ctl.Checked += (sender, args) =>
                    {
                        funAdd(flag);
                    };
                    ctl.Unchecked += (sender, args) =>
                    {
                        funDel(flag);
                    };
                    panel.Children.Add(ctl);
                }
            }
            else if(Target is Type && ((Type)Target).IsEnum)
            {
                var t = (Type)Target;
                var names = EnumUtil.GetEnumNames(t);
                var colors = EnumUtil.GetEnumColors(t);
                //EnumUtil.get
                var selectIndex = (int)AssemblyUtil.GetMemberValue(Parent, selectIndexPath);
                for (int i = 0; i < names.Length; i++)
                {
                    var ctl = new CheckBox();
                    ctl.Content = names[i];
                    if (colors[i] != null)
                    {
                        ctl.Foreground = new System.Windows.Media.SolidColorBrush(WinUtil.ToMediaColor(colors[i]));
                    }
                    ctl.Margin = new Thickness(0, 0, deltaWidth, 0);
                    var flag = 1 << i;
                    ctl.IsChecked = MathUtil.StateCheck(selectIndex, flag);

                    ctl.Checked += (sender, args) =>
                    {
                        funAdd(flag);
                    };
                    ctl.Unchecked += (sender, args) =>
                    {
                        funDel(flag);
                    };
                    panel.Children.Add(ctl);
                }
            }
            
        });
        return panel;
    }
    
}


