using CqCore;
using System;
using System.Windows;

public class DropTargetAttribute : LinkControlMemberAttribute
{
    public DropTargetAttribute( AttributeTarget at =  AttributeTarget.Self) :base(null,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        fe.Loaded += (sender, e) =>
        {
            //GongSolutions.Wpf.DragDrop.DragDrop.SetIsDropTarget(fe, true);
        };
    }
}
