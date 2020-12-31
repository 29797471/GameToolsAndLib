using System;
using System.Collections;
using System.IO;

internal class JsonSerialize
{
    Action<object, StringWriter, Type, string> SerializeObj;

    public JsonSerialize(JsonX.ObjectStyle style = JsonX.ObjectStyle.Field)
    {
        switch (style)
        {
            case JsonX.ObjectStyle.Field:
                SerializeObj  = SerializeObjByField;
                break;
            case JsonX.ObjectStyle.Property:
                SerializeObj = SerializeObjByProperty;
                break;
            default:
                throw new Exception("未知的序列化方式");
        }
    }
    /// <summary>
    /// 序列化任意一个对象
    /// </summary>
    /// <param name="obj">准备序列化的对象</param>
    /// <param name="sw">序列化字符流</param>
    /// <param name="inputType">输入类型</param>
    /// <param name="tab">换行时前面包含的制表符</param>
    /// <param name="inExpression">是否是表达式中的值,用于控制输出格式</param>
    public void SerializeValue(object obj, StringWriter sw,  Type inputType, string tab="", bool inExpression=false)
    {
        if (obj == null)
        {
            sw.Write("null");
            return;
        }

        var type = obj.GetType();
        if (type.IsEnum) // Enum
        {
            sw.Write(Enum.GetName(type, obj));
        }
        else if (AssemblyUtil.IsList(type))
        {
            if (inExpression) sw.WriteLine();
            SerializeList(obj as IList, sw, inputType, tab);
        }
        else if (AssemblyUtil.IsArray(type))
        {
            if (inExpression) sw.WriteLine();
            SerializeArray(obj as Array, sw, inputType, tab);
        }
        else if (AssemblyUtil.IsDictionary(type))//Dic<string,object>
        {
            if (inExpression) sw.WriteLine();
            SerializeDic(obj as IDictionary, sw, inputType, tab);
        }
        else
        {
            switch(AssemblyUtil.GetName(type))
            {
                case "string":
                    {
                        if (!inExpression) sw.Write(tab);
                        var str = obj.ToString();
                        str = str.Replace(@"\", @"\\");
                        str = str.Replace("\"", "\\\"");

                        sw.Write(string.Format("\"{0}\"", str));
                    }
                    break;
                case "char":
                    {
                        if (!inExpression) sw.Write(tab);
                        var chr = (char)obj;
                        switch(chr)
                        {
                            case '\\':
                            case '\'':
                                sw.Write(string.Format("\'\\{0}\'", chr));
                                break;
                            default:
                                sw.Write(string.Format("\'{0}\'", chr));
                                break;
                        }
                    }
                    break;
                case "long":
                case "ulong":
                case "int":
                case "uint":
                case "bool":
                    {
                        if (!inExpression) sw.Write(tab);
                        sw.Write(obj.ToString().ToLower());
                    }
                    break;
                case "float":
                case "double"://由于 float转string 会生成形如(-1.3E+03)的文本 ,需要特殊处理 
                    {
                        if (!inExpression) sw.Write(tab);
                        sw.Write(Convert.ToDecimal(obj).ToString().ToLower());
                    }
                    break;
                default:
                    {
                        if (inExpression) sw.WriteLine();
                        SerializeObj(obj, sw, inputType, tab);
                    }
                    break;
            }
        }
    }
    void SerializeObjByField(object obj, StringWriter sw,Type inputType, string tab = "")
    {
        var type = obj.GetType();
        var fields = type.GetFields();
        //if(inputType!=type)
        //{
        //    sw.WriteLine(tab +AssemblyUtil.GetName(type));
        //}
        sw.WriteLine(tab + "{");

        bool separator=false;
        foreach (var field in fields)
        {
            if (!field.IsStatic)
            {
                var data = field.GetValue(obj);
                if(separator) sw.WriteLine(",");
                sw.Write(string.Format("\t{0}\"{1}\" : ", tab, field.Name));
                SerializeValue(data, sw, field.FieldType, "\t" + tab, true);
                separator = true;
            }
        }
        if(separator) sw.WriteLine();
        sw.Write(tab + "}");
    }
    void SerializeObjByProperty(object obj, StringWriter sw, Type inputType, string tab = "")
    {
        var type = obj.GetType();
        var properties = type.GetProperties();
        if (inputType != type)
        {
            sw.WriteLine(tab + type.Name);
        }
        sw.WriteLine(tab + "{");

        bool separator = false;
        foreach (var property in properties)
        {
            var data = property.GetValue(obj,null);
            if (separator) sw.WriteLine(",");
            sw.Write(string.Format("\t{0}\"{1}\" : ", tab, property.Name));
            SerializeValue(data, sw, property.PropertyType, "\t" + tab, true);
            separator = true;
        }
        if (separator) sw.WriteLine();
        sw.Write(tab + "}");
    }
    void SerializeType(Type type, StringWriter sw)
    {
        if (type.IsGenericType)
        {
            sw.Write(AssemblyUtil.GetName(type.GetGenericTypeDefinition()));
            var args = type.GetGenericArguments();
            sw.Write("<");
            
            for(int i = 0;i<args.Length-1;i++ )
            {
                SerializeType(args[i], sw);
                sw.Write(",");
            }
            SerializeType(args[args.Length-1], sw);
            sw.Write(">");
        }
        else
        {
            sw.Write(AssemblyUtil.GetName(type));
        }
    }
    void SerializeList(IList obj, StringWriter sw, Type inputType, string tab = "")
    {
        var type = obj.GetType();
        Type itemType = type.GetGenericArguments()[0];
        //if (type!= inputType)
        //{
        //    sw.WriteLine(tab );
        //    SerializeType(type, sw);
        //}
        sw.WriteLine(tab + '[');

        int count = obj.Count;
        int i = 0;
        foreach(var item in obj)
        {
            //sw.Write(string.Format("\t{0}", tab));
            SerializeValue(item, sw, itemType, tab + "\t");
            if (i != count - 1) sw.WriteLine(',');
            else sw.WriteLine();
            i++;
        }
        sw.Write(tab + ']');
    }
    void SerializeArray(Array obj, StringWriter sw, Type inputType, string tab = "")
    {
        var type = obj.GetType();
        Type itemType = type.GetElementType();

        //if (type != inputType)
        //{
        //    sw.WriteLine(tab + AssemblyUtil.GetName(itemType)+"()" );
        //}

        sw.WriteLine(tab + "[");

        int count = obj.Length;
        int i = 0;
        foreach(var item in obj)
        {
            //sw.Write(string.Format("\t{0}", tab));
            SerializeValue(item, sw, itemType, tab + "\t");
            if (i != count - 1) sw.WriteLine(',');
            else sw.WriteLine();
            i++;
        }
        sw.Write(tab + ']');
    }
    void SerializeDic(IDictionary o, StringWriter sw, Type inputType, string tab = "")
    {
        var type = o.GetType();
        var keyType = type.GetGenericArguments()[0];
        var valueType = type.GetGenericArguments()[1];
        //if (inputType!=type)
        //{
        //    sw.WriteLine(tab);
        //    SerializeType(type, sw);
        //}
        sw.WriteLine(tab + '{');
        int i = 0;
        foreach (var key in o.Keys)
        {
            var data = o[key];
            sw.Write(string.Format("\t{0}", tab));
            SerializeValue(key, sw, keyType, tab);
            sw.Write(":");
            SerializeValue(data, sw, valueType, "\t" + tab,true);
            if (i != o.Keys.Count - 1) sw.WriteLine(",");
            else sw.WriteLine();
            i++;
        }
        sw.Write(tab + "}");
    }
}