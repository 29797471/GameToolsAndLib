using System;
using System.Data;
using System.Text;

/// <summary>
/// 编码函数
/// </summary>
public static partial class StringUtil
{
    /// <summary>
    /// 安卓手机上不支持调用国标编码
    /// </summary>
    public static Encoding GB2312
    {
        get
        {
            return  Encoding.GetEncoding("GB2312");
        }
    }

    public static Encoding UTF8
    {
        get
        {
            return Encoding.UTF8;
        }
    }
    public static Encoding UTF8NoBOM
    {
        get
        {
            if (_UTF8NoBOM == null)
            {
                _UTF8NoBOM = new UTF8Encoding( false);
                /*摘自StreamWriter.UTF8NoBOM的写法
                    UTF8Encoding uTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
                    Thread.MemoryBarrier();
                    _UTF8NoBOM = uTF8NoBOM;
                */
            }

            return _UTF8NoBOM;
        }
    }
    static Encoding _UTF8NoBOM;

    /// <summary>
    /// 中文字符数
    /// </summary>
    public static int GetChineseCharCount(string s)
    {
        int count = 0;
        foreach (char c in s)
        {
            if (Convert.ToInt32(c) > 255)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 返回大写字母和数字的组合
    /// </summary>
    public static string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 中文网络地址转码
    /// </summary>
    public static string UrlEncode(string url)
    {
        if (GB2312 == null) return url;
        byte[] bs = GB2312.GetBytes(url);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bs.Length; i++)
        {
            if (bs[i] < 128)
                sb.Append((char)bs[i]);
            else
            {
                sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 字符串编码转换
    /// </summary>
    /// <param name="srcEncoding">原编码</param>
    /// <param name="dstEncoding">目标编码</param>
    /// <param name="srcStr">原字符串</param>
    /// <returns>字符串</returns>
    public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
    {
        byte[] srcBytes = srcEncoding.GetBytes(srcStr);
        byte[] bytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
        return dstEncoding.GetString(bytes);
    }
}
