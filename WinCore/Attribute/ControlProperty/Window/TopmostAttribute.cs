using CqCore;
using System.Windows;

/// <summary>
/// 窗口置顶
/// </summary>
public class TopmostAttribute : LinkControlMemberAttribute
{
    public TopmostAttribute(bool value=true, AttributeTarget at = 0) : base(value, at)
    {
    }
    public TopmostAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if(fe is Window)
        {
            SetBindingOrValue(fe, Window.TopmostProperty);
        }
    }
}
