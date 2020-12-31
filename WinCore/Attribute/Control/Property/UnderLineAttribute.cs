using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WinCore;

/// <summary>
/// 点击下划线编辑数据结构
/// </summary>
public class UnderLineAttribute : ControlPropertyAttribute
{
    public UnderLineAttribute(string name=null, float width = 0f) : base(name, width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var rb = new RichTextBox { IsDocumentEnabled = true, IsReadOnly = true };
        var mcFlowDoc = new FlowDocument(); rb.Document = mcFlowDoc;
        var p = new Paragraph(); mcFlowDoc.Blocks.Add(p);
        var hl = new Hyperlink(); p.Inlines.Add(hl);
        var r = new Run(); hl.Inlines.Add(r);

        SetPropertyChanged(rb,() =>
        {
            if (Target == null) r.Text = null;
            else r.Text = Target.ToString();
            WinUtil.AddContextMenu(rb, Target);
        });
        

        return rb;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(UnderLineUserControl));
        
        fef.SetBinding(UnderLineUserControl.TargetProperty, new Binding(Info.Name));

        return fef;
    }
}


