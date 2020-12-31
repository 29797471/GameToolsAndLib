namespace System.Diagnostics
{
    public static class StackTraceUtil
    {
        /// <summary>
        /// 获得当前堆栈信息
        /// </summary>
        public static string GetStack()
        {
            return new StackTrace().ToString();
        }
    }
}
