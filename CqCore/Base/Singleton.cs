using System;

/// <summary>
/// 单例不应由反射来调用构造
/// </summary>
public class Singleton<T>:IDisposable where T:new()
{
    protected static T ms_instance = default(T);

    protected Singleton()
    {
    }

    public static T instance
    {
        get
        {
            if (ms_instance == null)
                ms_instance = new T();
            return ms_instance;
        }
    }

    public static void ClearSaveData()
    {
        ms_instance = default(T);
    }

    public virtual void Dispose()
    {
        ms_instance = default(T);
    }
}

