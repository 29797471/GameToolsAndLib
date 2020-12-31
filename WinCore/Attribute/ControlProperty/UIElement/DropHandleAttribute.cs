using CqCore;
using System;
using System.Windows;

/// <summary>
/// 拖拽到控件的回调
/// </summary>
public class DropHandleAttribute : LinkControlMemberAttribute
{
    public string methodName;
    public DropHandleAttribute(string methodName, AttributeTarget at = AttributeTarget.Self) : base(null,at)
    {
        this.methodName = methodName;
    }
    protected override void OnInit(FrameworkElement fe)
    {
        //fe.AllowDrop = true;
        GongSolutions.Wpf.DragDrop.DragDrop.SetIsDropTarget(fe, true);
        GongSolutions.Wpf.DragDrop.DragDrop.SetIsDragSource(fe, true);
        DragDrop.AddDropHandler(fe, (a, b) =>
        {
            AssemblyUtil.InvokeMethod(fe.Parent, methodName,a,b);
        });
    }
}

