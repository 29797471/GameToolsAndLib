using System;
using System.Collections;
using System.Reflection;

public static partial class AssemblyUtil
{
    /// <summary>
    /// 通过类名创建带默认构造的对象
    /// </summary>
    public static object CreateInstance(string typeName)
    {
         return CreateInstance(GetType(typeName));
    }
    
    /// <summary>
    /// 反射调用带参构造生成对象
    /// </summary>
    public static T CreateInstance<T>(Type type, params object[] args)
    {
        var obj= CreateInstance(type, args);
        if (obj is T)
        {
            return (T)obj;
        }
        else
        {
            throw new Exception("类型:" + type + "生成的对象不是" + typeof(T));
        }
    }

    /// <summary>
    /// 创建一个元素(List&lt;&gt;)
    /// </summary>
    public static object CreateListElement(IList list)
    {
        return CreateInstance(list.GetType().GetGenericArguments()[0]);
    }

    /// <summary>
    /// 创建一个元素([])
    /// </summary>
    public static object CreateArrayElement(IList list)
    {
        return CreateInstance(list.GetType().GetElementType());
    }

    /// <summary>
    /// 反射调用带参构造生成对象,
    /// 注:在ios平台上,由此函数创建对象的类,不能有名称空间,不然会抛找不到构造的异常
    /// </summary>
    public static object CreateInstance(Type type, params object[] args)
    {
        var obj=Activator.CreateInstance(type, args);
        if (obj == null)
        {
            throw new Exception("由类型:" + type + "生成对象失败");
        }
        return obj;
    }

    /// <summary>
    /// 反射调用带参构造生成对象,
    /// 注:在ios平台上,由此函数创建对象的类,不能有名称空间,不然会抛找不到构造的异常
    /// </summary>
    public static object CreatePrivateInstance(Type type, params object[] args)
    {
        var obj= Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null, null);
        if (obj == null)
        {
            throw new Exception("由类型:" + type + "生成对象失败");
        }
        return obj;
    }
}
