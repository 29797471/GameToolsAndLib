using CqCore;
using System.Windows;

/// <summary>
/// 自动调整宽度
/// </summary>
public class AutoWidthAttribute : LinkControlMemberAttribute
{
    public double value
    {
        get
        {
            return (double)Data;
        }
    }
    public AutoWidthAttribute(double value, AttributeTarget at = 0) : base(value, at)
    {
    }
    protected override void OnInitTargetControl(FrameworkElement fe)
    {
        fe.Loaded += (oo, ee) =>
        {
            FrameworkElement pe = fe;
            int count = 0;
            switch (at)
            {
                case AttributeTarget.Self:
                    break;
                case AttributeTarget.Parent:
                    count = 1;
                    break;
                case AttributeTarget.Grandparent:
                    count = 2;
                    break;
                case AttributeTarget.Window:
                    count = int.MaxValue;
                    break;
            }
            for (int i = 0; i<count; i++)
            {
                pe = (pe.Parent as FrameworkElement);
                if (pe.Parent == null)
                    break;
            }
            var delta = value - pe.ActualWidth;
            pe.SizeChanged += (o, e) =>
            {
                fe.Width = pe.ActualWidth + delta;
            };
            var t = fe;
            

            fe.Width = pe.ActualWidth + delta;
        };
    }
}