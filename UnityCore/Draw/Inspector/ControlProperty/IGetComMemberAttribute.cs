namespace UnityCore
{
    /// <summary>
    /// 修饰一个ControlAttribute ,该特性需要从修饰的成员所在的组件中获取其它成员
    /// </summary>
    public interface IGetComMemberAttribute
    {
        System.Func<object> GetOtherMemberValue { set;}
    }
}
