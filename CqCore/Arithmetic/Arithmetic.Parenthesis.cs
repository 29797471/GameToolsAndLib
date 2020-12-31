using ParserCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CqCore
{

    /// <summary>
    /// 三角函数运算
    /// </summary>
    public static partial class Arithmetic
    {
        /// <summary>
        /// 获取最里层的带括号的相关运算符：三角函数,符号运算符,括号
        /// </summary>
        const string parenthesisPattern = @"((sign)|(sin)|(arcsin)|(cos)|(arccos)|(tan)|(arctan)|)\((?<t>[^()]+)\)";
        static FunExprD ParExpr_Next;

        /// <summary>
        /// 带括号的表达式(sign,sin,arcsin,arcsin,cos,arccos,tan,arctan)
        /// </summary>
        static Func<object> ParExpr(string inFixExpression, Dictionary<string, Func<object>> dicVar)
        {
            while (RegexUtil.IsMatch(inFixExpression, parenthesisPattern))
            {
                inFixExpression = RegexUtil.Replace(inFixExpression, parenthesisPattern, x =>
                {
                    var f = ParExpr_Next(x.Groups["t"].Value, dicVar);
                    var varName = "temp" + dicVar.Count;
                    switch (x.Groups[1].Value)
                    {
                        case "sign":
                            dicVar[varName] = () => Math.Sign(Convert.ToDouble(f()));
                            break;
                        case "sin":
                            dicVar[varName] = () => Math.Sin(Convert.ToDouble(f()));
                            break;
                        case "arcsin":
                            dicVar[varName] = () => Math.Asin(Convert.ToDouble(f()));
                            break;
                        case "cos":
                            dicVar[varName] = () => Math.Cos(Convert.ToDouble(f()));
                            break;
                        case "arccos":
                            dicVar[varName] = () => Math.Acos(Convert.ToDouble(f()));
                            break;
                        case "tan":
                            dicVar[varName] = () => Math.Tan(Convert.ToDouble(f()));
                            break;
                        case "arctan":
                            dicVar[varName] = () => Math.Atan(Convert.ToDouble(f()));
                            break;
                        case "":
                            dicVar[varName] = () => f();
                            break;
                    }
                    
                    return varName;
                });
            }
            return ParExpr_Next(inFixExpression, dicVar);
        }
    }
    
}
