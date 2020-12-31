using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WinCore;

/// <summary>
/// 颜色编辑
/// </summary>
public class ColorBoxAttribute : ControlPropertyAttribute
{
    public bool alpha;
    public ColorBoxAttribute(string name = null, float width = 0f,bool alpha=true) : base(name, width)
    {
        this.alpha = alpha;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ColorNewUserControl();
        if (!alpha)
        {
            ctl.slider.Visibility = Visibility.Collapsed;
            ctl.label.Visibility = Visibility.Collapsed;
        }
        ctl.SetBinding(ColorNewUserControl.WindowsMediaColorProperty, Info.Name);
        return ctl;
    }
}


