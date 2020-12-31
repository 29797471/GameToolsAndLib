using System;

/// <summary>
/// 扩展带精度的判定
/// </summary>
public static class DoubleUtil
{
    /// <summary>
    /// 有精度的向下取整,比如1.99999虽然向下取整,也应该是2,而不是1
    /// </summary>
    public static double FloorByEpsilon(this double f, double Epsilon = 0.001)
    {
        double v;
        if (f.EqualsByEpsilon(v = Math.Ceiling(f), Epsilon))
        {
            return v;
        }
        else
        {
            return Math.Floor(f);
        }
    }

    /// <summary>
    /// a==b
    /// a,b在误差Epsilon范围内视为相等
    /// </summary>
    public static bool EqualsByEpsilon(this double a, double b, double Epsilon = 0.001) 
    {
        return Math.Abs(a-b)< Epsilon;
    }

    /// <summary>
    /// 带精度的判定a&gt;=b
    /// </summary>
    public static bool GreaterEqualsByEpsilon(this double a, double b, double Epsilon = 0.001)
    {
        return a > b || a.EqualsByEpsilon(b, Epsilon);
    }

    /// <summary>
    /// 带精度的判定a&lt;=b
    /// </summary>
    public static bool LessEqualsByEpsilon(this double a, double b, double Epsilon = 0.001)
    {
        return a < b || a.EqualsByEpsilon(b, Epsilon);
    }

    /// <summary>
    /// 在[-limit,limit]内视为0
    /// </summary>
    public static double Dcmp(this double v, double Epsilon = 0.001)
    {
        return EqualZero(v, Epsilon) ? 0 : v;
    }

    /// <summary>
    /// v在[-limit,limit]误差范围内返回true
    /// </summary>
    public static bool EqualZero(this double v, double Epsilon = 0.001)
    {
        return Math.Abs(v) < Epsilon;
    }
}
