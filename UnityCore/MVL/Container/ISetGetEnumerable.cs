
/// <summary>
/// 可由索引访问数组元素<para/>
/// 可获取元素数量<para/>
/// </summary>
public interface ISetGetEnumerable
{
    object this[int index] { get; set; }

    int Count
    {
        get;
    }
}
