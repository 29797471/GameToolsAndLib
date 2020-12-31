using CqCore;
using System;
using System.Linq;
using System.Reflection;

public static partial class EnumUtil
{
    /// <summary>
    /// 获得枚举的值名字列表,有标签([EnumValue])的时候名字使用标签名字
    /// </summary>
    public static string[] GetEnumNames<T>()
    {
        return GetEnumNames(typeof(T));
    }

    

    /// <summary>
    /// 获得枚举的值名字列表,有标签([EnumValue])的时候名字使用标签名字
    /// </summary>
    public static string[] GetEnumNames(Type enumType)
    {
        var enumNames = Enum.GetNames(enumType);
        var result = enumNames.ToList();
        foreach (string enumName in enumNames)
        {
            FieldInfo field = enumType.GetField(enumName);
            var attr = AssemblyUtil.GetMemberAttribute<EnumLabelAttribute>(field);
            if (attr != null)
            {
                if (result.Contains(enumName))
                {
                    result[result.IndexOf(enumName)] = attr.name;
                }
            }
        }
        return result.ToArray();
    }

    /// <summary>
    /// 获得枚举对应的html颜色列表,有标签([EnumValue])的时候名字使用标签名字
    /// </summary>
    public static string[] GetEnumColors(Type enumType)
    {
        var enumValues = Enum.GetValues(enumType) as System.Collections.IList;
        string[] colors = new string[enumValues.Count];
        for(int i=0;i<colors.Length;i++)
        {
            var att = GetEnumAttr<EnumColorAttribute>(enumType, enumValues[i]);
            if(att!=null)
            {
                colors[i] = att.htmlColor;
            }
        }
        return colors;
    }

    /// <summary>
    /// 获得某个枚举值的标注名字
    /// </summary>
    public static string GetEnumLabelName(object enumvalue)
    {
        return GetEnumLabelName(enumvalue.GetType(), enumvalue);
    }


    /// <summary>
    /// 获得某个枚举值的标注名字(EnumLabel),当没有定义标注时返回枚举定义的名字
    /// </summary>
    public static string GetEnumLabelName(Type enumType, object value)
    {
        var name = Enum.GetName(enumType, value);
        FieldInfo field = enumType.GetField(name);
        var attr = AssemblyUtil.GetMemberAttribute<EnumLabelAttribute>(field);
        return attr != null ? attr.name : name;
    }
    /// <summary>
    /// 获得某个枚举值的标注名字
    /// </summary>
    public static string GetEnumLabelName<T>(T value)
    {
        return GetEnumLabelName(typeof(T), value);
    }
    /// <summary>
    /// 获得枚举值的特性标签
    /// </summary>
    public static T GetEnumAttr<T>(Type enumType,object value) where T:Attribute
    {
        var field= enumType.GetField(Enum.GetName(enumType, value));
        return AssemblyUtil.GetMemberAttribute<T>(field);
    }
    /// <summary>
    /// 获得枚举值的特性标签
    /// </summary>
    public static T GetEnumAttr<T>(object value) where T : Attribute
    {
        return GetEnumAttr<T>(value.GetType(), value);
    }

    /// <summary>
    /// 字符串转相应的枚举类型
    /// </summary>
    public static T ConvertStringToEnum<T>(string enumStr) where T : new()
    {
        return (T)Enum.Parse(new T().GetType(), enumStr);
    }
}
