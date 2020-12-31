using ParserCore;
using System;
using System.IO;

/// <summary>
/// <para>对于一些来源与接口和基类定义的对象,在序列化时会注入类型,便于反序列化时生成 </para>
/// <para>在反序列化的基本识别中整数为int,带小数点为float  </para>
/// <para>序列化生成的文本带有格式控制符</para>
/// <para>当外部定义类型有名称空间时,需要注册,如: AssemblyUtil.RegType(typeof(ObservableCollection&lt;&gt;), "ObservableCollection");</para>
/// <para>支持数组,泛型,以及泛型嵌套</para>
/// <para>支持类型内部定义的类型</para>
/// </summary>
public class JsonX
{
    /// <summary>
    /// 对象序列化的两中方式1.按字段2.按属性
    /// </summary>
    public enum ObjectStyle
    {
        /// <summary>
        /// 对象按字段序列化和反序列化
        /// </summary>
        Field,

        /// <summary>
        /// 对象按属性序列化和反序列化
        /// </summary>
        Property
    }

    public static object TryDeserialize(string content, System.Type type)
    {
        try
        {
            return Deserialize(content, type);
        }
        catch(Exception )
        {
            return null;
        }
    }
    /// <summary>
    /// 反序列化(content中含类型时不传type)
    /// </summary>
    public static object Deserialize(string content, System.Type type)
    {
        if (string.IsNullOrEmpty(content)) return AssemblyUtil.CreateInstance(type);

        object o = null;
        new JsonParser(Parsing.CharParse(content)).TryParseValue(type, out o);
        if (o != null && o is IJsonSerialize)
        {
            (o as IJsonSerialize).OnDeserialize();
        }
        return o;
    }

    /// <summary>
    /// 反序列化(content中不含类型)
    /// </summary>
    public static T TryDeserialize<T>(string content)
    {
        try
        {
            return (T)TryDeserialize(content, typeof(T));
        }
        catch (Exception )
        {
            throw;
        }
    }
    /// <summary>
    /// 反序列化(content中不含类型)
    /// </summary>
    public static T Deserialize<T>(string content)
    {
        return (T)Deserialize(content, typeof(T));
    }

    /// <summary>
    /// 序列化
    /// </summary>
    public static string Serialize(object obj, ObjectStyle style = ObjectStyle.Field, bool format = true)
    {
        StringWriter sw = new StringWriter();
        new JsonSerialize(style).SerializeValue(obj, sw, obj.GetType(), "",false);
        if (obj != null && obj is IJsonSerialize)
        {
            (obj as IJsonSerialize).OnSerialize();
        }
        if(format)return sw.ToString();
        return sw.ToString().Replace("\r", "").Replace("\n", "").Replace("\t","");
    }

    /// <summary>
    /// 克隆对象
    /// </summary>
    public static object Clone(object obj, ObjectStyle style = ObjectStyle.Field)
    {
        return Deserialize(Serialize(obj, style), obj.GetType());
    }
    /// <summary>
    /// 克隆对象
    /// </summary>
    public static T Clone<T>(T obj, ObjectStyle style = ObjectStyle.Field)
    {
        return (T)Deserialize(Serialize(obj, style), obj.GetType());
    }
}