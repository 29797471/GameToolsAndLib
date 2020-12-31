using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 绘制List(object) 可以选中,
/// </summary>
public class ListBoxObjectAttribute : ControlPropertyAttribute
{
    public ListBoxObjectAttribute(string name=null) : base(name)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ListBox();

        SetPropertyChanged(ctl, () =>
        {
            ctl.Items.Clear();
            foreach (var it in Target as System.Collections.IList)
            {
                ctl.Items.Add(WinUtil.DrawPanel(it));
            }
        });
        return ctl;
    }
}