/// <summary>
/// 表达式接口
/// </summary>
public interface IExpression
{
    /// <summary>
    /// 编程语言
    /// </summary>
    string Language { get; set; }

    /// <summary>
    /// 返回类型
    /// </summary>
    string StyleType { get; }
    /// <summary>
    /// 对应代码
    /// </summary>
    string ExecContent { get; }


    /// <summary>
    /// 显示文本
    /// </summary>
    string Content { get; }
}