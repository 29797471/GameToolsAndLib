using System;
using UnityEngine;

/// <summary>
/// 缓动一个颜色
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenColor")]
public class CqTweenColor : CqTweenT<Color>
{
    protected override Func<Color, Color, float, Color> LerpUnclamped
    {
        get
        {
            return Color.LerpUnclamped;
        }
    }
}

/// <summary>
/// 缓动一个浮点数
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenFloat")]
public class CqTweenFloat : CqTweenT<float>
{
    protected override Func<float, float, float, float> LerpUnclamped
    {
        get
        {
            return Mathf.LerpUnclamped;
        }
    }
}
/// <summary>
/// 缓动一个四维向量
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenVector4")]
public class CqTweenVector4 : CqTweenT<Vector4>
{
    protected override Func<Vector4, Vector4, float, Vector4> LerpUnclamped
    {
        get
        {
            return Vector4.LerpUnclamped;
        }
    }
}

/// <summary>
/// 缓动一个三维向量
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenVector3")]
public class CqTweenVector3 : CqTweenT<Vector3>
{
    protected override Func<Vector3, Vector3, float, Vector3> LerpUnclamped
    {
        get
        {
            return Vector3.LerpUnclamped;
        }
    }
}

/// <summary>
/// 缓动一个二维向量
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenVector2")]
public class CqTweenVector2 : CqTweenT<Vector2>
{
    protected override Func<Vector2, Vector2, float, Vector2> LerpUnclamped
    {
        get
        {
            return Vector2.LerpUnclamped;
        }
    }
}

/// <summary>
/// 缓动一个四元数
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("缓动/CqTweenQuaternion")]
public class CqTweenQuaternion : CqTweenT<Quaternion>
{
    protected override Func<Quaternion, Quaternion, float, Quaternion> LerpUnclamped
    {
        get
        {
            return Quaternion.LerpUnclamped;
        }
    }
}

