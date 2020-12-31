namespace CqCore
{
    /// <summary>
    /// 主要用于提供给协程函数,传递返回值<para/>
    /// 由于协程无法参数传递ref,out,导致无法返回结果
    /// </summary>
    public class AsyncReturn<T>
    {
        public T data;
    }
}
