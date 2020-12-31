using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// 特性扩展类,提供
/// 1.在类定义中的查找特性
/// 2.获取类定义中的特性列表
/// 3.在对象所有成员中查找特定的特性,(每个成员最多只找出一个)返回列表
/// 4.返回某成员的特性列表
/// 5.在某成员中查找特性
/// </summary>
public static partial class AssemblyUtil
{
    #region 基础api扩展
    /// <summary>
    /// 向特性中注入它所修饰的成员所在的对象
    /// 返回某成员的特性列表
    /// </summary>
    static void SetAttributeTarget(Attribute attr, MemberInfo member, object target)
    {
        if (attr is IMemberAttribute)
        {
            (attr as IMemberAttribute).SetTarget(member, target);
        }
        if (attr is IObjectAttribute && target!=null)
        {
            (attr as IObjectAttribute).SetTarget(target);
        }
    }
    /// <summary>
    /// 基础api扩展
    /// </summary>
    static Attribute[] GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit = false, object target = null)
    {
        var attrs=Attribute.GetCustomAttributes(member, attributeType, inherit);
        foreach(var attr in attrs) SetAttributeTarget(attr, member, target);
        return attrs;
    }
    /// <summary>
    /// 基础api扩展
    /// </summary>
    static Attribute[] GetCustomAttributes(MemberInfo member,  bool inherit = false, object target = null)
    {
        var attrs = Attribute.GetCustomAttributes(member, inherit);
        foreach (var attr in attrs) SetAttributeTarget(attr, member, target);
        return attrs;
    }

    /// <summary>
    /// 基础api扩展
    /// </summary>
    static Attribute GetCustomAttribute(MemberInfo member, Type attributeType, bool inherit = false, object target = null)
    {
        var attr = Attribute.GetCustomAttribute(member, attributeType, inherit);
        SetAttributeTarget(attr, member, target);
        return attr;
    }

    #endregion

    /// <summary>
    /// 在某成员中查找特性
    /// </summary>
    public static T GetMemberAttribute<T>(MemberInfo member, bool inherit = false, object target = null) where T : Attribute
    {
        return (T)GetCustomAttribute(member, typeof(T), inherit,target);
    }

    /// <summary>
    /// 返回某成员的特性列表
    /// </summary>
    public static List<T> GetMemberAttributes<T>(MemberInfo member, bool inherit = false, object target = null) where T : Attribute
    {
        return ((T[])GetCustomAttributes(member, typeof(T), inherit, target)).ToList();
    }

    /// <summary>
    /// 在类定义中的查找特性
    /// </summary>
    public static T GetClassAttribute<T>(object target, bool inherit = false) where T : Attribute
    {
        return (T)GetCustomAttribute(target.GetType(), typeof(T), inherit,target);
    }

    /// <summary>
    /// 获取类定义中的特性
    /// </summary>
    public static Attribute[] GetClsssAttributes(object target, bool inherit = false)
    {
        return GetCustomAttributes(target.GetType(), inherit, target);
    }


    /// <summary>
    /// 在对象所有成员中查找特定的特性,(每个成员最多只找出一个)返回列表
    /// </summary>
    public static List<T> GetMemberAttributesInObject<T>(object target, bool inherit = false) where T : Attribute
    {
        if (target == null) return null;
        var members = target.GetType().GetMembers();
        var list = new List<T>();
        foreach(var member in members)
        {
            var attr = AssemblyUtil.GetMemberAttribute<T>(member, false, target);
            if (attr != null) list.Add(attr);
        }
        return list;
    }

    /// <summary>
    /// 在类的所有静态方法中查找特定的特性,(每个成员最多只找出一个)返回列表
    /// </summary>
    public static List<T> GetMethodsAttributesInType<T>(Type type, bool inherit = false) where T : Attribute
    {
        if (type == null) return null;
        var members = type.GetMethods();
        var list = new List<T>();
        foreach (var member in members)
        {
            var attr = AssemblyUtil.GetMemberAttribute<T>(member, false, null);
            if (attr != null) list.Add(attr);
        }
        return list;
    }
}
