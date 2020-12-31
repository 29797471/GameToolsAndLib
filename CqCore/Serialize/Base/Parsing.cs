using System.Collections.Generic;

namespace ParserCore
{
    /// <summary>
    /// 语法分析器
    /// </summary>
    public static class Parsing
    {
        public static List<Token> CharParse(string content)
        {
            return new CharParser(content).Parse();
        }
    }
}
