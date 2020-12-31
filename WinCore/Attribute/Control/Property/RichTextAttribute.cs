using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WinCore;

/// <summary>
/// 修饰List<RichTextUCItem> 
/// 显示html文本
/// </summary>
public class RichTextAttribute : ControlPropertyAttribute
{
    public RichTextAttribute(string name = null,float width=0f) : base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new RichTextUserControl();
        ctl.SetBinding(RichTextUserControl.ItemsSourceProperty, Info.Name);
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(RichTextUserControl));
        var binding = new Binding(Info.Name);
        fef.SetBinding(RichTextUserControl.ItemsSourceProperty, binding);
        return fef;
    }
}


