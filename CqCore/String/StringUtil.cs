using CqCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public static partial class StringUtil
{
    static string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };

    const char returnChr = '\n';

    /// <summary>
    /// 返回行数
    /// </summary>
    public static int NumberOfLines(this string x)
    {
        return x.Split(returnChr).Length;
    }

    /// <summary>
    /// 字节单位转文本显示,常用于表示文件大小,内存大小,硬盘大小<para/>
    /// </summary>
    /// <param name="size">字节大小</param>
    /// <param name="hasSymbol">固定带上正负符号</param>
    /// <returns></returns>
    public static string FormatBytes(long size,bool hasSymbol=false)
    { 
        if(size<0)return "-"+FormatBytes(-size);
        int unitsStrIndex = 0;
        double s = size;
        while (s >= 1024)
        {
            s /= 1024;
            unitsStrIndex++;
        }

        return string.Format("{2}{0:0.00} {1}",s, units[unitsStrIndex], hasSymbol?"+":"");
    }
    /// <summary>
    /// 查找全部
    /// </summary>
    public static int[] IndexOfAll(this string x, string value)
    {
        var list = new List<int>();
        var index = x.IndexOf(value);
        while (index != -1)
        {
            list.Add(index);
            index = x.IndexOf(value,index+1);
        }
        return list.ToArray();
    }
    /// <summary>
    /// 替换全部
    /// </summary>
    public static string ReplaceAll(this string x,string oldValue,string newValue)
    {
        var indexs = x.IndexOfAll(oldValue);
        if (indexs.Length == 0) return x;
        StringBuilder sb = new StringBuilder();
        var len = oldValue.Length;
        for (int i=0;i< indexs.Length;i++)
        {
            var start = (i == 0 ? 0 : (indexs[i - 1] + len));
            sb.Append(x, start, indexs[i]- start);
            sb.Append(newValue);
        }
        var lastIndex = indexs[indexs.Length - 1] + len;
        sb.Append(x, lastIndex, x.Length- lastIndex);
        return sb.ToString();
    }

    /// <summary>
    /// 字符串分隔
    /// </summary>
    public static string[] Split(this string x, string separator)
    {
        return Regex.Split(x, separator, RegexOptions.IgnoreCase);
    }
   

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }


    public static bool IsDigit(char keyChar)
    {
        if (char.IsDigit(keyChar) || char.IsControl(keyChar))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 转百分数显示
    /// </summary>
    public static bool FormatNumber(string value, int decimalNum, ref string result)
    {
        try
        {
            result = String.Format("{0:N" + decimalNum.ToString() + "}", Convert.ToDecimal(Decimal.Parse(value, System.Globalization.NumberStyles.Float)));
            return true;
        }
        catch /*(System.Exception ex)*/
        {
            //MsgBoxException(ex.Message, "FormatNumber");
            return false;
        }
    }
    /// <summary>
    /// 转百分数显示
    /// </summary>
    public static string FormatNumberPercent(string value, int decimalNum)
    {
        try
        {
            value = String.Format("{0:N" + decimalNum.ToString() + "}%", 100 * Convert.ToDecimal(Decimal.Parse(value, System.Globalization.NumberStyles.Float)));
            return value;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message, "FormatNumber");
            return value;
        }
    }

    /// <summary>
    /// 按深度拆分字符串
    /// 拆分字符串,对[]内容整体视为下一级
    /// </summary>
    public static List<string> SplitWithDepth(string content, char Separator)
    {
        List<string> ary = new List<string>();
        int lastIndex = 0;
        int depth = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] == Separator && depth == 0)
            {
                ary.Add(content.Substring(lastIndex, i - lastIndex));
                lastIndex = i + 1;
            }
            else
            {
                switch (content[i])
                {
                    case '[':
                        depth++;
                        break;
                    case ']':
                        depth--;
                        break;
                }

            }
        }
        ary.Add(content.Substring(lastIndex));
        return ary;
    }
    /// <summary>
    /// 得到复合枚举值 的多项合成表达式
    /// 例:Enum Et{A=1,B=2}
    /// Et t=(Et)3;
    /// GetEnumComplexString(t)  => "A|B"
    /// </summary>
    public static string GetEnumComplexString<T>(T t)
    {
        StringWriter sw = new StringWriter();
        var ary = Enum.GetValues(typeof(T));
        var d = (int)Enum.ToObject(typeof(T), t);
        foreach (var it in ary)
        {
            int i = (int)it;
            if (MathUtil.StateCheck(t, (T)it))
            {
                sw.Write(it.ToString() + " | ");
            }
        }
        var str = sw.ToString();

        return str.Remove(str.Length - 3, 3);
    }


    /// <summary>
    /// 按换行符\r\n拆分成字符串数组
    /// </summary>
    public static string[] SplitLine(this string Comment)
    {
        string[] ary = new string[] { };
        if (Comment != null)
        {
            ary = Comment.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        return ary;
    }



    /// <summary>
    /// 枚举项名字转值
    /// </summary>
    public static T EnumNameToValue<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }
    /// <summary>
    /// 四则运算+-*/(
    /// </summary>
    public static string MidExpression(string exp)
    {
        return "";
    }
    

    /// <summary>
    /// 获取多个相同字符组成的字符串
    /// </summary>
    public static string Repeat(this char c, int count)
    {
        return new string(c, count);
    }
}
