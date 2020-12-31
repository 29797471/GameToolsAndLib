using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public static class BinaryReaderUtil
    {
        /// <summary>
        /// 让二进制流读取能像SteamReader一样方便地ReadToEnd
        /// </summary>
        public static byte[] ReadToEnd(this BinaryReader reader,long total, ref float progress)
        {
            byte[] bytes = new byte[1024];
            int count = reader.Read(bytes, 0, bytes.Length);
            var list = new List<byte>();
            list.AddRange(bytes.Take(count));
            while (count > 0)
            {
                count = reader.Read(bytes, 0, bytes.Length);
                list.AddRange(bytes.Take(count));
                progress = ((float)list.Count) / total;
            }
            return list.ToArray();
        }
        /// <summary>
        /// 让二进制流读取能像SteamReader一样方便地ReadToEnd
        /// </summary>
        public static byte[] ReadToEnd(this BinaryReader reader)
        {
            byte[] bytes = new byte[1024];
            int count = reader.Read(bytes, 0, bytes.Length);
            var list = new List<byte>();
            list.AddRange(bytes.Take(count));
            while (count > 0)
            {
                count = reader.Read(bytes, 0, bytes.Length);
                list.AddRange(bytes.Take(count));
            }
            return list.ToArray();
        }
    }
}
