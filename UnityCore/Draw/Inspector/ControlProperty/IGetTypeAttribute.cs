namespace UnityCore
{
    /// <summary>
    /// 修饰一个ControlAttribute ,该特性需要从其他属性中获取类型
    /// </summary>
    public interface IGetTypeAttribute
    {
        System.Func<System.Type> GetPropertyType { set;}
    }
}
