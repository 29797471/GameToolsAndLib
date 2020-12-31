using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一段注释(规则:以/*开头中间包含任意字符,以*/结尾)
        /// </summary>
        bool TryParseComment(out object data)
        {
            data = null;
            if (!TryParseConstStr( "/*"))
            {
                return false;
            }

            StringWriter sw = new StringWriter();

            while(!TryParseConstStr("*/"))
            {
                sw.Write(Value);
                Next();
            }
            data= sw.ToString();
            return true;
        }
    }
        
}
