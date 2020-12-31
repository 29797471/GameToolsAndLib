using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 优先级
/// 适用于排列显示控件的顺序
/// 从低到高,当添加成员时逐渐增加排序索引
/// </summary>
[AttributeUsage(AttributeTargets.Property|AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PriorityAttribute : Attribute
{
    public int pri1
    {
        get
        {
            return prioritys.Length>0?prioritys[0]:0;
        }
    }

    public int[] prioritys;

    /// <summary>
    /// 从上到下排列
    /// </summary>
    public PriorityAttribute(params int[] prioritys)
    {
        this.prioritys = prioritys;
    }


    /// <summary>
    /// 返回属性列表,属性之间按优先级从低到高排列,同优先级基类成员排前面
    /// </summary>
    /// <param name="reverse">false:逆序(从高到底) true:顺序从低到高</param>
    /// <returns></returns>
    public static List<MemberInfo> GetMembersBySort(Type type,bool reverse=false)
    {
        var list = type.GetMembers().ToList();
        ListUtil.Sort(list, o =>
        {
            int priority = 0;
            var attr = AssemblyUtil.GetMemberAttribute<PriorityAttribute>(o);
            if (attr != null)
                priority = attr.pri1 * 2;
            if (o.DeclaringType != o.ReflectedType) priority++;
            return priority;
        });
        if (reverse) list.Reverse();
        return list;
    }

    

}

