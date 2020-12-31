using CqCore;
using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WinCore;

/// <summary>
/// 对List(object)排列显示,和ListBox的区别是不需要选中每个
/// 并且会绘制对象内部控件
/// </summary>
public class ItemsControlAttribute : ControlPropertyAttribute
{
    public Orientation ori;
    public ItemsControlAttribute(string name = null, Orientation ori = Orientation.Vertical) : base(name)
    {
        this.ori = ori;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ItemsControl();
        if (ori == Orientation.Horizontal)
        {
            SetHorizontal(ctl);
        }
        SetPropertyChanged(ctl, () =>
        {
            ctl.Items.Clear();
            foreach ( var it in Target as IList)
            {
                ctl.Items.Add(WinUtil.DrawPanel(it));
            }
        });
        return ctl;
    }
    public void SetItemsPanel(ItemsControl ctl, string template)
    {
        var txt = "<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + template + "</ItemsPanelTemplate>";
        ctl.ItemsPanel = (ItemsPanelTemplate)XamlReader.Parse(txt);
    }

    public void SetHorizontal(ItemsControl ctl)
    {
        SetItemsPanel(ctl, "<StackPanel Orientation=\"Horizontal\"/>");
    }

}





