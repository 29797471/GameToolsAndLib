using System;
using System.ComponentModel;

/// <summary>
/// 委托管理器(由于lua不支持直接执行Action,目前提供给lua来执行回调)
/// </summary>
public static class ActionMgr
{
    /// <summary>
    /// 执行一个Delegate
    /// </summary>
    public static object DoDelegate(Delegate deg, params object[] args)
    {
        return deg.DynamicInvoke(args);
    }

    /// <summary>
    /// 执行一个action
    /// </summary>
    public static void DoAction(Action action)
    {
        action?.Invoke();
    }
    /// <summary>
    /// 执行一个action
    /// </summary>
    public static void DoAction_string(Action<string> action, string obj)
    {
        action?.Invoke(obj);
    }
    
    /// <summary>
    /// 执行一个action
    /// </summary>
    public static void DoAction_int(Action<int> action, int obj)
    {
        action?.Invoke(obj);
    }

    /// <summary>
    /// 执行一个action
    /// </summary>
    public static void DoAction(Action<ListChangedType,int,int> action, ListChangedType listChangedType,int newIndex,int oldIndex)
    {
        action?.Invoke(listChangedType, newIndex, oldIndex);
    }
    /// <summary>
    /// 执行一个action
    /// </summary>
    public static void DoAction<T>(Action<T> action, T obj)
    {
        action?.Invoke(obj);
    }
}
