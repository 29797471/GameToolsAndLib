using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WinCore;

/// <summary>
/// 点击代码值或者代码表达式后在打开窗口中编辑
/// </summary>
public class ClickToEditCodeAttribute : LinkControlPropertyAttribute
{
    public string eventName;
    public ClickToEditCodeAttribute(string eventName=null) : base(null)
    {
        this.eventName = eventName;
    }

    void OnClick(object obj,EventArgs e)
    {
        IExpression CodeTemplate = (IExpression)Target;
        var editor =  EditorCodeTemplate.ToEditor( CodeTemplate,
            (eventName != null)? (EventExp)AssemblyUtil.GetMemberValue(Parent, eventName):null);
        
        editor = WinUtil.OpenEditorWindow(editor);
        if (editor != null)
        {
            if (CodeTemplate.StyleType != editor.StyleType)
            {
                EventMgr.MsgPrint.Notify("返回类型不同,操作失败", 5);
            }
            else
            {
                Target = editor.GetValue();
            }
        };
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is RichTextBox)
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
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        if(fef.Type==typeof(UnderLineUserControl))
        {
            fef.AddHandler(UnderLineUserControl.ClickEvent, (RoutedEventHandler)OnClickInFactory);
        }
    }
    void OnClickInFactory(object obj, EventArgs e)
    {
        var p = (obj as FrameworkElement).DataContext;
        IExpression CodeTemplate = (IExpression)(Info.GetValue(p,null));
        var editor = EditorCodeTemplate.ToEditor(CodeTemplate);
        if (eventName != null) editor.eExp = (EventExp)AssemblyUtil.GetMemberValue(Parent, eventName);
        //editor.Init();
        editor = WinUtil.OpenEditorWindow(editor);
        if (editor != null)
        {
            if (CodeTemplate.StyleType != editor.StyleType)
            {
                EventMgr.MsgPrint.Notify("返回类型不同,操作失败", 5);
            }
            else
            {
                Info.SetValue(p,  editor.GetValue(),null);
            }
        };
    }
}
