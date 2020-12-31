namespace System
{
    public static class SystemUtil
    {
        /// <summary>
        /// 将 8 位无符号整数的数组转换为其用 Base64 数字编码的等效字符串表示形式
        /// </summary>
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 将指定的字符串（它将二进制数据编码为 Base64 数字）转换为等效的 8 位无符号整数数组
        /// </summary>
        public static byte[] ToBase64(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
