using System;
using System.ComponentModel;
using System.Globalization;

public class NameSpacesTypesConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }
        return base.CanConvertFrom(context, sourceType);
    }
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string)
        {
            return AssemblyUtil.GetTypesByNamespace(value as string);
        }
        return base.ConvertFrom(context, culture, value);
    }
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,object value,Attribute[] filter)
    {
        return TypeDescriptor.GetProperties(value, filter);
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
        return true;
    }

}
