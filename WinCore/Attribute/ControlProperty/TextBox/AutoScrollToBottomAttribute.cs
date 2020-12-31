using CqCore;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

/// <summary>
/// 自动滚动到底部
/// </summary>
public class AutoScrollToBottomAttribute : LinkControlPropertyAttribute
{
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is TextBox)
        {
            var tb = fe as TextBox;
            SetPropertyChanged(fe,() =>
            {
                if (tb.LineCount <= 1) lastMaxOffset = 0;
                if (lastMaxOffset==0 || tb.VerticalOffset>=lastMaxOffset)
                {
                    tb.ScrollToEnd();
                    lastMaxOffset = tb.VerticalOffset;
                }
            });
        }
    }
    double lastMaxOffset;
}
