using CqCore;
using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// 利用位操作来作状态记录的枚举
/// </summary>
public static partial class EnumUtil
{
	/// <summary>
    /// 可打印叠加态
    /// </summary>
    public static string ToString<T>(T enumValue)
    {
        var enumType = typeof(T);
        if (!enumType.IsEnum) throw new Exception("泛型只包含enum");
        var intValue = Convert.ToInt32(enumValue);
        if (intValue == -1) return "Everything";
        if (intValue == 0) return "Nothing";

        string result=null;
        for(int flag=1;flag<2048;flag=flag<<1)
        {
            if(MathUtil.StateCheck(intValue, flag))
            {
                intValue = MathUtil.StateDel(intValue, flag);
				if(result==null)
                {
                    result = GetEnumLabelName(enumType, flag);
                }
                else
                {
                    result+="|"+ GetEnumLabelName(enumType, flag);
                }
            }
            if (intValue == 0) break;
        }
        return result;
    }
}
