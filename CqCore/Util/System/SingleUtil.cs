using System;

/// <summary>
/// 扩展带精度的判定
/// </summary>
public static class SingleUtil
{
    public static bool IsIntData(this float a)
    {
        return (a % 1) == 0;
    }

    /// <summary>
    /// a==b
    /// a,b在误差Epsilon范围内视为相等
    /// </summary>
    public static bool EqualsByEpsilon(this float a,float b,float Epsilon=0.001f)
    {
        return Math.Abs(a-b)< Epsilon;
    }

    /// <summary>
    /// 带精度的判定a&gt;=b
    /// </summary>
    public static bool GreaterEqualsByEpsilon(this float a, float b, float Epsilon = 0.001f)
    {
        return a > b || a.EqualsByEpsilon(b, Epsilon);
    }

    /// <summary>
    /// 带精度的判定a&lt;=b
    /// </summary>
    public static bool LessEqualsByEpsilon(this float a, float b, float Epsilon = 0.001f)
    {
        return a < b || a.EqualsByEpsilon(b, Epsilon);
    }

    /// <summary>
    /// 在[-limit,limit]内视为0
    /// </summary>
    public static float Dcmp(this float v, float Epsilon = 0.001f)
    {
        return EqualZero(v, Epsilon) ? 0 : v;
    }

    /// <summary>
    /// v在[-limit,limit]误差范围内返回true
    /// </summary>
    public static bool EqualZero(this float v, float Epsilon = 0.001f)
    {
        return Math.Abs(v) < Epsilon;
    }
}
