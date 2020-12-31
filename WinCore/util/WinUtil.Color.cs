using CqCore;
using System.Linq;
using System.Windows;

public static partial class WinUtil
{
    public static System.Windows.Media.Color GetScreenPixel(Point p)
    {
        return GetScreenPixel((int)p.X,(int)p.Y);
    }
    public static System.Windows.Media.Color GetScreenPixel(int x,int y)
    {
        var bit = new System.Drawing.Bitmap(1, 1);
        var g = System.Drawing.Graphics.FromImage(bit);
        g.CopyFromScreen(new System.Drawing.Point(x, y), new System.Drawing.Point(0, 0), bit.Size);
        var color = bit.GetPixel(0, 0);
        bit.Dispose();
        g.Dispose();
        return ToMediaColor(color);
    }
    public static System.Windows.Media.Color ToMediaColor(string htmlColor)
    {
        return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(htmlColor);
    }
    public static System.Drawing.Color ToWinColor(System.Windows.Media.Color htmlColor)
    {
        return System.Drawing.ColorTranslator.FromHtml(htmlColor.ToString());
    }
    public static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
    {
        return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(ColorUtil.ColorIntToHtml(color.ToArgb()));
    }
}
namespace WinCore
{
    public static class ColorUtil
    {
        /// <summary>
        /// 转颜色代码,形如: #ffffff
        /// </summary>
        public static string ToRGB(this System.Windows.Media.Color color)
        {
            var list = color.ToString().ToCharArray().ToList();
            list.RemoveRange(1, 2);
            return string.Join("", list);
        }
    }
}
