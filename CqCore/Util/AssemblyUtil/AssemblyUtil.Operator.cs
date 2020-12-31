using CqCore;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 运算符扩展
/// </summary>
public static partial class AssemblyUtil
{
    

    /// <summary>
    /// 调用a的+运算符函数执行计算
    /// </summary>
    public static object Add(object a,object b)
    {
        return InvokeOperator(a, b, ArithmeticCalc.Dic["+"]);
    }

    /// <summary>
    /// 调用a的-运算符函数执行计算
    /// </summary>
    public static object Sub(object a, object b)
    {
        return InvokeOperator(a, b, ArithmeticCalc.Dic["-"]);
    }
    /// <summary>
    /// 调用a的*运算符函数执行计算
    /// </summary>
    public static object Multiply(object a, object b)
    {
        return InvokeOperator(a, b, ArithmeticCalc.Dic["*"]);
    }
    /// <summary>
    /// 调用a的/运算符函数执行计算
    /// </summary>
    public static object Division(object a, object b)
    {
        return InvokeOperator(a, b, ArithmeticCalc.Dic["/"]);
    }

    static object InvokeOperator(object a, object b, CalcEnumAttribute ce)
    {
        if(a is double)
        {
            switch (ce.value)
            {
                case CalcOperator.Add:
                    return (double)a + Convert.ToDouble(b);
                case CalcOperator.Sub:
                    return (double)a - Convert.ToDouble(b);
                case CalcOperator.Mul:
                    return (double)a * Convert.ToDouble(b);
                case CalcOperator.Div:
                    return (double)a / Convert.ToDouble(b);
            }
            return null;
        }
        var aType = a.GetType();
        var bType = b.GetType();

        var method = GetOperator(aType, ce.methodName, bType);
        if (method != null)
        {
            return InvokeOperator(a, method, b);
        }
        if(_dic!=null)
        {
            Dictionary<Type, Delegate> v;
            _dic.TryGetValue(aType, out v);
            if(v!=null)
            {
                Delegate calc;
                v.TryGetValue(bType, out calc);
                if(calc!=null)
                {
                    return calc.DynamicInvoke(a, b);
                }
            }
        }
        if(bType== typeof(double))
        {
            method = GetOperator(aType, ce.methodName, typeof(float));
            
            if (method != null)
            {
                return InvokeOperator(a,method, Convert.ToSingle(b));
            }
        }
        
        throw new Exception(string.Format("找不到计算{0}{1}{2}的函数", aType, ce.op, bType));
    }

    static Dictionary<Type, Dictionary<Type,Delegate >> _dic;

    /// <summary>
    /// 自定义运算方式
    /// </summary>
    public static void ImportCustomOperator<Ta,Tb,Tc>(CalcOperator calcStyle,Func<Ta,Tb, Tc> Calc)
    {
        var a = typeof(Ta); var b = typeof(Tb);
        if (_dic == null) _dic = new Dictionary<Type, Dictionary<Type, Delegate>>();
        if (!_dic.ContainsKey(a)) _dic[a] = new Dictionary<Type, Delegate>();
        _dic[a][b] = Calc;
    }

    /// <summary>
    /// 获取类型中的运算重载函数
    /// </summary>
    static MethodInfo GetOperator(Type aType,string methodName, Type bType)
    {
        temp_types[0] = aType;
        temp_types[1] = bType;
        return aType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, temp_types, null);
    }
    static Type[] temp_types = new Type[2];

    static object InvokeOperator(object a, MethodInfo method,object b)
    {
        temp_oper[0] = a;
        temp_oper[1] = b;
        return method.Invoke(null, temp_oper);
    }
    static object[] temp_oper = new object[2];
}
