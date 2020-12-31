using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// 注册的类型不含泛型的派生类型(派生类型由MakeType合成)
/// 通过类名,在程序集中查询类
/// 通过成员名,查询FieldInfo
/// 
/// 类型注册的策略:
/// 为部分名称较长的类名定义一个简单名称,方便序列化和反序列化
/// 同时也可以支持不用注册,直接通过类型全名反射相应的类型
/// </summary>
public static partial class AssemblyUtil
{
    static Dictionary<string, Type> dicType = new Dictionary<string,Type>();
    static Dictionary<Type, string> dicName= new Dictionary<Type, string>();

    public static string PrintDicType()
    {
        return Torsion.Serialize(dicType);
    }

    /// <summary>
    /// 检查是不是基础类型
    /// </summary>
    public static bool IsFundamental(this Type type)
    {
        return type.IsPrimitive || type.Equals(typeof(string)) || type.Equals(typeof(DateTime));
    }

    static AssemblyUtil()
    {
        InitAssemblies();

        RegType(typeof(char), "char");
        RegType(typeof(string) ,"string");

        RegType(typeof(long), "long");
        RegType(typeof(ulong), "ulong");

        RegType(typeof(uint), "uint");
        RegType(typeof(int), "int");

        RegType(typeof(double), "double");
        RegType(typeof(float),"float");

        RegType(typeof(object), "object");
        RegType(typeof(bool), "bool");
        RegType(typeof(List<>), "List");
        RegType(typeof(Dictionary<,>), "Dictionary");
    }
    #region 检查类型
    /// <summary>
    /// 判定形如List&lt;T&gt;的类型
    /// </summary>
    public static bool IsList(Type type)
    {
        return type.IsGenericType && type.GetInterface("IList") != null;
    }

    /// <summary>
    /// 判定形如T[]的类型
    /// </summary>
    public static bool IsArray(Type type)
    {
        return type.BaseType == typeof(Array) && type.IsArray && type.HasElementType;
    }

    /// <summary>
    /// 判定形如Dictionary&lt;T1,T2&gt;的类型
    /// </summary>
    public static bool IsDictionary(Type type)
    {
        return type.IsGenericType && type.GetInterface("IDictionary") != null;
    }

    #endregion
    /// <summary>
    /// 通过类型名的简写在注册的名称中查找类型
    /// </summary>
    public static Type GetType(string typeName)
    {
        Type type;
        //首先检查有没有注册
        if(dicType.TryGetValue(typeName,out type))
        {
            return type;
        }
        return dicType[typeName] = FindTypeByAllAssembly(typeName);
    }

    /// <summary>
    /// 检查是不是一个合法的注册类型(普通类型,或者泛型定义类型)
    /// </summary>
    public static bool IsRegType(Type type)
    {
        if (type.IsGenericType)
            return type.IsGenericTypeDefinition && type.Name.IndexOf('`') != -1;
        else return true;
    }

    /// <summary>
    /// 获取类型全名,当类型已注册一个简单名称时返回这个名称
    /// </summary>
    public static string GetName(Type type)
    {
        //首先检查是不是一个合法类型
        if (!IsRegType(type))
            throw new Exception("不合法的类型:"+type.AssemblyQualifiedName);

        //再检查
        if (dicName.ContainsKey(type)) return dicName[type];
        else return type.FullName;
    }

    /// <summary>
    /// 注册类型
    /// </summary>
    static bool RegType(Type type,string name=null)
    {
        //首先检查是不是一个合法类型
        if (!IsRegType(type)) return false;// throw new Exception("不合法的类型:" + type.Name);

        if (dicName.ContainsKey(type)) return false;//已注册
        if (type.IsGenericType)
        {
            if (name == null)name = type.Name.Substring(0, type.Name.IndexOf('`'));
        }
        else
        {
            if (name == null) name = type.Name;
        }

        dicType[name] = type;
        dicName[type] = name;
        return true;
    }

    /// <summary>
    /// 注册类型,使之可以由GetTypeByName,GetNameByType获取
    /// </summary>
    public static void RegType(params Type[] types)
    {
        foreach(var it in types)
        {
            RegType(it);
        }
    }
    

    /// <summary>
    /// 为指定对象分配参数??
    /// </summary>
    public static T Assign<T>(Dictionary<string, string> dic) where T : new()
    {
        Type t = typeof(T);
        T entity = new T();
        var fields = t.GetProperties();

        string val = string.Empty;
        object obj = null;
        foreach (var field in fields)
        {
            if (!dic.Keys.Contains(field.Name))
                continue;
            val = dic[field.Name];
            //非泛型
            if (!field.PropertyType.IsGenericType)
                obj = string.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, field.PropertyType);
            else //泛型Nullable<>
            {
                Type genericTypeDefinition = field.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    obj = string.IsNullOrEmpty(val)
                      ? null
                      : Convert.ChangeType(val, Nullable.GetUnderlyingType(field.PropertyType));
                }
            }
            field.SetValue(entity, obj, null);
        }


        return entity;
    }
}
