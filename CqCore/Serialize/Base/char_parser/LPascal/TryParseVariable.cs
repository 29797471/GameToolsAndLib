using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一个变量名 (规则:以字母或者_开头,中间可以包含字母数字和_和`和.和+)
        /// </summary>
        bool TryParseVariable( out object data)
        {
            data = null;
            if (!char.IsLetter(Value) && Value != '_') return false;
            StringWriter sw = new StringWriter();
            sw.Write(Value);
            Next();
            while ((char.IsLetter(Value) || Value == '_' || Value == '`' || Value == '.' || Value == '+') || char.IsNumber(Value))
            {
                sw.Write(Value);
                var bl=Next();
                if (bl) break;
            }
            data=sw.ToString();
            return true;
        }
    }
        
}
