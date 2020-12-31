using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一个字符(规则:以'开头中间包含一个字符,以'结尾 ) \"例外
        /// \不作转译
        /// </summary>
        bool TryParseChar(out object data)
        {
            data = null;
            if (Value != '\'') return false;
            Next();
            if (Value == '\\')
            {
                Next();
            }
            data = Value;
            Next();
            if (Value != '\'') return false;
            Next();
            return true;
        }
    }
        
}
