using System.Collections.Generic;

/// <summary>
/// 对象池
/// </summary>
public static class ObjectPoolX<T>
{
    static Stack<T> List
    {
        get
        {
            if (m_List == null)
                m_List = new Stack<T>();
            return m_List;
        }
    }
    static Stack<T> m_List;
    public static void Push(T t)
    {
        List.Push(t);
    }
    public static T Pop()
    {
        return List.Pop();
    }
    public static T Peek()
    {
        if (List.Count == 0) return default(T);
        return List.Peek();
    }
}