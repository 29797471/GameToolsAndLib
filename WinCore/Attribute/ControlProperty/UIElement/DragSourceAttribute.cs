using CqCore;
using System;
using System.Windows;

public class DragSourceAttribute : LinkControlMemberAttribute
{
    public DragSourceAttribute(AttributeTarget at = AttributeTarget.Self) :base(null,at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        GongSolutions.Wpf.DragDrop.DragDrop.SetIsDragSource(fe, true);
    }
}
