using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CustomHash
{
    /// <summary>
    /// 32位FNV算法
    /// </summary>
    public static ushort FNVHash(string a)
    {
        uint hash = 2166136261;
        foreach (char b in a)
        {
            hash = (hash * 16777619) ^ b;
        }
        return (ushort)hash;
    }
    /// <summary>
    ///  BKDR Hash   
    /// </summary>
    public static ushort BKDRHash(string a)
    {
        uint seed = 131; // 31 131 1313 13131 131313 etc..  
        uint hash = 0;

        foreach (byte b in a)
        {
            hash = hash * seed + (b);
        }
        return (ushort)(hash & 0x7FFFFFFF);
    }

    /// <summary>
    /// CRCHash
    /// </summary>
    public static ushort CRCHash(string a)
    {
        //return MathX.toUnshort(MathX.Md5Sum(a));
        byte[] s = Encoding.UTF8.GetBytes(a);
        byte[] b = new byte[s.Length + 2];

        s.CopyTo(b, 0);
        CRC16.ConCRC(ref b, Convert.ToByte(b.Length - 2));
        return BitConverter.ToUInt16(b, b.Length - 2);
    }
}