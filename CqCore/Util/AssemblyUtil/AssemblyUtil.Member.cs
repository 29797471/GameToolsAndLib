using CqCore;
using System;
using System.Reflection;

public static partial class AssemblyUtil
{
    /// <summary>
    /// 获取成员值<para/>
    /// 如果尝试在获取属性中抛出异常,则返回null<para/>
    /// 当需要获取静态成员时,obj传null
    /// </summary>
    public static object GetValue(this MemberInfo memberInfo, object obj = null)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Property://由于这实际上是一个方法,并且可能方法中会直接抛出异常,所以必定要try_catch
                try
                {
                    //ios不支持(memberInfo as PropertyInfo).GetValue(obj, null)
                    //由[]运算符重载定义的属性,在获取值时需要参数,目前还没有方法区分(貌似是看成一个名称是Item的属性),所以此处会抛异常.
                    return (memberInfo as PropertyInfo).GetGetMethod().Invoke(obj, null);
                    //return (memberInfo as PropertyInfo).GetValue(obj, null);
                }
                catch (Exception)
                {
                    return null;
                }
            case MemberTypes.Field:
                return (memberInfo as FieldInfo).GetValue(obj);
            case MemberTypes.Method:
                Func<object[], object> call = null;
                call= (args) => (memberInfo as MethodInfo).Invoke(obj, args);
                return call;
        }
        return null;
    }

    static object[] temp_Property_args = new object[1];

    /// <summary>
    /// 通过MemberInfo对对象的成员赋值
    /// </summary>
    public static void SetValue(this MemberInfo memberInfo,object obj,object value)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Property:
                {
                    temp_Property_args[0] = value;
                    
                    ((PropertyInfo)memberInfo).GetSetMethod().Invoke(obj, temp_Property_args);
                    //((PropertyInfo)memberInfo).SetValue(obj, value, null);

                    //(info as PropertyInfo).GetSetMethod().Invoke(obj, new object[] {value });
                    break;
                }
            case MemberTypes.Field:
                {
                    var info = memberInfo as FieldInfo;
                    info.SetValue(obj, value);
                    break;
                }
        }
    }
    /// <summary>
    /// 获取成员信息对应的成员的类型
    /// </summary>
    public static Type GetMemberType(this MemberInfo memberInfo)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Property:
                return (memberInfo as PropertyInfo).PropertyType;
            case MemberTypes.Field:
                return (memberInfo as FieldInfo).FieldType;
        }
        return null;
    }
    
    
    /// <summary>
    /// 获取类中的静态成员的值
    /// </summary>
    public static object GetStaticMemberValue(Type type, string memberName)
    {
        var info = GetMemberInfo(type, memberName, BindingFlags.Static | BindingFlags.Public);
        if (info == null) return null;
        return info.GetValue();
    }

    public static MemberInfo GetMemberInfo<T>(string memberName,
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
        MemberTypes type = MemberTypes.All)
    {
        return GetMemberInfo(typeof(T), memberName, bindingAttr, type);
    }

    /// <summary>
    /// 在类型(包含基类)中获取对应名称的成员信息.
    /// </summary>
    public static MemberInfo GetMemberInfo(Type objType, string memberName, 
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic,
        MemberTypes type = MemberTypes.All)
    {
        //在生成APK编译时,成员有可能被编译优化掉.导致找不到成员.
        var memberInfos = objType.GetMember(memberName, type, bindingAttr);
        
        if (memberInfos.Length==0)
        {
            if (objType.BaseType != null) return GetMemberInfo(objType.BaseType, memberName, bindingAttr);
            CqDebug.Log("在类型(" + objType + ")中找不到成员(" + memberName + ")",LogType.Error);
            return null;
        }
        return memberInfos[0];
    }


    /// <summary>
    /// 获取类型的特定成员<para/>
    /// 默认获取所有非静态成员<para/>
    /// 主要提供给对象序列化时使用
    /// </summary>
    public static MemberInfo[] GetMembers(Type objType, SerializeTypeStyle type, BindingFlags bindingAttr= BindingFlags.Public | BindingFlags.Instance)
    {
        switch (type)
        {
            case SerializeTypeStyle.Field:
                {
                    return objType.GetFields(bindingAttr);
                }
            case SerializeTypeStyle.Property:
                {
                    return objType.GetProperties(bindingAttr);
                }
        }
        return objType.GetMembers(bindingAttr);
    }

    /*
    /// <summary>
    /// 获取类型的所有非静态字段
    /// </summary>
    public static FieldInfo[] GetFields(Type objType, bool isPrivate = false)
    {
        return objType.GetFields((isPrivate ? BindingFlags.NonPublic : BindingFlags.Public) | BindingFlags.Instance);
    }

    /// <summary>
    /// 获取类型的所有非静态属性
    /// </summary>
    public static PropertyInfo[] GetProperties(Type objType, bool isPrivate = false)
    {
        return objType.GetProperties((isPrivate ? BindingFlags.NonPublic : BindingFlags.Public) | BindingFlags.Instance|BindingFlags.GetProperty|BindingFlags.SetProperty);
    }

    public static object GetPrivateMemberValue(object obj, string memberName)
    {
        var type = obj.GetType();
        MemberInfo[] privateList = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance);
        var mem = privateList.ToList().Find(x => x.Name == memberName);
        if (mem != null)
        {
            return mem.GetValue(obj);
        }
        return null;
    }
    */
}
