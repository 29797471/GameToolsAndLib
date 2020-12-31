using CqCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 列表扩展
/// </summary>
public static class ListUtil
{
    /// <summary>
    /// 列表相等(顺序不一定相同)
    /// </summary>
    public static bool Equal<T>(IList<T> a,IList<T> b)
    {
        return a.Count == b.Count && a.All(b.Contains);
    }
    /// <summary>
    /// 去重复元素
    /// </summary>
    public static List<T> MakeListByRemoveEqualItem<T>(this IList<T> list)
    {
        return list.Distinct().ToList();
    }

    /// <summary>
    /// 循环获取列表元素<para/>
    /// 当索引等于数量时返回列表第一个元素,当索引等于-1时返回列表最后一个元素,以此类推
    /// </summary>
    public static T GetItemByRound<T>(this IList<T> list, int index)
    {
        return list[(index+list.Count) % list.Count];
    }

    /// <summary>
    /// 范围获取列表元素
    /// 当索引大于等于数量时返回列表最后一个元素,当索引小于0时返回列表第一个元素
    /// </summary>
    public static T GetItemByRange<T>(this IList<T> list, int index)
    {
        if (index < 0) index = 0;
        else if (index >= list.Count) index=list.Count - 1;
        return list[index];
    }

    /// <summary>
    /// 由成员计算得的优先级排序
    /// </summary>
    public static void Sort<T>(this List<T> list, ComparisonPriority<T> cp)
    {
        list.Sort(new FunctorComparer<T>(cp));
    }

    /// <summary>
    /// 由成员计算得的优先级排序
    /// </summary>
    public static void Sort<T>(this List<T> list,ComparisonFloatPriority<T> cp)
    {
        list.Sort(new FunctorComparer<T>(cp));
    }

    /// <summary>
    /// 交换
    /// </summary>
    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    /// <summary>
    /// 移动数组中的元素
    /// </summary>
    public static bool ListMove(IList list,object item, bool toEnd)
    {
        if (list.Contains(item))
        {
            var index = list.IndexOf(item);
            if(toEnd)
            {
                if (index < list.Count - 1)
                {
                    list.Remove(item);
                    list.Insert(index + 1, item);
                    return true;
                }
            }
            else
            {
                if (index > 0)
                {
                    list.Remove(item);
                    list.Insert(index - 1, item);
                    return true;
                }
            }
        }
        return false;
    }

    public static int Add(IList list,object item,ICancelHandle cancelHandle=null)
    {
        var index=list.Add(item);
        if(cancelHandle!=null)
        {
            cancelHandle.CancelAct+=() =>list.Remove(item);
        }
        return index;
    }

}