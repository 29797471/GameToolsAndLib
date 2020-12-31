using System;

/// <summary>
/// 单例管理类
/// </summary>
public class SingletonMgr<T>:IDisposable where T:class,new()
{
    protected static T mInst;

    /// <summary>
    /// 不应由反射来调用构造
    /// </summary>
    protected SingletonMgr()
    {
    }

    public static T Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = new T();
            }
            return mInst;
        }
    }

    public virtual void Dispose()
    {
        mInst = null;
    }
}

