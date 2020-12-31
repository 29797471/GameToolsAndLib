using CqCore;
using System.Windows;
using System.Windows.Controls;
/// <summary>
/// 用于定义含标题头的显示内容
/// </summary>
public class HeaderAttribute : LinkControlMemberAttribute
{
    public HeaderAttribute(string value, AttributeTarget at = 0) : base(value, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, HeaderedContentControl.HeaderProperty);
    }
}

