/// <summary>
/// 可由成员名称访问成员,主要应用于lua层面不一样的成员操作<para/>
/// 实现这个接口的类可以通过AssemblyUtil.SetMemberValue调用这个接口设置成员
/// </summary>
public interface ISetGetMemberValue
{
    object this[string key] { get; set; }
}