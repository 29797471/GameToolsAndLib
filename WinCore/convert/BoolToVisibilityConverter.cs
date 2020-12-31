using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
/// <summary>
/// 当数据从Binding的Source流向Target的时候，Convert方法将被调用；
/// 反之ConvertBack将被调用
/// </summary>
namespace WinCore
{
    [ValueConversion(/*sourceType*/ typeof(bool), /*targetType*/ typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter!=null && parameter is string)
            {
                var p = bool.Parse(parameter.ToString());
                return ( (bool)value== p) ? Visibility.Visible : Visibility.Collapsed;
            }
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
