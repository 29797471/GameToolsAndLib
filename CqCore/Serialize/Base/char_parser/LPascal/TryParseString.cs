using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一个字符串(规则:以"开头中间包含任意字符,以"结尾 ) \"例外
        /// \不作转译
        /// </summary>
        bool TryParseString(out object data)
        {
            data = null;
            if (Value != '"') return false;
            Next();
            StringWriter sw = new StringWriter();
            while ( Value != '"')
            {
                if (Value == '\\')
                {
                    Next();
                    sw.Write(Value);
                    Next();
                }
                else
                {
                    sw.Write(Value);
                    Next();
                }
            }
            Next();
            data = sw.ToString();
            return true;
        }
    }
        
}
