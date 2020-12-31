using System;
using UnityEngine;

namespace UnityCore
{
    public static class ColorUtil
    {

        public static string ColorToRGBA(Color color)
        {
            var c = ((Color32)color);
            return c.r.ToString("X2") + c.g.ToString("X2") + c.b.ToString("X2") + c.a.ToString("X2");
        }

        /// <summary>
        /// #ARGB Html格式字符串转Color
        /// </summary>
        public static Color ARGBHtmlToColor(string argbHtml)
        {
            return RGBAToColor(ARGBHtmlToRGBA(argbHtml));
        }

        /// <summary>
        /// #F000FF00=>00FF00F0
        /// </summary>
        static string ARGBHtmlToRGBA(string argbHtml)
        {
            argbHtml = argbHtml.Replace("#", "");
            return argbHtml.Substring(2, 6) + argbHtml.Substring(0, 2);
        }

        /// <summary>
        /// #ARGB Html格式字符串转Color
        /// </summary>
        public static Color RGBAToColor(string rgba)
        {
            //float r=0, g=0, b=0, a=0;
            //Debug.LogError(rgba);
            //HexToColor(rgba, ref r, ref g, ref b, ref a);
            //Color d = new Color(r, g, b, a);
            //Debug.LogError(d);
            //return d;
            var ary = ColorIntToBytes(System.Convert.ToInt32(rgba, 16));
            return new Color32(ary[0], ary[1], ary[2], ary[3]);
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
    }
}
