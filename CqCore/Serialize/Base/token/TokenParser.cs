//定义序列化过程输出打印
//#define Print_Parser
using System;
using System.Collections.Generic;

namespace ParserCore
{
    /// <summary>
    /// 标记解析器
    /// </summary>
    public class TokenParser : BaseParser<Token>
    {
        public TokenParser(List<Token> list) : base(list)
        {
#if Print_Parser
            CqCore.CqDebug.Log(Json.Serialize(list));
#endif
        }
        public bool NextSkipComment()
        {
            var bl = false;
            do
            {
                bl=Next();
            }
            while (!IsEnd() && Value.type == TokenType.COMMENT);
            return bl;
        }

        /// <summary>
        /// 拿list进行解析func解析,解析成功时执行callBack 传入解析结果,返回解析成功还是失败
        /// 解析步骤 1.判断匹配 (不匹配时回溯)2.捕获 3.退出
        /// </summary>
        public bool TryParseLogic(FuncOut<Type, object, bool> func, Type type, out object data)
        {
            EnterParser();
            var result = func(type, out data);
            if (!result) Back();
#if Print_Parser
            CqCore.CqDebug.Log(result?"Return":"Back");
#endif
            return result;
        }

        /// <summary>
        /// 拿list进行解析func解析,解析成功时执行callBack 传入解析结果,返回解析成功还是失败
        /// 解析步骤 1.判断匹配 (不匹配时回溯)2.捕获 3.退出
        /// </summary>
        public bool TryParseLogic(FuncOut<object, bool> func, out object data)
        {
            EnterParser();
#if Print_Parser
            CqCore.CqDebug.Log("TryParseLogic"+func.ToString());
#endif
            var result = func(out data);
            if (!result) Back();
#if Print_Parser
            CqCore.CqDebug.Log(result ? "Return" : "Back");
#endif
            return result;
        }
    }
}
