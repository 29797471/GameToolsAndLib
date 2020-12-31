using CqCore;
using System.Windows;

public class TagAttribute : LinkControlMemberAttribute
{
    public TagAttribute(object value, AttributeTarget at = 0) : base(value, at)
    {
    }
    public TagAttribute(string path, AttributeTarget at = 0) : base(path, at)
    {
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, FrameworkElement.TagProperty);
    }
}