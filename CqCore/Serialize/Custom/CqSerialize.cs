using CqCore;
using ParserCore;
using System;

public static partial class CqSerialize
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">序列化对象</param>
    /// <param name="format">序列化样式</param>
    /// <returns>序列化后的文本内容</returns>
    public static string Serialize(object obj, SerializeFormat format = null)
    {
        if (obj == null) return "null";
        if (format == null) format = SerializeFormat.Torsion;
        var ts = new Serializer(format);
        ts.SerializeValue(obj, obj.GetType());
        var str = ts.GetString();
        if (format.startWithType)
        {
            str = AssemblyUtil.GetName(obj.GetType()) + str;
        }
        if (obj is ICqSerialize)
        {
            (obj as ICqSerialize).OnSerialize();
        }
        return str;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    public static T Parse<T>(string content,ParserFormat format=null)
    {
        return (T)Parse(content, typeof(T), format);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    public static object Parse(string content, Type type = null, ParserFormat format = null)
    {
        if (string.IsNullOrEmpty(content)) return null;
        if (format == null) format = ParserFormat.Torsion;
        if (type == null || type == typeof(object))
        {
            return new Parser(format,Parsing.CharParse(content)).ParseObject();
        }
        else
        {
            return new Parser(format,Parsing.CharParse(content)).ParseValue(type);
        }
    }
}
