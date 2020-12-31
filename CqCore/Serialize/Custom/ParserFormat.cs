using ParserCore;

namespace CqCore
{
    /// <summary>
    /// 序列化样式
    /// </summary>
    public class ParserFormat
    {
        static ParserFormat mJson;
        public static ParserFormat Json
        {
            get
            {
                if (mJson == null) mJson = new ParserFormat(SerializeFormatStyle.Json);
                return mJson;
            }
        }

        static ParserFormat mTorsion;
        public static ParserFormat Torsion
        {
            get
            {
                if (mTorsion == null) mTorsion = new ParserFormat(SerializeFormatStyle.Torsion);
                return mTorsion;
            }
        }

        /// <summary>
        /// 表达式分隔符
        /// </summary>
        public TokenType ExpSeparator { get; private set; }

        /// <summary>
        /// 表达式结束符
        /// </summary>
        public TokenType ExpEnd { get; private set; }

        /// <summary>
        /// 表达式起始
        /// </summary>
        public TokenType ExpStart { get; private set; }


        /// <summary>
        /// 解析序列化内容样式
        /// </summary>
        public ParserFormat(SerializeFormatStyle se_style)
        {
            switch (se_style)
            {
                case SerializeFormatStyle.Json:
                    {
                        ExpStart = TokenType.STRING;
                        ExpSeparator = TokenType.COLON;
                        ExpEnd =  TokenType.COMMA;
                        break;
                    }
                case SerializeFormatStyle.Torsion:
                    {
                        ExpStart = TokenType.VARIABLE;
                        ExpSeparator = TokenType.EQUAL;
                        ExpEnd = TokenType.SEMICOLON;
                        break;
                    }
            }
        }

    }
}
