using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WinCore;

/// <summary>
/// 对List(string)编辑 
/// </summary>
public class CustomListTextAttribute : ControlPropertyAttribute
{
    public CustomListTextAttribute(string name) : base(name)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new CustomListText();
        ctl.SetBinding(CustomListText.ItemSourceProperty, Info.Name);
        //SetPropertyChanged(ctl, () =>
        //{
        //    ctl.UpdateViewData_ItemSource();
        //});
        return ctl;
    }
}



