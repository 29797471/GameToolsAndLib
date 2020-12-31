using CqCore;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WinCore;

/// <summary>
/// 绘制List(object) 可以选中,
/// 对象只显示ToString()内容,或者由DisplayMember定义的成员
/// </summary>
public class ListBoxAttribute : ControlPropertyAttribute
{
    public Orientation ori;
    public ListBoxAttribute(string name=null,Orientation ori= Orientation.Vertical) : base(name)
    {
        this.ori = ori;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ListBox();
        
        ctl.SetBinding(ItemsControl.ItemsSourceProperty, Info.Name);
        
        if(ori==Orientation.Horizontal)
        {
            SetHorizontal(ctl);
        }
        SetPropertyChanged(ctl,()=>
        {
            WinUtil.AddContextMenu(ctl, Target, () =>
            new object[] 
            {
                ctl.SelectedItem
            });
        });
        return ctl;
    }

    public void SetItemsPanel(ListBox ctl,string template)
    {
        var txt = "<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + template + "</ItemsPanelTemplate>";
        ctl.ItemsPanel = (ItemsPanelTemplate)XamlReader.Parse(txt);
    }

    public void SetHorizontal(ListBox ctl)
    {
        SetItemsPanel(ctl, "<StackPanel Orientation=\"Horizontal\"/>");
    }


}




