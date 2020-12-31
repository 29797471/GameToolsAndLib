using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using WinCore;

public enum ClickStyle
{
    /// <summary>
    /// 无
    /// </summary>
    None,
    /// <summary>
    /// 打开窗口克隆这个对象来编辑
    /// </summary>
    OpenEidtorWindow,
    /// <summary>
    /// 打开窗口编辑这个对象
    /// </summary>
    OpenEidtorWindowByThis,
}
/// <summary>
/// 链接一个点击的回调函数
/// 或者 点击后在打开窗口中编辑修饰的对象
/// </summary>
public class ClickAttribute : LinkControlPropertyAttribute
{
    public string methodName;
    public ClickStyle style;
    public ClickAttribute(string methodName) : base(null)
    {
        this.methodName = methodName;
        style = ClickStyle.None;
    }
    /// <summary>
    /// 通过内置的点击处理方式来执行
    /// </summary>
    /// <param name="style">点击处理方式</param>
    public ClickAttribute(ClickStyle style=ClickStyle.OpenEidtorWindow) : base(null)
    {
        this.style = style;
    }

    void OnClick(object obj,EventArgs e)
    {
        GongSolutions.Wpf.DragDrop.DragDrop.CancelDrag();
        switch (style)
        {
            case ClickStyle.None:
                AssemblyUtil.InvokeMethod(Parent, methodName, Target);
                break;
            case ClickStyle.OpenEidtorWindow:
                {
                    var value = WinUtil.OpenEditorWindow(Target);
                    if (value != null)
                    {
                        Target = value;
                    }
                }
                break;
            case ClickStyle.OpenEidtorWindowByThis:
                {
                    var value = WinUtil.OpenEditorWindow(Target, false);
                    if (value != null)
                    {
                        Target = value;
                    }
                }
                break;
        }
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is ButtonBase)
        {
            var ctl = fe as ButtonBase;
            ctl.Click += OnClick;
            
            ctl.Unloaded += (sender, e) =>
            {
                ctl.Click -= OnClick;
            };
            ctl.Loaded+= (sender, e) =>
            {
                ctl.Click -= OnClick;
                ctl.Click += OnClick;
            };
        }
        else if (fe is RichTextBox)
        {
            var ctl = fe as RichTextBox;
            
            if(ctl.Document.Blocks.FirstBlock is Paragraph)
            {
                var p = ctl.Document.Blocks.FirstBlock as Paragraph;
                if(p.Inlines.FirstInline is Hyperlink)
                {
                    var hl = p.Inlines.FirstInline as Hyperlink;
                    hl.Click += OnClick;
                    ctl.Unloaded += (sender, e) => hl.Click -= OnClick;
                }
            }
        }
        else if (fe is Control)
        {
            var ctl = fe as Control;
            ctl.MouseDown += OnClick;
            ctl.Unloaded += (sender, e) => ctl.MouseDown -= OnClick;
        }
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        if(fef.Type==typeof(UnderLineUserControl))
        {
            fef.AddHandler(UnderLineUserControl.ClickEvent, (RoutedEventHandler)OnClickInFactory);
        }
        else if (fef.Type == typeof(Button))
        {
            fef.AddHandler(Button.ClickEvent, (RoutedEventHandler)OnClickInFactory);
        }
    }
    void OnClickInFactory(object obj, EventArgs e)
    {
        GongSolutions.Wpf.DragDrop.DragDrop.CancelDrag();
        switch (style)
        {
            case ClickStyle.None:
                object data = (obj as FrameworkElement).DataContext;
                AssemblyUtil.InvokeMethod((obj as FrameworkElement).DataContext, methodName, Info.GetValue(data, null));
                break;
        }
    }
}
