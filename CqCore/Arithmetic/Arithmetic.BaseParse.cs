using ParserCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
/*.向数字栈中压入一个数字

2.向运算符栈中压入一个运算符

3.向数字栈中压入一个数字

此时数字栈中有两个元素，字符栈中有一个元素。

当读到第二个运算符时，判断当前读到的运算符与运算符栈栈顶运算符的优先级顺序(  (  < + - < * < ) )，
若栈顶的运算符优先级大于或等于当前读到的运算符，则弹出两次数字栈栈顶元素和一次运算符栈栈顶元素，并进行运算，将运算结果重新压回数字栈中，然后将当前的读到的运算符压入运算符栈中。（例如：数字栈中含有4（栈顶）3，运算符栈中含有+（栈顶），读到* 时，则先弹出4,3,和+，4+3进行计算得到7，并将7压入数字栈，然后将* 压入栈中，完成后数字栈中含有7（栈顶），运算符栈中含有*（栈顶）

若优先级小于当前运算符，则直接将当前运算符直接压入栈中。（例如：运算符中存在+，当读到* 时，直接压入栈中，不进行运算。）

因此数字栈中的元素至多有3个,操作符栈中的元素至多2个，且每次操作完成后，数字栈中的元素一定比操作符栈的元素多一个。

当对表达式读取完成后，对栈进行运算（数字栈弹出两个数字，操作符栈弹出一个操作符，并将运算结果压回数字栈中），直至操作符栈位空结束，数字栈中剩余的元素即为该中缀表达式的运算结果
*/
namespace CqCore
{
    /// <summary>
    /// 基本运算(不含括号,三角函数,绝对值)
    /// </summary>
    public static partial class Arithmetic
    {
        /// <summary>
        /// 匹配成(数字/变量,运算符,数字/变量,...,运算符,数字/变量)
        /// </summary>
        const string matchX = @"((?<num>[-]?\d+([.]\d)?\d*)|(?<v>[a-zθπ]+\d*))(?<e>[\+\-\*\/\^])|((?<num>[-]?\d+([.]\d)?\d*)|(?<v>[a-zθπ]+\d*))";

        /// <summary>
        /// 由基本的中缀表达式得到一个函数
        /// </summary>
        static Func<object> BaseExpr(string inFixExpression, Dictionary<string,Func<object>> parenthesiFuns )
        {
            Stack<CalcEnumAttribute> opStack = new Stack<CalcEnumAttribute>();//运算符栈
            Stack<Func<object>> numStack = new Stack<Func<object>>();//数据栈

            //弹出两个数字和一个符号进行一次运算,并将得到结果压入数字栈中.
            Action<Stack<Func<object>>, Stack<CalcEnumAttribute>> calStack = (ns, os) =>
            {
                var num2 = ns.Pop();
                var num1 = ns.Pop();
                var style = os.Pop().value;
                ns.Push(() => ArithmeticCalc.Calc(num1(), num2(), style));
            };
            var list = RegexUtil.Matches(inFixExpression, matchX);

            Action<Match> PushVar = m =>
            {
                var num = m.Groups["num"].Value;
                if (!string.IsNullOrEmpty(num))
                {
                    var d = Convert.ToDouble(num);
                    numStack.Push(() => d);
                }
                else
                {
                    var v = m.Groups["v"].Value;
                    if(parenthesiFuns.ContainsKey(v))
                    {
                        numStack.Push(parenthesiFuns[v]);
                    }
                    else if(v=="π")
                    {
                        numStack.Push(()=>Math.PI);
                    }
                    else if (v == "e")
                    {
                        numStack.Push(() => Math.E);
                    }
                }
            };
            foreach (var it in list)
            {
                PushVar(it);
                var opr=it.Groups["e"].Value;
                //当没有匹配到运算符的时候是表达式结尾了,除非是错误的表达式
                if(!string.IsNullOrEmpty(opr))
                {
                    var temp = ArithmeticCalc.Dic[opr];
                    while (opStack.Count > 0 && opStack.Peek().pri >= temp.pri)
                    {
                        calStack(numStack, opStack);
                    }
                    opStack.Push(temp);
                }
            }
            while (opStack.Count > 0) calStack(numStack, opStack);
            return numStack.Pop();
        }
    }
}
