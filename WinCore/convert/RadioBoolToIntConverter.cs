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
    [ValueConversion(/*sourceType*/ typeof(bool), /*targetType*/ typeof(int))]
    public class RadioBoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}

