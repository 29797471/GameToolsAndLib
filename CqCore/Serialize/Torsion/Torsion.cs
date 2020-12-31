using ParserCore;
using System;
using System.Text;

/// <summary>
/// <para>对于一些来源与接口和基类定义的对象,在序列化时会注入类型,便于反序列化时生成 </para>
/// <para>在反序列化的基本识别中整数为int,带小数点为float  </para>
/// <para>序列化生成的文本带有格式控制符</para>
/// <para>当外部定义类型有名称空间时,需要注册,如: AssemblyUtil.RegType(typeof(ObservableCollection&lt;&gt;), "ObservableCollection");</para>
/// <para>支持数组,泛型,以及泛型嵌套</para>
/// <para>支持类型内部定义的类型</para>
/// </summary>
public static class Torsion
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

    /// <summary>
    /// 反序列化
    /// 当反序列化失败时返回null
    /// </summary>
    public static object TryDeserialize(string content, System.Type type)
    {
        if (string.IsNullOrEmpty(content)) return null;
        try
        {
            return BaseDeserialize(content, type);
        }
        catch(Exception )
        {
            return null;
        }
    }

    /// <summary>
    /// 反序列化(content中含类型)
    /// </summary>
    public static object Deserialize(string content)
    {
        return Deserialize<object>(content);
    }



    /// <summary>
    /// 反序列化(content中不含类型)
    /// </summary>
    public static T Deserialize<T>(string content)
    {
        return (T)Deserialize(content, typeof(T));
    }
    /// <summary>
    /// 反序列化(content中含类型时不传type)
    /// </summary>
    public static object Deserialize(string content, Type type=null)
    {
        if (string.IsNullOrEmpty(content)) return null;
        return BaseDeserialize(content,type);
    }
    static object BaseDeserialize(string content, Type type=null)
    {
        if (type == null || type==typeof(object))
        {
            return new TorsionParser(Parsing.CharParse(content)).ParseObject();
        }
        else
        {
            return new TorsionParser(Parsing.CharParse(content)).ParseValue(type);
        }
    }

    /// <summary>
    /// 反序列化(content中不含类型)
    /// 当反序列化失败时返回new T
    /// </summary>
    public static T TryDeserialize<T>(string content)where T:new()
    {
        if (string.IsNullOrEmpty(content)) return new T();
        try
        {
            return (T)BaseDeserialize(content, typeof(T));
        }
        catch (Exception)
        {
            //CqDebug.Log(e, LogType.Exception);
            return new T();
        }
    }


    /// <summary>
    /// 序列化
    /// 当类型有SerializeByPropertyAttribute特性时,对属性做序列化
    /// </summary>
    /// <param name="obj">序列化对象</param>
    /// <param name="format">是否加上换行和制表符等,格式控制符</param>
    /// <param name="withObjType">是否包含序列化对象自己的类型</param>
    /// <param name="excludeRecursive">排除递归序列化(同一个自定义对象不会被重复序列化)</param>
    /// <param name="depth">序列化深度</param>
    /// <returns></returns>
    public static string Serialize(object obj, bool format = true,bool withObjType=false,bool excludeRecursive=false,int depth = int.MaxValue)
    {
        if (obj == null) return "null";
        var ts = new TorsionSerialize(excludeRecursive, depth);
        ts.SerializeValue(obj,  obj.GetType());
        if (obj != null && obj is ICqSerialize)
        {
            (obj as ICqSerialize).OnSerialize();
        }
        var str= ts.GetString();
        if(withObjType)
        {
            str = AssemblyUtil.GetName(obj.GetType()) + str;
        }
        if (format) return str;
        return str.Replace("\r", "").Replace("\n", "").Replace("\t", "");
    }

    /// <summary>
    /// 克隆对象
    /// </summary>
    public static object Clone(object obj)
    {
        return Deserialize(Serialize(obj), obj.GetType());
    }
    /// <summary>
    /// 克隆对象
    /// </summary>
    public static T Clone<T>(T obj)
    {
        return (T)Deserialize(Serialize(obj), obj.GetType());
    }

    /// <summary>
    /// 对象序列化的数据内容相同
    /// </summary>
    public static bool Equal(object obj1,object obj2)
    {
        return Serialize(obj1, false) == Serialize(obj2, false);
    }

    #region 字节流序列化
    public static byte[] SerializeBinary(object obj)
    {
        return Encoding.UTF8.GetBytes(Serialize(obj, false, true));
    }
    public static object Deserialize(byte[] bytes)
    {
        return Deserialize(Encoding.UTF8.GetString(bytes));
    }
    #endregion

}