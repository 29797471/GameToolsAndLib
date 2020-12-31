using CqCore;
using System.Windows;

public class DataContextAttribute : LinkControlMemberAttribute
{
    public DataContextAttribute(object value) : base(value)
    {
    }
    public DataContextAttribute(string path) : base(path)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.DataContextProperty);
    }
}