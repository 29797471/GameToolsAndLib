namespace CqCore
{
    /// <summary>
    /// 对象优先级计算函数
    /// </summary>
    public delegate int ComparisonPriority<in T>(T x);

    /// <summary>
    /// 对象优先级计算函数
    /// </summary>
    public delegate float ComparisonFloatPriority<in T>(T x);
}
