using System;
using System.Collections.Generic;
using System.Reflection;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {

        /// <summary>
        /// 解析一个符号
        /// </summary>
        bool TryParseSign(out object data)
        {
            data = null;
            if (CharTokenTypeDic.Dic.ContainsKey(Value))
            {
                data = CharTokenTypeDic.Dic[Value];
            }
            Next();
            return data != null;
        }
    }
    internal static class CharTokenTypeDic
    {
        static Dictionary<char, TokenType> dic;
        internal static Dictionary<char, TokenType> Dic
        {
            get { return dic; }
        }
        static CharTokenTypeDic()
        {
            dic = new Dictionary<char, TokenType>();
            var enumType = typeof(TokenType);
            var values = Enum.GetValues(enumType);
            foreach (var value in values)
            {
                FieldInfo field = enumType.GetField(Enum.GetName(enumType, value));
                var attr = AssemblyUtil.GetMemberAttribute<TokenEnumElementAttribute>(field);
                if (attr != null && attr.args != null)
                {
                    foreach (var chr in attr.args)
                    {
                        dic[chr] = (TokenType)value;
                    }
                }
            }
        }
        
    }
}
