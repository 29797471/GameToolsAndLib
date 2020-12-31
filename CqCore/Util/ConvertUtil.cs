using System;
using System.Globalization;

public static class ConvertUtil
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static object ChangeType(object value, Type conversionType)
    {
        if (conversionType == typeof(string))
        {
            return Convert.ToString(value);
        }
        try
        {
            if (value == null) return null;
            if (value.GetType().BaseType == conversionType) return value;
            return Convert.ChangeType(value, conversionType);
        }
        catch (Exception)
        {
            CqCore.CqDebug.Log(string.Format("不能转换{0}到类型{1}",value,conversionType));
            return null;
        }
    }
    /// <summary>
    /// 主要操作基本类型转换,比如int 转uint,float 转double之类
    /// 也可以支持自定义类型,需要实现System.IConvertible接口
    /// </summary>
    public static object ConvertType(object v, Type type)
    {
        //if (v == null) return null;
        return Convert.ChangeType(v, type);
    }
    /// <summary>
    /// 泛型转换
    /// </summary>
    public static T BaseChangeType<T>(object v)
    {
        try
        {
            return (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
        }
        catch
        {
            return default(T);
        }
    }

    public static T ChangeType<T>(object v)
    {
        if (typeof(T).IsEnum)
        {
            return (T)Enum.ToObject(typeof(T), v);
        }
        return (T)Convert.ChangeType(v, typeof(T));
    }
}