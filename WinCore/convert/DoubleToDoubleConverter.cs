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
    [ValueConversion(/*sourceType*/ typeof(double), /*targetType*/ typeof(double))]
    public class DoubleToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value)) return double.NaN;
            return System.Convert.ToDouble(value) + System.Convert.ToDouble(parameter);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value)) return double.NaN;
            return System.Convert.ToDouble(value) - System.Convert.ToDouble(parameter);
        }
    }
}
