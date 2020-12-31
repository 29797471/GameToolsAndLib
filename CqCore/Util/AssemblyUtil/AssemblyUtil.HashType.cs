using System;
using System.Collections.Generic;

/// <summary>
/// 哈希类成id
/// </summary>
public static partial class AssemblyUtil
{
    static Dictionary<ushort, Type> hashType = new Dictionary<ushort, Type>();
    static Dictionary<Type, ushort> hashTypeId= new Dictionary<Type, ushort>();

    /// <summary>
    /// 建立hash映射
    /// </summary>
    public static void InitHashMap(params Type[] typeList)
    {
        foreach (Type t in typeList)
        {
            ushort n = CustomHash.CRCHash(t.Name);
            if (hashType.ContainsKey(n) )
            {
                if(hashType[n]!=t)throw new Exception("InitHashMap哈希冲突");
            }
            else
            {
                hashType.Add(n, t);
                hashTypeId.Add(t, n);
            }
        }
    }
    /// <summary>
    /// 由类型获得哈希id
    /// </summary>
    public static ushort GetHashId(Type type)
    {
        if (hashTypeId.ContainsKey(type)) return hashTypeId[type];
        else return 0;
    }
    /// <summary>
    /// 由哈希id获得类型
    /// </summary>
    public static Type GetHashType(ushort hashId)
    {
        if (hashType.ContainsKey(hashId)) return hashType[hashId];
        else return null;
    }
}
