using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

public class SelectedIndexAttribute : LinkControlPropertyAttribute
{

    public SelectedIndexAttribute(string path) : base(path)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is Selector)
        {
            //(fe as Selector).SelectedIndex = (int)Data;
            (fe as Selector).SetBinding(Selector.SelectedIndexProperty, path);
        }
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetBinding(Selector.SelectedIndexProperty, new Binding(path));
    }
}