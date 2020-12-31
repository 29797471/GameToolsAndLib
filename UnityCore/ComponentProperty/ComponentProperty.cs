using CqCore;
using UnityEngine;

/// <summary>
/// 一个组件的属性代理类
/// </summary>
[System.Serializable]
public class ComponentProperty
{
    /// <summary>
    /// 由于同一个GameObject有可能挂载多个同类型组件,所以不能通过名字的方式引用外部组件
    /// </summary>
    public Component com;

    /// <summary>
    /// 属性名
    /// </summary>
    public string name;
    
    public override string ToString()
    {
        if (com == null) return "null";
        else return com.GetType().Name + "--" + name;
    }

    MemberProxy mMemProxy;
    public MemberProxy MemProxy
    {
        get
        {
            if (mMemProxy == null)
            {
                mMemProxy = MemberProxy.GetMemberProxy(com, name);
            }
            return mMemProxy;
        }
    }
    public object Value
    {
        set
        {
            if (com == null) return;
            MemProxy.Value = value;
        }
        get
        {
            if (com == null) return null;
            return MemProxy.Value;
        }
    }
}
