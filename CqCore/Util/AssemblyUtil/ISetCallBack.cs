/// <summary>
/// 为实例中函数对象赋值的接口,主要应用于lua层面不一样的成员操作<para/>
/// 实现这个接口的类可以通过AssemblyUtil.SetMemberValue调用
/// </summary>
public interface ISetCallBack
{
    System.Delegate this[string key] { set; }
}
