
/// <summary>
/// 可由方法名称调用内部方法,主要应用于lua层面不一样的成员操作<para/>
/// 实现这个接口的类,可由AssemblyUtil.InvokeMethod访问接口
/// </summary>
public interface IInvokeMethod
{
    object InvokeMethod(string methodName, params object[] args);
}
