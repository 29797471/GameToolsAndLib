using CqCore;
using ParserCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// 通过类名,在程序集中查询类
/// 通过成员名,查询MethodInfo
/// </summary>
public static partial class AssemblyUtil
{
    static Dictionary<Type, Dictionary<string, List<MethodInfo>>> _dicMethodInfo =
        new Dictionary<Type, Dictionary<string, List<MethodInfo>>>();
    /// <summary>
    /// 获取类型的成员信息字典
    /// </summary>
    static Dictionary<string, List<MethodInfo>> GetMethodInfoDic(Type objType)
    {
        if (!_dicMethodInfo.ContainsKey(objType))
        {
            var memberInfos = objType.GetMethods();
            Dictionary<string, List<MethodInfo>> _dic = new Dictionary<string, List<MethodInfo>>();
            foreach (var fi in memberInfos)
            {
                if (!_dic.ContainsKey(fi.Name)) _dic[fi.Name] = new List<MethodInfo>();
                _dic[fi.Name].Add(fi);
            }
            _dicMethodInfo[objType] = _dic;
        }
        return _dicMethodInfo[objType];
    }

    /// <summary>
    /// 传入的类型符合是否符合方法本身参数定义的类型
    /// </summary>
    public static bool CallMethodArgTypeByType(Type inputType,Type parameterType)
    {
        if (parameterType == typeof(object) ) return true;
        if(parameterType.IsGenericType)
        {
            
            if (!inputType.IsGenericType) return false;
            
            var inputGenType = inputType.GetGenericTypeDefinition();
            var parameterGenType = parameterType.GetGenericTypeDefinition();
            if (inputGenType != parameterGenType) return false;
            
            var inputGenTypes = inputType.GetGenericArguments();
            var parameterGenTypes= parameterType.GetGenericArguments();
            var count = inputGenTypes.Length;
            for (int i=0;i<count;i++)
            {
                if(!CallMethodArgTypeByType(inputGenTypes[i], parameterGenTypes[i]))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return parameterType == inputType || inputType.BaseType == parameterType;
        }
    }

    static bool InvokeMethod(MethodInfo _info,object obj,object[] args,out object result)
    {
        result = null;
        var list = args.ToList();
        ParameterInfo[] _parameters = _info.GetParameters();
        for (int i = 0; i < _parameters.Length; i++)
        {
            var _param = _parameters[i];
            if (i < list.Count)
            {
                if(list[i]!=null)
                {
                    var t = list[i].GetType();

                    //CqDebug.Log("i:" + i + " _param.ParameterType:" + _param.ParameterType + " t:" + t + " t.BaseType:"+ t.BaseType);

                    if(!CallMethodArgTypeByType(t, _param.ParameterType))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (_param.DefaultValue != DBNull.Value) list.Add(_param.DefaultValue);
                else list.Add(null);
            }
        }
        result= _info.Invoke(obj, list.ToArray());
        return true;
    }
    /// <summary>
    /// 是否有方法
    /// </summary>
    public static bool HasMethod(Type obj, string methodName)
    {
        return obj.GetMethod(methodName) != null;
    }

    /// <summary>
    /// 设置对象的成员函数对象
    /// </summary>
    public static void SetCallBack(object obj,string methodName,System.Delegate act)
    {
        if(obj is ISetCallBack)
        {
            (obj as ISetCallBack)[methodName] = act;
        }
        else
        {
            SetMemberValue(obj, methodName, act);
        }
    }

    /// <summary>
    /// 执行对象方法,可调私有方法
    /// 可以有默认值,并支持重载函数 以及params的不定参数表
    /// </summary>
    public static object InvokeObjMethod(object obj, string methodName, params object[] args)
    {
        if (obj is IInvokeMethod)
        {
            return ((IInvokeMethod)obj).InvokeMethod(methodName, args);
        }
        return _InvokeMethod(obj, methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, args);
    }

    /// <summary>
    /// 执行方法
    /// 可以有默认值,并支持重载函数 以及params的不定参数表
    /// </summary>
    public static object InvokePrivateMethod(object objorType, string methodName, params object[] args)
    {
        return _InvokeMethod(objorType, methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic, args);
    }


    /// <summary>
    /// 执行方法
    /// 可以有默认值,并支持重载函数 以及params的不定参数表
    /// </summary>
    public static object InvokeMethod(object objorType,string methodName,params object[] args)
    {
        if(objorType is IInvokeMethod)
        {
            return ((IInvokeMethod)objorType).InvokeMethod(methodName, args);
        }
        return _InvokeMethod(objorType, methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, args);
    }
    static object _InvokeMethod(object objorType, string methodName,BindingFlags bindingAttr, params object[] args)
    {
        if (objorType is Type)
        {
            var type = objorType as Type;
            var method= type.GetMethod(methodName,bindingAttr);
            if(method!=null)
            {
                object result;
                var bl = InvokeMethod(method, null, args, out result);
                if (bl) return result;
            }
            else
            {
                return CreateInstance(type, args);
            }
        }
        else
        {
#if CashMethod
            //CqDebug.Log(string.Format("执行对象({0})中方法({1})", objorType, methodName));
            //CqDebug.Log("args" + Torsion.Serialize(args));
            var _dic = GetMethodInfoDic(objorType.GetType());

            if (!_dic.ContainsKey(methodName))
            {
                CqDebug.Log(string.Format("对象({0})中没有方法({1})", objorType, methodName),LogType.Error);
                return null;
            }


            var _methodInfoList = GetMethodInfoDic(objorType.GetType())[methodName];
            //CqDebug.Log("_methodInfoList.Count:"+_methodInfoList.Count);
            foreach (var _methodInfo in _methodInfoList)
            {
                object result;
                //CqDebug.Log("_methodInfo:" + _methodInfo.Name);
                //CqDebug.Log("args:" + args.Length);

                var bl = InvokeMethod(_methodInfo, objorType, args, out result);
                if (bl) return result;
            }
            
#else
            var mi = objorType.GetType().GetMethod(methodName, bindingAttr);
            if(mi!=null)
            {
                return mi.Invoke(objorType, args);
            }
            else
            {
                CqDebug.Log(string.Format("对象({0})中没有方法({1})", objorType, methodName), LogType.Error);
                return null;
            }
#endif
        }
        CqDebug.Log(string.Format("对象({0})中方法({1})的参数表不匹配", objorType, methodName), LogType.Error);
        return null;
    }
    const string methodPattern = @"((\w+\.)+)(\w+)\(([^;]*)\)";
    const string equationPattern = @"((\w+\.)+)(\w+)=([^;]*)";

    /// <summary>
    /// 是一个赋值表达式
    /// </summary>
    public static bool IsEquation(string expression)
    {
        return RegexUtil.IsMatch(expression, equationPattern);
    }
    /// <summary>
    /// 赋值表达式解析
    /// </summary>
    public static object EquationParser(string expression)
    {
        var g = RegexUtil.Matches(expression, equationPattern)[0].Groups;
        var objName = g[1].Value;
        objName = objName.Remove(objName.Length - 1);
        var pName = g[3].Value;
        var v = g[4].Value; 
        var obj = MemberParser(objName);
        SetMemberValue(obj, pName, CompileAndExec(v),true);
        return obj;
    }
    /// <summary>
    /// 是一个函数调用表达式
    /// 形如a.b.c(xx,yy)
    /// </summary>
    public static bool IsMethod(string expression)
    {
        return RegexUtil.IsMatch(expression, methodPattern);
    }
    /// <summary>
    /// 编译执行函数
    /// 形式\(m.\)( \(v,\)v)
    /// </summary>
    public static object MethodParser(string expression)
    {
        var g = RegexUtil.Matches(expression, methodPattern)[0].Groups;
        var objName = g[1].Value;
        var methodName = g[3].Value;
        var argsName = g[4].Value;

        objName = objName.Remove(objName.Length - 1);
        var obj = MemberParser(objName);
        var list = Parsing.CharParse(argsName);
        List<object> args = new List<object>();
        foreach (var token in list)
        {
            switch (token.type)
            {
                case TokenType.VARIABLE:
                    {
                        args.Add(MemberParser(token.value.ToString()));
                    }
                    break;
                case TokenType.STRING:
                    {
                        args.Add(token.value);
                    }
                    break;
                case TokenType.NUMBER:
                    {
                        args.Add(token.value);
                    }
                    break;
                case TokenType.COMMA:
                    {
                    }
                    break;
                default:
                    {
                        if(token.IsNull())
                        {
                            args.Add(null);
                        }
                    }
                    break;
            }
        }
        return InvokeMethod(obj, methodName, args.ToArray());
    }
}
