using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WinCore;

public class HotKeyAttribute : ControlPropertyAttribute
{
    public HotKeyAttribute(string name = null,float width=0f) : base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new HotKeyTextBoxUserControl();
        ctl.SetBinding(HotKeyTextBoxUserControl.ShortcutKeyProperty, Info.Name);
        return ctl;
    }
}


