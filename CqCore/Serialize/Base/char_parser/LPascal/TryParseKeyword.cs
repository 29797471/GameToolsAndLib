namespace ParserCore
{
    /// <summary>
    /// 词法分析器-解析关键字
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        static string[] keyword = new string[] { "true", "false", "null","ref" };
        static object[] keyword_value = new object[] { true, false, null,"ref" };

        /// <summary>
        /// 解析True
        /// </summary>
        bool TryParseKeyword(out object data)
        {
            data = null;
            for(int i=0;i< keyword.Length;i++)
            {
                if (TryParseConstStr(keyword[i]))
                {
                    data = keyword_value[i];
                    return true;
                }
            }
            return false;
        }

    }
        
}
