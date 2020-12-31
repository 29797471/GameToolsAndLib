using System.Collections;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 自动在范围内排列的容器
/// </summary>
public class WrapPanelAttribute : ControlPropertyAttribute
{
    public WrapPanelAttribute(string name = null) : base(name)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new WrapPanel();
        
        SetPropertyChanged(ctl, () =>
        {
            ctl.Children.Clear();
            foreach (var it in Target as IList)
            {
                ctl.Children.Add(WinUtil.DrawPanel(it));
            }
        });
        
        return ctl;
    }
    
}






