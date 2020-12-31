using System;

namespace CqCore
{
    public static class ColorUtil
    {/// <summary>
     /// ARGB格式字符串转Color
     /// </summary>
        public static byte[] ColorHtmlToBytes(string argbHtml)
        {
            return ColorIntToBytes(ColorHtmlToInt(argbHtml));
        }

        /// <summary>
        /// #F000FF00=>00FF00F0
        /// </summary>
        public static string ARGBHtmlToRGBA(string argbHtml)
        {
            argbHtml = argbHtml.Replace("#", "");
            int argb = System.Convert.ToInt32(argbHtml, 16);
            byte a = (byte)(argb >> 24);
            int rgba = (argb & (a << 24)) << 8 | a;
            return rgba.ToString();
        }

        public static int ColorHtmlToInt(string rgba)
        {
            rgba = rgba.Replace("#", "");

            return System.Convert.ToInt32(rgba, 16);
        }
        public static byte[] ColorIntToBytes(int rgba)
        {
            byte[] a = new byte[4];
            a[0] = (byte)(rgba >> 24);
            a[1] = (byte)(rgba >> 16);
            a[2] = (byte)(rgba >> 8);
            a[3] = (byte)(rgba);
            return a;
        }
        public static string ColorIntToHtml(int rgba)
        {
            return String.Format("#{0:X}", rgba);
        }
    }
}
