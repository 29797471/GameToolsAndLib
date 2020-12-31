using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CqCore
{
    public static class RegexUtil
    {
        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="startat">开始搜索的字符位置</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, int startat=0, bool ifIgnoreCase = false, bool ifValidateWhiteSpace = false)
        {
            if (!ifValidateWhiteSpace && (inputStr == null || inputStr == ""))
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr, startat);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中提供的正则表达式的匹配项。
        /// </summary>
        public static Match Match(string inputStr, string patternStr, RegexOptions options=RegexOptions.None)
        {
            return Regex.Match(inputStr, patternStr, options);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中提供的正则表达式的匹配项。
        /// </summary>
        public static Match Match(string inputStr, string patternStr, int startat)
        {
            return new Regex(patternStr).Match(inputStr, startat);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中指定的正则表达式的所有匹配项。
        /// </summary>
        public static List<Match> Matches(string inputStr, string patternStr, RegexOptions options= RegexOptions.None)
        {
            return Regex.Matches(inputStr, patternStr, options).OfType<Match>().ToList();
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中指定的正则表达式的所有匹配项。
        /// </summary>
        public static string Replace(string inputStr, string patternStr, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(inputStr, patternStr, replacement, options);
        }

        /// <summary>
        /// 在指定输入字符串内，使用指定替换字符串替换与某个正则表达式模式匹配的字符串（其数目为指定的最大数目）。
        /// </summary>
        public static string Replace(string inputStr, string patternStr, string replacement, int count)
        {
            return new Regex(patternStr).Replace(inputStr, replacement, count);
        }
        public static string Replace(string inputStr, string patternStr, MatchEvaluator evaluator, int count)
        {
            return new Regex(patternStr).Replace(inputStr, evaluator, count);
        }
        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        public static string Replace(string inputStr, string patternStr, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(inputStr, patternStr, evaluator, options);
        }

        /// <summary>
        /// 查找pattern捕获替换为函数OnMatch调用的返回值
        /// </summary>
        public static string MatchReplace( string inputStr, string patternStr,
            Func<int, Match, string> OnMatch)
        {
            int i = 0;
            return Regex.Replace(inputStr, patternStr,m=>
            {
                return OnMatch(i++, m);
            });
        }
    }
}
