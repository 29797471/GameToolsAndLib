/// <summary>
/// 对象序列化和反序列化后的回调接口
/// </summary>
public interface ICqSerialize
{
    void OnDeserialize();
    void OnSerialize();
}
