using CqCore;
using System;
using System.Reflection;

public static partial class AssemblyUtil
{
    /// <summary>
    /// 设置成员
    /// </summary>
    public static bool SetMemberValue(object obj, string memberName, object value, bool convert = false)
    {
        if (obj is ISetGetMemberValue)
        {
            ISetGetMemberValue objX = (obj as ISetGetMemberValue);
            if (convert && objX[memberName]!=null)
            {
                value = ConvertUtil.ConvertType(value, objX[memberName].GetType());
            }
            objX[memberName] = value;
            return true;
        }
        if (obj == null) return false;
        var mi=GetMemberInfo(obj.GetType(), memberName);
        if (mi!=null && !MathUtil.StateCheck(mi.MemberType, MemberTypes.Method))
        {
            if (convert) value = ConvertUtil.ConvertType(value, mi.GetMemberType());
            mi.SetValue(obj, value);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取成员
    /// </summary>
    public static object GetMemberValue(object obj, string memberName)
    {
        if(obj is ISetGetMemberValue)
        {
            return (obj as ISetGetMemberValue)[memberName];
        }
        var mi = GetMemberInfo(obj.GetType(), memberName);
        if (mi!=null)
        {
            return mi.GetValue(obj);
        }
        return null;
    }
    
    /// <summary>
    /// 对象是否有成员
    /// </summary>
    public static bool HasMember(object obj, string memberName)
    {
        return GetMemberInfo(obj.GetType(), memberName) != null;
    }


    /// <summary>
    /// 通过对象的成员表达式获取成员,形如:a.list[0].name
    /// </summary>
    public static object GetMemberValueByExpression(object obj, string memberExpression)
    {
        if (memberExpression.IsNullOrEmpty())
        {
            return obj;
        }
        if (memberExpression[0] == '.')
        {
            memberExpression = memberExpression.Substring(1);
        }
        if (AssemblyUtil.IsArray(obj.GetType()) || AssemblyUtil.IsList(obj.GetType()))
        {
            if (RegexUtil.IsMatch(memberExpression, @"^\[(?<index>\d*)\]"))
            {
                var group = RegexUtil.Matches(memberExpression, @"^\[(?<index>\d*)\]")[0];
                var index = int.Parse(group.Groups["index"].Value);
                memberExpression = memberExpression.Substring(group.Length);
                var list = (obj as System.Collections.IList);
                if (index < 0 || index >= list.Count) return null;
                return GetMemberValueByExpression(list[index], memberExpression);
            }
        }
        else
        {
            if (RegexUtil.IsMatch(memberExpression, @"^\w+"))
            {
                var group = RegexUtil.Matches(memberExpression, @"^\w+")[0];
                var memberName = group.Groups[0].Value;
                memberExpression = memberExpression.Substring(group.Length);
                var result = AssemblyUtil.GetMemberValue(obj, memberName);

                return GetMemberValueByExpression(result, memberExpression);
            }
        }
        return null;
    }

    const string stringPattern = @"^""(.*)""$";

    /// <summary>
    /// 解析a.b.c得到一个对象
    /// 或者一个数字或者字符串
    /// </summary>
    public static object MemberParser(string expression)
    {
        int result;
        if(int.TryParse(expression,out result))
        {
            return result;
        }
        if(RegexUtil.IsMatch(expression, stringPattern))
        {
            var g = RegexUtil.Matches(expression, stringPattern);
            return g[0].Groups[1].Value;
        }
        string typeName = null;
        
        var nn = expression.Split('.');
        Type type = null;
        object obj = null;
        foreach(var it in nn)
        {
            if (type == null)
            {
                if (typeName == null) typeName = it;
                else typeName += "." + it;
                type = AssemblyUtil.GetType(typeName);
            }
            else if (obj == null)
            {
                obj=GetStaticMemberValue(type, it);
            }
            else
            {
                obj = GetMemberValue(obj, it);
            }
        }
        if (obj == null) return type;
        return obj;
    }
}
