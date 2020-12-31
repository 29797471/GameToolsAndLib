using System;

namespace CqCore
{
    /// <summary>
    /// 类型序列化方式
    /// </summary>
    public enum SerializeTypeStyle
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 字段
        /// </summary>
        Field = 1,

        /// <summary>
        /// 属性
        /// </summary>
        Property = 2, 
    }
}
