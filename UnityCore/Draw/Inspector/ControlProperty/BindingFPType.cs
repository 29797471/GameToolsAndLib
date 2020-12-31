using CqCore;

namespace UnityCore
{
    /// <summary>
    /// 绑定属性类型
    /// </summary>
    public enum BindingFPType
    {
        /// <summary>
        /// 字符串(string)
        /// </summary>
        [EnumLabel("字符串(string)")]
        System_String,

        /// <summary>
        /// 布尔(bool)
        /// </summary>
        [EnumLabel("布尔(bool)")]
        System_Boolean,

        /// <summary>
        /// 整数(int)
        /// </summary>
        [EnumLabel("整数(int)")]
        System_Int32,

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

        /// <summary>
        /// 颜色32(Color32)
        /// </summary>
        [EnumLabel("颜色(Color32)")]
        UnityEngine_Color32,

        /// <summary>
        /// 图标(Sprite)
        /// </summary>
        [EnumLabel("图标(Sprite)")]
        UnityEngine_Sprite,

        /// <summary>
        /// 纹理(Texture)
        /// </summary>
        [EnumLabel("图标(Texture)")]
        UnityEngine_Texture,

        /// <summary>
        /// 对象(Type_object)
        /// </summary>
        [EnumLabel("对象(object)")]
        System_Object,
    }
}
