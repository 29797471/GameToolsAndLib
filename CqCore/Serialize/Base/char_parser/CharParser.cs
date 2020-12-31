using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserCore
{
    /// <summary>
    /// 字符解析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        public CharParser(string content) : base(content.ToList())
        {

        }
        /// <summary>
        /// 解析主体逻辑
        /// </summary>
        bool TryParseLogic(FuncOut<object, bool> TryParseFun, out object data)
        {
            EnterParser();
            bool result = TryParseFun( out data);
            if (!result)Back();
            return result;
        }

        /// <summary>
        /// 解析一个常量字符串
        /// </summary>
        bool TryParseConstStr(string str)
        {
            EnterParser();
            for (int i = 0; i < str.Length; i++)
            {
                if (IsEnd()) return false;
                if (Value != str[i])
                {
                    Back();
                    return false;
                }
                Next();
            }
            if (char.IsLetterOrDigit(Value)) return false;
            if (IsEnd()) return true;
            return true;
        }
        
        /// <summary>
        /// 解析字符,生成符号,数字,名字,注释,字符串列表
        /// </summary>
        public List<Token> Parse()
        {
            List<Token> list = new List<Token>();
            while (!IsEnd())
            {
                object obj;
                if (TryParseLogic(TryParseSign, out obj))
                {
                    if ((TokenType)obj != TokenType.IGNORE) list.Add(new Token() { type = (TokenType)obj, value = null });
                }
                else if (TryParseLogic(TryParseString, out obj))
                {
                    list.Add(new Token() { type = TokenType.STRING, value = obj });
                }
                else if (TryParseLogic(TryParseChar, out obj))
                {
                    list.Add(new Token() { type = TokenType.CHAR, value = obj });
                }
                else if (TryParseLogic(TryParseNumber, out obj))
                {
                    list.Add(new Token() { type = TokenType.NUMBER, value = obj });
                }
                else if (TryParseLogic(TryParseKeyword, out obj))//关键字作为特殊的变量名,必须在ParserVariable前
                {
                    list.Add(new Token() { type = TokenType.KEYWORD, value = obj });
                }
                else if (TryParseLogic(TryParseVariable, out obj))
                {
                    list.Add(new Token() { type = TokenType.VARIABLE, value = obj });
                }
                else if (TryParseLogic(TryParseCommentLine, out obj))// 行注释优先 不然解析文件末尾的//***会出错
                {
                    list.Add(new Token() { type = TokenType.COMMENT, value = obj });
                }
                else if (TryParseLogic(TryParseComment, out obj))
                {
                    list.Add(new Token() { type = TokenType.COMMENT, value = obj });
                }
                else
                {
                    throw new Exception();
                }
            }
            return list;
        }
    }
}
