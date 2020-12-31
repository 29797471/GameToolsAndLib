using CqCore;

namespace ParserCore
{
    /// <summary>
    /// 一个由文本内容解析成的标记
    /// </summary>
    public class Token
    {
        public TokenType type;
        public object value;
        public Token()
        {
            type = TokenType.UNKNOWN;
            value = null;
        }

        public bool IsNull()
        {
            return type == TokenType.KEYWORD && value.ToString() == "null";
        }
        public override string ToString()
        {
            switch(type)
            {
                case TokenType.STRING:
                    {
                        return "\"" + value.ToString() + "\"";
                    }
                case TokenType.CHAR:
                    {
                        return EnumUtil.GetEnumLabelName(type.GetType(), type) + "(" + value.ToString() + ")";
                    }
                case TokenType.NUMBER:
                    {
                        return value.ToString();
                    }
                case TokenType.VARIABLE:
                    {
                        return value.ToString();
                    }
                default:
                    {
                        if(value==null) return EnumUtil.GetEnumLabelName(type.GetType(), type);
                        return value.ToString();
                    }
            }
        }
    }
}
