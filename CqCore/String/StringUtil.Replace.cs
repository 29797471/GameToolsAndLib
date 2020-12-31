using CqCore;
using System;
using System.Text;
using System.Text.RegularExpressions;

public static partial class StringUtil
{
    #region 扩展

    /// <summary>
    /// 返回从起始偏移startIndex 从末尾偏移endIndexR的一段子串
    /// </summary>
    public static string SubstringEx(this string str, int startIndex, int endIndexR)
    {
        return str.Substring(startIndex, str.Length - startIndex - endIndexR);
    }

    /// <summary>
    /// 查找pattern捕获替换为函数OnMatch调用的返回值
    /// </summary>
    public static string MatchReplace(this string str, string pattern,
        Func<int, Match, string> OnMatch)
    {
        var g = RegexUtil.Matches(str, pattern);
        StringBuilder f = new StringBuilder(str);
        int temp = 0;
        for (int i = 0, c = g.Count; i < c; i++)
        {
            var x = g[i];
            f.Remove(x.Index + temp, x.Length);
            var resultStr = OnMatch(i, x);
            f.Insert(x.Index + temp, resultStr);
            temp += resultStr.Length - x.Length;
        }
        return f.ToString();
    }
    #endregion


    /// <summary>
    /// 通过func替换以start起始,以end结尾的中间内容子串,返回结果
    /// 替换多次
    /// </summary>
    public static string ReplaceSubStr(/*this */string content, string start, string end, Func<string, string> func = null)
    {
        start = start.ReplaceAll("]", @"\]");
        start = start.ReplaceAll("[", @"\[");
        end = end.ReplaceAll("]", @"\]");
        end = end.ReplaceAll("[", @"\[");
        return RegexUtil.MatchReplace(content,start + @"([\s\S]*?)" + end, (ii, m) =>
        {
            return func(m.Groups[1].Value);
            //return func(m.Value.SubstringEx(start.Length, end.Length));
        });
        //tempStartLength = start.Length;
        //tempEndLength = end.Length;
        //_func = func;
        //return content.MatchReplace(start + @"[\s\S]*?" + end, _OnMatch);
    }


    
    /// <summary>
    /// 通过bool 值,确定保留start-middle 还是 middle-end, 返回结果
    /// true:start-middle
    /// false:middle-end
    /// </summary>
    public static string ReplaceSubStrByBool(/*this */string content, bool value, string start, string middle, string end)
    {
        //tempMiddle = middle;
        //if (value)
        //{
        //    return ReplaceSubStr(content, start, end, _ReplaceSubStrByBool_Inter1);
        //}
        //else
        //{
        //    return ReplaceSubStr(content, start, end, _ReplaceSubStrByBool_Inter2);
        //}
        return ReplaceSubStr(content, start, end, x =>
        {
            var index = x.IndexOf(middle);
            if (value)
            {
                return x.Substring(0, index);
            }
            else
            {
                return x.Substring(index + middle.Length);
            }
        });
    }


    /// <summary>
    /// 在content中找寻{*}段落,并由flag选出对应的段落,按位选取
    /// 通过{}定义一系列段落,由flag来确定包含哪些段落(flag:1,2,4,8)
    /// 例:_FlagChoose("x{a}{b}{c}y",3)结果为"xaby"
    /// </summary>
    public static string FlagChoose(string content, int flag)
    {
        return content.MatchReplace("{.+?}", (index, m) =>
        MathUtil.StateCheck(flag, 1 << index) ? m.Value.SubstringEx(1, 1) : "");
    }

    /// <summary>
    /// 在content中找寻{*}段落,并由flag选出对应的段落,按位选取
    /// 通过{}定义一系列段落,由flag来确定包含哪些段落(flag:1,2,3,4)
    /// 例:_FlagChooseOne("x{a}{b}{c}y",3)结果为"xcy"  
    /// </summary>
    public static string FlagChooseOne(string content, int flag)
    {
        return content.MatchReplace("{.+?}", (index, m) =>
        flag == index + 1 ? m.Value.SubstringEx(1, 1) : "");
    }
}

