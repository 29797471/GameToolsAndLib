using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 对象编辑框
/// 对修饰的对象使用通用绘制方式
/// </summary>
public class GroupBoxAttribute : ControlPropertyAttribute
{
    public GroupBoxAttribute(string name = null, float width = 0f) : base(name, width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var gb = new ContentControl();
        SetPropertyChanged(gb,() =>
        {
            gb.Content = WinUtil.DrawHCC<GroupBox>(Target);
        });
        return gb;
    }
}


