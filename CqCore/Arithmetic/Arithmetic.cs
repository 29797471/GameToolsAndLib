using System;
using System.Collections.Generic;

namespace CqCore
{
    internal delegate Func<object> FunExprD(string inFixExpression, Dictionary<string, Func<object>> dicVar );

    /// <summary>
    /// 函数表达式解析
    /// </summary>
    public static partial class Arithmetic
    {
        static FunExprD StartExpr;
        static Arithmetic()
        {
            StartExpr = ParExpr;
            ParExpr_Next = AbsExpr;
            AbsExpr_Next = BaseExpr;
        }

        /// <summary>
        /// 包含条件运算符的函数
        /// </summary>
        public static Func<object, object> Parse_FxWithEqual(string inFixExpression)
        {
            if (inFixExpression == null) return null;
            if (inFixExpression.Contains("!="))
            {
                var list = inFixExpression.Split("!=");
                var fun0 = Parse_Fx(list[0]);
                var fun1 = Parse_Fx(list[1]);
                return o =>
                {
                    var a = fun0(o);
                    var b = fun1(o);
                    return Convert.ToSingle(a) != Convert.ToSingle(b);
                };
            }
            else if (inFixExpression.Contains("="))
            {
                var list = inFixExpression.Split("=");
                var fun0 = Parse_Fx(list[0]);
                var fun1 = Parse_Fx(list[1]);
                return o =>
                {
                    var a = fun0(o);
                    var b = fun1(o);
                    return Convert.ToSingle(a) == Convert.ToSingle(b);
                };
            }
            else if (inFixExpression.Contains(">"))
            {
                var list = inFixExpression.Split('>');
                var fun0 = Parse_Fx(list[0]);
                var fun1 = Parse_Fx(list[1]);
                return o =>
                {
                    var a = fun0(o);
                    var b = fun1(o);
                    return Convert.ToSingle(a) > Convert.ToSingle(b);
                };
            }
            else if (inFixExpression.Contains("<"))
            {
                var list = inFixExpression.Split('<');
                var fun0 = Parse_Fx(list[0]);
                var fun1 = Parse_Fx(list[1]);
                return o =>
                {
                    var a = fun0(o);
                    var b = fun1(o);
                    return Convert.ToSingle(a) < Convert.ToSingle(b);
                };
            }
            else return Parse_Fx(inFixExpression);

        }
        /// <summary>
        /// 表达式中的x代表第一个参数
        /// </summary>
        public static Func<object, object> Parse_Fx(string inFixExpression)
        {
            if (inFixExpression == null) return null;
            var dic = new Dictionary<string, Func<object>>();

            object temp = null;
            dic["x"] = () => temp;
            var f = StartExpr(inFixExpression, dic);
            return x =>
            {
                temp = x;
                return f();
            };
        }

        /// <summary>
        /// 表达式中的x代表第一个参数
        /// </summary>
        public static Func<object, object, double,object> Parse_Fabt(string inFixExpression)
        {
            var dic = new Dictionary<string, Func<object>>();

            object a=null,b = null;
            double t=0d;
            dic["a"] = () => a;
            dic["b"] = () => b;
            dic["t"] = () => t;
            var f = StartExpr(inFixExpression, dic);
            return (arg1,arg2,arg3)=>
            {
                a = arg1;
                b = arg2;
                t = arg3;
                return f();
            };
        }

        /*
        /// <summary>
        /// 表达式中的x代表第一个参数
        /// </summary>
        public static Func<object, object, object, double> Parse_Fabv(string inFixExpression)
        {
            var dic = new Dictionary<string, Func<object>>();

            object a = null, b = null;
            double t = 0d;
            dic["a"] = () => a;
            dic["b"] = () => b;
            dic["v"] = () => v;
            var f = StartExpr(inFixExpression, dic);
            return (arg1, arg2, arg3) =>
            {
                a = arg1;
                b = arg2;
                t = arg3;
                return f();
            };
        }
        */

        /// <summary>
        /// 表达式中的θ代表第一个参数
        /// </summary>
        public static Func<object, object> Parse_Fθ(string inFixExpression)
        {
            var dic = new Dictionary<string, Func<object>>();

            object temp = null;
            dic["θ"] = () => temp;
            var f = StartExpr(inFixExpression, dic);
            return x =>
            {
                temp = x;
                return f();
            };
        }

        /// <summary>
        /// 解析一个表达式,返回函数<para/>
        /// 函数调用时第一个参数会替换表达式中的变量a<para/>
        /// 函数调用时第一个参数会替换表达式中的变量b<para/>
        /// 函数调用时第一个参数会替换表达式中的变量c
        /// </summary>
        public static Func<object[], object> ExpressionParser(string exp)
        {
            var dic = new Dictionary<string, Func<object>>();

            object a = null, b = null, c = null;
            dic["a"] = () => a;
            dic["b"] = () => b;
            dic["c"] = () => c;
            var f = StartExpr(exp, dic);
            return (args) =>
            {
                if (args.Length > 0) a = args[0];
                if (args.Length > 1) b = args[1];
                if (args.Length > 2) c = args[2];
                return f();
            };
        }
    }
}
