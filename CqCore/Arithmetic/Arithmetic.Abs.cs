using System;
using System.Collections.Generic;

namespace CqCore
{

    /// <summary>
    /// 绝对值运算
    /// </summary>
    public static partial class Arithmetic
    {
        /// <summary>
        /// 获取绝对值符号,不能有嵌套
        /// </summary>
        const string absPattern = @"\|(?<e>[^|]+)\|";

        static FunExprD AbsExpr_Next;
        /// <summary>
        /// 带绝对值的表达式函数
        /// </summary>
        static Func<object> AbsExpr(string inFixExpression, Dictionary<string,Func<object>> dicVar )
        {
            while (RegexUtil.IsMatch(inFixExpression, absPattern))
            {
                //每次绝对值(绝对值无法定位里层(前后标识一致),所以不能嵌套)
                inFixExpression = RegexUtil.Replace(inFixExpression, absPattern, x =>
                {
                    var f= AbsExpr_Next(x.Groups["e"].Value, dicVar);
                    var varName = "temp" + dicVar.Count;
                    dicVar[varName] = () => Math.Abs(Convert.ToDouble(f()));
                    return varName;
                });
            }
            return AbsExpr_Next(inFixExpression, dicVar);
        }

        
    }
    
}
