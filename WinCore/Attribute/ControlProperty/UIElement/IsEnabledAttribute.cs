using CqCore;
using System;
using System.ComponentModel;
using System.Windows;

public class IsEnabledAttribute : LinkControlMemberAttribute
{
    public IsEnabledAttribute(bool value) : base(value)
    {
    }
    public IsEnabledAttribute(string path, string convertMethod = null)
        : base(path,  AttributeTarget.Self,convertMethod)
    {
    }
    protected override void OnInit(FrameworkElementFactory fef)
    {
        //fef.AddHandler(RoutedEvent, () => { });
        Action<object> fun=(xx)=>fef.SetValue(UIElement.IsEnabledProperty, Data);
        PropertyChangedEventHandler f = (obj, e) =>
        {
            if (e.PropertyName == path) fun?.Invoke(Data);
        };
        //fun?.Invoke(Data);
        var o = Parent as INotifyPropertyChanged;
        if (o == null) return ;
        o.PropertyChanged += f;
        
    }
    protected override void OnInit(FrameworkElement fe)
    {
        SetBindingOrValue(fe, UIElement.IsEnabledProperty);
    }
}