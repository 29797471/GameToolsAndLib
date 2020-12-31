using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一行注释(规则:以//开头中间包含任意字符,行结尾或者文件末尾)
        /// </summary>
        bool TryParseCommentLine(out object data)
        {
            data = null;
            if (Value!='/' && GetOffsetValue(1)!='/')//( "//"))
            {
                return false;
            }

            StringWriter sw = new StringWriter();

            while (!IsEnd() &&  Value != '\n' && Value != '\r')
            {
                sw.Write(Value);
                Next();
            }
            data= sw.ToString();
            return true;
        }
    }
        
}
