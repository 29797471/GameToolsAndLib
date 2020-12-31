using System;

/// <summary>
/// 横纵坐标或者宽高
/// </summary>
[Editor("坐标位置"), Width(250), Height(200)]
public class UVData:NotifyObject
{
    /// <summary>
    /// 横坐标或者宽
    /// </summary>
    [Export("%U%")]
    [TextBox("X="), MinWidth(150),Priority(1)]
    public float U { get { return mU; } set {mU = value; Update("U"); } }
    public float mU;

    /// <summary>
    /// 纵坐标或者高
    /// </summary>
    [Export("%V%")]
    [TextBox("Y="), MinWidth(150),Priority(2)]
    public float V{ get { return mV; } set { mV = value; Update("V"); } }
    public float mV;

    public string Name
    {
        get
        {
            return U + "," + V;
        }
    }
    public override string ToString()
    {
        return Name;
    }
}

/// <summary>
/// 二维整型宽高
/// </summary>
[Editor("二维整型"), Width(250), Height(200)]
public class Vector2Int : NotifyObject
{
    /// <summary>
    /// 宽
    /// </summary>
    [Export("%X%")]
    [TextBox("X="), MinWidth(150), Priority(1)]
    public int X { get { return mX; } set { mX = value; Update("X"); } }
    public int mX;

    /// <summary>
    /// 高
    /// </summary>
    [Export("%Y%")]
    [TextBox("Y="), MinWidth(150), Priority(2)]
    public int Y { get { return mY; } set { mY = value; Update("Y"); } }
    public int mY;

    public string Name
    {
        get
        {
            return X + "," + Y;
        }
    }
    public override string ToString()
    {
        return Name;
    }
}
/// <summary>
/// 二维整型宽高
/// </summary>
[Editor("二维浮点"), Width(250), Height(200)]
public class Vector2Float : NotifyObject
{
    /// <summary>
    /// 宽
    /// </summary>
    [Export("%X%")]
    [TextBox("X="), MinWidth(150), Priority(1)]
    public float X { get { return x; } set { x = value; Update("X"); } }
    public float x;

    /// <summary>
    /// 高
    /// </summary>
    [Export("%Y%")]
    [TextBox("Y="), MinWidth(150), Priority(2)]
    public float Y { get { return y; } set { y = value; Update("Y"); } }
    public float y;

    public string Name
    {
        get
        {
            return X + "," + Y;
        }
    }
    public override string ToString()
    {
        return Name;
    }
}