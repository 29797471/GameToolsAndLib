using CqCore;

namespace UnityCore
{
    /// <summary>
    /// 绑定属性类型
    /// </summary>
    public enum EnumTweenType
    {
        /// <summary>
        /// 浮点数(float)
        /// </summary>
        [EnumLabel("浮点数(float)")]
        System_Single,

        /// <summary>
        /// 二维向量(Vector2)
        /// </summary>
        [EnumLabel("二维向量(Vector2)")]
        UnityEngine_Vector2,

        /// <summary>
        /// 三维向量(Vector3)
        /// </summary>
        [EnumLabel("三维向量(Vector3)")]
        UnityEngine_Vector3,

        /// <summary>
        /// 四维向量(Vector4)
        /// </summary>
        [EnumLabel("四维向量(Vector4)")]
        UnityEngine_Vector4,

        /// <summary>
        /// 四元数(Quaternion)
        /// </summary>
        [EnumLabel("四元数(Quaternion)")]
        UnityEngine_Quaternion,

        /// <summary>
        /// 颜色(Color)
        /// </summary>
        [EnumLabel("颜色(Color)")]
        UnityEngine_Color,
    }
}
