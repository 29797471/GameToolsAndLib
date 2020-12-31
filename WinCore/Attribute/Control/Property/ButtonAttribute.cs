using System.Windows.Controls;
using System.Reflection;
using System.Windows;

/// <summary>
/// 绘制一个按钮,修饰一个文本,或者一个对象,本身不包含点击逻辑,
/// 当定义了按钮名字时显示这个名称
/// 当没有定义名字,并且修饰的是一个对象时,绘制这个对象到按钮内
/// 点击处理放在ClickAtttribute
/// </summary>
public class ButtonAttribute : ControlPropertyAttribute
{
    public string btnName;
    public ButtonAttribute(string btnName = null) : base()
    {
        this.btnName = btnName;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var btn = new Button();
        if (btnName != null)
        {
            btn.Content = btnName;
        }
        else
        {
            SetPropertyChanged(btn, () =>
            {
                if (Target is string)
                {
                    btn.Content = Target;
                }
                else
                {
                    btn.Content = WinUtil.DrawStackPanel(Target);
                }
            });
        }
        
        return btn;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Button));
        if (btnName != null)
        {
            fef.SetValue(Button.ContentProperty, btnName);
        }
        return fef;
    }
}
