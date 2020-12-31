/// <summary>
/// 按相应的内容格式将数据注入到相应内容中 
/// 主要用于生成代码文件
/// </summary>
public interface ICustomSerialize
{
    /// <summary>
    /// 注入数据
    /// </summary>
    string InjectData(string content);
}