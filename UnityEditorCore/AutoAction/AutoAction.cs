using CqCore;
using System.Reflection;

/// <summary>
/// 定义被调用的静态方法的显示名称
/// 该方法所在的类需包含在名称空间Automation下
/// </summary>
public class AutoAction : MemberAttribute
{
    public string name;

    public AutoAction(string name)
    {
        this.name = name;
    }
    public void Invoke()
    {
        (Info as MethodInfo).Invoke(null, null);
    }
}
