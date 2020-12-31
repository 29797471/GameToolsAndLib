namespace UnityCore
{
    /// <summary>
    /// 可以比较的脚本
    /// </summary>
    public interface ICompareMono
    {
        /// <summary>
        /// 执行比较
        /// </summary>
        void Compare(ICompareMono other);
    }
}
