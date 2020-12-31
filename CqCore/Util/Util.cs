using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CqCore
{
    public static class Util
    {
        public static double GetDouble(string val)
        {
            try
            {
                return FlashCompatibleConvert.ToDouble(val);
            }
            catch
            {
                if (val.Contains("."))
                    return FlashCompatibleConvert.ToDouble(val.Replace('.', ','));
                return FlashCompatibleConvert.ToDouble(val.Replace(',', '.'));
            }
        }


        /// <summary>
        /// 以字符数组中的回车符分隔成字符串数组
        /// </summary>
        public static string CharAryToString(char[] ary, uint start, uint end)
        {
            end = Math.Min((uint)ary.Length, end);

            StringBuilder sb = new StringBuilder();
            for (uint i = start; i < end; i++)
            {
                sb.Append(ary[i]);
            }
            return sb.ToString();
        }
    }
}
    