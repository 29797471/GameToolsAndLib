using CqCore;
using System;
using System.Collections.Generic;


/// <summary>
/// 定义对指定类型的序列化方式(1.字段(默认) 2.属性 )<para/>
/// 1.当有强制指定类型的序列化样式时使用指定方式处理<para/>
/// 2.当有MarkSerializeByPropertyAttribute特性修饰时,用属性来序列化<para/>
/// 3.否则按序列化
/// </summary>
public static class SerializeTypeUtil
{
    static Dictionary<Type, SerializeType> mTypeDic;
    static Dictionary<Type, SerializeType> TypeDic
    {
        get
        {
            if (mTypeDic == null)
            {
                mTypeDic = new Dictionary<Type, SerializeType>();
            }
            return mTypeDic;
        }
    }

    static Dictionary<string, SerializeType> mNameDic;
    static Dictionary<string, SerializeType> NameDic
    {
        get
        {
            if (mNameDic == null)
            {
                mNameDic = new Dictionary<string, SerializeType>();
            }
            return mNameDic;
        }
    }

    /// <summary>
    /// 强制指定类型的序列化样式和名称
    /// </summary>
    public static SerializeType RegType(Type type,string name, SerializeTypeStyle style= SerializeTypeStyle.Field)
    {
        SerializeType st;
        if (!TypeDic.TryGetValue(type,out st))
        {
            st= new SerializeType() { type = type, name = name, style = style };
            TypeDic[type] = st;
            NameDic[name] = st;
        }
        return st;
    }
    /// <summary>
    /// 获取类型的序列化样式<para/>
    /// 1.当有强制指定类型的序列化样式时使用指定方式处理<para/>
    /// 2.当有MarkSerializeByPropertyAttribute特性修饰时,用属性来序列化<para/>
    /// 3.否则按序列化
    /// </summary>
    public static SerializeTypeStyle GetStyle(Type type,bool serializeObjByProperty=false)
    {
        SerializeType st;
        if(!TypeDic.TryGetValue(type,out st))
        {
            var style = SerializeTypeStyle.Default;
            var attr = (MarkSerializeAttribute)Attribute.GetCustomAttribute(type, typeof(MarkSerializeAttribute));
            if(attr!=null)
            {
                style = attr.style;
            }
            if (type.IsGenericType)
            {
                st=RegType(type, type.Name, style);
            }
            else
            {
                st=RegType(type, type.Name, style);
            }
        }
        
        switch(st.style)
        {
            case SerializeTypeStyle.Default:
                {
                    return serializeObjByProperty ? SerializeTypeStyle.Property : SerializeTypeStyle.Field;
                }
            default:
                {
                    return st.style;
                }
        }
        
    }
}