using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// 程序集相关接口
/// </summary>
public static partial class AssemblyUtil
{
    static List<Assembly> assemblies;

    static void InitAssemblies()
    {
        assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        var entry       = Assembly.GetEntryAssembly();// 入口程序程序集289
        var calling     = Assembly.GetCallingAssembly();// 当前正在执行的方法的方法的程序集289
        var executing   = Assembly.GetExecutingAssembly();// 当前执行代码的程序集248
        ListUtil.Sort(assemblies, x => x == entry ? 0 : x == calling ? 1 : x == executing ? 2 : 3);
    }
    /// <summary>
    /// 查找在某名称空间下的类型表
    /// </summary>
    public static Type[] GetTypesByNamespace(string Namespace, Type[] types )
    {
        return types.Where(obj => Namespace.Equals(obj.Namespace)).ToArray();
    }

    /// <summary>
    /// 查找在名称空间下定义的所有的类型(只会匹配一个程序集)
    /// </summary>
    public static Type[] GetTypesByNamespace(string nameSpace)
    {
        for (int i = 0; i < assemblies.Count; i++)
        {
            var types = assemblies[i].GetTypesByNamespace(nameSpace);
            if ( types.Length > 0) return types;
        }
        return new Type[0];
    }

    /// <summary>
    /// 在这个程序集下查找特定名称空间的所有类型
    /// </summary>
    public static Type[] GetTypesByNamespace(this Assembly assemblie,string nameSpace)
    {
        var types = assemblie.GetTypes().Where(obj => nameSpace.Equals(obj.Namespace)).ToArray();
        if (types != null ) return types;
        return new Type[0];
    }

    static Type FindTypeByAllAssembly(string typeName)
    {
        for (int i=0;i< assemblies.Count; i++)
        {
            var type=assemblies[i].GetType(typeName);
            if (type != null)
                return type;
        }
        return null;
        //var c = AppDomain.CurrentDomain.GetAssemblies();
        //var ass = Assembly.GetExecutingAssembly();
        //var type = ass.GetType(typeName);
        //if (type != null) return type;
        //ass = Assembly.GetCallingAssembly();
        //type = ass.GetType(typeName);
        //if (type != null) return type;
        //ass = Assembly.GetEntryAssembly();
        //type = ass.GetType(typeName);
        //if (type != null) return type;
        //return Type.GetType(typeName);
    }
}