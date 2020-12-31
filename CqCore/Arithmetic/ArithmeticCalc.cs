using CqCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// 存储运算符信息
    /// </summary>
    internal static class ArithmeticCalc
    {
        static Dictionary<string, CalcEnumAttribute> dic;
        internal static Dictionary<string, CalcEnumAttribute> Dic
        {
            get 
            {
                if(dic==null)
                {
                    dic = new Dictionary<string, CalcEnumAttribute>();
                    var enumType = typeof(CalcOperator);
                    var values = Enum.GetValues(enumType);
                    foreach (var value in values)
                    {
                        FieldInfo field = enumType.GetField(Enum.GetName(enumType, value));
                        var attr = AssemblyUtil.GetMemberAttribute<CalcEnumAttribute>(field);
                        attr.value = (CalcOperator)value;
                        if (attr != null)
                        {
                            dic[attr.op] = attr;
                        }
                    }
                }
                return dic;
            }
        }

        public static object Calc(object left, object right, CalcOperator _operator)
        {
            switch (_operator)
            {
                case CalcOperator.Add:
                    return AssemblyUtil.Add(left,right);
                case CalcOperator.Sub:
                    return AssemblyUtil.Sub(left, right);
                case CalcOperator.Mul:
                    return AssemblyUtil.Multiply(left, right);
                case CalcOperator.Div:
                    return AssemblyUtil.Division(left, right);
            }
            return null;
        }
    }
}
